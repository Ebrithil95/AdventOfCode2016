using System;
using System.IO;

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
			bool[] tmp = new bool[50];

			for (int i = 0; i < 50; i++) {
				tmp[(i + amount) % 50] = display[row, i];
			}

			for (int i = 0; i < 50; i++) {
				display[row, i] = tmp[i];
			}
		}

		private static void ShiftColumn(int column, int amount) {
			amount = amount % 6;
			bool[] tmp = new bool[6];

			for (int i = 0; i < 6; i++) {
				tmp[(i + amount) % 6] = display[i, column];
			}

			for (int i = 0; i < 6; i++) {
				display[i, column] = tmp[i];
			}
		}

		private static int GetLitPixelAmount() {
			int amount = 0;
			foreach(bool b in display) {
				if (b) amount++;
			}

			return amount;
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
							ShiftRow(int.Parse(commands[2].Substring(2)), int.Parse(commands[4]));
						} else if (commands[1].Equals("column")) {
							ShiftColumn(int.Parse(commands[2].Substring(2)), int.Parse(commands[4]));
						}
					}

				}
				sr.Close();
			} catch (Exception e) {
				Console.WriteLine(e.Message);
			}
		}


		static void Main(string[] args) {
			ProcessInput();
			PrintDisplay();
			Console.WriteLine(GetLitPixelAmount() + " Pixels are Lit.");
		}
	}
}
