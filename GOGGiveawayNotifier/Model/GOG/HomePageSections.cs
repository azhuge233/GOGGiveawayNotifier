using Newtonsoft.Json;
using System.Collections.Generic;

namespace GOGGiveawayNotifier.Model.GOG {
	public class HomePageSections {
		[JsonProperty("sections")]
		public List<Section> Sections { get; set; } = [];
	}

	public class Section {
		[JsonProperty("sectionId")]
		public string SectionID { get; set; }
		[JsonProperty("sectionType")]
		public string SectionType { get; set; }
	}
}
