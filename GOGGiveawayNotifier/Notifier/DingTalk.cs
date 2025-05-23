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

				var url = string.Format(NotifyFormatStrings.dingTalkUrlFormat, config.DingTalkBotToken);
				
				foreach(var game in games) {
					var content = new DingTalkPostContent();

					if(game.Type == ParseStrings.typeGiveaway)
						content.Text.Content_ = $"{string.Format(NotifyFormatStrings.dingTalkMessageFormat[0], game.Title, game.EndDate, game.Url)}{NotifyFormatStrings.projectLink}";
					else content.Text.Content_ = $"{string.Format(NotifyFormatStrings.dingTalkMessageFormat[1], game.Title, game.Url)}{NotifyFormatStrings.projectLink}";

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
