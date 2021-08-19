using System;
using System.Globalization;
using HtmlAgilityPack;
using Microsoft.Extensions.Logging;
using GOGGiveawayNotifier.Model;

namespace GOGGiveawayNotifier.Module {
	class Parser : IDisposable {
		private readonly ILogger<Parser> _logger;

		#region debug strings
		private readonly string debugParse = "Parsing";
		#endregion

		public Parser(ILogger<Parser> logger) {
			_logger = logger;
		}

		public GiveawayRecord Parse(HtmlDocument htmlDoc, GiveawayRecord oldRecord) {
			try {
				_logger.LogDebug(debugParse);
				//_logger.LogInformation(oldRecord.Name);

				var titleHref = htmlDoc.DocumentNode.SelectSingleNode(ParseStrings.titleLableXpath);

				if (titleHref == null) {
					_logger.LogDebug($"Done: {debugParse}");
					return null;
				}

				var newGiveaway = new GiveawayRecord {
					Name = titleHref.Attributes["ng-href"].Value.Split("/game/")[^1].Replace("_", " ")
				};
				var textInfo = new CultureInfo("en-US", false).TextInfo;
				newGiveaway.Name = textInfo.ToTitleCase(newGiveaway.Name);

				if (newGiveaway.Name == oldRecord.Name) {
					_logger.LogDebug($"{newGiveaway.Name} is found in previous record");
					_logger.LogDebug($"Done: {debugParse}");
					return null;
				}

				_logger.LogInformation($"Found Giveaway {newGiveaway.Name}");
				_logger.LogDebug($"Done: {debugParse}");
				return newGiveaway;
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
