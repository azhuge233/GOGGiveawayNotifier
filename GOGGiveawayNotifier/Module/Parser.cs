using GOGGiveawayNotifier.Model;
using GOGGiveawayNotifier.Model.GOG;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GOGGiveawayNotifier.Module {
	class Parser(ILogger<Parser> logger, Scraper scraper) : IDisposable {
		private readonly ILogger<Parser> _logger = logger;

		#region debug strings
		private readonly string debugParseGiveaway = "Parsing giveaway";
		private readonly string debugParseFreeGames = "Parsing free games";
		#endregion

		public async Task<Tuple<List<GiveawayRecord>, List<GiveawayRecord>>> ParseGiveaway(string data, List<GiveawayRecord> oldRecords) {
			try {
				_logger.LogDebug(debugParseGiveaway);

				var resultList = new List<GiveawayRecord>();
				var notifyList = new List<GiveawayRecord>();

				var sectionsJsonData = JsonConvert.DeserializeObject<HomePageSections>(data);

				var giveawaySection = sectionsJsonData.Sections.FirstOrDefault(section => section.SectionType == ParseStrings.giveawaySectionType, null);

				if (giveawaySection == null) {
					_logger.LogDebug("No giveaway section detected");
					_logger.LogDebug($"Done: {debugParseGiveaway}");
					return new (resultList, notifyList);
				}

				_logger.LogDebug($"Found giveaway section ID: {giveawaySection.SectionID}");

				var giveawayData = await scraper.GetGOGGiveawaySource(giveawaySection.SectionID);

				var giveawayJsonData = JsonConvert.DeserializeObject<Giveaway>(giveawayData);

				var newGiveaway = new GiveawayRecord {
					ID = giveawayJsonData.Properties.Product.ID,
					Type = ParseStrings.typeGiveaway,
					Url = ParseStrings.GiveawayUrl,
					Title = giveawayJsonData.Properties.Product.Title,
					Slug = giveawayJsonData.Properties.Product.Slug,
					EndDate = giveawayJsonData.Properties.EndDate,
					ProductType = giveawayJsonData.Properties.Product.ProductType,
					ProductState = giveawayJsonData.Properties.Product.ProductState,
				};

				_logger.LogDebug($"{newGiveaway.Title} | {newGiveaway.EndDate} | {newGiveaway.ProductType} | {newGiveaway.ProductState}");

				if (!oldRecords.Any(record => record.ID == newGiveaway.ID)) {
					_logger.LogInformation($"Found Giveaway: {newGiveaway.Title}");
					notifyList.Add(newGiveaway);
				} else _logger.LogDebug($"{newGiveaway.Title} is found in previous record");

				resultList.Add(newGiveaway);

				_logger.LogDebug($"Done: {debugParseGiveaway}");
				return new (resultList, notifyList);
			} catch (Exception) {
				_logger.LogError($"Error: {debugParseGiveaway}");
				throw;
			}
		}

		public Tuple<List<GiveawayRecord>, List<GiveawayRecord>> ParseFreeGames(string source, List<GiveawayRecord> oldRecords, List<GiveawayRecord> prevResultList, List<GiveawayRecord> prevNotifyList) {
			try {
				_logger.LogDebug(debugParseFreeGames);

				var resultList = new List<GiveawayRecord>(prevResultList);
				var notifyList = new List<GiveawayRecord>(prevNotifyList);

				var catalogsJsonData = JsonConvert.DeserializeObject<Catalogs>(source);

				// Console.WriteLine($"{catalogsJsonData.ProductCount} {catalogsJsonData.Products.Count}");

				if (catalogsJsonData.ProductCount == 0 || catalogsJsonData.Products.Count == 0) {
					_logger.LogDebug("No free games detected");
					_logger.LogDebug($"Done: {debugParseFreeGames}");
					return new (resultList, notifyList);
				} else _logger.LogDebug($"Found free games count: {catalogsJsonData.ProductCount}");

				var products = catalogsJsonData.Products;

				foreach (var product in products) {
					var newFreeGame = new GiveawayRecord() {
						ID = product.ID,
						Type = ParseStrings.typeFreeGame,
						Url = product.StoreLink,
						Title = product.Title,
						Slug = product.Slug,
						ProductState = product.ProductState,
						ProductType = product.ProductType
					};

					if (!oldRecords.Any(record => record.ID == newFreeGame.ID)) {
						_logger.LogInformation($"Found new free game: {newFreeGame.Title}");

						if (!prevResultList.Any(record => record.ID == newFreeGame.ID)) notifyList.Add(newFreeGame);
						else _logger.LogInformation($"{newFreeGame.Title} is also on giveaway, stop adding to notify list");
					} else _logger.LogDebug($"{newFreeGame.Title} is found in previous record");

					resultList.Add(newFreeGame);
				}

				_logger.LogDebug($"Done: {debugParseFreeGames}");
				return new (resultList, notifyList);
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
