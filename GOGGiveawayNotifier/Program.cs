using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using NLog;
using GOGGiveawayNotifier.Module;

namespace GOGGiveawayNotifier {
	class Program {
		private static readonly Logger logger = LogManager.GetCurrentClassLogger();

		static async Task Main() {
			try {
				logger.Info("- Start Job -");

				var services = DI.BuildDiAll();

				using (services as IDisposable) {
					var jsonOp = services.GetRequiredService<JsonOP>();
					var config = jsonOp.LoadConfig();
					services.GetRequiredService<ConfigValidator>().CheckValid(config);

					var htmlDoc = services.GetRequiredService<Scraper>().GetHtmlSource();
					//var htmlDoc = new HtmlAgilityPack.HtmlDocument();
					//htmlDoc.LoadHtml(System.IO.File.ReadAllText("test.html"));

					var newGiveaway = services.GetRequiredService<Parser>().Parse(htmlDoc, jsonOp.LoadData());

					await services.GetRequiredService<NotifyOP>().Notify(config, newGiveaway);

					jsonOp.WriteData(newGiveaway);

					var claimResult = await services.GetRequiredService<AutoClaimer>().Claim(config, newGiveaway);
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
