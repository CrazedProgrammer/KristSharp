using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crazed.KristSharp
{
    public class KristBlock : IEquatable<KristBlock>
    {
        public readonly long Height;
        public readonly KristAddress Address;
        public readonly string Hash;
        public readonly string ShortHash;
        public int Value;
        public long Difficulty;
        public DateTime Time;

        internal KristBlock(KBlock block)
        {
            Height = block.height;
            Address = new KristAddress(block.address);
            Hash = block.hash;
            ShortHash = block.short_hash;
            Value = block.value;
            Difficulty = block.difficulty;
            Time = block.time;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            KristBlock block = obj as KristBlock;
            if (block == null)
                return false;
            else
                return this == block;
        }

        public bool Equals(KristBlock other)
        {
            return Equals((object)other);
        }

        public override int GetHashCode()
        {
            return Height.GetHashCode();
        }

        public static bool operator ==(KristBlock block1, KristBlock block2)
        {
            if (object.ReferenceEquals(block1, block2))
                return true;
            if ((object)block1 == null || (object)block2 == null)
                return false;

            return block1.Height == block2.Height;
        }

        public static bool operator !=(KristBlock block1, KristBlock block2)
        {
            return !(block1 == block2);
        }
    }
}
