namespace GOGGiveawayNotifier.Model {
	public static class ParseStrings {
		public static readonly string GiveawayUrl = "https://www.gog.com/#giveaway";

		public static readonly string removeSpecialCharsRegex = @"[^0-9a-zA-Z]+";

		#region Xpath
		public static readonly string giveawayDivXPath = ".//div[contains(@id, \'giveaway\')]";
		public static readonly string giveawayALinkXPath = ".//a[contains(@class, \'giveaway__overlay-link\')]";
		//public static readonly string titleLableXpath = ".//a[@giveaway-banner]";
		public static readonly string productTileXPath = ".//div[contains(@class, \'grid\')]//product-tile";
		public static readonly string productLinkXPath = ".//a[contains(@class, \'product-tile\')]";
		public static readonly string productTitleSpanXPath = ".//a[contains(@class, 'product-tile')]//div[contains(@class, \'product-tile__info\')]//div[contains(@class, \'product-tile__title\')]//product-title//span";
		#endregion
	}
}
