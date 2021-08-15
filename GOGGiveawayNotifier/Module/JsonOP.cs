using System;
using System.IO;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;
using GOGGiveawayNotifier.Model;

namespace GOGGiveawayNotifier.Module {
	public class JsonOP : IDisposable {
		private readonly ILogger<JsonOP> _logger;

		#region path strings
		private readonly string configPath = $"{AppDomain.CurrentDomain.BaseDirectory}Config{Path.DirectorySeparatorChar}config.json";
		#endregion

		public JsonOP(ILogger<JsonOP> logger) {
			_logger = logger;
		}

		public Config LoadConfig() {
			try {
				_logger.LogDebug("Loading config");
				var content = JsonConvert.DeserializeObject<Config>(File.ReadAllText(configPath));
				_logger.LogDebug("Done");
				return content;
			} catch (Exception) {
				_logger.LogError("Loading config failed.");
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