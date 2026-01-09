using GOGGiveawayNotifier.Model;
using GOGGiveawayNotifier.Model.PostContent;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace GOGGiveawayNotifier.Notifier {
	class DingTalk(ILogger<DingTalk> logger, IOptions<Config> config) : INotifiable {
		private readonly ILogger<DingTalk> _logger = logger;
		private readonly Config config = config.Value;

		#region debug strings
		private readonly string debugSendMessage = "Send notifications to DingTalk";
		#endregion

		public async Task SendMessage(List<GiveawayRecord> games) {
			try {
				_logger.LogDebug(debugSendMessage);

				var url = string.Format(NotifyFormatStrings.dingTalkUrlFormat, config.DingTalkBotToken);
				
				foreach(var game in games) {
					var content = new DingTalkPostContent();

					if(game.Type == ParseStrings.typeGiveaway)
						content.Text.Content_ = $"{string.Format(NotifyFormatStrings.dingTalkMessageFormat[0], game.Title, game.EndDate, game.Url)}{NotifyFormatStrings.projectLink}";
					else content.Text.Content_ = $"{string.Format(NotifyFormatStrings.dingTalkMessageFormat[1], game.Title, game.Url)}{NotifyFormatStrings.projectLink}";

					var data = new StringContent(JsonSerializer.Serialize(content), Encoding.UTF8, "application/json");
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
