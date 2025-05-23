using System;
using System.Text;
using System.Web;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using GOGGiveawayNotifier.Model;
using System.Collections.Generic;
using System.Net.Http;

namespace GOGGiveawayNotifier.Notifier {
	class PushPlus: INotifiable {
		private readonly ILogger<PushPlus> _logger;

		#region debug strings
		private readonly string debugSendMessage = "Send notification to PushPlus";
		#endregion

		public PushPlus(ILogger<PushPlus> logger) {
			_logger = logger;
		}

		public async Task SendMessage(NotifyConfig config, List<GiveawayRecord> games) {
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
