using System;
using HtmlAgilityPack;
using Microsoft.Extensions.Logging;

namespace GOGGiveawayNotifier.Module {
	class Scraper: IDisposable {
		private readonly ILogger<Scraper> _logger;
		private readonly string url = "https://www.gog.com/";

		#region debug strings
		private readonly string debugGetSource = "Getting page source";
		#endregion

		public Scraper(ILogger<Scraper> logger) {
			_logger = logger;
		}

		public HtmlDocument GetHtmlSource() {
			try {
				_logger.LogDebug(debugGetSource);
				var webGet = new HtmlWeb();
				var htmlDoc = webGet.Load(url);
				_logger.LogDebug($"Done: {debugGetSource}");
				return htmlDoc;
			} catch (Exception) {
				_logger.LogError($"Error: {debugGetSource}");
				throw;
			} finally {
				Dispose();
			}
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
		}
	}
}
