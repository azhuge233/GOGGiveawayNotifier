using Newtonsoft.Json;

namespace GOGGiveawayNotifier.Model.GOG {
	public class Product {
		[JsonProperty("id")]
		public string ID { get; set; }
		[JsonProperty("title")]
		public string Title { get; set; }
		[JsonProperty("slug")]
		public string Slug { get; set; }
		[JsonProperty("productType")]
		public string ProductType { get; set; }
		[JsonProperty("productState")]
		public string ProductState { get; set; }
		[JsonProperty("storeLink")]
		public string StoreLink { get; set; }
	}
}
