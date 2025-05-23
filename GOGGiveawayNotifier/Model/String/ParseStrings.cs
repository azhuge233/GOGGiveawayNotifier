namespace GOGGiveawayNotifier.Model {
	public static class ParseStrings {
		public static readonly string GiveawayUrl = "https://www.gog.com/#giveaway";

		public static readonly string removeSpecialCharsRegex = @"[^0-9a-zA-Z]+";

		public static readonly string giveawaySectionType = "GIVEAWAY_SECTION";

		public static readonly string typeGiveaway = "GIVEAWAY";
		public static readonly string typeFreeGame = "FREE_GAME";
	}
}
