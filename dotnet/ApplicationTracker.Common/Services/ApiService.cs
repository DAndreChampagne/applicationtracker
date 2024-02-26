

using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using PluralizeService.Core;

namespace ApplicationTracker.Common.Services {
    public class ApiService<T> {
        public const string CONFIG_PATH = "ApplicationTrackerApi:BaseAddress";

        public string Name { get; internal set; }
        public Uri BaseAddress { get; internal set; }
        public HttpClient Client { get; internal set; }    

        internal readonly JsonSerializerOptions _SerializerOptions = new() {
            PropertyNameCaseInsensitive = true,
        };

        public ApiService(IConfiguration config): this(Path.Combine(config[CONFIG_PATH] ?? throw new InvalidProgramException($"Value for \"{CONFIG_PATH}\" not found in configuration file"), PluralizationProvider.Pluralize(typeof(T).Name), " ").Trim()) {

        }

        public ApiService(string baseAddress) {
            Name = PluralizationProvider.Pluralize(typeof(T).Name);
            BaseAddress = new(baseAddress);
            Client = new() {
                BaseAddress = BaseAddress,
            };
            Client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }


        #region Helper methods

        internal string _BuildEndpoint(string endpoint, params object[] args) {
            ArgumentNullException.ThrowIfNull(endpoint);
            return (args.Length > 0) ? Path.Combine(endpoint, string.Join('/', args)) : endpoint;
        }

        internal async Task<HttpResponseMessage> _ExecuteGetRequestAsync(string endpoint, params object[] args) {
            return await Client.GetAsync(_BuildEndpoint(endpoint, args));
        }

        internal async Task<X?> _ProcessResponseAsync<X>(HttpResponseMessage response) {
            var stream = await response.Content.ReadAsStreamAsync() ?? Stream.Null;
            return await JsonSerializer.DeserializeAsync<X>(stream, _SerializerOptions);
        }

        #endregion
        

        internal async Task<IEnumerable<T>> ExecuteGetAsync(string endpoint, params object[] args) {
            var response = await _ExecuteGetRequestAsync(endpoint, args);
            if (!response.IsSuccessStatusCode)
                return [];
            var results = await _ProcessResponseAsync<IEnumerable<T>>(response);

            return results ?? [];
        }

        internal async Task<T?> ExecuteGetOneAsync(string endpoint, params object[] args) {
            var response = await _ExecuteGetRequestAsync(endpoint, args);
            if (!response.IsSuccessStatusCode)
                return default;
            return await _ProcessResponseAsync<T>(response) ?? default;
        }

        internal async Task<T?> ExecutePutAsync(string endpoint, int id, T entity) {
            var response = await Client.PutAsJsonAsync(_BuildEndpoint(endpoint), entity, _SerializerOptions);
            if (!response.IsSuccessStatusCode)
                throw new Exception(response.StatusCode.ToString(), new Exception(response.ReasonPhrase));
            return entity;
        }

        internal async Task<T?> ExecutePostAsync(string endpoint, T entity) {
            var response = await Client.PostAsJsonAsync(_BuildEndpoint(endpoint), entity, _SerializerOptions);
            if (!response.IsSuccessStatusCode)
                throw new Exception(response.StatusCode.ToString(), new Exception(response.ReasonPhrase));
            return entity;
        }

        internal async Task<bool> ExecuteDeleteAsync(string endpoint, int id) {
            var response = await Client.DeleteAsync(_BuildEndpoint(endpoint, id));
            if (!response.IsSuccessStatusCode)
                throw new Exception(response.StatusCode.ToString(), new Exception(response.ReasonPhrase));
            return true;
        }


        public async Task<IEnumerable<T>> GetAsync() => await ExecuteGetAsync("");
        public async Task<T?> GetAsync(int id) => await ExecuteGetOneAsync("", id);
        public async Task<T?> PutAsync(int id, T entity) => await ExecutePutAsync("", id, entity);
        public async Task<T?> PostAsync(T entity) => await ExecutePostAsync("", entity);
        public async Task<bool> DeleteAsync(int id) => await ExecuteDeleteAsync("", id);
        public async Task<bool> ExistsAsync(int id) => (await ExecuteGetOneAsync("exists", id)) is not null;

    }

}