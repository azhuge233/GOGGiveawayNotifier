using System;
using System.Web;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using GOGGiveawayNotifier.Model;
using System.Collections.Generic;
using System.Net.Http;

namespace GOGGiveawayNotifier.Notifier {
	class Barker : INotifiable {
		private readonly ILogger<Barker> _logger;

		#region debug strings
		private readonly string debugSendMessage = "Send notification to Bark";
		#endregion

		public Barker(ILogger<Barker> logger) {
			_logger = logger;
		}

		public async Task SendMessage(NotifyConfig config, List<GiveawayRecord> games) {
			try {
				string url = string.Format(NotifyFormatStrings.barkUrlWithTitleFormat, config.BarkAddress, config.BarkToken);
				string projectLink = HttpUtility.UrlEncode(NotifyFormatStrings.projectLink);

				var client = new HttpClient();

				foreach (var game in games) {
					_logger.LogDebug($"{debugSendMessage} : {game.Title}");

					string args = string.Format(NotifyFormatStrings.barkUrlArgs, HttpUtility.UrlEncode(game.Url));
					string message = string.Empty;

					if (game.Type == ParseStrings.typeGiveaway)
						message = $"Giveaway\n\n{game.Title}\n\nEnd Date: {game.EndDate} (CST)";
					else message = $"Free Game\n\n{game.Title}";

					var resp = await client.GetAsync($"{url}{HttpUtility.UrlEncode(message)}{projectLink}{args}");

					_logger.LogDebug(await resp.Content.ReadAsStringAsync());
				}

				_logger.LogDebug($"Done: {debugSendMessage}");
			} catch (Exception) {
				_logger.LogDebug($"Error: {debugSendMessage}");
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