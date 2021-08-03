using System;
using System.Globalization;
using HtmlAgilityPack;
using Microsoft.Extensions.Logging;

namespace GOGGiveawayNotifier {
	class Parser : IDisposable {
		private readonly ILogger<Parser> _logger;
		private readonly string debugParse = "Parsing";
		private readonly string titleLableXpath = ".//a[@giveaway-banner]";

		public Parser(ILogger<Parser> logger) {
			_logger = logger;
		}

		public string Parse(HtmlDocument htmlDoc) {
			try {
				_logger.LogDebug(debugParse);

				var titleHref = htmlDoc.DocumentNode.SelectSingleNode(titleLableXpath);

				if (titleHref == null) {
					_logger.LogDebug($"Done: {debugParse}");
					return string.Empty;
				}

				var gameName = titleHref.Attributes["ng-href"].Value.Split("/game/")[^1].Replace("_", " ");
				var textInfo = new CultureInfo("en-US", false).TextInfo;
				gameName = textInfo.ToTitleCase(gameName);

				_logger.LogInformation($"Found Giveaway {gameName}");
				_logger.LogDebug($"Done: {debugParse}");
				return gameName;
			} catch (Exception) {
				_logger.LogError($"Error: {debugParse}");
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
