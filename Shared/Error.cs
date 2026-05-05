namespace Shared
{
    public enum ErrorType
    {
        /// <summary>
        /// Unknown error
        /// </summary>
        NONE,

        /// <summary>
        /// Validation error
        /// </summary>
        VALIDATION,

        /// <summary>
        /// Not found error
        /// </summary>
        NOT_FOUND,

        /// <summary>
        /// Server error
        /// </summary>
        FAILURE,

        /// <summary>
        /// Conflict error
        /// </summary>
        CONFLICT,

        /// <summary>
        /// Authorize error 
        /// </summary>
        UNATHORIZED,

        /// <summary>
        /// Unexpected error
        /// </summary>
        INTERNAL
    }
    public record Error(
        string Code,
        string Message,
        ErrorType Type,
        string? InvalidField = null)
    {
        public static Error NotFound(string message, string? code = null)
            => new(code ?? "record.not.found", message, ErrorType.NOT_FOUND);

        public static Error Validation(string message, string? code = null, string? invalidField = null)
            => new(code ?? "value.is.validation", message, ErrorType.VALIDATION, invalidField);

        public static Error Conflict(string message, string? code = null)
            => new(code ?? "value.is.conflict", message, ErrorType.CONFLICT);

        public static Error Failure(string message, string? code = null)
            => new(code ?? "failure", message, ErrorType.FAILURE);

        public static Error Unauthorized(string message, string? code = null)
            => new(code ?? "unathorized", message, ErrorType.UNATHORIZED);

        public static Error Internal(string? message = null, string? code = null)
            => new(code ?? "internal", message ?? "An unexpected error occurred.", ErrorType.INTERNAL);

        public Failure ToFailure() => this;
    }
}
