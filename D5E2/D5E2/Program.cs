using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;

namespace D5E2 {
	class Program {

		private const int INDEXES_TO_CALC = 30000;

		private static int searchedIndexes = 0;
		private static readonly Mutex searchedIndexesLock = new Mutex();

		private static Dictionary<int, string> passwordDictionary = new Dictionary<int, string>();
		private static readonly Mutex passwordLock = new Mutex();

		private static string password = "";

		private static bool passwordFilled() {
			bool[] positions = new bool[8];

			foreach (KeyValuePair<int, string> entry in passwordDictionary) {
				positions[int.Parse(entry.Value.Substring(0, 1))] = true;
			}

			foreach (bool b in positions) {
				if (!b) {
					return false;
				}
			}
			return true;
		}

		private static void Worker(string line) {
			UTF8Encoding encoding = new UTF8Encoding();
			MD5 md5hasher = MD5.Create();

			int indexSteps = INDEXES_TO_CALC;

			passwordLock.WaitOne();
			while (!passwordFilled()) {
				passwordLock.ReleaseMutex();

				searchedIndexesLock.WaitOne();
				int startIndex = searchedIndexes;
				searchedIndexes += indexSteps;
				searchedIndexesLock.ReleaseMutex();

				for (int index = startIndex; index < startIndex + indexSteps; index++) {
					byte[] byteLine = encoding.GetBytes(line + index);
					byte[] hash = md5hasher.ComputeHash(byteLine);
					
					if ((hash[0] | hash[1] | (hash[2] >> 4)) == 0) {
						string hex = BitConverter.ToString(hash);
						if (hex[7] >= '0' && hex[7] < '8') {
							Console.WriteLine("Letter Found: " + hex.Substring(7, 3) + " at Index: " + index);
							passwordLock.WaitOne();
							passwordDictionary.Add(index, hex.Substring(7, 3));
							passwordLock.ReleaseMutex();
						}
					}
				}

				passwordLock.WaitOne();
			}
			passwordLock.ReleaseMutex();

			return;
		}

		private static void ProcessInput() {
			Console.Write("Enter Door ID: ");
			string line = Console.ReadLine();

			int startTime = Environment.TickCount;

			List<Thread> threads = new List<Thread>();

			Console.Write("Enter Thread Amount: ");
			int threadCount = int.Parse(Console.ReadLine());
			for (int i = 0; i < threadCount; i++) {
				Thread thread = new Thread(() => Worker(line));
				thread.Start();
				threads.Add(thread);
			}

			foreach (Thread t in threads) {
				t.Join();
			}

			Console.WriteLine("Generation Took " + (Environment.TickCount - startTime) / 1000.0f + " Seconds");

			List<KeyValuePair<int, string>> sortedList = passwordDictionary.ToList();
			sortedList.Sort((pair1, pair2) => pair1.Key.CompareTo(pair2.Key));

			char[] pw = new char[8];

			for (int i = 0; i < 8; i++) {
				bool filled = false;
				for (int j = 0; j < sortedList.Count && !filled; j++) {
					if (int.Parse(sortedList[j].Value.Substring(0, 1)) == i) {
						filled = true;
						pw[i] = sortedList[j].Value[2];
					}
				}
			}

			for (int i = 0; i < 8; i++) {
				password += pw[i];
			}

		}

		static void Main(string[] args) {
			ProcessInput();
			Console.WriteLine("Password: " + password.ToLower());
			Console.ReadKey();
		}
	}
}
