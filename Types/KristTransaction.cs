using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crazed.KristSharp
{
    public class KristTransaction : IEquatable<KristTransaction>
    {
        public readonly long ID;
        public readonly KristAddress From;
        public readonly KristAddress To;
        public readonly int Value;
        public readonly DateTime Time;
        public readonly string Name;
        public readonly string MetaData;

        internal KristTransaction(KTransaction transaction)
        {
            ID = transaction.id;
            From = new KristAddress(transaction.from);
            To = new KristAddress(transaction.to);
            Value = transaction.value;
            Time = transaction.time;//KristUtils.ParseDateTime(transaction.time);
            Name = transaction.name;
            MetaData = transaction.metadata;
        }



        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            KristTransaction transaction = obj as KristTransaction;
            if (transaction == null)
                return false;
            else
                return this == transaction;
        }

        public bool Equals(KristTransaction other)
        {
            return Equals((object)other);
        }

        public override int GetHashCode()
        {
            return ID.GetHashCode();
        }

        public static bool operator ==(KristTransaction transaction1, KristTransaction transaction2)
        {
            if (object.ReferenceEquals(transaction1, transaction2))
                return true;
            if ((object)transaction1 == null || (object)transaction2 == null)
                return false;

            return transaction1.ID == transaction2.ID;
        }

        public static bool operator !=(KristTransaction transaction1, KristTransaction transaction2)
        {
            return !(transaction1 == transaction2);
        }

        public override string ToString()
        {
            return "Krist transaction #" + ID.ToString();
        }
    }
}
