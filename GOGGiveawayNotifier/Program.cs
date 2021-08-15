using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using NLog;
using NLog.Extensions.Logging;
using GOGGiveawayNotifier.Module;

namespace GOGGiveawayNotifier {
	class Program {
		private static readonly Logger logger = LogManager.GetCurrentClassLogger();

		static async Task Main() {
			try {
				logger.Info("- Start Job -");

				var services = DI.BuildDiAll();

				using (services as IDisposable) {
					var config = services.GetRequiredService<JsonOP>().LoadConfig();
					services.GetRequiredService<ConfigValidator>().CheckValid(config);

					var htmlDoc = services.GetRequiredService<Scraper>().GetHtmlSource();
					//var htmlDoc = new HtmlAgilityPack.HtmlDocument();
					//htmlDoc.LoadHtml(System.IO.File.ReadAllText("test.html"));

					var gameName = services.GetRequiredService<Parser>().Parse(htmlDoc);

					await services.GetRequiredService<NotifyOP>().Notify(config, gameName);
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
