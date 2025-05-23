using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using GOGGiveawayNotifier.Model;
using System.Collections.Generic;
using GOGGiveawayNotifier.Model.PostContent;
using System.Net.Http;
using Newtonsoft.Json;

namespace GOGGiveawayNotifier.Notifier {
	class QQHttp: INotifiable {
		private readonly ILogger<QQHttp> _logger;

		#region debug strings
		private readonly string debugSendMessage = "Send notifications to QQ Http";
		#endregion

		public QQHttp(ILogger<QQHttp> logger) {
			_logger = logger;
		}

		public async Task SendMessage(NotifyConfig config, List<GiveawayRecord> games) {
			try {
				_logger.LogDebug(debugSendMessage);

				string url = string.Format(NotifyFormatStrings.qqUrlFormat, config.QQHttpAddress, config.QQHttpPort, config.QQHttpToken);

				var client = new HttpClient();

				var content = new QQHttpPostContent {
					UserID = config.ToQQID
				};

				var data = new StringContent(string.Empty);
				var resp = new HttpResponseMessage();

				foreach (var game in games) {
					_logger.LogDebug($"{debugSendMessage} : {game.Title}");

					if(game.Type == ParseStrings.typeGiveaway)
						content.Message = $"{string.Format(NotifyFormatStrings.qqMessageFormat[0], game.Title, game.EndDate, game.Url)}{NotifyFormatStrings.projectLink}";
					else content.Message = $"{string.Format(NotifyFormatStrings.qqMessageFormat[1], game.Title, game.Url)}{NotifyFormatStrings.projectLink}";

					data = new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json");
					resp = await client.PostAsync(url, data);

					_logger.LogInformation(await resp.Content.ReadAsStringAsync());
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
