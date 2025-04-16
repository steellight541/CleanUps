using CleanUps.MAUI.Shared.AuthServices.AuthInterfaces;
using Microsoft.JSInterop;

namespace CleanUps.MAUI.Shared.AuthServices
{
    /// <summary>
    /// Simple session service implementation using browser localStorage
    /// </summary>
    public class LocalStorageSessionService : ISessionService
    {
        private readonly IJSRuntime _jsRuntime;

        /// <summary>
        /// Initialize a new instance of the LocalStorageSessionService
        /// </summary>
        /// <param name="jsRuntime">JS runtime for accessing localStorage</param>
        public LocalStorageSessionService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        /// <inheritdoc/>
        public async Task Clear()
        {
            try
            {
                await _jsRuntime.InvokeVoidAsync("sessionStorage.clear");
            }
            catch
            {
                // Ignore errors during JS interop
            }
        }

        /// <inheritdoc/>
        public async Task<int?> GetInt(string key)
        {
            try
            {
                var value = await _jsRuntime.InvokeAsync<string>("sessionStorage.getItem", key);
                if (string.IsNullOrEmpty(value) || !int.TryParse(value, out var intValue))
                {
                    return null;
                }
                return intValue;
            }
            catch
            {
                // Ignore errors during JS interop
                return null;
            }
        }

        /// <inheritdoc/>
        public async Task<string?> GetString(string key)
        {
            try
            {
                return await _jsRuntime.InvokeAsync<string>("sessionStorage.getItem", key);
            }
            catch
            {
                // Ignore errors during JS interop
                return null;
            }
        }

        /// <inheritdoc/>
        public async Task SetInt(string key, int value)
        {
            try
            {
                await _jsRuntime.InvokeVoidAsync("sessionStorage.setItem", key, value.ToString());
            }
            catch
            {
                // Ignore errors during JS interop
            }
        }

        /// <inheritdoc/>
        public async Task SetString(string key, string value)
        {
            try
            {
                await _jsRuntime.InvokeVoidAsync("sessionStorage.setItem", key, value);
            }
            catch
            {
                // Ignore errors during JS interop
            }
        }
    }
}