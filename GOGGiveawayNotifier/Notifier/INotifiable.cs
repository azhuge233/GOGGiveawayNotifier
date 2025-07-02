using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GOGGiveawayNotifier.Model;

namespace GOGGiveawayNotifier.Notifier {
	interface INotifiable : IDisposable {
		public Task SendMessage(List<GiveawayRecord> game);
	}
}