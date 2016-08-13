using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crazed.KristSharp
{
    public class KristName : IEquatable<KristName>
    {
        public readonly string Name;
        public readonly KristAddress Owner;
        public readonly DateTime Registered;
        public readonly DateTime Updated;
        public readonly string ARecord;

        internal KristName(KName name)
        {
            Name = name.name;
            Owner = new KristAddress(name.owner);
            Registered = name.registered;
            Updated = name.updated;
            ARecord = name.a;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            KristName name = obj as KristName;
            if (name == null)
                return false;
            else
                return this == name;
        }

        public bool Equals(KristName other)
        {
            return Equals((object)other);
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode() ^ Owner.GetHashCode();
        }

        public static bool operator ==(KristName name1, KristName name2)
        {
            if (object.ReferenceEquals(name1, name2))
                return true;
            if ((object)name1 == null || (object)name2 == null)
                return false;

            return name1.Name == name2.Name && name1.Owner == name2.Owner;
        }

        public static bool operator !=(KristName name1, KristName name2)
        {
            return !(name1 == name2);
        }

        public override string ToString()
        {
            return Name + ".kst owned by " + Owner.Address;
        }
    }
}
