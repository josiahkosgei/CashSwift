// Utilities.ByteExtentions


using System;

namespace CashSwift.Library.Standard.Utilities
{
    public static class ByteExtentions
    {
        public static bool IsBitSet(this char b, int pos) => Convert.ToByte(b).IsBitSet(pos);
    }
}
