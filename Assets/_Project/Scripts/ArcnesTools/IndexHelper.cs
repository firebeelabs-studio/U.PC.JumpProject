using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArcnesTools
{
    public static class IndexHelper
    {
        /// <summary>
        /// Method increments and return index in loop. If index will exceed bounds it will switch to first/last number.
        /// </summary>
        /// <param name="incrementNumber">Number by which we will change the index, to iterate backwards make it negative.</param>
        /// <param name="currentIndex">Index which you wanna change.</param>
        /// <param name="listToLoop">List on which you are iterating with index.</param>
        public static int LoopIndex<T>(int incrementNumber, int currentIndex, List<T> listToLoop)
        {
            if (incrementNumber > 0)
            {
                if (currentIndex + incrementNumber > listToLoop.Count - 1)
                {
                    currentIndex = 0;
                }
                else
                {
                    currentIndex += incrementNumber;
                }
            }
            else if (incrementNumber < 0)
            {
                if (currentIndex + incrementNumber < 0)
                {
                    currentIndex = listToLoop.Count - 1;
                }
                else
                {
                    currentIndex += incrementNumber;
                }
            }

            return currentIndex;
        }
    
        /// <summary>
        /// Method increments and return index in loop. If index will exceed bounds it will switch to first/last number.
        /// </summary>
        /// <param name="incrementNumber">Number by which we will change the index, to iterate backwards make it negative.</param>
        /// <param name="currentIndex">Index which you wanna change.</param>
        /// <param name="maxLength">LAST index number, don't pass element count here. If you are working with List and not single elements, consider using LoopIndex method.</param>
        public static int LoopIndexControlledByNumber(int incrementNumber, int currentIndex, int maxLength)
        {
            if (incrementNumber > 0)
            {
                if (currentIndex + incrementNumber > maxLength)
                {
                    currentIndex = 0;
                }
                else
                {
                    currentIndex += incrementNumber;
                }
            }
            else if (incrementNumber < 0)
            {
                if (currentIndex + incrementNumber < 0)
                {
                    currentIndex = maxLength;
                }
                else
                {
                    currentIndex += incrementNumber;
                }
            }

            return currentIndex;
        }
    }
}

