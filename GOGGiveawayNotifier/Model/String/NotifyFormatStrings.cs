﻿using System.IO;

namespace GOGGiveawayNotifier.Model {
	public static class NotifyFormatStrings {
		#region notify format
		public static readonly string[] telegramFormat = [
			"<b>GOG Giveaway</b>\n\n" +
			"<i>{0}</i>\n\n" +
			"End Date: {1} (CST)\n\n" +
			"Link: <a href=\"{2}\">{2}</a>\n\n" +
			"#GOG #Giveaway #{3}",

			"<b>GOG Free Game</b>\n\n" +
			"<i>{0}</i>\n\n" +
			"Link: <a href=\"{1}\">{1}</a>\n\n" +
			"#GOG #FreeGame #{2}",
		];

		public static readonly string barkUrlWithTitleFormat = "{0}/{1}/GOGGiveawayNotifier/";
		public static readonly string barkUrlArgs = "?group=goggiveawaynotifier" +
			"&isArchive=1" +
			"&sound=calypso" +
			"&url={0}" +
			"&copy={0}";

		public static readonly string emailTitleFormat = "New GOG Free Games - GOGGiveawayNotifier";
		public static readonly string[] emailBodyFormat = [
			"<br>{0} <i>Giveaway</i><br>" + "End Date: {1} (CST)<br>" + "Link: <a href=\"{2}\">{2}</a>",
			"<br>{0}<br>" + "Link: <a href=\"{1}\">{1}</a>"
		];

		public static readonly string qqUrlFormat = "http://{0}:{1}/send_private_msg?access_token={2}";
		internal static readonly string qqRedUrlFormat = "ws://{0}:{1}/?access_token={2}";
		internal static readonly string qqWebSocketSendAction = "send_private_msg";
		public static readonly string[] qqMessageFormat = [
			"GOG Giveaway\n\n" +
			"{0}\n" +
			"End Date: {1} (CST)\n" +
			"Link: {2}",

			"GOG Free Game\n\n" +
			"{0}\n" +
			"Link: {1}"
		];

		public static readonly string pushPlusTitleFormat = "New GOG Free Game - GOGGiveawayNotifier";
		public static readonly string[] pushPlusBodyFormat = [
			"<br>{0} <i>Giveaway</i><br>" + "End Date: {1} (CST)<br>" + "Link: <a href=\"{2}\">{2}</a>",
			"<br>{0}<br>" + "Link: <a href=\"{1}\">{1}</a>"
		];
		public static readonly string pushPlusUrlFormat = "http://www.pushplus.plus/send?token={0}&template=html&title={1}&content=";

		public static readonly string dingTalkUrlFormat = "https://oapi.dingtalk.com/robot/send?access_token={0}";
		public static readonly string[] dingTalkMessageFormat = [
			"GOG Giveaway\n\n" +
			"{0}\n" +
			"End Date: {1} (CST)\n" +
			"Link: {2}",

			"GOG Free Game\n\n" +
			"{0}\n" +
			"Link: {1}"
		];

		public static readonly string pushDeerUrlFormat = "https://api2.pushdeer.com/message/push?pushkey={0}&&text={1}";
		public static readonly string[] pushDeerFormat = [
			"GOG Giveaway\n\n" +
			"{0}\n\n" +
			"End Date: {1} (CST)\n\n" +
			"Link: {2}",

			"GOG Free Game\n\n" +
			"{0}\n\n" +
			"Link: {1}"
		];

		public static readonly string[] discordFormat = [
			"End Date: {0} (CST)" + "Link: {1}",
			"Link: {0}"
		];

		public static readonly string meowUrlFormat = "{0}/{1}";
		public static readonly string meowTitle = "GOGGiveawayNotifier";
		public static readonly string[] meowMessageFormat = [
			"GOG Giveaway\n\n" +
			"{0}\n" +
			"End Date: {1} (CST)\n" +
			"Link: {2}",

			"GOG Free Game\n\n" +
			"{0}\n" +
			"Link: {1}"
		];
		#endregion

		public static readonly string projectLink = "\n\nFrom https://github.com/azhuge233/GOGGiveawayNotifier";
		public static readonly string projectLinkHTML = "<br><br>From <a href=\"https://github.com/azhuge233/GOGGiveawayNotifier\">GOGGiveawayNotifier</a>";
	}
}