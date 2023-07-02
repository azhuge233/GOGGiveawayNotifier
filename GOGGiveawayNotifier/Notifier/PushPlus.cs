using System;
using System.Text;
using System.Web;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Microsoft.Extensions.Logging;
using GOGGiveawayNotifier.Model;
using System.Collections.Generic;

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

				var url = new StringBuilder().AppendFormat(NotifyFormatStrings.pushPlusUrlFormat, config.PushPlusToken, HttpUtility.UrlEncode(NotifyFormatStrings.pushPlusTitleFormat));

				var sb = new StringBuilder();
				foreach (var game in games) {
					sb.AppendFormat(NotifyFormatStrings.pushPlusBodyFormat, game.Name, game.Url);
				}

				var message = HttpUtility.UrlEncode(sb.ToString());

				sb.Clear();
				var resp = await new HtmlWeb().LoadFromWebAsync(
					sb.Append(url)
						.Append(message)
						.Append(NotifyFormatStrings.projectLinkHTML)
						.ToString()
				);
				_logger.LogDebug(resp.Text);

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
