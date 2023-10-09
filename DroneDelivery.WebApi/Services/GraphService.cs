using DroneDelivery.Domain.Interfaces;
using Newtonsoft.Json;

namespace DroneDelivery.WebApi.Services
{
	public class GraphService : IGraphService
	{
		private readonly IHttpClientFactory _httpClientFactory;

		public GraphService(IHttpClientFactory httpClientFactory)
		{
			_httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
		}

		public async Task<Dictionary<string, Dictionary<string, double>>> GetGraphAsync()
		{
			try
			{
				var httpClient = _httpClientFactory.CreateClient();
				var response = await httpClient.GetAsync("https://mocki.io/v1/10404696-fd43-4481-a7ed-f9369073252f");

				if (!response.IsSuccessStatusCode) throw new HttpRequestException("Falha ao buscar o JSON da URL.");

				var json = await response.Content.ReadAsStringAsync();
				var graph = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, double>>>(json);
				return graph;
			}
			catch (Exception ex)
			{
				throw new Exception($"Erro ao buscar o JSON da URL. Messagem de erro: {ex.Message}");
			}
		}
	}
}
