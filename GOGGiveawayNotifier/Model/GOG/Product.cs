using System.Text.Json.Serialization;

namespace GOGGiveawayNotifier.Model.GOG {
	public class Product {
		[JsonPropertyName("id")]
		public string ID { get; set; }
		[JsonPropertyName("title")]
		public string Title { get; set; }
		[JsonPropertyName("slug")]
		public string Slug { get; set; }
		[JsonPropertyName("productType")]
		public string ProductType { get; set; }
		[JsonPropertyName("productState")]
		public string ProductState { get; set; }
		[JsonPropertyName("storeLink")]
		public string StoreLink { get; set; }
	}
}
