using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace GOGGiveawayNotifier.Model.GOG {
	public class Catalogs {
		[JsonPropertyName("productCount")]
		public int ProductCount { get; set; }
		[JsonPropertyName("products")]
		public List<Product> Products { get; set; } = [];
	}
}
