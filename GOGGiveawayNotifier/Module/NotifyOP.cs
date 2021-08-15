using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using GOGGiveawayNotifier.Model;
using GOGGiveawayNotifier.Notifier;

namespace GOGGiveawayNotifier.Module {
	class NotifyOP : IDisposable {
		#region DI
		private readonly ILogger<NotifyOP> _logger;
		private readonly IServiceProvider services = DI.BuildDiNotifierOnly();
		#endregion

		#region debug strings
		private readonly string debugNotify = "Notify";
		private readonly string debugDisabledFormat = "{0} notify is disabled, skipping";
		#endregion

		public NotifyOP(ILogger<NotifyOP> logger) {
			_logger = logger;
		}

		public async Task Notify(NotifyConfig config, string gameName) {
			if (gameName == string.Empty) {
				_logger.LogInformation("There's no giveaway currently.");
				return;
			}

			try {
				_logger.LogDebug(debugNotify);

				// Telegram notifications
				if (config.EnableTelegram)
					await services.GetRequiredService<TgBot>().SendMessage(config, gameName);
				else _logger.LogInformation(debugDisabledFormat, "Telegram");

				// Bark notifications
				if (config.EnableBark)
					await services.GetRequiredService<Barker>().SendMessage(config, gameName);
				else _logger.LogInformation(debugDisabledFormat, "Bark");

				//Email notifications
				if (config.EnableEmail)
					await services.GetRequiredService<Email>().SendMessage(config, gameName);
				else _logger.LogInformation(debugDisabledFormat, "Email");

				_logger.LogDebug($"Done: {debugNotify}");
			} catch (Exception) {
				_logger.LogError($"Error: {debugNotify}");
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