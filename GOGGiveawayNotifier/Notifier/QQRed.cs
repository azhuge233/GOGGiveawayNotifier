using GOGGiveawayNotifier.Model.WebSocketContent;
using GOGGiveawayNotifier.Model;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using System;
using Websocket.Client;
using Telegram.Bot.Types;
using System.Linq;

namespace GOGGiveawayNotifier.Notifier {
	internal class QQRed : INotifiable {
		private readonly ILogger<QQRed> _logger;

		#region debug strings
		private readonly string debugSendMessage = "Send notifications to QQ Red (Chronocat)";
		private readonly string debugWSReconnection = "Reconnection happened, type: {0}";
		private readonly string debugWSMessageRecieved = "Message received: {0}";
		private readonly string debugWSDisconnected = "Disconnected: {0}";
		#endregion

		public QQRed(ILogger<QQRed> logger) {
			_logger = logger;
		}

		private WebsocketClient GetWSClient(NotifyConfig config) {
			var url = new Uri(new StringBuilder().AppendFormat(NotifyFormatStrings.qqRedUrlFormat, config.RedAddress, config.RedPort).ToString());

			#region new websocket client
			var client = new WebsocketClient(url);
			client.ReconnectionHappened.Subscribe(info => _logger.LogDebug(debugWSReconnection, info.Type));
			client.MessageReceived.Subscribe(msg => _logger.LogDebug(debugWSMessageRecieved, msg));
			client.DisconnectionHappened.Subscribe(msg => _logger.LogDebug(debugWSDisconnected, msg));
			#endregion

			return client;
		}

		private static WSPacket GetConnectPacket(NotifyConfig config) {
			return new WSPacket() {
				Type = NotifyFormatStrings.qqRedWSConnectPacketType,
				Payload = new ConnectPayload() {
					Token = config.RedToken
				}
			};
		}

		private static List<WSPacket> GetSendPacket(NotifyConfig config, List<GiveawayRecord> games) {
			return games.Select(game => new WSPacket() {
				Type = NotifyFormatStrings.qqRedWSSendPacketType,
				Payload = new MessagePayload() {
					Peer = new Peer() {
						ChatType = 1,
						PeerUin = config.ToQQID
					},
					Elements = new List<object>() {
						new TextElementRoot() {
							TextElement = new TextElement() {
								Content = new StringBuilder().AppendFormat(NotifyFormatStrings.qqMessageFormat, game.Name, game.Url).ToString()
							}
						}
					}
				}
			}).ToList();
		}

		public async Task SendMessage(NotifyConfig config, List<GiveawayRecord> games) {
			try {
				_logger.LogDebug(debugSendMessage);

				var packets = GetSendPacket(config, games);

				using var client = GetWSClient(config);

				await client.Start();

				await client.SendInstant(JsonConvert.SerializeObject(GetConnectPacket(config)));

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
