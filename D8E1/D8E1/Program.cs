using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D8E1 {
	class Program {

		private static bool[,] display = new bool[6, 50];

		private static void PrintDisplay() {
			for (int i = 0; i < 6; i++) {
				for (int j = 0; j < 50; j++) {
					Console.Write(display[i, j] ? '#' : ' ');
				}
				Console.Write('\n');
			}
		}

		private static void Light(int x, int y) {
			for (int i = 0; i < y; i++) {
				for (int j = 0; j < x; j++) {
					display[i, j] = true;
				}
			}
		}

		private static void ShiftRow(int row, int amount) {
			amount = amount % 50;
			for (int i = 0; i < amount; i++) {

			}
		}

		private static void ProcessInput() {
			try {
				StreamReader sr = new StreamReader("input.txt");
				while (!sr.EndOfStream) {
					string line = sr.ReadLine();
					string[] commands = line.Split(' ');

					if (commands[0].Equals("rect")) {
						string[] parameters = commands[1].Split('x');
						Light(int.Parse(parameters[0]), int.Parse(parameters[1]));
					} else if (commands[0].Equals("rotate")) {
						if (commands[1].Equals("row")) {
							ShiftRow(0, 2);
						} else if (commands[1].Equals("column")) {

						}
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
			PrintDisplay();
		}
	}
}
