using System;
using HtmlAgilityPack;
using Microsoft.Extensions.Logging;

namespace GOGGiveawayNotifier.Module {
	class Scraper: IDisposable {
		private readonly ILogger<Scraper> _logger;
		private readonly string GogHomeUrl = "https://www.gog.com/";
		private readonly string GogProductUrl = "https://www.gog.com/games?priceRange=0,0&discounted=true";

		#region debug strings
		private readonly string debugGetSource = "Getting page source: {0}";
		#endregion

		public Scraper(ILogger<Scraper> logger) {
			_logger = logger;
		}

		public HtmlDocument GetGOGHomeSource() {
			try {
				_logger.LogDebug(debugGetSource, GogHomeUrl);
				var webGet = new HtmlWeb();
				var htmlDoc = webGet.Load(GogHomeUrl);
				_logger.LogDebug($"Done: {debugGetSource}", GogHomeUrl);
				return htmlDoc;
			} catch (Exception) {
				_logger.LogError($"Error: {debugGetSource}", GogHomeUrl);
				throw;
			}
		}

		public HtmlDocument GetGOGProductSource() {
			try {
				_logger.LogDebug(debugGetSource, GogProductUrl);
				var webGet = new HtmlWeb();
				var htmlDoc = webGet.Load(GogProductUrl);
				_logger.LogDebug($"Done: {debugGetSource}", GogProductUrl);
				return htmlDoc;
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
