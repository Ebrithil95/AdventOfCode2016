using System.Collections.Generic;

namespace D10 {
	internal class Output : IGiveTarget {

		private List<Microchip> chips = new List<Microchip>();


		public void AddMicrochip(Microchip chip) {
			chips.Add(chip);
		}

		public List<Microchip> GetChips() {
			return chips;
		}

		public override string ToString() {
			string s = "";
			foreach (Microchip chip in chips) {
				s += chip.value + " ";
			}

			return s.TrimEnd();
		}
	}
}
