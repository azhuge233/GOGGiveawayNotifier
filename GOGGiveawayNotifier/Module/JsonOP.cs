using System;
using System.IO;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;
using GOGGiveawayNotifier.Model;
using System.Collections.Generic;

namespace GOGGiveawayNotifier.Module {
	public class JsonOP(ILogger<JsonOP> logger) : IDisposable {
		private readonly ILogger<JsonOP> _logger = logger;

		#region path strings
		private readonly string configPath = $"{AppDomain.CurrentDomain.BaseDirectory}Config{Path.DirectorySeparatorChar}config.json";
		private readonly string recordPath = $"{AppDomain.CurrentDomain.BaseDirectory}Record{Path.DirectorySeparatorChar}record.json";
		#endregion

		public void WriteData(List<GiveawayRecord> data) {
			try {
				if (data != null && data.Count > 0) {
					_logger.LogDebug("Writing records!");
					string json = JsonConvert.SerializeObject(data, Formatting.Indented);
					File.WriteAllText(recordPath, string.Empty);
					File.WriteAllText(recordPath, json);
					_logger.LogDebug("Done");
				} else _logger.LogDebug("No data, quit writing records");
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

		public void Dispose() {
			GC.SuppressFinalize(this);
		}
	}
}