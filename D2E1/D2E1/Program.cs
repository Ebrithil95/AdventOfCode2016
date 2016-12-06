using System;


namespace D2E1 {

	enum Direction { UP, DOWN, LEFT, RIGHT, DIRECTION_AMOUNT }

	struct Position {
		public int x;
		public int y;

		public Position(int _x, int _y) {
			x = _x;
			y = _y;
		}
	}

	class Program {

		private static int[,] keypad = new int[3, 3] {  { 1, 2, 3 },
														{ 4, 5, 6 },
														{ 7, 8, 9 } };
		private static Position pos = new Position(1, 1);
		private static string code = "";

		private static void Move(Direction dir) {
			switch (dir) {
				case Direction.DOWN:
					pos.y = ++pos.y > 2 ? 2 : pos.y;
					break;
				case Direction.UP:
					pos.y = --pos.y < 0 ? 0 : pos.y;
					break;
				case Direction.LEFT:
					pos.x = --pos.x < 0 ? 0 : pos.x;
					break;
				case Direction.RIGHT:
					pos.x = ++pos.x > 2 ? 2 : pos.x;
					break;
			}
		}

		private static void ProcessInput(string[] args) {
			foreach (string s in args) {
				foreach (char c in s) {
					switch (c) {
						case 'U':
							Move(Direction.UP);
							break;
						case 'D':
							Move(Direction.DOWN);
							break;
						case 'L':
							Move(Direction.LEFT);
							break;
						case 'R':
							Move(Direction.RIGHT);
							break;
					}
				}

				code += keypad[pos.y, pos.x];
			}
		}

		static void Main(string[] args) {
			ProcessInput(args);
			Console.WriteLine("Code: " + code);
		}
	}
}