using System;
using System.IO;
using System.Text.RegularExpressions;

namespace D3E1 {
	class Program {

		private static void ProcessInput() {
			int validAmount = 0;
			try {
				StreamReader sr = new StreamReader("input.txt");
				while (!sr.EndOfStream) {
					string line = sr.ReadLine();
					line = Regex.Replace(line, @"\s+", " ");
					string[] sides = line.Trim().Split(' ');

					int sideA = int.Parse(sides[0]);
					int sideB = int.Parse(sides[1]);
					int sideC = int.Parse(sides[2]);

					if (sideA + sideB > sideC && sideA + sideC > sideB && sideB + sideC > sideA) {
						validAmount++;
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
