using System;
using System.Text;
using System.Web;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Microsoft.Extensions.Logging;
using GOGGiveawayNotifier.Model;

namespace GOGGiveawayNotifier.Notifier {
	class PushPlus: INotifiable {
		private readonly ILogger<PushPlus> _logger;

		#region debug strings
		private readonly string debugSendMessage = "Send notification to PushPlus";
		#endregion

		public PushPlus(ILogger<PushPlus> logger) {
			_logger = logger;
		}

		public async Task SendMessage(NotifyConfig config, GiveawayRecord record) {
			try {
				_logger.LogDebug(debugSendMessage);

				var url = new StringBuilder().AppendFormat(NotifyFormatStrings.pushPlusUrlFormat, config.PushPlusToken, HttpUtility.UrlEncode(NotifyFormatStrings.pushPlusTitleFormat));
				var message = HttpUtility.UrlEncode(new StringBuilder().AppendFormat(NotifyFormatStrings.pushPlusBodyFormat, record.Name).ToString());

				var resp = await new HtmlWeb().LoadFromWebAsync(
					new StringBuilder()
						.Append(url)
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
