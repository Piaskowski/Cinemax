using Cinemax.Shared.Contracts.Common;
using Cinemax.Shared.Resources.Common;
using System.Net.Http.Json;

namespace Cinemax.Client.Common.Http
{
    public static class HttpClientExtensions
    {
        public static async Task<ApiResult<TResponse>> GetAndReadAsync<TResponse>(
            this HttpClient httpClient,
            string url)
        {
            try
            {
                var response = await httpClient.GetAsync(url);

                return await ReadAsync<TResponse>(response);
            }
            catch
            {
                return ApiResult<TResponse>.Failure(CommonMessages.Error_LostConnection);
            }
        }

        public static async Task<ApiResult<TResponse>> PostAndReadAsync<TRequest, TResponse>(
            this HttpClient httpClient,
            string url,
            TRequest request)
        {
            try
            {
                var response = await httpClient.PostAsJsonAsync(url, request);
                
                return await ReadAsync<TResponse>(response);
            }
            catch
            {
                return ApiResult<TResponse>.Failure(CommonMessages.Error_LostConnection);
            }
        }

        public static async Task<ApiResult> PostAndReadAsync<TRequest>(
            this HttpClient httpClient,
            string url,
            TRequest request)
        {
            try
            {
                var response = await httpClient.PostAsJsonAsync(url, request);

                return await ReadAsync(response);
            }
            catch
            {
                return ApiResult.Failure(CommonMessages.Error_LostConnection);
            }
        }

        private static async Task<ApiResult<TResponse>> ReadAsync<TResponse>(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadFromJsonAsync<TResponse>();

                if (data is null)
                {
                    return ApiResult<TResponse>.Failure(CommonMessages.Error_UnexpectedError);
                }

                return ApiResult<TResponse>.Success(data);
            }

            var error = await response.Content.ReadFromJsonAsync<ErrorResponse>();

            if (error is null)
            {
                return ApiResult<TResponse>.Failure(CommonMessages.Error_UnexpectedError);
            }

            return ApiResult<TResponse>.Failure(error.Message, error.Errors);
        }

        private static async Task<ApiResult> ReadAsync(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
            {
                return ApiResult.Success();
            }

            var error = await response.Content.ReadFromJsonAsync<ErrorResponse>();

            if (error is null)
            {
                return ApiResult.Failure(CommonMessages.Error_UnexpectedError);
            }

            return ApiResult.Failure(error.Message, error.Errors);
        }
    }
}
