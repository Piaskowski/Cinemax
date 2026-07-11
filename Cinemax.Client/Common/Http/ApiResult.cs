namespace Cinemax.Client.Common.Http
{
    public class ApiResult
    {
        public bool IsSuccess { get; protected init; }

        public string? ErrorMessage { get; protected init; }

        public IEnumerable<string> Errors { get; protected init; } = [];

        public static ApiResult Success()
        {
            return new ApiResult
            {
                IsSuccess = true
            };
        }

        public static ApiResult Failure(
            string errorMessage,
            IEnumerable<string>? errors = null)
        {
            return new ApiResult
            {
                IsSuccess = false,
                ErrorMessage = errorMessage,
                Errors = errors ?? []
            };
        }
    }

    public class ApiResult<T> : ApiResult
    {
        public T? Data { get; private init; }

        public static ApiResult<T> Success(T data)
        {
            return new ApiResult<T>
            {
                IsSuccess = true,
                Data = data
            };
        }

        public new static ApiResult<T> Failure(
            string errorMessage,
            IEnumerable<string>? errors = null)
        {
            return new ApiResult<T>
            {
                IsSuccess = false,
                ErrorMessage = errorMessage,
                Errors = errors ?? []
            };
        }
    }
}
