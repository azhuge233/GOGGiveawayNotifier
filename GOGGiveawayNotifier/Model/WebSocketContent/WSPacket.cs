using Newtonsoft.Json;
using System.Collections.Generic;

namespace GOGGiveawayNotifier.Model.WebSocketContent {
	public class WSPacket {
		[JsonProperty("type")]
		public string Type { get; set; }
		[JsonProperty("payload")]
		public object Payload { get; set; }
	}

	#region Connect Classes
	public class ConnectPayload {
		[JsonProperty("token")]
		public string Token { get; set; }
	}
	#endregion

	#region Message Classes
	public class MessagePayload {
		[JsonProperty("peer")]
		public Peer Peer { get; set; }
		[JsonProperty("elements")]
		public List<object> Elements { get; set; }
	}

	public class Peer {
		[JsonProperty("chatType")]
		public int ChatType { get; set; }
		[JsonProperty("peerUin")]
		public string PeerUin { get; set; }
	}

	public class TextElementRoot {
		[JsonProperty("elementType")]
		public int ElementType { get; set; } = 1;
		[JsonProperty("textElement")]
		public TextElement TextElement { get; set; }
	}

	public class TextElement {
		[JsonProperty("content")]
		public string Content { get; set; }
	}

	//public class ReplyElementRoot {
	//	[JsonProperty("elementType")]
	//	public int ElementType { get; set; } = 7;
	//	[JsonProperty("replyElement")]
	//	public ReplyElement ReplyElement { get; set; }
	//}

	//public class ReplyElement {
	//	[JsonProperty("replayMsgSeq")]
	//	public string ReplayMsgSeq { get; set; }
	//	[JsonProperty("sourceMsgIdInRecords")]
	//	public string SourceMsgIdInRecords { get; set; }
	//	[JsonProperty("senderUid")]
	//	public string SenderUid { get; set; }
	//}
	#endregion
}
