using System;
using System.Web;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Microsoft.Extensions.Logging;
using GOGGiveawayNotifier.Model;

namespace GOGGiveawayNotifier.Notifier {
	class Barker : INotifiable {
		private readonly ILogger<Barker> _logger;

		#region debug strings
		private readonly string debugSendMessage = "Send notification to Bark";
		#endregion

		public Barker(ILogger<Barker> logger) {
			_logger = logger;
		}

		public async Task SendMessage(NotifyConfig config, string gameName) {
			try {
				var sb = new StringBuilder();
				string url = sb.AppendFormat(NotifyFormatStrings.barkUrlWithTitleFormat, config.BarkAddress, config.BarkToken).ToString();
				var webGet = new HtmlWeb();

				sb.Clear();
				_logger.LogDebug($"{debugSendMessage} : {gameName}");
				await webGet.LoadFromWebAsync(
					sb.Append(url)
					.Append(HttpUtility.UrlEncode(gameName))
					.Append(NotifyFormatStrings.barkUrlArgs)
					.ToString()
				);

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