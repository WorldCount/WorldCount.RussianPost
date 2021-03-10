using System.Collections.Generic;

namespace WorldCount.RussianPost.PostApi.Barcodes
{
    internal static class Const
    {
        internal static readonly List<char> Nums = new List<char> { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
        internal static readonly List<int> Multipliers = new List<int> { 8, 6, 4, 2, 3, 5, 9, 7 };
    }
}
