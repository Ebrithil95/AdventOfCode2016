using System;
using System.IO;
using System.Linq;
using System.Text;

namespace D9 {
	class Program {

		private static StringBuilder decompressedString = new StringBuilder();

		private static void ProcessInput() {
			try {
				StreamReader sr = new StreamReader("input.txt");

				while (!sr.EndOfStream) {
					string line = sr.ReadLine();

					int currentIndex = 0;
					while(currentIndex < line.Length) {
						char currentChar = line[currentIndex];

						if (currentChar == '(') {
							int closingIndex = line.IndexOf(')', currentIndex + 1);
							int length = closingIndex - currentIndex;
							string command = line.Substring(currentIndex + 1, length - 1);
							string[] values = command.Split('x');

							int repeats = int.Parse(values[1]);
								length = int.Parse(values[0]);
							string appendString = line.Substring(closingIndex + 1, length);
							for (int i = 0; i < repeats; i++) {
								decompressedString.Append(appendString);
							}
							currentIndex = closingIndex + 1 + length; 
						} else {
							decompressedString.Append(currentChar);
							currentIndex++;
						}
					}
				}
				sr.Close();
			} catch (Exception e) {
				Console.WriteLine(e.Message);
				Console.WriteLine(e.StackTrace);
			}
		}

		private static Int64 ComputeLength(string line) {
			Int64 strLength = 0;

			StringBuilder returnLine = new StringBuilder(line);
			while (returnLine.ToString().Contains('(')) {
				string current = returnLine.ToString();
				int openingIndex = current.IndexOf('(');
				int closingIndex = current.IndexOf(')', openingIndex + 1);
				int length = closingIndex - openingIndex;

				string command = current.Substring(openingIndex + 1, length - 1);
				string[] values = command.Split('x');

				returnLine.Remove(openingIndex, length + 1 + int.Parse(values[0]));

				int repeats = int.Parse(values[1]);
				length = int.Parse(values[0]);

				strLength += repeats * ComputeLength(current.Substring(closingIndex + 1, length));
			}

			strLength += returnLine.ToString().Length;

			return strLength;
		}

		private static void ProcessInputB() {
			try {
				StreamReader sr = new StreamReader("input.txt");
				Int64 len = 0;
				while (!sr.EndOfStream) {
					string line = sr.ReadLine();

					len += ComputeLength(line);
				}
				sr.Close();
				Console.WriteLine("Length V2: " + len);
			} catch (Exception e) {
				Console.WriteLine(e.Message);
				Console.WriteLine(e.StackTrace);
			}
		}

		static void Main(string[] args) {
			ProcessInput();
			Console.WriteLine("Length V1: " + decompressedString.Length);
			ProcessInputB();
		}
	}
}
