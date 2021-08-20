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
		private readonly string debugEnabledFormat = "Sending notifications to {0}";
		private readonly string debugDisabledFormat = "{0} notify is disabled, skipping";
		#endregion

		public NotifyOP(ILogger<NotifyOP> logger) {
			_logger = logger;
		}

		public async Task Notify(NotifyConfig config, GiveawayRecord game) {
			if (game == null || string.IsNullOrEmpty(game.Name)) {
				_logger.LogInformation("There's no giveaway currently.");
				return;
			}

			try {
				_logger.LogDebug(debugNotify);

				// Telegram notifications
				if (config.EnableTelegram) {
					_logger.LogInformation(debugEnabledFormat, "Telegram");
					await services.GetRequiredService<TgBot>().SendMessage(config, game);
				} else _logger.LogInformation(debugDisabledFormat, "Telegram");

				// Bark notifications
				if (config.EnableBark) {
					_logger.LogInformation(debugEnabledFormat, "Bark");
					await services.GetRequiredService<Barker>().SendMessage(config, game);
				} else _logger.LogInformation(debugDisabledFormat, "Bark");

				//QQ notifications
				if (config.EnableQQ) {
					_logger.LogInformation(debugEnabledFormat, "QQ");
					await services.GetRequiredService<QQPusher>().SendMessage(config, game);
				} else _logger.LogInformation(debugDisabledFormat, "QQ");

				// PushPlus notifications
				if (config.EnablePushPlus) {
					_logger.LogInformation(debugEnabledFormat, "PushPlus");
					await services.GetRequiredService<PushPlus>().SendMessage(config, game);
				} else _logger.LogInformation(debugDisabledFormat, "PushPlus");

				//Email notifications
				if (config.EnableEmail) {
					_logger.LogInformation(debugEnabledFormat, "Email");
					await services.GetRequiredService<Email>().SendMessage(config, game);
				} else _logger.LogInformation(debugDisabledFormat, "Email");

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