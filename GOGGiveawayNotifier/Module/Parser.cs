using System;
using System.Globalization;
using HtmlAgilityPack;
using Microsoft.Extensions.Logging;
using GOGGiveawayNotifier.Model;
using System.Collections.Generic;
using System.Linq;

namespace GOGGiveawayNotifier.Module {
	class Parser : IDisposable {
		private readonly ILogger<Parser> _logger;

		#region debug strings
		private readonly string debugParseGiveaway = "Parsing giveaway";
		private readonly string debugParseFreeGames = "Parsing free games";
		#endregion

		public Parser(ILogger<Parser> logger) {
			_logger = logger;
		}

		public Tuple<List<GiveawayRecord>, List<GiveawayRecord>> ParseGiveaway(string source, List<GiveawayRecord> oldRecords) {
			try {
				_logger.LogDebug(debugParseGiveaway);

				var htmlDoc = new HtmlDocument();
				htmlDoc.LoadHtml(source);

				var giveawayDiv = htmlDoc.DocumentNode.SelectSingleNode(ParseStrings.giveawayDivXPath);
				var resultList = new List<GiveawayRecord>();
				var notifyList = new List<GiveawayRecord>();

				if (giveawayDiv == null) {
					_logger.LogDebug("No giveaway detected");
					_logger.LogDebug($"Done: {debugParseGiveaway}");

					resultList.Add(oldRecords.FirstOrDefault(record => record.Url == ParseStrings.GiveawayUrl));

					return new Tuple<List<GiveawayRecord>, List<GiveawayRecord>>(resultList, notifyList);
				}

				var giveawayALink = giveawayDiv.SelectSingleNode(ParseStrings.giveawayALinkXPath);

				if (giveawayALink == null) {
					_logger.LogDebug("Get giveaway link failed");
					_logger.LogDebug($"Done: {debugParseGiveaway}");

					resultList.Add(oldRecords.FirstOrDefault(record => record.Url == ParseStrings.GiveawayUrl));

					return new Tuple<List<GiveawayRecord>, List<GiveawayRecord>>(resultList, notifyList);
				}

				var newGiveaway = new GiveawayRecord {
					Name = giveawayALink.Attributes["href"].Value.Split("/game/").Last().Replace("_", " "),
					Url = ParseStrings.GiveawayUrl
				};
				var textInfo = new CultureInfo("en-US", false).TextInfo;
				newGiveaway.Name = textInfo.ToTitleCase(newGiveaway.Name);

				if (!oldRecords.Any(record => record.Name == newGiveaway.Name)) {
					_logger.LogInformation($"Found Giveaway {newGiveaway.Name}");
					notifyList.Add(newGiveaway);
				} else _logger.LogDebug($"{newGiveaway.Name} is found in previous record");

				resultList.Add(newGiveaway);

				_logger.LogDebug($"Done: {debugParseGiveaway}");
				return new Tuple<List<GiveawayRecord>, List<GiveawayRecord>>(resultList, notifyList);
			} catch (Exception) {
				_logger.LogError($"Error: {debugParseGiveaway}");
				throw;
			}
		}

		public Tuple<List<GiveawayRecord>, List<GiveawayRecord>> ParseFreeGames(string source, List<GiveawayRecord> oldRecords, List<GiveawayRecord> prevResultList, List<GiveawayRecord> prevNotifyList) {
			try {
				_logger.LogDebug(debugParseFreeGames);

				var htmlDoc = new HtmlDocument();
				htmlDoc.LoadHtml(source);

				var tiles = htmlDoc.DocumentNode.SelectNodes(ParseStrings.productTileXPath);
				var resultList = new List<GiveawayRecord>(prevResultList);
				var notifyList = new List<GiveawayRecord>(prevNotifyList);

				if (tiles == null || tiles.Count == 0) {
					_logger.LogDebug("No free games detected");

					resultList.AddRange(oldRecords.Where(record => record.Url != ParseStrings.GiveawayUrl));

					_logger.LogDebug($"Done: {debugParseFreeGames}");
					return new Tuple<List<GiveawayRecord>, List<GiveawayRecord>>(resultList, notifyList);
				} else _logger.LogDebug($"Found free games count: {tiles.Count}");

				foreach (var tile in tiles) {
					var name = tile.SelectNodes(ParseStrings.productTitleSpanXPath).Last().InnerText;
					var url = tile.SelectSingleNode(ParseStrings.productLinkXPath).Attributes["href"].Value;

					var newFreeGame = new GiveawayRecord() {
						Name = name, Url = url
					};

					if (!oldRecords.Any(record => record.Name == newFreeGame.Name || record.Url == newFreeGame.Url) ||
						prevNotifyList.Any(record => record.Name == newFreeGame.Name)) {
						_logger.LogInformation($"Found new free game: {newFreeGame.Name}");
						notifyList.Add(newFreeGame);
					} else _logger.LogDebug($"{newFreeGame.Name} is found in previous record");

					resultList.Add(newFreeGame);
				}

				_logger.LogDebug($"Done: {debugParseFreeGames}");
				return new Tuple<List<GiveawayRecord>, List<GiveawayRecord>>(resultList, notifyList);
			} catch (Exception) {
				_logger.LogError($"Error: {debugParseFreeGames}");
				throw;
			}
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
		}
	}
}
