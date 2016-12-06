using System;
using System.Collections.Generic;
using System.Linq;

namespace D1E2 {

	enum Direction { NORTH, EAST, SOUTH, WEST, DIRECTION_AMOUNT };

	struct Position : IEquatable<Position> {
		public int x;
		public int y;

		public Position(int _x, int _y) {
			x = _x;
			y = _y;
		}

		public bool Equals(Position other) {
			return x == other.x && y == other.y;
		}
	}

	class Program {

		private static Direction currentDir = Direction.NORTH;
		private static Position currentPos = new Position(0, 0);

		private static List<Position> visitedPositions = new List<Position>();

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
			if (steps > 0) {
				Position newPos = new Position(currentPos.x, currentPos.y);
				switch (currentDir) {
					case Direction.NORTH:
						newPos.y += 1;
						break;
					case Direction.SOUTH:
						newPos.y -= 1;
						break;
					case Direction.EAST:
						newPos.x += 1;
						break;
					case Direction.WEST:
						newPos.x -= 1;
						break;
				}

				if (visitedPositions.Contains(newPos)) {
					Console.WriteLine("Distance: " + GetDistance(newPos));
					Environment.Exit(0);
				} else {
					visitedPositions.Add(newPos);
					currentPos = newPos;
					Walk(steps - 1);
				}
			}
		}

		private static int GetDistance(Position pos) {
			return Math.Abs(pos.x) + Math.Abs(pos.y);
		}

		private static void ProcessInput(string[] args) {
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
					return;
				}
			}
		}

		static void Main(string[] args) {
			visitedPositions.Add(currentPos);
			ProcessInput(args);
		}
	}
}
