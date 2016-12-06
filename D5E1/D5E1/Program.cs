using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;

namespace D5E1 {
	class Program {

		private static ulong searchedIndexes = 0;
		private static readonly Mutex searchedIndexesLock =  new Mutex();

		private static Dictionary<ulong, char> passwordDictionary = new Dictionary<ulong, char>();
		private static readonly Mutex passwordLock = new Mutex();

		private static string password = "";

		private static void Worker(string line) {
			UTF8Encoding encoding = new UTF8Encoding();
			MD5 md5hasher = MD5.Create();

			passwordLock.WaitOne();
			while(passwordDictionary.Count < 8) {
				passwordLock.ReleaseMutex();

				searchedIndexesLock.WaitOne();
				ulong startIndex = searchedIndexes;
				searchedIndexes += 30000;
				searchedIndexesLock.ReleaseMutex();

				for (ulong index = startIndex; index < startIndex + 30000; index++) {
					byte[] byteLine = encoding.GetBytes(line + index);
					byte[] hash = md5hasher.ComputeHash(byteLine);

					string hex = BitConverter.ToString(hash);

					if (hex.StartsWith("00-00-0")) {
						Console.WriteLine("Letter Found: " + hex[7] + " at Index: " + index);
						passwordLock.WaitOne();
						passwordDictionary.Add(index, hex[7]);
						passwordLock.ReleaseMutex();
					}
				}


				passwordLock.WaitOne();
			}
			passwordLock.ReleaseMutex();
		}

		private static void ProcessInput() {
			Console.Write("Enter Door ID: ");
			string line = Console.ReadLine();

			List<Thread> threads = new List<Thread>();

			for (int i = 0; i < Environment.ProcessorCount; i++) {
				Thread thread = new Thread(() => Worker(line));
				thread.Start();
				threads.Add(thread);
			}

			foreach (Thread t in threads) {
				t.Join();
			}

			List<KeyValuePair<ulong, char>> sortedList = passwordDictionary.ToList();
			sortedList.Sort((pair1, pair2) => pair1.Key.CompareTo(pair2.Key));

			for (int i = 0; i < 8; i++) {
				password += sortedList[i].Value;
			}

		}


		static void Main(string[] args) {
			ProcessInput();
			Console.WriteLine("Password: " + password);
			Console.ReadKey();
		}
	}
}
