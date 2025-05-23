using System;

namespace GOGGiveawayNotifier.Model {
	public class GiveawayRecord {
		public string Type { get; set; }
		public string ID { get; set; }
		public string Url { get; set; }
		public string Title { get; set; }
		public string Slug { get; set; }
		public DateTime? EndDate { get; set; }
		public string ProductType { get; set; }
		public string ProductState { get; set; }
	}
}
