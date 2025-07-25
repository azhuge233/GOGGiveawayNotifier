﻿using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using NLog;
using GOGGiveawayNotifier.Module;
using System.Linq;
using GOGGiveawayNotifier.Model;

namespace GOGGiveawayNotifier {
	class Program {
		private static readonly Logger logger = LogManager.GetCurrentClassLogger();

		static async Task Main() {
			try {
				logger.Info("- Start Job -");

				var services = DI.BuildAll();

				using (services as IDisposable) {
					var jsonOp = services.GetRequiredService<JsonOP>();
					var scraper = services.GetRequiredService<Scraper>();
					var parser = services.GetRequiredService<Parser>();

					services.GetRequiredService<ConfigValidator>().CheckValid();

					var gogHomeData = await scraper.GetGOGHomeSource();
					var gogCatalogData = await scraper.GetGOGCatalogSource();

					//var gogHomeHtmlDoc = new HtmlAgilityPack.HtmlDocument();
					//gogHomeHtmlDoc.LoadHtml(System.IO.File.ReadAllText("TestHtml\\test2.html"));
					//var gogProductPageHtmlDoc = new HtmlAgilityPack.HtmlDocument();
					//gogProductPageHtmlDoc.LoadHtml(System.IO.File.ReadAllText("test2.html"));

					var oldRecords = jsonOp.LoadData();

					var tuple1 = await parser.ParseGiveaway(gogHomeData, oldRecords);
					var tuple2 = parser.ParseFreeGames(gogCatalogData, oldRecords, tuple1.Item1, tuple1.Item2);

					await services.GetRequiredService<NotifyOP>().Notify(tuple2.Item2);

					jsonOp.WriteData(tuple2.Item1);

					var claimResult = await services.GetRequiredService<AutoClaimer>().Claim(tuple2.Item2.FirstOrDefault(game => game.Url == ParseStrings.GiveawayUrl));
				}

				logger.Info("- End Job -\n\n");
			} catch (Exception ex) {
				logger.Error(ex.Message);
				logger.Error($"{ex.InnerException.Message}\n\n");
			} finally {
				LogManager.Shutdown();
			}
		}
	}
}
