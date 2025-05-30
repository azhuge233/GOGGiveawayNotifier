﻿using GOGGiveawayNotifier.Model.PostContent;
using GOGGiveawayNotifier.Model;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace GOGGiveawayNotifier.Notifier {
	public class Discord : INotifiable {
		private readonly ILogger<Discord> _logger;

		#region debug strings
		private readonly string debugSendMessage = "Send notification to Discord";
		#endregion

		public Discord(ILogger<Discord> logger) {
			_logger = logger;
		}

		public async Task SendMessage(NotifyConfig config, List<GiveawayRecord> games) {
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
