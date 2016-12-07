using System;
using System.Collections.Generic;
using System.IO;

namespace D7E1 {
	class Program {

		private static bool IsTLS(string line) {
			bool abba = false;
			bool hypernet = true;
			bool foundOpenBracket = false;
			for (int i = 0; i < line.Length - 3; i++) {
				if (line[i] == line[i + 3] && line[i + 1] == line[i + 2] && line[i] != line[i + 1]) {
					abba = true;
					if (foundOpenBracket) {
						hypernet = false;
					}
				}
				if (line[i] == '[') foundOpenBracket = true;
				if (line[i] == ']') foundOpenBracket = false;
			}

			return hypernet && abba;
		}

		private static bool IsSSL(string line) {
			List<string> aba = new List<string>();
			List<string> bab = new List<string>();

			bool foundOpenBracket = false;
			for (int i = 0; i < line.Length - 2; i++) {
				if (line[i] == line[i + 2] && line[i] != line[i + 1]) {
					if (!foundOpenBracket) aba.Add(line.Substring(i, 3));
					else bab.Add(line.Substring(i, 3));
				}
				if (line[i] == '[') foundOpenBracket = true;
				if (line[i] == ']') foundOpenBracket = false;
			}

			foreach (string s in aba) {
				string correspondingBAB = s[1].ToString() + s[0].ToString() + s[1].ToString();
				if (bab.Contains(correspondingBAB)) return true;
			}
			return false;
		}

		private static void ProcessInput() {
			try {
				StreamReader sr = new StreamReader("input.txt");
				int tlsCount = 0;
				int sslCount = 0;
				while (!sr.EndOfStream) {
					string line = sr.ReadLine();					
					if (IsTLS(line)) tlsCount++;
					if (IsSSL(line)) sslCount++;
				}
				Console.WriteLine("TLS Amount: " + tlsCount);
				Console.WriteLine("SSL Amount: " + sslCount);
				sr.Close();
			} catch (Exception e) {
				Console.WriteLine("Couldn't open the input file:");
				Console.WriteLine(e.Message);
			}
		}

		static void Main(string[] args) {
			ProcessInput();
		}
	}
}
