using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SumToTen
{
    /// <summary>
    /// The mission of this interview question is, given an unsorted array of integers, 
    /// return the indices of the first pair of integers which sum to 10. 
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            int[] testArray = new int[] { 1, 7, 5, 3, 9 };
            int[] testArray2 = new int[] { 4, 5, 2, 3, 1, 7, 8, 2, 1, 1, 1, 1, 1, 1, 6 };

            Console.WriteLine("Test array 1: [1 7 5 3 9]");
            Console.WriteLine("Test array 2: [4 5 2 3 1 7 8 2 1 1 1 1 1 1 6]");

            var answer = SumToTen_Slow(testArray, testArray.Length);

            if (answer[0] >= 0 && answer[1] >= 0)
                Console.WriteLine("[SLOW][TA1] The values at indices {0} and {1} are the first pair that sum to 10", answer[0], answer[1]);

            answer = SumToTen_Slow(testArray2, testArray2.Length);

            if (answer[0] >= 0 && answer[1] >= 0)
                Console.WriteLine("[SLOW][TA2] The values at indices {0} and {1} are the first pair that sum to 10", answer[0], answer[1]);

            answer = SumToTen_Fast(testArray, testArray.Length);

            if (answer[0] >= 0 && answer[1] >= 0)
                Console.WriteLine("[FAST][TA1] The values at indices {0} and {1} are the first pair that sum to 10", answer[0], answer[1]);

            answer = SumToTen_Fast(testArray2, testArray2.Length);

            if (answer[0] >= 0 && answer[1] >= 0)
                Console.WriteLine("[FAST][TA2] The values at indices {0} and {1} are the first pair that sum to 10", answer[0], answer[1]);

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey(); 
        }

        /// <summary>
        /// This slow implementation takes O(n^2) time. 
        /// </summary>
        /// <param name="A"></param>
        /// <returns></returns>
        public static int[] SumToTen_Slow(int[] A, int size)
        {
            int i = 0, j = 1;
            int leftIndex = -1, rightIndex = -1;
            int[] returnArray = new int[2];


            while (i < size)
            {
                j = i + 1;

                while (j < size)
                {
                    if (A[i] + A[j] == 10)       // We've found a pair of elements that sum to 10 
                    {
                        if (leftIndex < 0 || rightIndex < 0)
                        {
                            // leftIndex, rightIndex are uninitialized, meaning we haven't found a pair yet
                            leftIndex = i;
                            rightIndex = j;
                        }
                        else if (j < rightIndex)
                        {
                            // Update the indices since the new pair we found comes before the old pair 
                            leftIndex = i;
                            rightIndex = j;
                        }
                    }

                    j++;
                }

                i++;
            }


            returnArray[0] = leftIndex;
            returnArray[1] = rightIndex;

            return returnArray;
        }

        /// <summary>
        /// This faster implementation performs binary search on the array to 
        /// find a pair of elements which sum to 10, then returns the "first pair"
        /// meaning the pair that ends with the lowest array index.
        /// The time complexity of this function is O(n^2 logn)
        /// </summary>
        /// <param name="A"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static int[] SumToTen_Fast(int[] A, int size)
        {
            int[] returnArray = new int[2];
            int leftIndex = -1, rightIndex = -1;
            int origLeftIndex = -1, origRightIndex = -1;        // Used to keep track of the indices from the original array
            var validPairs = new Dictionary<int, int>();        // Used to keep track of valid pairs of numbers we've found 
            int i = 0;

            // Make a copy of A, then sort the copied array so we can use Array.BinarySearch below
            // Assume sorting the array is O(n logn)
            // Don't forget the cost of the array copy, a memory cost of N
            var sortedArray = new int[size];
            Array.Copy(A, sortedArray, A.Length);
            Array.Sort(sortedArray); 


            // This loop will iterate n times, so its total cost is O(n^2 logn)
            for (i = 0; i < size; i++)
            {
                // Get the index of the other number in the array such that A[i] + A[pairingNumber] = 10
                // Assume binary search costs O(n logn)
                var searchValue = 10 - sortedArray[i]; 
                var pairingNumber = Array.BinarySearch(sortedArray, searchValue);

                if (pairingNumber > 0 && i != pairingNumber)
                {
                    if (leftIndex < 0 || rightIndex < 0)        
                    {
                        // leftIndex, rightIndex haven't been initiated yet indicating we haven't found any pair yet
                        leftIndex = i;
                        rightIndex = pairingNumber;
                    }
                    else if (pairingNumber < rightIndex)
                    {
                        leftIndex = i;
                        rightIndex = pairingNumber;
                    }

                    if (!validPairs.ContainsKey(leftIndex) && !validPairs.ContainsKey(rightIndex))
                        validPairs.Add(leftIndex, rightIndex); 

                    
                }
            }

            // Now we have to find the indices of the pair of elements from the original array
            // We'll use our validPairs dictionary and determine the dictionary who's key value 
            // (which represents the right most index) is lowest. That will be our final answer. 
            int finalKey = 0;
            int lowestRightIndex = A.Length;
            foreach (var key in validPairs.Keys)
            {
                // Ignore duplicate entries (i.e., (1,3) = (3,1))
                if (key > validPairs[key])
                    break;

                // Get the indices for the pair in the original array 
                origRightIndex = A.ToList<int>().FindIndex(x => x == sortedArray[key]);
                origLeftIndex = A.ToList<int>().FindIndex(x => x == sortedArray[validPairs[key]]);

                var lowestIndex = Math.Min(origLeftIndex, origRightIndex);
                var highestIndex = Math.Max(origLeftIndex, origRightIndex);

                if(highestIndex < lowestRightIndex)
                {
                    lowestRightIndex = highestIndex;
                    finalKey = lowestIndex;

                }

            }
            
            returnArray[0] = finalKey;
            returnArray[1] = lowestRightIndex;

            return returnArray; 
        }



    }
}
