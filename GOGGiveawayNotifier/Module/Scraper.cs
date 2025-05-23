using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace GOGGiveawayNotifier.Module {
	class Scraper: IDisposable {
		private readonly ILogger<Scraper> _logger;

		#region debug strings
		private readonly string debugGetSource = "Getting page source: {0}";
		#endregion

		#region GOG Urls
		private readonly string GogHomeUrl = "https://sections.gog.com/v1/pages/2f?locale=zh-Hans";
		private readonly string GogCatalogUrl = "https://catalog.gog.com/v1/catalog?&price=between:0,0&&discounted=eq:true&page=1";
		private readonly string GogGiveawaySectionUrl = @"https://sections.gog.com/v1/pages/2f/sections/{0}?locale=zh-Hans";
		#endregion

		#region Http Client Headers
		private readonly string UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/136.0.0.0 Safari/537.36 Edg/136.0.0.0";
		private readonly string CacheControl = "no-cache";
		#endregion

		private readonly HttpClient Client = new();

		public Scraper(ILogger<Scraper> logger) {
			_logger = logger;

			Client.DefaultRequestHeaders.Add("User-Agent", UserAgent);
			Client.DefaultRequestHeaders.Add("Cache-Control", CacheControl);
		}

		public async Task<string> GetGOGHomeSource() {
			try {
				_logger.LogDebug(debugGetSource, GogHomeUrl);

				var resp = await Client.GetAsync(GogHomeUrl);
				var result = await resp.Content.ReadAsStringAsync();

				// await File.WriteAllTextAsync($"{AppDomain.CurrentDomain.BaseDirectory}Test{Path.DirectorySeparatorChar}home.json", result);

				_logger.LogDebug($"Done: {debugGetSource}", GogHomeUrl);
				return result;
			} catch (Exception) {
				_logger.LogError($"Error: {debugGetSource}", GogHomeUrl);
				throw;
			}
		}

		public async Task<string> GetGOGGiveawaySource(string sectionID) {
			string url = string.Format(GogGiveawaySectionUrl, sectionID);

			try {
				_logger.LogDebug(debugGetSource, url);

				var resp = await Client.GetAsync(url);
				var result = await resp.Content.ReadAsStringAsync();

				// await File.WriteAllTextAsync($"{AppDomain.CurrentDomain.BaseDirectory}Test{Path.DirectorySeparatorChar}giveaway.json", result);

				_logger.LogDebug($"Done: {debugGetSource}", url);
				return result;
			} catch (Exception) {
				_logger.LogError($"Error: {debugGetSource}", url);
				throw;
			}
		}

		public async Task<string> GetGOGCatalogSource() {
			try {
				_logger.LogDebug(debugGetSource, GogCatalogUrl);

				var resp = await Client.GetAsync(GogCatalogUrl);
				var result = await resp.Content.ReadAsStringAsync();

				// await File.WriteAllTextAsync($"{AppDomain.CurrentDomain.BaseDirectory}Test{Path.DirectorySeparatorChar}catalog.json", result);

				_logger.LogDebug($"Done: {debugGetSource}", GogCatalogUrl);
				return result;
			} catch (Exception) {
				_logger.LogError($"Error: {debugGetSource}", GogCatalogUrl);
				throw;
			}
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
		}
	}
}
