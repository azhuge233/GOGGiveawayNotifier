using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using Microsoft.Extensions.Logging;

namespace GOGGiveawayNotifier {
	class TgBot : IDisposable {
		private readonly ILogger<TgBot> _logger;
		private readonly string debugSendMessage = "Sending Message";
		private readonly string giveawayNotification = "<b>GOG Giveaway</b>\n\n<i>{0}</i>\n领取链接: https://www.gog.com/#giveaway";
		private TelegramBotClient BotClient { get; set; }

		public TgBot(ILogger<TgBot> logger) {
			_logger = logger;
		}

		public async Task SendMessage(string token, string chatID, string gameName, bool htmlMode = false) {
			BotClient = new TelegramBotClient(token: token);
			try {
				_logger.LogDebug(debugSendMessage);
				await BotClient.SendTextMessageAsync(
						chatId: chatID,
						text: string.Format(giveawayNotification, gameName),
						parseMode: htmlMode ? ParseMode.Html : ParseMode.Default
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
