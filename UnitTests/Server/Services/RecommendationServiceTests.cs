using FluentAssertions;
using Moq;
using Server.Data.Entities;
using Server.Data.MLNet;
using Server.Repositories.Interfaces;
using Server.Services.Recommendation;
using Server.Services.Recommendation.Interfaces;
using Server.UnitOfWork;
using Shared.DTOs.Repositories;
using Shared.Enums;
using System.Linq.Expressions;

namespace UnitTests.Server.Services
{
    public class RecommendationServiceTests
    {
        private readonly Mock<IUnitOfWork> _uowMock;
        private readonly Mock<IUserInteractionRepository> _userInteractionsRepoMock;
        private readonly Mock<IMediaRepository> _mediaContentRepoMock;
        private readonly Mock<IRatingRepository> _ratingsRepoMock;
        private readonly Mock<IGenreRepository> _genresRepoMock;

        private readonly IRecommendationService _service;

        public RecommendationServiceTests()
        {
            _uowMock = new Mock<IUnitOfWork>();
            _userInteractionsRepoMock = new Mock<IUserInteractionRepository>();
            _mediaContentRepoMock = new Mock<IMediaRepository>();
            _ratingsRepoMock = new Mock<IRatingRepository>();
            _genresRepoMock = new Mock<IGenreRepository>();

            _uowMock.Setup(u => u.UserInteractions).Returns(_userInteractionsRepoMock.Object);
            _uowMock.Setup(u => u.MediaContent).Returns(_mediaContentRepoMock.Object);
            _uowMock.Setup(u => u.Ratings).Returns(_ratingsRepoMock.Object);
            _uowMock.Setup(u => u.Genres).Returns(_genresRepoMock.Object);

            _service = new RecommendationService(_uowMock.Object);
        }

