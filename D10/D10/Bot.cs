using System.Collections.Generic;

namespace D10 {
	internal class Bot : IGiveTarget {

		List<Microchip> chips = new List<Microchip>();
		public IGiveTarget highTarget = null;
		public IGiveTarget lowTarget = null;

		public List<KeyValuePair<int, int>> compares = new List<KeyValuePair<int, int>>();

		public void AddMicrochip(Microchip chip) {
			chips.Add(chip);
			CheckDistribution();
		}

		private void CheckDistribution() {
			if (chips.Count == 2) {
				if (highTarget != null) highTarget.AddMicrochip(chips[0].value > chips[1].value ? chips[0] : chips[1]);
				if (lowTarget != null) lowTarget.AddMicrochip(chips[0].value > chips[1].value ? chips[1] : chips[0]);

				compares.Add(new KeyValuePair<int, int>(chips[0].value, chips[1].value));

				chips = new List<Microchip>();
			}
		}

		public override string ToString() {
			string s = "";
			foreach (Microchip chip in chips) {
				s += chip.value + " ";
			}

			s += "\tCompares: ";
			foreach (KeyValuePair<int, int> pair in compares) {
				s += pair.Key + " with " + pair.Value + " ";
			}

			return s.TrimEnd();
		}
	}
}
