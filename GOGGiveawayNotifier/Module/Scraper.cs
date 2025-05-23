using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace GOGGiveawayNotifier.Module {
	class Scraper: IDisposable {
		private readonly ILogger<Scraper> _logger;
		private readonly string GogHomeUrl = "https://www.gog.com/";
		private readonly string GogProductUrl = "https://www.gog.com/games?priceRange=0,0&discounted=true";

		private readonly HttpClient Client = new();

		#region debug strings
		private readonly string debugGetSource = "Getting page source: {0}";
		#endregion

		public Scraper(ILogger<Scraper> logger) {
			_logger = logger;
		}

		public async Task<string> GetGOGHomeSource() {
			try {
				_logger.LogDebug(debugGetSource, GogHomeUrl);

				var resp = await Client.GetAsync(GogHomeUrl);
				var result = await resp.Content.ReadAsStringAsync();

				_logger.LogDebug($"Done: {debugGetSource}", GogHomeUrl);
				return result;
			} catch (Exception) {
				_logger.LogError($"Error: {debugGetSource}", GogHomeUrl);
				throw;
			}
		}

		public async Task<string> GetGOGProductSource() {
			try {
				_logger.LogDebug(debugGetSource, GogProductUrl);

				var resp = await Client.GetAsync(GogProductUrl);
				var result = await resp.Content.ReadAsStringAsync();

				_logger.LogDebug($"Done: {debugGetSource}", GogProductUrl);
				return result;
			} catch (Exception) {
				_logger.LogError($"Error: {debugGetSource}", GogProductUrl);
				throw;
			}
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
		}
	}
}
