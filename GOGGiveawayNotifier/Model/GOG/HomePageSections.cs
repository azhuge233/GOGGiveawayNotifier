using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace GOGGiveawayNotifier.Model.GOG {
	public class HomePageSections {
		[JsonPropertyName("sections")]
		public List<Section> Sections { get; set; } = [];
	}

	public class Section {
		[JsonPropertyName("sectionId")]
		public string SectionID { get; set; }
		[JsonPropertyName("sectionType")]
		public string SectionType { get; set; }
	}
}
