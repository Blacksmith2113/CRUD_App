using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace CRUD_App {
    internal class CategoryComparer : System.Collections.IComparer {
        private readonly Dictionary<string, int> _weights;

        public CategoryComparer(Dictionary<string, int> weights) {
            _weights = weights;
        }

        public int Compare(object x, object y) {
            string nameA, nameB;

            // Sprawdzamy, czy to grupy, czy surowe jednostki
            if (x is CollectionViewGroup groupA && y is CollectionViewGroup groupB) {
                nameA = groupA.Name.ToString();
                nameB = groupB.Name.ToString();
            }
            else if (x is Unit unitA && y is Unit unitB) {
                nameA = unitA.Category;
                nameB = unitB.Category;
            }
            else return 0;

            int weightA = _weights.ContainsKey(nameA) ? _weights[nameA] : 99;
            int weightB = _weights.ContainsKey(nameB) ? _weights[nameB] : 99;

            return weightA.CompareTo(weightB);
        }
    }
}
