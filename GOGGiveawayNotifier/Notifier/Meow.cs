using GOGGiveawayNotifier.Model;
using GOGGiveawayNotifier.Models.PostContent;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace GOGGiveawayNotifier.Notifier {
	internal class Meow: INotifiable {
		private readonly ILogger<Meow> _logger;

		#region debug strings
		private readonly string debugSendMessage = "Send notification to Meow";
		#endregion

		public Meow(ILogger<Meow> logger) {
			_logger = logger;
		}

		public async Task SendMessage(NotifyConfig config, List<GiveawayRecord> games) {
			try {
				_logger.LogDebug(debugSendMessage);

				var url = string.Format(NotifyFormatStrings.meowUrlFormat, config.MeowAddress, config.MeowNickname);

				var content = new MeowPostContent() {
					Title = NotifyFormatStrings.meowTitle
				};

				var client = new HttpClient();

				foreach (var game in games) {
					string message = string.Empty;

					if (game.Type == ParseStrings.typeGiveaway)
						message = string.Format(NotifyFormatStrings.meowMessageFormat[0], game.Title, game.EndDate, game.Url);
					else message = string.Format(NotifyFormatStrings.meowMessageFormat[1], game.Title, game.Url);

					content.Message = $"{message}{NotifyFormatStrings.projectLink}";
					content.Url = game.Url;

					var data = new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json");
					var resp = await new HttpClient().PostAsync(url, data);

					_logger.LogDebug(await resp.Content.ReadAsStringAsync());
					await Task.Delay(3000); // rate limit
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
