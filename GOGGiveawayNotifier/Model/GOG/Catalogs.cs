using Newtonsoft.Json;
using System.Collections.Generic;

namespace GOGGiveawayNotifier.Model.GOG {
	public class Catalogs {
		[JsonProperty("productCount")]
		public int ProductCount { get; set; }
		[JsonProperty("products")]
		public List<Product> Products { get; set; } = [];
	}
}
