using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;

namespace D4E {
	class Program {

		private static List<Room> rooms = new List<Room>();

		private static void FilterRooms() {
			for (int i = rooms.Count - 1; i >= 0; i--) {
				if (!rooms[i].IsValidRoom()) {
					rooms.RemoveAt(i);
				}
			}
		}

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
		}

		static void Main(string[] args) {
			ProcessInput();
			FilterRooms();

			foreach (Room r in rooms) {
				if (r.GetDecryptedName().Contains("north")) {
					Console.WriteLine(r.GetDecryptedName() + " SectorID: " + r.GetSectorID());
				}
			}

			Console.ReadKey();
		}
	}

	internal class Room {

		private Dictionary<char, int> nameDictionary = new Dictionary<char, int>();
		private int sectorID;
		private string checksum;
		private string encryptedName;

		internal Room(string encryptedRoom) {
			string[] substrings = encryptedRoom.Split('-');

			for (int i = 0; i < substrings.Length; i++) {
				if (i < substrings.Length - 1) {
					encryptedName += substrings[i] + " ";
					foreach (char c in substrings[i]) {
						if (!nameDictionary.ContainsKey(c)) {
							nameDictionary.Add(c, 1);
						} else {
							nameDictionary[c]++;
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

			encryptedName.TrimEnd();
		}

		internal bool IsValidRoom() {
			List<KeyValuePair<char, int>> sortedList = nameDictionary.ToList();
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

		private char RotateLetter(char letter, int amount) {
			int residual = amount % 26;
			int baseVal = char.ToLower(letter) - 'a';

			int returnVal = residual + baseVal;

			while (returnVal > 25) {
				returnVal -= 26;
			}

			return (char)(returnVal + 'a');
		}

		internal string GetDecryptedName() {
			if (!IsValidRoom()) {
				return null;
			} else {
				StringBuilder decryptedName = new StringBuilder();
				foreach (char c in encryptedName) {
					if (c == ' ') {
						decryptedName.Append(c);
					} else {
						decryptedName.Append(RotateLetter(c, sectorID));
					}
				}

				return decryptedName.ToString();
			}
		}
	}
}
