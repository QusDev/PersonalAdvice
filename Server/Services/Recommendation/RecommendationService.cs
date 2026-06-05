using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.ML;
using Microsoft.ML.Trainers;
using Server.Data.Entities;
using Server.Data.MLNet;
using Server.Services.Recommendation.Interfaces;
using Server.UnitOfWork;
using Shared.Enums;

namespace Server.Services.Recommendation
{
    public class RecommendationService : IRecommendationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly MLContext _mlContext;
        private ITransformer _model;

        public RecommendationService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _mlContext = new MLContext();
            _model = null!;
        }

        public async Task UpdateCollaborativeRecs(int userId)
        {
            if (_model == null) await TrainModel();

            var viewedResult = await _unitOfWork.UserInteractions.GetAllAsync(
                pageSize: int.MaxValue,
                filter: ui => ui.UserId == userId,
                selector: ui => ui.MediaId);

            var viewedIds = viewedResult.Items.ToList();

            var candidateMediaResult = await _unitOfWork.MediaContent.GetAllAsync(
                pageNumber: 1,
                pageSize: int.MaxValue,
                filter: m => !viewedIds.Contains(m.Id));

            var candidateMedia = candidateMediaResult.Items.ToList();

            var predictionEngine = _mlContext.Model.CreatePredictionEngine<MediaRatingPrediction, PredictionResult>(_model);

            var newRatings = candidateMedia.Select(m => new RatingEntity
            {
                UserId = userId,
                MediaId = m.Id,
                Score = predictionEngine.Predict(new MediaRatingPrediction { UserId = (uint)userId, MediaId = (uint)m.Id }).Score,
                Algorithm = RecommendationAlgorithm.Collaborative,
            }).OrderByDescending(r => r.Score).Take(20).ToList();

            var oldCollaborative = await _unitOfWork.Ratings.GetAllAsync(
                pageSize: int.MaxValue,
                selector: r => r,
                filter: r => r.UserId == userId && r.Algorithm == RecommendationAlgorithm.Collaborative);

            if (oldCollaborative.Items != null && oldCollaborative.Items.Any())
            {
                _unitOfWork.Ratings.RemoveRange(oldCollaborative.Items.ToList());
            }

            await _unitOfWork.Ratings.AddRangeAsync(newRatings);
            await _unitOfWork.SaveAsync();
        }

        public async Task UpdateContentBasedRecs(int userId)
        {
            var result = await _unitOfWork.Genres.GetAllAsync(
                selector: g => g.Id,
                filter: null,
                transform: query => query
                    .Where(g => g.MediaContents.Any(m =>
                        m.UserInteractions.Any(ui => ui.UserId == userId && ui.Weight >= 5)))
                    .OrderByDescending(g => g.MediaContents
                        .SelectMany(m => m.UserInteractions)
                        .Count(ui => ui.UserId == userId && ui.Weight >= 5))
                    .Distinct(),
                pageNumber: 1,
                pageSize: 3
);

            var favoriteGenres = result.Items;

            var recs = await _unitOfWork.MediaContent.GetAllAsync(
                includes: m => m.Genres,
                pageSize: 20,
                filter: m => m.Genres.Any(g => favoriteGenres.Contains(g.Id)),
                transform: q => q.OrderByDescending(m => m.AverageRating),
                selector: m => new RatingEntity
                {
                    UserId = userId,
                    MediaId = m.Id,
                    Score = m.AverageRating,
                    Algorithm = RecommendationAlgorithm.ContentBased,
                });

            var oldContetBased = await _unitOfWork.Ratings.GetAllAsync(
                pageSize: int.MaxValue,
                selector: r => r,
                filter: r => r.UserId == userId && r.Algorithm == RecommendationAlgorithm.ContentBased);

            if (oldContetBased.Items != null && oldContetBased.Items.Any())
            {
                _unitOfWork.Ratings.RemoveRange(oldContetBased.Items.ToList());
            }

            await _unitOfWork.Ratings.AddRangeAsync(recs.Items.ToList());
            await _unitOfWork.SaveAsync();
        }

        public async Task UpdateHybridRecs(int userId)
        {
            var colabResult = await _unitOfWork.Ratings.GetAllAsync(
                pageSize: int.MaxValue,
                filter: r => r.UserId == userId && r.Algorithm == RecommendationAlgorithm.Collaborative);

            var contentResult = await _unitOfWork.Ratings.GetAllAsync(
                pageSize: int.MaxValue,
                filter: r => r.UserId == userId && r.Algorithm == RecommendationAlgorithm.ContentBased);

            var colab = colabResult.Items.ToList();
            var content = contentResult.Items.ToList();

            if (!colab.Any())
            {
                await UpdateCollaborativeRecs(userId);
                colab = (await _unitOfWork.Ratings.GetAllAsync(
                    pageSize: int.MaxValue,
                    selector: r => r,
                    filter: r => r.UserId == userId && r.Algorithm == RecommendationAlgorithm.Collaborative)).Items.ToList();
            }

            if (!content.Any())
            {
                await UpdateContentBasedRecs(userId);
                content = (await _unitOfWork.Ratings.GetAllAsync(
                    pageSize: int.MaxValue,
                    selector: r => r,
                    filter: r => r.UserId == userId && r.Algorithm == RecommendationAlgorithm.ContentBased)).Items.ToList();
            }

            var hybrid = colab.Concat(content)
                .GroupBy(r => r.MediaId)
                .Select(g => new RatingEntity
                {
                    UserId = userId,
                    MediaId = g.Key,
                    Score = g.Average(x => x.Score),
                    Algorithm = RecommendationAlgorithm.Hybrid,
                })
                .OrderByDescending(r => r.Score)
                .Take(20).ToList();

            var oldHybrid = await _unitOfWork.Ratings.GetAllAsync(
                pageSize: int.MaxValue,
                selector: r => r,
                filter: r => r.UserId == userId && r.Algorithm == RecommendationAlgorithm.Hybrid);

            if (oldHybrid.Items != null && oldHybrid.Items.Any())
            {
                _unitOfWork.Ratings.RemoveRange(oldHybrid.Items.ToList());
            }

            await _unitOfWork.Ratings.AddRangeAsync(hybrid);
            await _unitOfWork.SaveAsync();
        }

        private async Task TrainModel()
        {
            var data = await _unitOfWork.UserInteractions.GetAllAsync(
                pageSize: int.MaxValue,
                selector: ui => new MediaRatingPrediction
                {
                    UserId = (uint)ui.UserId,
                    MediaId = (uint)ui.MediaId,
                    Label = (float)ui.Weight
                });

            var trainData = _mlContext.Data.LoadFromEnumerable(data.Items);

            var pipeline = _mlContext.Transforms.Conversion.MapValueToKey(
                    outputColumnName: "UserIdEncoded",
                    inputColumnName: nameof(MediaRatingPrediction.UserId))
                .Append(_mlContext.Transforms.Conversion.MapValueToKey(
                    outputColumnName: "MediaIdEncoded",
                    inputColumnName: nameof(MediaRatingPrediction.MediaId)))
                .Append(_mlContext.Recommendation().Trainers.MatrixFactorization(new MatrixFactorizationTrainer.Options
                {
                    MatrixColumnIndexColumnName = "UserIdEncoded",
                    MatrixRowIndexColumnName = "MediaIdEncoded",
                    LabelColumnName = nameof(MediaRatingPrediction.Label),
                    NumberOfIterations = 20,
                    ApproximationRank = 100
                }));

            _model = pipeline.Fit(trainData);
        }
    }
}
