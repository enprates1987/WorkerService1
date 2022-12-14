using System.Net.Http;

namespace WorkerService1
{
	public class Worker : BackgroundService
	{
		private const string C_Token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3ByaW1hcnlzaWQiOiIxIiwiZXhwIjoxNjcxNTI4MjExLCJpc3MiOiJKd3Q6SXNzdWVyIiwiYXVkIjoiSnd0OkF1ZGllbmNlIn0.wqmYOabaI4jA1CeyNfu0VCYI7J9xflpcI-E5oRwaBsM";

		private readonly ILogger<Worker> _logger;

		public Worker(ILogger<Worker> logger)
		{
			_logger = logger;
		}

		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			_logger.LogInformation("O serviço está iniciando.");

			stoppingToken.Register(() => _logger.LogInformation("Tarefa de segundo plano está parando."));

			while (!stoppingToken.IsCancellationRequested)
			{
				_logger.LogInformation("Executando tarefa: {time}", DateTimeOffset.Now);
				await Task.Delay(10000, stoppingToken);

				HttpClient client = new HttpClient();
				HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, "http://monitdb-dev.ddns.net:5000/api/execcomponent/RLJOBFAIL/5");
			
				client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", C_Token);
				HttpResponseMessage response = client.Send(httpRequestMessage);

				if (response.IsSuccessStatusCode)
					_logger.LogInformation(response.Content.ReadAsStringAsync().GetAwaiter().GetResult());
				else
					_logger.LogInformation("HttpResponseMessage.IsSuccessStatusCode is false ");
				
			}

			_logger.LogInformation("O serviço está parando.");
		}
	}
}