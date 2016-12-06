using System;
using System.IO;
using System.Text.RegularExpressions;

namespace D3E2 {
	class Program {

		private static void ProcessInput() {
			int validAmount = 0;
			try {
				StreamReader sr = new StreamReader("input.txt");
				while (!sr.EndOfStream) {
					string line = sr.ReadLine();
					line = Regex.Replace(line, @"\s+", " ");
					string[] sides1 = line.Trim().Split(' ');

					string line2 = sr.ReadLine();
					line2 = Regex.Replace(line2, @"\s+", " ");
					string[] sides2 = line2.Trim().Split(' ');

					string line3 = sr.ReadLine();
					line3 = Regex.Replace(line3, @"\s+", " ");
					string[] sides3 = line3.Trim().Split(' ');

					for (int i = 0; i < 3; i++) {
						int sideA = int.Parse(sides1[i]);
						int sideB = int.Parse(sides2[i]);
						int sideC = int.Parse(sides3[i]);

						if (sideA + sideB > sideC && sideA + sideC > sideB && sideB + sideC > sideA) {
							validAmount++;
						}
					}
				}
				sr.Close();

				Console.WriteLine("Valid Amount: " + validAmount);
			} catch (FormatException e) {
				Console.WriteLine("Parsing the input file failed:");
				Console.WriteLine(e.Message);
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
