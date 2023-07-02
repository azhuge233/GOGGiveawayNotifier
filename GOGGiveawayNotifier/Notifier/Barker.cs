using System;
using System.Web;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Microsoft.Extensions.Logging;
using GOGGiveawayNotifier.Model;
using System.Collections.Generic;

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
				var sb = new StringBuilder();
				string url = sb.AppendFormat(NotifyFormatStrings.barkUrlWithTitleFormat, config.BarkAddress, config.BarkToken).ToString();
				var webGet = new HtmlWeb();

				foreach (var game in games) {
					sb.Clear();
					_logger.LogDebug($"{debugSendMessage} : {game.Name}");
					await webGet.LoadFromWebAsync(
						sb.Append(url)
						.Append(HttpUtility.UrlEncode(game.Name))
						.Append(HttpUtility.UrlEncode(NotifyFormatStrings.projectLink))
						.AppendFormat(NotifyFormatStrings.barkUrlArgs, HttpUtility.UrlEncode(game.Url))
						.ToString()
					);
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