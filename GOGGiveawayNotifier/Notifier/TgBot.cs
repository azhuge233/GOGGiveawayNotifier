using GOGGiveawayNotifier.Model;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace GOGGiveawayNotifier.Notifier {
	class TgBot(ILogger<TgBot> logger, IOptions<Config> config) : INotifiable {
		private readonly ILogger<TgBot> _logger = logger;
		private readonly Config config = config.Value;

		#region debug strings
		private readonly string debugSendMessage = "Sending Message";
		#endregion

		public async Task SendMessage(List<GiveawayRecord> games) {
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
