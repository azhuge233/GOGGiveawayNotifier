using GOGGiveawayNotifier.Model.WebSocketContent;
using GOGGiveawayNotifier.Model;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Threading.Tasks;
using System;
using Websocket.Client;
using System.Linq;

namespace GOGGiveawayNotifier.Notifier {
	internal class QQWebSocket : INotifiable {
		private readonly ILogger<QQWebSocket> _logger;

		#region debug strings
		private readonly string debugSendMessage = "Send notifications to QQ WebSocket";
		private readonly string debugWSReconnection = "Reconnection happened, type: {0}";
		private readonly string debugWSMessageRecieved = "Message received: {0}";
		private readonly string debugWSDisconnected = "Disconnected: {0}";
		#endregion

		public QQWebSocket(ILogger<QQWebSocket> logger) {
			_logger = logger;
		}

		private WebsocketClient GetWSClient(NotifyConfig config) {
			var url = new Uri(string.Format(NotifyFormatStrings.qqRedUrlFormat, config.QQWebSocketAddress, config.QQWebSocketPort, config.QQWebSocketToken));

			#region new websocket client
			var client = new WebsocketClient(url);
			client.ReconnectionHappened.Subscribe(info => _logger.LogDebug(debugWSReconnection, info.Type));
			client.MessageReceived.Subscribe(msg => _logger.LogDebug(debugWSMessageRecieved, msg));
			client.DisconnectionHappened.Subscribe(msg => _logger.LogDebug(debugWSDisconnected, msg));
			#endregion

			return client;
		}

		private static List<WSPacket> GetSendPacket(NotifyConfig config, List<GiveawayRecord> games) {
			return games.Select(game => new WSPacket {
				Action = NotifyFormatStrings.qqWebSocketSendAction,
				Params = new Param { 
					UserID = config.ToQQID,
					Message = $"{string.Format(NotifyFormatStrings.qqMessageFormat, game.Name, game.Url)}{NotifyFormatStrings.projectLink}"
				}
			}).ToList();
		}

		public async Task SendMessage(NotifyConfig config, List<GiveawayRecord> games) {
			try {
				_logger.LogDebug(debugSendMessage);

				var packets = GetSendPacket(config, games);

				using var client = GetWSClient(config);

				await client.Start();

				foreach (var packet in packets) {
					await client.SendInstant(JsonConvert.SerializeObject(packet));
					await Task.Delay(600);
				}

				await client.Stop(WebSocketCloseStatus.NormalClosure, string.Empty);

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
