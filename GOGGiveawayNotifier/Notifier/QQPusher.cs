﻿using System;
using System.Web;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Microsoft.Extensions.Logging;
using GOGGiveawayNotifier.Model;

namespace GOGGiveawayNotifier.Notifier {
	class QQPusher: INotifiable {
		private readonly ILogger<QQPusher> _logger;

		#region debug strings
		private readonly string debugSendMessage = "Send notifications to QQ";
		#endregion

		public QQPusher(ILogger<QQPusher> logger) {
			_logger = logger;
		}

		public async Task SendMessage(NotifyConfig config, GiveawayRecord game) {
			try {
				_logger.LogDebug(debugSendMessage);

				string url = new StringBuilder().AppendFormat(NotifyFormatStrings.qqUrlFormat, config.QQAddress, config.QQPort, config.ToQQID).ToString();
				var webGet = new HtmlWeb();

				_logger.LogDebug($"{debugSendMessage} : {game.Name}");
				await webGet.LoadFromWebAsync(
					new StringBuilder()
						.Append(url)
						.Append(HttpUtility.UrlEncode(new StringBuilder().AppendFormat(NotifyFormatStrings.qqMessageFormat, game.Name).ToString()))
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
