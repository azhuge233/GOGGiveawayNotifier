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
					string message = string.Empty;

					if (game.Type == ParseStrings.typeGiveaway)
						message = string.Format(NotifyFormatStrings.telegramFormat[0], game.Title, game.EndDate, game.Url, RemoveSpecialCharacters(game.Title));
					else message = string.Format(NotifyFormatStrings.telegramFormat[1], game.Title, game.Url, RemoveSpecialCharacters(game.Title));

					await BotClient.SendMessage(
						chatId: config.TelegramChatID,
						text: $"{message}{NotifyFormatStrings.projectLinkHTML.Replace("<br>", "\n")}",
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
