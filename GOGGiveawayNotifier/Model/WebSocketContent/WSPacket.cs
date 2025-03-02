using Newtonsoft.Json;

namespace GOGGiveawayNotifier.Model.WebSocketContent {
	public class WSPacket {
		[JsonProperty("action")]
		public string Action { get; set; }

		[JsonProperty("params")]
		public Param Params { get; set; }
	}

	public class Param {
		[JsonProperty("user_id")]
		public string UserID { get; set; }

		[JsonProperty("message")]
		public string Message { get; set; }
	}
}
