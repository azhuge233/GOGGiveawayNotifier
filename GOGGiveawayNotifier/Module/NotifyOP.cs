using GOGGiveawayNotifier.Model;
using GOGGiveawayNotifier.Notifier;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GOGGiveawayNotifier.Module {
	class NotifyOP(ILogger<NotifyOP> logger, IOptions<Config> config, TgBot tgBot, Barker barker, QQHttp qqHttp, QQWebSocket qqWS, PushPlus pushPlus, DingTalk dingTalk, PushDeer pushDeer, Discord discord, Email email, Meow meow) : IDisposable {
		#region DI
		private readonly ILogger<NotifyOP> _logger = logger;
		private readonly Config config = config.Value;
		#endregion

		#region debug strings
		private readonly string debugNotify = "Notify";
		private readonly string debugEnabledFormat = "Sending notifications to {0}";
		private readonly string debugDisabledFormat = "{0} notify is disabled, skipping";
		#endregion

		public async Task Notify(List<GiveawayRecord> game) {
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
					notifyTask.Add(tgBot.SendMessage(game));
				} else _logger.LogInformation(debugDisabledFormat, "Telegram");

				// Bark notifications
				if (config.EnableBark) {
					_logger.LogInformation(debugEnabledFormat, "Bark");
					notifyTask.Add(barker.SendMessage(game));
				} else _logger.LogInformation(debugDisabledFormat, "Bark");

				// QQ Http notifications
				if (config.EnableQQHttp) {
					_logger.LogInformation(debugEnabledFormat, "QQ Http");
					notifyTask.Add(qqHttp.SendMessage(game));
				} else _logger.LogInformation(debugDisabledFormat, "QQ Http");

				// QQ WebSocket notifications
				if (config.EnableQQWebSocket) {
					_logger.LogInformation(debugEnabledFormat, "QQ WebSocket");
					notifyTask.Add(qqWS.SendMessage(game));
				} else _logger.LogInformation(debugDisabledFormat, "QQ WebSocket");

				// PushPlus notifications
				if (config.EnablePushPlus) {
					_logger.LogInformation(debugEnabledFormat, "PushPlus");
					notifyTask.Add(pushPlus.SendMessage(game));
				} else _logger.LogInformation(debugDisabledFormat, "PushPlus");

				// DingTalk notifications
				if (config.EnableDingTalk) {
					_logger.LogInformation(debugEnabledFormat, "DingTalk");
					notifyTask.Add(dingTalk.SendMessage(game));
				} else _logger.LogInformation(debugDisabledFormat, "DingTalk");

				// PushDeer notifications
				if (config.EnablePushDeer) {
					_logger.LogInformation(debugEnabledFormat, "PushDeer");
					notifyTask.Add(pushDeer.SendMessage(game));
				} else _logger.LogInformation(debugDisabledFormat, "PushDeer");

				// Discord notifications
				if (config.EnableDiscord) {
					_logger.LogInformation(debugEnabledFormat, "Discord");
					notifyTask.Add(discord.SendMessage(game));
				} else _logger.LogInformation(debugDisabledFormat, "Discord");

				//Email notifications
				if (config.EnableEmail) {
					_logger.LogInformation(debugEnabledFormat, "Email");
					notifyTask.Add(email.SendMessage(game));
				} else _logger.LogInformation(debugDisabledFormat, "Email");

				// Meow notifications
				if (config.EnableMeow) {
					_logger.LogInformation(debugEnabledFormat, "Meow");
					notifyTask.Add(meow.SendMessage(game));
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