        [Fact]
        public async Task UpdateCollaborativeRecs_Should_ExecuteSuccessfully()
        {
            int userId = 1;

            _userInteractionsRepoMock.Setup(r => r.GetAllAsync(
                It.IsAny<Expression<Func<UserInteractionEntity, MediaRatingPrediction>>>(),
                It.IsAny<Expression<Func<UserInteractionEntity, bool>>>(),                
                It.IsAny<Func<IQueryable<UserInteractionEntity>, IOrderedQueryable<UserInteractionEntity>>>(), 
                It.IsAny<Func<IQueryable<UserInteractionEntity>, IQueryable<UserInteractionEntity>>>(), 
                It.IsAny<int>(),                                                          
                It.IsAny<int>(),                                                          
                It.IsAny<Expression<Func<UserInteractionEntity, object>>[]>()           
            )).ReturnsAsync(new PagedResponse<MediaRatingPrediction>
            {
                Items = new List<MediaRatingPrediction>
                {
                    new() { UserId = (uint)userId, MediaId = 1, Label = 5f },
                    new() { UserId = (uint)userId, MediaId = 2, Label = 4f },
                    new() { UserId = 2, MediaId = 1, Label = 3f },
                    new() { UserId = 2, MediaId = 3, Label = 5f }
                }
            });

            _userInteractionsRepoMock.Setup(r => r.GetAllAsync(
                It.IsAny<Expression<Func<UserInteractionEntity, int>>>(), 
                It.IsAny<Expression<Func<UserInteractionEntity, bool>>>(), 
                It.IsAny<Func<IQueryable<UserInteractionEntity>, IOrderedQueryable<UserInteractionEntity>>>(),
                It.IsAny<Func<IQueryable<UserInteractionEntity>, IQueryable<UserInteractionEntity>>>(), 
                It.IsAny<int>(), 
                It.IsAny<int>(), 
                It.IsAny<Expression<Func<UserInteractionEntity, object>>[]>() 
            )).ReturnsAsync(new PagedResponse<int>
            {
                Items = new List<int> { 1, 2 }
            });

            _mediaContentRepoMock.Setup(r => r.GetAllAsync(
                It.IsAny<Expression<Func<MediaContentEntity, bool>>>(), 
                It.IsAny<Func<IQueryable<MediaContentEntity>, IOrderedQueryable<MediaContentEntity>>>(),
                It.IsAny<Func<IQueryable<MediaContentEntity>, IQueryable<MediaContentEntity>>>(),
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<Expression<Func<MediaContentEntity, object>>[]>()
            )).ReturnsAsync(new PagedResponse<MediaContentEntity>
            {
                Items = new List<MediaContentEntity> { new MovieEntity { Id = 3 }, new MovieEntity { Id = 4 } }
            });

            var oldCollaborative = new List<RatingEntity> { new() { Id = 10, UserId = userId, Algorithm = RecommendationAlgorithm.Collaborative } };

            _ratingsRepoMock.Setup(r => r.GetAllAsync(
                It.IsAny<Expression<Func<RatingEntity, RatingEntity>>>(),
                It.IsAny<Expression<Func<RatingEntity, bool>>>(),        
                It.IsAny<Func<IQueryable<RatingEntity>, IOrderedQueryable<RatingEntity>>>(),
                It.IsAny<Func<IQueryable<RatingEntity>, IQueryable<RatingEntity>>>(),
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<Expression<Func<RatingEntity, object>>[]>()
            )).ReturnsAsync(new PagedResponse<RatingEntity> { Items = oldCollaborative });

            await _service.UpdateCollaborativeRecs(userId);

            _ratingsRepoMock.Verify(r => r.RemoveRange(It.IsAny<List<RatingEntity>>()), Times.Once);
            _ratingsRepoMock.Verify(r => r.AddRangeAsync(It.IsAny<List<RatingEntity>>()), Times.Once);
            _uowMock.Verify(u => u.SaveAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateContentBasedRecs_Should_ExecuteSuccessfully()
        {
            int userId = 1;
            var favoriteGenreIds = new List<int> { 5, 8 };

            var newRatings = new List<RatingEntity>
            {
                new() { MediaId = 12, Score = 4.2f, Algorithm = RecommendationAlgorithm.ContentBased }
            };

            var oldRatings = new List<RatingEntity>
            {
                new() { Id = 11, UserId = userId, Algorithm = RecommendationAlgorithm.ContentBased }
            };

            SetupGenresRepository(favoriteGenreIds);
            SetupMediaContentRepository(newRatings);
            SetupRatingsRepository(oldRatings);

            await _service.UpdateContentBasedRecs(userId);

            _ratingsRepoMock.Verify(r => r.RemoveRange(It.IsAny<List<RatingEntity>>()), Times.Once);
            _ratingsRepoMock.Verify(r => r.AddRangeAsync(It.IsAny<List<RatingEntity>>()), Times.Once);
            _uowMock.Verify(u => u.SaveAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateHybridRecs_Should_ExecuteSuccessfully()
        {
            int userId = 1;

            var collaborativeRatings = new List<RatingEntity>
            {
                new() { UserId = userId, MediaId = 10, Score = 4.0f, Algorithm = RecommendationAlgorithm.Collaborative }
            };

            var contentBasedRatings = new List<RatingEntity>
            {
                new() { UserId = userId, MediaId = 10, Score = 5.0f, Algorithm = RecommendationAlgorithm.ContentBased }
            };

            var oldHybrid = new List<RatingEntity>
            {
                new() { UserId = userId, MediaId = 10, Score = 2.0f, Algorithm = RecommendationAlgorithm.Hybrid }
            };

            _ratingsRepoMock.SetupSequence(r => r.GetAllAsync(
                It.IsAny<Expression<Func<RatingEntity, bool>>>(),
                It.IsAny<Func<IQueryable<RatingEntity>, IOrderedQueryable<RatingEntity>>>(),
                It.IsAny<Func<IQueryable<RatingEntity>, IQueryable<RatingEntity>>>(),
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<Expression<Func<RatingEntity, object>>[]>()
            ))
            .ReturnsAsync(new PagedResponse<RatingEntity> { Items = collaborativeRatings }) // Для colabResult
            .ReturnsAsync(new PagedResponse<RatingEntity> { Items = contentBasedRatings }); // Для contentResult

            _ratingsRepoMock.Setup(r => r.GetAllAsync(
                It.IsAny<Expression<Func<RatingEntity, RatingEntity>>>(), // selector: r => r
                It.IsAny<Expression<Func<RatingEntity, bool>>>(),         // filter
                It.IsAny<Func<IQueryable<RatingEntity>, IOrderedQueryable<RatingEntity>>>(),
                It.IsAny<Func<IQueryable<RatingEntity>, IQueryable<RatingEntity>>>(),
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<Expression<Func<RatingEntity, object>>[]>()
            )).ReturnsAsync(new PagedResponse<RatingEntity> { Items = oldHybrid });

            List<RatingEntity> capturedHybridRecs = null!;
            _ratingsRepoMock.Setup(r => r.AddRangeAsync(It.IsAny<List<RatingEntity>>()))
                .Callback<List<RatingEntity>>(res => capturedHybridRecs = res)
                .ReturnsAsync(true);

            await _service.UpdateHybridRecs(userId);

            _ratingsRepoMock.Verify(r => r.RemoveRange(It.IsAny<List<RatingEntity>>()), Times.Once);
            _uowMock.Verify(u => u.SaveAsync(), Times.Once);

            // Перевірка математики: (4.0 Collaborative + 5.0 ContentBased) / 2 = 4.5 Hybrid
            capturedHybridRecs.Should().NotBeNull();
            capturedHybridRecs.Count.Should().Be(1);
            capturedHybridRecs.Single().Score.Should().Be(4.5f);
            capturedHybridRecs.Single().Algorithm.Should().Be(RecommendationAlgorithm.Hybrid);
        }

        #region Mock Helpers

        private void SetupGenresRepository(List<int> genreIds)
        {
            _genresRepoMock.Setup(r => r.GetAllAsync(
                It.IsAny<Expression<Func<GenreEntity, int>>>(),
                It.IsAny<Expression<Func<GenreEntity, bool>>>(),
                It.IsAny<Func<IQueryable<GenreEntity>, IOrderedQueryable<GenreEntity>>>(),
                It.IsAny<Func<IQueryable<GenreEntity>, IQueryable<GenreEntity>>>(),
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<Expression<Func<GenreEntity, object>>[]>()
            )).ReturnsAsync(new PagedResponse<int> { Items = genreIds });
        }

        private void SetupMediaContentRepository(List<RatingEntity> ratingsToReturn)
        {
            _mediaContentRepoMock.Setup(r => r.GetAllAsync(
                It.IsAny<Expression<Func<MediaContentEntity, RatingEntity>>>(),
                It.IsAny<Expression<Func<MediaContentEntity, bool>>>(),
                It.IsAny<Func<IQueryable<MediaContentEntity>, IOrderedQueryable<MediaContentEntity>>>(),
                It.IsAny<Func<IQueryable<MediaContentEntity>, IQueryable<MediaContentEntity>>>(),
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<Expression<Func<MediaContentEntity, object>>[]>()
            )).ReturnsAsync(new PagedResponse<RatingEntity> { Items = ratingsToReturn });
        }

        private void SetupRatingsRepository(List<RatingEntity> oldRatingsToReturn)
        {
            // Налаштування для GetAllAsync (версія з селектором selector: r => r)
            _ratingsRepoMock.Setup(r => r.GetAllAsync(
                It.IsAny<Expression<Func<RatingEntity, RatingEntity>>>(),
                It.IsAny<Expression<Func<RatingEntity, bool>>>(),
                It.IsAny<Func<IQueryable<RatingEntity>, IOrderedQueryable<RatingEntity>>>(),
                It.IsAny<Func<IQueryable<RatingEntity>, IQueryable<RatingEntity>>>(),
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<Expression<Func<RatingEntity, object>>[]>()
            )).ReturnsAsync(new PagedResponse<RatingEntity> { Items = oldRatingsToReturn });

            // Налаштування для AddRangeAsync
            _ratingsRepoMock.Setup(r => r.AddRangeAsync(It.IsAny<List<RatingEntity>>()))
                .ReturnsAsync(true);
        }

        #endregion
    }

    public class PagedResult<T>
    {
        public IEnumerable<T> Items { get; set; } = new List<T>();
    }
}
