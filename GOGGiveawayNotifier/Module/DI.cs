using System;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NLog.Extensions.Logging;
using GOGGiveawayNotifier.Notifier;
using GOGGiveawayNotifier.Model;
using System.IO;

namespace GOGGiveawayNotifier.Module {
    public static class DI {
		private static readonly string BasePath = AppDomain.CurrentDomain.BaseDirectory;

		private static readonly IConfigurationRoot logConfig = new ConfigurationBuilder()
           .SetBasePath(BasePath)
           .Build();
		private static readonly IConfigurationRoot configuration = new ConfigurationBuilder()
		   .SetBasePath(BasePath)
           .AddJsonFile($"Config{Path.DirectorySeparatorChar}config.json", optional: false, reloadOnChange: true)
		   .Build();

		public static IServiceProvider BuildAll() {
            return new ServiceCollection()
               .AddTransient<JsonOP>()
               .AddTransient<ConfigValidator>()
               .AddTransient<Scraper>()
               .AddTransient<Parser>()
               .AddTransient<NotifyOP>()
               .AddTransient<Barker>()
               .AddTransient<TgBot>()
               .AddTransient<Email>()
               .AddTransient<QQHttp>()
               .AddTransient<QQWebSocket>()
               .AddTransient<PushPlus>()
               .AddTransient<DingTalk>()
               .AddTransient<PushDeer>()
			   .AddTransient<Discord>()
               .AddTransient<Meow>()
			   .AddTransient<AutoClaimer>()
               .Configure<Config>(configuration)
               .AddLogging(loggingBuilder => {
                   // configure Logging with NLog
                   loggingBuilder.ClearProviders();
                   loggingBuilder.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
                   loggingBuilder.AddNLog(logConfig);
               })
               .BuildServiceProvider();
        }
	}
}