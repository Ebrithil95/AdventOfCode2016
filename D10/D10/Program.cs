using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace D10 {
	class Program {

		private static Dictionary<int, Bot> bots = new Dictionary<int, Bot>();
		private static Dictionary<int, Output> outputs = new Dictionary<int, Output>();

		private static void ProcessInput() {
			try {
				StreamReader sr = new StreamReader("input.txt");
				//Store values to be added after bots are set up
				List<KeyValuePair<int, int>> valueAdds = new List<KeyValuePair<int, int>>();

				while (!sr.EndOfStream) {
					string line = sr.ReadLine();

					string[] commands = line.Split(' ');

					if (commands[0].Equals("value")) {
						int bot = int.Parse(commands[5]);
						int value = int.Parse(commands[1]);

						if (!bots.ContainsKey(bot)) {
							bots.Add(bot, new Bot());
						}

						valueAdds.Add(new KeyValuePair<int, int>(bot, value));
					} else {
						int bot = int.Parse(commands[1]);
						int low = int.Parse(commands[6]);
						int high = int.Parse(commands[11]);

						if (!bots.ContainsKey(bot)) {
							bots.Add(bot, new Bot());
						}

						if (commands[5].Equals("bot")) {
							if (!bots.ContainsKey(low)) {
								bots.Add(low, new Bot());
							}

							bots[bot].lowTarget = bots[low];
						} else {
							if (!outputs.ContainsKey(low)) {
								outputs.Add(low, new Output());
							}

							bots[bot].lowTarget = outputs[low];
						}

						if (commands[10].Equals("bot")) {
							if (!bots.ContainsKey(high)) {
								bots.Add(high, new Bot());
							}

							bots[bot].highTarget = bots[high];
						} else {
							if (!outputs.ContainsKey(high)) {
								outputs.Add(high, new Output());
							}

							bots[bot].highTarget = outputs[high];
						}
					}

				}

				//Add values to bots now that they know where to store them
				foreach(KeyValuePair<int, int> pair in valueAdds) {
					bots[pair.Key].AddMicrochip(new Microchip(pair.Value));
				}

				sr.Close();
			} catch (Exception e) {
				Console.WriteLine(e.Message);
				Console.WriteLine(e.StackTrace);
			}
		}

		static void Main(string[] args) {
			ProcessInput();

			foreach (KeyValuePair<int, Bot> pair in bots.ToList()) {
				if (pair.Value.compares.Contains(new KeyValuePair<int, int>(17, 61)) ||
					pair.Value.compares.Contains(new KeyValuePair<int, int>(61, 17))) {
					Console.WriteLine(String.Format("Bot {0:D3} has Chips: {1}", pair.Key, pair.Value.ToString()));
				}
			}

			int result = 1;
			for (int i = 0; i < 3; i++) {
				foreach (Microchip chip in outputs[i].GetChips()) {
					result *= chip.value;
				}
			}

			Console.WriteLine("Multiplied value: " + result);

			Console.ReadKey();
		}
	}
}
