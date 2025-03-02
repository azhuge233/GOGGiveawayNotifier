using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using GOGGiveawayNotifier.Model;
using GOGGiveawayNotifier.Notifier;
using System.Collections.Generic;

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

		public async Task Notify(NotifyConfig config, List<GiveawayRecord> game) {
			if (game == null || game.Count == 0) {
				_logger.LogInformation("There's no (new) free games currently.");
				return;
			}

			try {
				_logger.LogDebug(debugNotify);

				var notifyTask = new List<Task>();

				// Telegram notifications
				if (config.EnableTelegram) {
					_logger.LogInformation(debugEnabledFormat, "Telegram");
					notifyTask.Add(services.GetRequiredService<TgBot>().SendMessage(config, game));
				} else _logger.LogInformation(debugDisabledFormat, "Telegram");

				// Bark notifications
				if (config.EnableBark) {
					_logger.LogInformation(debugEnabledFormat, "Bark");
					notifyTask.Add(services.GetRequiredService<Barker>().SendMessage(config, game));
				} else _logger.LogInformation(debugDisabledFormat, "Bark");

				// QQ Http notifications
				if (config.EnableQQHttp) {
					_logger.LogInformation(debugEnabledFormat, "QQ Http");
					notifyTask.Add(services.GetRequiredService<QQHttp>().SendMessage(config, game));
				} else _logger.LogInformation(debugDisabledFormat, "QQ Http");

				// QQ WebSocket notifications
				if (config.EnableQQWebSocket) {
					_logger.LogInformation(debugEnabledFormat, "QQ WebSocket");
					notifyTask.Add(services.GetRequiredService<QQWebSocket>().SendMessage(config, game));
				} else _logger.LogInformation(debugDisabledFormat, "QQ WebSocket");

				// PushPlus notifications
				if (config.EnablePushPlus) {
					_logger.LogInformation(debugEnabledFormat, "PushPlus");
					await services.GetRequiredService<PushPlus>().SendMessage(config, game);
				} else _logger.LogInformation(debugDisabledFormat, "PushPlus");

				// DingTalk notifications
				if (config.EnableDingTalk) {
					_logger.LogInformation(debugEnabledFormat, "DingTalk");
					notifyTask.Add(services.GetRequiredService<DingTalk>().SendMessage(config, game));
				} else _logger.LogInformation(debugDisabledFormat, "DingTalk");

				// PushDeer notifications
				if (config.EnablePushDeer) {
					_logger.LogInformation(debugEnabledFormat, "PushDeer");
					notifyTask.Add(services.GetRequiredService<PushDeer>().SendMessage(config, game));
				} else _logger.LogInformation(debugDisabledFormat, "PushDeer");

				// Discord notifications
				if (config.EnableDiscord) {
					_logger.LogInformation(debugEnabledFormat, "Discord");
					notifyTask.Add(services.GetRequiredService<Discord>().SendMessage(config, game));
				} else _logger.LogInformation(debugDisabledFormat, "Discord");

				//Email notifications
				if (config.EnableEmail) {
					_logger.LogInformation(debugEnabledFormat, "Email");
					notifyTask.Add(services.GetRequiredService<Email>().SendMessage(config, game));
				} else _logger.LogInformation(debugDisabledFormat, "Email");

				// Meow notifications
				if (config.EnableMeow) {
					_logger.LogInformation(debugEnabledFormat, "Meow");
					notifyTask.Add(services.GetRequiredService<Meow>().SendMessage(config, game));
				} else _logger.LogInformation(debugDisabledFormat, "Meow");

				await Task.WhenAll(notifyTask);

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