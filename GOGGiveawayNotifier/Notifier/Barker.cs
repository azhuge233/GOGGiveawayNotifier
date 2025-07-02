using GOGGiveawayNotifier.Model;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace GOGGiveawayNotifier.Notifier {
	class Barker(ILogger<Barker> logger, IOptions<Config> config) : INotifiable {
		private readonly ILogger<Barker> _logger = logger;
		private readonly Config config = config.Value;

		#region debug strings
		private readonly string debugSendMessage = "Send notification to Bark";
		#endregion

		public async Task SendMessage(List<GiveawayRecord> games) {
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