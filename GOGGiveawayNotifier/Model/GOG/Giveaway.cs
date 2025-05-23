using Newtonsoft.Json;
using System;

namespace GOGGiveawayNotifier.Model.GOG {
	public class Giveaway {
		[JsonProperty("properties")]
		public Properties Properties { get; set; }
	}

	public class Properties {
		[JsonProperty("endDate")]
		public DateTime EndDate { get; set; }
		[JsonProperty("product")]
		public Product Product { get; set; }
	}
}
