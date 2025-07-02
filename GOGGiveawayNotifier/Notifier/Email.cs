using GOGGiveawayNotifier.Model;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GOGGiveawayNotifier.Notifier {
	class Email(ILogger<Email> logger, IOptions<Config> config) : INotifiable {
		private readonly ILogger<Email> _logger = logger;
		private readonly Config config = config.Value;

		#region debug strings
		private readonly string debugSendMessage = "Send notification to Email";
		private readonly string debugCreateMessage = "Create notification message";
		#endregion

		public async Task SendMessage(List<GiveawayRecord> games) {
			try {
				_logger.LogDebug(debugSendMessage);

				var message = CreateMessage(games, config.FromEmailAddress, config.ToEmailAddress);

				using var client = new SmtpClient();
				client.Connect(config.SMTPServer, config.SMTPPort, true);
				client.Authenticate(config.AuthAccount, config.AuthPassword);
				await client.SendAsync(message);
				client.Disconnect(true);

				_logger.LogDebug($"Done: {debugSendMessage}");
			} catch (Exception) {
				_logger.LogError($"Error: {debugSendMessage}");
				throw;
			} finally {
				Dispose();
			}
		}

		private MimeMessage CreateMessage(List<GiveawayRecord> games, string fromAddress, string toAddress) {
			try {
				_logger.LogDebug(debugCreateMessage);

				var message = new MimeMessage();

				message.From.Add(new MailboxAddress("GOG-FreeGames", fromAddress));
				message.To.Add(new MailboxAddress("Receiver", toAddress));

				message.Subject = NotifyFormatStrings.emailTitleFormat;

				var sb = new StringBuilder();

				foreach (var game in games) {
					if (game.Type == ParseStrings.typeGiveaway)
						sb.AppendFormat(NotifyFormatStrings.emailBodyFormat[0], game.Title, game.EndDate, game.Url);
					else sb.AppendFormat(NotifyFormatStrings.emailBodyFormat[1], game.Title, game.Url);
				}

				sb.Append(NotifyFormatStrings.projectLinkHTML);

				message.Body = new TextPart("html") {
					Text = sb.ToString()
				};

				_logger.LogDebug($"Done: {debugCreateMessage}");
				return message;
			} catch (Exception) {
				_logger.LogError($"Error: {debugCreateMessage}");
				throw;
			}
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
		}
	}
}