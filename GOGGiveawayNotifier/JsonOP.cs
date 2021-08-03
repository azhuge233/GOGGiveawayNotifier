using System;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;

namespace GOGGiveawayNotifier {
	class JsonOP : IDisposable {
		private readonly ILogger<JsonOP> _logger;
		private readonly string debugJsonOP = "Loading config";
		private readonly string configPath = $"{AppDomain.CurrentDomain.BaseDirectory}{Path.DirectorySeparatorChar}config.json";

		public JsonOP(ILogger<JsonOP> logger) {
			_logger = logger;
		}

		public Dictionary<string, string> LoadConfig() {
			try {
				_logger.LogDebug(debugJsonOP);
				var content = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(configPath));
				if (content["TOKEN"] == string.Empty) {
					throw new Exception(message: "No Token provided!");
				}
				if (content["CHAT_ID"] == string.Empty) {
					throw new Exception(message: "No ChatID provided!");
				}
				_logger.LogDebug($"Done: {debugJsonOP}");
				return content;
			} catch (Exception) {
				_logger.LogError($"Error: {debugJsonOP}");
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
