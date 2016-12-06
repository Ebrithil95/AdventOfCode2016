using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace D6E1 {

	class Program {

		private static List<Dictionary<char, int>> dictionaries = new List<Dictionary<char, int>>();

		private static void ProcessInput() {
			try {
				StreamReader sr = new StreamReader("input.txt");
				while (!sr.EndOfStream) {
					string line = sr.ReadLine();
					int column = 0;
					foreach(char c in line) {
						if (dictionaries.Count < column + 1) {
							dictionaries.Add(new Dictionary<char, int>());
						}

						if (!dictionaries[column].ContainsKey(c)) {
							dictionaries[column].Add(c, 1);
						} else {
							dictionaries[column][c]++;
						}

						column++;
					}
				}
				sr.Close();
			} catch (Exception e) {
				Console.WriteLine("Couldn't open the input file:");
				Console.WriteLine(e.Message);
			}
		}

		static void Main(string[] args) {
			ProcessInput();

			string password = "";
			foreach (Dictionary<char, int> dic in dictionaries) {
				List<KeyValuePair<char, int>> sortedList = dic.ToList();
				sortedList.Sort((pair1, pair2) => pair2.Value.CompareTo(pair1.Value) == 0 ? pair1.Key.CompareTo(pair2.Key) : pair2.Value.CompareTo(pair1.Value));

				password += sortedList[0].Key;
			}

			Console.WriteLine("Password: " + password);
		}
	}
}
