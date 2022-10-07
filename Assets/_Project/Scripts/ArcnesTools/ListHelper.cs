using System;
using System.Collections.Generic;

namespace ArcnesTools
{
    public static class ListHelper
    {
        public static List<List<T>> SplitList<T>(this List<T> listToSplit, int newMaxSize=1)
        {
            var partitions = new List<List<T>>();
            
            for (int i = 0; i < listToSplit.Count; i += newMaxSize)
            {
                partitions.Add(listToSplit.GetRange(i, Math.Min(newMaxSize, listToSplit.Count - i)));
            }

            return partitions;
        }
    }
}
