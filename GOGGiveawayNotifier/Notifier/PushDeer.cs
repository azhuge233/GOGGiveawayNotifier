using System;
using System.Web;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using HtmlAgilityPack;
using GOGGiveawayNotifier.Model;

namespace GOGGiveawayNotifier.Notifier {
	internal class PushDeer: INotifiable {
		private readonly ILogger<PushDeer> _logger;

		#region debug strings
		private readonly string debugSendMessage = "Send notification to PushDeer";
		#endregion

		public PushDeer(ILogger<PushDeer> logger) {
			_logger = logger;
		}

		public async Task SendMessage(NotifyConfig config, GiveawayRecord record) {
			try {
				var webGet = new HtmlWeb();
				_logger.LogDebug($"{debugSendMessage} : {record.Name}");
				await webGet.LoadFromWebAsync(
					 new StringBuilder()
					 .AppendFormat(NotifyFormatStrings.pushDeerUrlFormat, 
								config.PushDeerToken, 
								HttpUtility.UrlEncode(new StringBuilder().AppendFormat(NotifyFormatStrings.pushDeerFormat, record.Name).ToString()))
					 .Append(HttpUtility.UrlEncode(NotifyFormatStrings.projectLink))
					 .ToString()
				 );

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
