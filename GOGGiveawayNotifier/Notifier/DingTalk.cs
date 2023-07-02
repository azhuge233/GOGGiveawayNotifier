using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;
using GOGGiveawayNotifier.Model.PostContent;
using GOGGiveawayNotifier.Model;
using System.Collections.Generic;

namespace GOGGiveawayNotifier.Notifier {
	class DingTalk: INotifiable {
		private readonly ILogger<DingTalk> _logger;

		#region debug strings
		private readonly string debugSendMessage = "Send notifications to DingTalk";
		#endregion

		public DingTalk(ILogger<DingTalk> logger) {
			_logger = logger;
		}

		public async Task SendMessage(NotifyConfig config, List<GiveawayRecord> games) {
			try {
				_logger.LogDebug(debugSendMessage);

				var url = new StringBuilder().AppendFormat(NotifyFormatStrings.dingTalkUrlFormat, config.DingTalkBotToken).ToString();
				
				foreach(var game in games) {
					var content = new DingTalkPostContent();
					content.Text.Content_ = $"{new StringBuilder().AppendFormat(NotifyFormatStrings.dingTalkMessageFormat, game.Name, game.Url)}{NotifyFormatStrings.projectLink}";

					var data = new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json");
					var resp = await new HttpClient().PostAsync(url, data);
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
