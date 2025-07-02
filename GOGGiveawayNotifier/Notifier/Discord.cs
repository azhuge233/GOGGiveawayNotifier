using GOGGiveawayNotifier.Model;
using GOGGiveawayNotifier.Model.PostContent;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace GOGGiveawayNotifier.Notifier {
	public class Discord(ILogger<Discord> logger, IOptions<Config> config) : INotifiable {
		private readonly ILogger<Discord> _logger = logger;
		private readonly Config config = config.Value;

		#region debug strings
		private readonly string debugSendMessage = "Send notification to Discord";
		#endregion

		public async Task SendMessage(List<GiveawayRecord> games) {
			try {
				_logger.LogDebug(debugSendMessage);

				var url = config.DiscordWebhookURL;
				var content = new DiscordPostContent() {
					Content = "New Free Game - GOG"
				};

				foreach (var game in games) {
					string description = string.Empty;

					if(game.Type == ParseStrings.typeGiveaway)
						description = string.Format(NotifyFormatStrings.discordFormat[0], game.EndDate, game.Url, game.Title);
					else description = string.Format(NotifyFormatStrings.discordFormat[1], game.Url, game.Title);

					content.Embeds.Add(
						new Embed() {
							Title = game.Title,
							Url = game.Url,
							Description = description,
							Footer = new Footer() { Text = NotifyFormatStrings.projectLink }
						}
					);
				}

				var data = new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json");
				var resp = await new HttpClient().PostAsync(url, data);
				_logger.LogDebug(await resp.Content.ReadAsStringAsync());

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
