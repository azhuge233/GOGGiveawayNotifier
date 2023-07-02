using System;
using System.IO;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;
using GOGGiveawayNotifier.Model;
using System.Collections.Generic;

namespace GOGGiveawayNotifier.Module {
	public class JsonOP : IDisposable {
		private readonly ILogger<JsonOP> _logger;

		#region path strings
		private readonly string configPath = $"{AppDomain.CurrentDomain.BaseDirectory}Config{Path.DirectorySeparatorChar}config.json";
		private readonly string recordPath = $"{AppDomain.CurrentDomain.BaseDirectory}Record{Path.DirectorySeparatorChar}record.json";
		#endregion

		public JsonOP(ILogger<JsonOP> logger) {
			_logger = logger;
		}

		public void WriteData(List<GiveawayRecord> data) {
			try {
				if (data != null) {
					_logger.LogDebug("Writing records!");
					string json = JsonConvert.SerializeObject(data, Formatting.Indented);
					File.WriteAllText(recordPath, string.Empty);
					File.WriteAllText(recordPath, json);
					_logger.LogDebug("Done");
				} else _logger.LogDebug("Null data, quit writing records");
			} catch (Exception) {
				_logger.LogError("Writing data failed.");
				throw;
			}
		}

		public List<GiveawayRecord> LoadData() {
			try {
				_logger.LogDebug("Loading previous records");
				var content = JsonConvert.DeserializeObject<List<GiveawayRecord>>(File.ReadAllText(recordPath));
				_logger.LogDebug("Done");
				return content;
			} catch (Exception) {
				_logger.LogError("Loading previous records failed.");
				throw;
			}
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
			}
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
		}
	}
}