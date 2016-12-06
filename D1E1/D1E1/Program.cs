using System;
using System.Linq;

namespace D1E1 {

	enum Direction { NORTH, EAST, SOUTH, WEST, DIRECTION_AMOUNT };

	struct Position {
		public int x;
		public int y;

		public Position(int _x, int _y) {
			x = _x;
			y = _y;
		}
	}

	class Program {

		private static Direction currentDir = Direction.NORTH;
		private static Position currentPos = new Position(0, 0);

		private static void TurnRight() {
			if (++currentDir == Direction.DIRECTION_AMOUNT) {
				currentDir = Direction.NORTH;
			}
		}

		private static void TurnLeft() {
			if ((int)(--currentDir) == -1) {
				currentDir = Direction.WEST;
			}
		}

		private static void Walk(int steps) {
			switch (currentDir) {
				case Direction.NORTH:
					currentPos.y += steps;
					break;
				case Direction.SOUTH:
					currentPos.y -= steps;
					break;
				case Direction.EAST:
					currentPos.x += steps;
					break;
				case Direction.WEST:
					currentPos.x -= steps;
					break;
			}
		}

		private static int GetDistance() {
			return Math.Abs(currentPos.x) + Math.Abs(currentPos.y);
		}

		private static bool ProcessInput(string[] args) {
			foreach (string s in args) {
				string command = s.Contains(',') ? s.Remove(s.Length - 1) : s;
				if (command[0].Equals('R')) {
					TurnRight();
				} else {
					TurnLeft();
				}
				try {
					Walk(int.Parse(command.Substring(1)));
				} catch (Exception e) {
					Console.WriteLine("Invalid input: " + command);
					return false;
				}
			}

			return true;
		}

		static void Main(string[] args) {
			if (ProcessInput(args)) {
				Console.WriteLine("Distance: " + GetDistance());
			}
		}
	}
}
