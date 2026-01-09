using System;
using System.Text.Json.Serialization;

namespace GOGGiveawayNotifier.Model.GOG {
	public class Giveaway {
		[JsonPropertyName("properties")]
		public Properties Properties { get; set; }
	}

	public class Properties {
		[JsonPropertyName("endDate")]
		public DateTime EndDate { get; set; }
		[JsonPropertyName("product")]
		public Product Product { get; set; }
	}
}
