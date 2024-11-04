using System;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using Microsoft.Extensions.Logging;
using GOGGiveawayNotifier.Model;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace GOGGiveawayNotifier.Notifier {
	class TgBot : INotifiable {
		private readonly ILogger<TgBot> _logger;

		#region debug strings
		private readonly string debugSendMessage = "Sending Message";
		#endregion

		public TgBot(ILogger<TgBot> logger) {
			_logger = logger;
		}

		public async Task SendMessage(NotifyConfig config, List<GiveawayRecord> games) {
			var BotClient = new TelegramBotClient(token: config.TelegramToken);
			try {
				_logger.LogDebug(debugSendMessage);

				foreach (var game in games) {
					await BotClient.SendMessage(
						chatId: config.TelegramChatID,
						text: $"{string.Format(NotifyFormatStrings.telegramFormat, game.Name, game.Url, RemoveSpecialCharacters(game.Name))}{NotifyFormatStrings.projectLinkHTML.Replace("<br>", "\n")}",
						parseMode: ParseMode.Html
					);
				}

				_logger.LogDebug($"Done: {debugSendMessage}");
			} catch (Exception) {
				_logger.LogError($"Error: {debugSendMessage}");
				throw;
			} finally {
				Dispose();
			}
		}

		private static string RemoveSpecialCharacters(string str) {
			return Regex.Replace(str, ParseStrings.removeSpecialCharsRegex, string.Empty);
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
		}
	}
}
