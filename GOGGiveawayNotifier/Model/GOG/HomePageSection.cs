using Newtonsoft.Json;

namespace GOGGiveawayNotifier.Model.GOG {
	public class HomePageSection {
		[JsonProperty("sectionId")]
		public string SectionID { get; set; }
		[JsonProperty("sectionType")]
		public string SectionType { get; set; }
	}
}
