using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;

namespace D5E2 {
    internal class Program {
        private const int IndexesToCalc = 30000;

        private static int _searchedIndexes;
        private static readonly Mutex SearchedIndexesLock = new Mutex();

        private static readonly Dictionary<int, string> PasswordDictionary = new Dictionary<int, string>();
        private static readonly Mutex PasswordLock = new Mutex();

        private static string _password = "";

        private static bool PasswordFilled() {
            var positions = new bool[8];

            foreach (KeyValuePair<int, string> entry in PasswordDictionary) {
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
            var encoding = new UTF8Encoding();
            MD5 md5Hasher = MD5.Create();

            const int indexSteps = IndexesToCalc;

            PasswordLock.WaitOne();
            while (!PasswordFilled()) {
                PasswordLock.ReleaseMutex();

                SearchedIndexesLock.WaitOne();
                int startIndex = _searchedIndexes;
                _searchedIndexes += indexSteps;
                SearchedIndexesLock.ReleaseMutex();

                for (int index = startIndex; index < startIndex + indexSteps; index++) {
                    byte[] byteLine = encoding.GetBytes(line + index);
                    byte[] hash = md5Hasher.ComputeHash(byteLine);

                    if ((hash[0] | hash[1] | (hash[2] >> 4)) == 0) {
                        string hex = BitConverter.ToString(hash);
                        if (hex[7] >= '0' && hex[7] < '8') {
                            Console.WriteLine("Letter Found: " + hex.Substring(7, 3) + " at Index: " + index);
                            PasswordLock.WaitOne();
                            PasswordDictionary.Add(index, hex.Substring(7, 3));
                            PasswordLock.ReleaseMutex();
                        }
                    }
                }

                PasswordLock.WaitOne();
            }
            PasswordLock.ReleaseMutex();
        }

        private static void ProcessInput() {
            Console.Write("Enter Door ID: ");
            string line = Console.ReadLine();

            int startTime = Environment.TickCount;

            var threads = new List<Thread>();

            Console.Write("Enter Thread Amount: ");
            int threadCount = int.Parse(Console.ReadLine());
            for (int i = 0; i < threadCount; i++) {
                var thread = new Thread(() => Worker(line));
                thread.Start();
                threads.Add(thread);
            }

            foreach (Thread t in threads) {
                t.Join();
            }

            Console.WriteLine("Generation Took " + (Environment.TickCount - startTime) / 1000.0f + " Seconds");

            List<KeyValuePair<int, string>> sortedList = PasswordDictionary.ToList();
            sortedList.Sort((pair1, pair2) => pair1.Key.CompareTo(pair2.Key));

            var pw = new char[8];

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
                _password += pw[i];
            }
        }

        private static void Main() {
            ProcessInput();
            Console.WriteLine("Password: " + _password.ToLower());
            Console.ReadKey();
        }
    }
}