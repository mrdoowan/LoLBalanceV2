using System;

namespace LoLBalancing
{
    // This abstract class is primarily used to bypass spaces and lowercases of each ign

    [Serializable]
    public class Name : IEquatable<Name>
    {
        private string name;

        // Default ctor
        public Name() {
            name = "";
        }

        // Init ctor
        public Name(string name_) {
            name = name_;
        }

        // The name in its spaceless and lowercase form
        private string filter() {
            return name.Replace(" ", string.Empty).ToLower();
        }

        // Actual comparison function
        public bool Equals(Name other) {
            if (other == null) { return false; }
            return filter() == other.filter();
        }

        // Abstract override for 'Name' equivalent
        public override bool Equals(object obj) {
            if (obj == null) { return false; }
            Name objAsName = obj as Name;
            if (objAsName == null) { return false; }
            else { return Equals(objAsName); }
        }

        // This is so that bucketing for keys is done appropriately
        public override int GetHashCode() {
            int val = 0;
            foreach (char c in filter()) {
                val += Convert.ToInt32(c);
            }
            return val;
        }

        // It outputs the string instead of its base object
        public override string ToString() {
            return name;
        }

        // Sets name
        public void SetName(string nameIn) {
            name = nameIn;
        }

        // Override == and !=
        public static bool operator ==(Name a, Name b) {
            if (ReferenceEquals(a, b)) {
                return true;
            }
            if ((object)a == null || ((object)b == null)) {
                return false;
            }
            return a.Equals(b);
        }
        public static bool operator !=(Name a, Name b) {
            return !(a == b);
        }
    }
}
