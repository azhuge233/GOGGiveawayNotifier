using GOGGiveawayNotifier.Model;
using GOGGiveawayNotifier.Model.String;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace GOGGiveawayNotifier.Module {
	class AutoClaimer(ILogger<AutoClaimer> logger, IOptions<Config> config) : IDisposable {
		private readonly ILogger<AutoClaimer> _logger = logger;
		private readonly Config config = config.Value;

		#region debug strings
		private readonly string debugAutoClaim = "Auto claim giveaway";
		private readonly string infoACDiabled = "Auto claim disabled, skipping";
		private readonly string infoNoCookie = "No cookie configured, skipping";
		private readonly string infoNoNewGiveaway = "No (new) giveaway detected, autoclaim abort";
		private readonly string infoClaimSuccess = "Game claimed successfully!";
		private readonly string infoAlreadyClaimed = "Game was already claimed!";
		private readonly string warnClaimFailed = "Game claim may have failed, check the result!";
		#endregion

		public async Task<string> Claim(GiveawayRecord game) {
			if (!config.EnableAutoClaim) {
				_logger.LogInformation(infoACDiabled);
				return string.Empty;
			}

			if (string.IsNullOrWhiteSpace(config.Cookie) || string.IsNullOrEmpty(config.Cookie)) {
				_logger.LogInformation(infoNoCookie);
				return string.Empty;
			}

			if (game == null || string.IsNullOrEmpty(game.ID)) {
				_logger.LogInformation(infoNoNewGiveaway);
				return string.Empty;
			}

			_logger.LogDebug(debugAutoClaim);

			try {
				var httpClient = new HttpClient();

				var request = new HttpRequestMessage() {
					Method = HttpMethod.Get,
					RequestUri = new Uri(AutoClaimerStrings.Url),
					Headers = {
						{ AutoClaimerStrings.UAKey, AutoClaimerStrings.UAValue },
						{ AutoClaimerStrings.CookieKey, config.Cookie }
					}
				};

				var resp = await httpClient.SendAsync(request);
				var result = await resp.Content.ReadAsStringAsync();

				_logger.LogDebug($"Claim result: {result}");

				if(result == "{}") _logger.LogInformation(infoClaimSuccess);
				else if(result.ToLower().Contains("already claimed")) _logger.LogInformation(infoAlreadyClaimed);
				else _logger.LogWarning(warnClaimFailed);

				_logger.LogDebug($"Done: {debugAutoClaim}");

				return result;
			} catch (Exception) {
				_logger.LogError($"Error: {debugAutoClaim}");
				throw;
			} finally {
				Dispose();
			}
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
		}
	}
}
