using GOGGiveawayNotifier.Model;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace GOGGiveawayNotifier.Notifier {
	class PushPlus(ILogger<PushPlus> logger, IOptions<Config> config) : INotifiable {
		private readonly ILogger<PushPlus> _logger = logger;
		private readonly Config config = config.Value;

		#region debug strings
		private readonly string debugSendMessage = "Send notification to PushPlus";
		#endregion

		public async Task SendMessage(List<GiveawayRecord> games) {
			try {
				_logger.LogDebug(debugSendMessage);

				var url = string.Format(NotifyFormatStrings.pushPlusUrlFormat, config.PushPlusToken, HttpUtility.UrlEncode(NotifyFormatStrings.pushPlusTitleFormat));

				var sb = new StringBuilder();
				foreach (var game in games) {
					if(game.Type == ParseStrings.typeGiveaway)
						sb.AppendFormat(NotifyFormatStrings.pushPlusBodyFormat[0], game.Title, game.EndDate, game.Url);
					else sb.AppendFormat(NotifyFormatStrings.pushPlusBodyFormat[1], game.Title, game.Url);
				}

				var message = HttpUtility.UrlEncode(sb.ToString());

				sb.Clear();
				var client = new HttpClient();
				var resp = await client.GetAsync(
					sb.Append(url)
						.Append(message)
						.Append(NotifyFormatStrings.projectLinkHTML)
						.ToString()
				);
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
