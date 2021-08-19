using System;
using System.Threading.Tasks;
using GOGGiveawayNotifier.Model;

namespace GOGGiveawayNotifier.Notifier {
	interface INotifiable : IDisposable {
		public Task SendMessage(NotifyConfig coonfig, GiveawayRecord game);
	}
}