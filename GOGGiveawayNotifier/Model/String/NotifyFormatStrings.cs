namespace GOGGiveawayNotifier.Model {
	public static class NotifyFormatStrings {
		private static readonly string gogGiveawayUrl = "https://www.gog.com/#giveaway";

		public static readonly string telegramFormat = "<b>GOG Giveaway</b>\n\n<i>{0}</i>\n" + $"领取链接: {gogGiveawayUrl}";
		public static readonly string barkFormat = "GOG Giveaway\n{0}\n" + $"领取链接: {gogGiveawayUrl}";
		public static readonly string emailFormat = "<b>{0}</b><br>" + $"领取链接: {gogGiveawayUrl}";

		public static readonly string barkUrlWithTitleFormat = "{0}/{1}/GOGGiveawayNotifier/";
		public static readonly string barkUrlArgs = "?group=goggiveawaynotifier" +
			"&isArchive=1" +
			"&sound=calypso" +
			$"&url={gogGiveawayUrl}" +
			$"&copy={gogGiveawayUrl}";

		public static readonly string emailTitleFormat = "New GOG Giveaway - GOGGiveawayNotifier";
		public static readonly string emailBodyFormat = "<br>{0}<br>" + $"领取链接: {gogGiveawayUrl}";
	}
}