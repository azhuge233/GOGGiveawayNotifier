using GOGGiveawayNotifier.Model;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace GOGGiveawayNotifier.Notifier {
	internal class PushDeer(ILogger<PushDeer> logger, IOptions<Config> config) : INotifiable {
		private readonly ILogger<PushDeer> _logger = logger;
		private readonly Config config = config.Value;

		#region debug strings
		private readonly string debugSendMessage = "Send notification to PushDeer";
		#endregion

		public async Task SendMessage(List<GiveawayRecord> games) {
			try {
				var client = new HttpClient();

				foreach (var game in games) {
					_logger.LogDebug($"{debugSendMessage}: {game.Title}");

					string text = string.Empty;

					if (game.Type == ParseStrings.typeGiveaway)
						text = $"{string.Format(NotifyFormatStrings.pushDeerFormat[0], game.Title, game.EndDate, game.Url)}{NotifyFormatStrings.projectLink}";
					else text = $"{string.Format(NotifyFormatStrings.pushDeerFormat[1], game.Title, game.Url)}{NotifyFormatStrings.projectLink}";

					var resp = await client.GetAsync(string.Format(NotifyFormatStrings.pushDeerUrlFormat, config.PushDeerToken, HttpUtility.UrlEncode(text)));

					_logger.LogDebug(await resp.Content.ReadAsStringAsync());
				}

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
