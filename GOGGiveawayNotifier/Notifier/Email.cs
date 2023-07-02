using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MailKit.Net.Smtp;
using MimeKit;
using GOGGiveawayNotifier.Model;
using System.Collections.Generic;

namespace GOGGiveawayNotifier.Notifier {
	class Email : INotifiable {
		private readonly ILogger<Email> _logger;

		#region debug strings
		private readonly string debugSendMessage = "Send notification to Email";
		private readonly string debugCreateMessage = "Create notification message";
		#endregion

		public Email(ILogger<Email> logger) {
			_logger = logger;
		}

		private MimeMessage CreateMessage(List<GiveawayRecord> games, string fromAddress, string toAddress) {
			try {
				_logger.LogDebug(debugCreateMessage);

				var message = new MimeMessage();

				message.From.Add(new MailboxAddress("GOG-FreeGames", fromAddress));
				message.To.Add(new MailboxAddress("Receiver", toAddress));

				var sb = new StringBuilder();

				message.Subject = sb.Append(NotifyFormatStrings.emailTitleFormat).ToString();
				
				sb.Clear();

				games.ForEach(game => sb.AppendFormat(NotifyFormatStrings.emailBodyFormat, game.Name, game.Url));

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

		public async Task SendMessage(NotifyConfig config, List<GiveawayRecord> games) {
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

		public void Dispose() {
			GC.SuppressFinalize(this);
		}
	}
}