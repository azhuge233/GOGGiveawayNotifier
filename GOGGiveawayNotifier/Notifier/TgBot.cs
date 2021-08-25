using System;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using Microsoft.Extensions.Logging;
using GOGGiveawayNotifier.Model;

namespace GOGGiveawayNotifier.Notifier {
	class TgBot : INotifiable {
		private readonly ILogger<TgBot> _logger;

		#region debug strings
		private readonly string debugSendMessage = "Sending Message";
		#endregion

		public TgBot(ILogger<TgBot> logger) {
			_logger = logger;
		}

		public async Task SendMessage(NotifyConfig config, GiveawayRecord game) {
			var BotClient = new TelegramBotClient(token: config.TelegramToken);
			try {
				_logger.LogDebug(debugSendMessage);
				await BotClient.SendTextMessageAsync(
						chatId: config.TelegramChatID,
						text: $"{string.Format(NotifyFormatStrings.telegramFormat, game.Name)}{NotifyFormatStrings.projectLinkHTML.Replace("<br>", "\n")}",
						parseMode: ParseMode.Html
					);
				_logger.LogDebug($"Done: {debugSendMessage}");
			} catch (Exception) {
				_logger.LogError($"Error: {debugSendMessage}");
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
