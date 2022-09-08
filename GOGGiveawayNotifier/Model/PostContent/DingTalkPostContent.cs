using Newtonsoft.Json;

namespace GOGGiveawayNotifier.Model.PostContent
{
	public class Content {
		[JsonProperty("content")]
		public string Content_ { get; set; }
	}
	public class DingTalkPostContent {
		[JsonProperty("msgtype")]
		public string MessageType { get; set; } = "text";
		[JsonProperty("text")]
		public Content Text { get; set; } = new Content();
	}
}
