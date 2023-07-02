namespace GOGGiveawayNotifier.Model {
	public static class NotifyFormatStrings {
		public static readonly string telegramFormat = "<b>GOG Free Games</b>\n\n" +
			"<i>{0}</i>\n" +
			"领取链接: <a href=\"{1}\">{1}</a>\n\n" +
			"#GOG #{2}";

		public static readonly string barkUrlWithTitleFormat = "{0}/{1}/GOGGiveawayNotifier/";
		public static readonly string barkUrlArgs = "?group=goggiveawaynotifier" +
			"&isArchive=1" +
			"&sound=calypso" +
			"&url={0}" +
			"&copy={0}";

		public static readonly string emailTitleFormat = "New GOG Free Games - GOGGiveawayNotifier";
		public static readonly string emailBodyFormat = "<br>{0}<br>" + "领取链接: <a href=\"{1}\">{1}</a>";

		public static readonly string qqUrlFormat = "http://{0}:{1}/send_private_msg?user_id={2}&message=";
		public static readonly string qqMessageFormat = "GOG Free Game\n\n" +
			"{0}\n" +
			"领取链接: {1}";

		public static readonly string pushPlusTitleFormat = "New GOG Free Game - GOGGiveawayNotifier";
		public static readonly string pushPlusBodyFormat = "<br>{0}<br>" + "领取链接: <a href=\"{1}\">{1}</a><br><br>";
		public static readonly string pushPlusUrlFormat = "http://www.pushplus.plus/send?token={0}&template=html&title={1}&content=";

		public static readonly string dingTalkUrlFormat = "https://oapi.dingtalk.com/robot/send?access_token={0}";
		public static readonly string dingTalkMessageFormat = "GOG Free Game\n\n" +
			"{0}\n" +
			"领取链接: {1}";

		public static readonly string pushDeerUrlFormat = "https://api2.pushdeer.com/message/push?pushkey={0}&&text={1}";
		public static readonly string pushDeerFormat = "GOG Free Game\n\n" +
			"{0}\n\n" +
			"领取链接: {1}";

		public static readonly string discordFormat = "领取链接: {0}";

		public static readonly string projectLink = "\n\nFrom https://github.com/azhuge233/GOGGiveawayNotifier";
		public static readonly string projectLinkHTML = "<br><br>From <a href=\"https://github.com/azhuge233/GOGGiveawayNotifier\">GOGGiveawayNotifier</a>";
	}
}