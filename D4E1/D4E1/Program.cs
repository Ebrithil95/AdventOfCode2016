using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace D4E1 {
	class Program {

		private static List<Room> rooms = new List<Room>();

		private static void ProcessInput() {
			try {
				StreamReader sr = new StreamReader("input.txt");
				while (!sr.EndOfStream) {
					string line = sr.ReadLine();
					rooms.Add(new Room(line));
				}
				sr.Close();
			} catch (Exception e) {
				Console.WriteLine("Couldn't open the input file:");
				Console.WriteLine(e.Message);
			}

			int sum = 0;
			foreach (Room r in rooms) {
				if (r.IsValidRoom()) sum += r.GetSectorID();
			}

			Console.WriteLine("Sum: " + sum);
		}

		static void Main(string[] args) {
			ProcessInput();
		}
	}

	internal class Room {

		private Dictionary<char, int> encryptedName = new Dictionary<char, int>();
		private int sectorID;
		private string checksum;

		internal Room(string encryptedRoom) {
			string[] substrings = encryptedRoom.Split('-');

			for (int i = 0; i < substrings.Length; i++) {
				if (i < substrings.Length - 1) {
					foreach (char c in substrings[i]) {
						if (!encryptedName.ContainsKey(c)) {
							encryptedName.Add(c, 1);
						} else {
							encryptedName[c]++;
						}
					}
				} else {
					try {
						sectorID = int.Parse(substrings[i].Substring(0, substrings[i].LastIndexOf("[")));
						checksum = substrings[i].Substring(substrings[i].LastIndexOf("[") + 1, substrings[i].LastIndexOf("]") - substrings[i].LastIndexOf("[") - 1);
					} catch (FormatException e) {
						Console.WriteLine(e.Message);
					}
				}
			}
		}

		internal bool IsValidRoom() {
			List<KeyValuePair<char, int>> sortedList = encryptedName.ToList();
			sortedList.Sort((pair1, pair2) => pair2.Value.CompareTo(pair1.Value) == 0 ? pair1.Key.CompareTo(pair2.Key) : pair2.Value.CompareTo(pair1.Value));

			bool isValid = true;
			for (int i = 0; i < checksum.Length && isValid; i++) {
				isValid = checksum[i] == sortedList[i].Key;
			}

			return isValid;
		}

		internal int GetSectorID() {
			return sectorID;
		}
	}
}
