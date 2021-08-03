using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using NLog;
using NLog.Extensions.Logging;

namespace GOGGiveawayNotifier {
	class Program {
		private static readonly Logger logger = LogManager.GetCurrentClassLogger();
		private static readonly IConfigurationRoot config = new ConfigurationBuilder()
			.SetBasePath(Directory.GetCurrentDirectory())
			.Build();

		public static IServiceProvider BuildDi() {
			return new ServiceCollection()
				.AddTransient<JsonOP>()
				.AddTransient<TgBot>()
				.AddTransient<Parser>()
				.AddTransient<Scraper>()
				.AddLogging(loggingBuilder => {
					loggingBuilder.ClearProviders();
					loggingBuilder.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
					loggingBuilder.AddNLog(config);
				})
				.BuildServiceProvider();
		}

		static async Task Main() {
			try {
				logger.Info("- Start Job -");
				var services = BuildDi();

				var jsonOp = services.GetRequiredService<JsonOP>();
				var config = jsonOp.LoadConfig();

				var scraper = services.GetRequiredService<Scraper>();
				var htmlDoc = scraper.GetHtmlSource();

				var parser = services.GetRequiredService<Parser>();
				var gameName = parser.Parse(htmlDoc);

				if (gameName == string.Empty) {
					logger.Info("There's no giveaway currently.");
					logger.Info("- End Job -\n\n");
					return;
				}

				var tgBot = services.GetRequiredService<TgBot>();
				await tgBot.SendMessage(token: config["TOKEN"], chatID: config["CHAT_ID"], gameName: gameName, htmlMode: true);
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
