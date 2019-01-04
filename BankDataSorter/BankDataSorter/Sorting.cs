using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankDataSorter
{
    public struct SortingStatistics
    {
        private string _sort;
        private string _order;
        private ulong _comparisons;
        private ulong _itterations;
        private ulong _recursions;
        private ulong _swaps;

        public SortingStatistics(string sort, string order, ulong comparisons, ulong itterations, ulong recursions, ulong swaps)
        {
            _sort = sort;
            _order = order;
            _comparisons = comparisons;
            _itterations = itterations;
            _recursions = recursions;
            _swaps = swaps;
        }

        /// <summary>
        /// Gets the name of the sorting algoritm used.
        /// </summary>
        public string Sort
        {
            get
            {
                return _sort;
            }
        }

        /// <summary>
        /// Gets the order the values where sorted into.
        /// </summary>
        public string Order
        {
            get
            {
                return _order;
            }
        }

        /// <summary>
        /// Gets the amount of comparisons in the last sort.
        /// </summary>
        public ulong Comparisons
        {
            get
            {
                return _comparisons;
            }
        }

        /// <summary>
        /// Gets the amount of comparisons in the last sort.
        /// </summary>
        public ulong Itterations
        {
            get
            {
                return _itterations;
            }
        }

        public ulong Recursions
        {
            get
            {
                return _recursions;
            }
        }

        /// <summary>
        /// Gets the amount of swaps in the last sort.
        /// </summary>
        public ulong Swaps
        {
            get
            {
                return _swaps;
            }
        }
    }

    class Sorting
    {
        #region Singleton
        private static Sorting instance = null;

        private Sorting()
        {

        }

        /// <summary>
        /// Returns the singleton instance of the sorting class.
        /// </summary>
        public static Sorting Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Sorting();
                }
                return instance;
            }
        }
        #endregion

        #region Constants
        #region Sorting
        private static readonly string SORTING_ALGORITHMNS = @"Sorthing Algorithms:
    0 - Merge Sort  - O(n log(n))
    1 - Quick Sort  - O(n log(n))
    2 - Bubble Sort - O(n^2)
    3 - Heap Sort   - O(n log(n))
    4 - Bogo Sort   - O(n!)             - Warning chance of [near / or] infinite loop possible.
    5 - Slow Sort   - O(n ^ log(n))
    6 - Mathmatica  - O(n * n!)         - Warning do not use on large data sets.";

        public const int MERGE_SORT = 0,
                            QUICK_SORT = 1,
                            BUBBLE_SORT = 2,
                            HEAP_SORT = 3,
                            BOGO_SORT = 4,
                            SLOW_SORT = 5,
                            MATHMATICA_SORT = 6;

        private static readonly int[] SORTING_ALG_I = new int[] { MERGE_SORT, QUICK_SORT, BUBBLE_SORT, HEAP_SORT, BOGO_SORT, SLOW_SORT, MATHMATICA_SORT };
        #endregion

        #region Order
        private static readonly string SORTING_ORDER = @"Searching Algoritms:
    0 - Decending Order
    1 - Accending Order";

        public const int ASCENDING_ORDER = 0,
                            DESCENDING_ORDER = 1;

        private static readonly int[] SORTING_ORDER_I = new int[] { ASCENDING_ORDER, DESCENDING_ORDER };
        #endregion
        #endregion

        #region Variables
        private string sort;
        private string order;
        private ulong comparisons = 0ul;
        private ulong itterations = 0ul;
        private ulong recursions = 0ul;
        private ulong swaps = 0ul;

        /// <summary>
        /// Gets all of the statistics from the last sort.
        /// </summary>
        public SortingStatistics GetStatistics
        {
            get
            {
                return new SortingStatistics(sort, order, comparisons, itterations, recursions, swaps);
            }
        }

        /// <summary>
        /// Gets the name of the last sorting algorithm used.
        /// </summary>
        public string Sort
        {
            get
            {
                return sort;
            }
        }

        /// <summary>
        /// Gets the order the last sorting algorithm followed.
        /// </summary>
        public string Order
        {
            get
            {
                return order;
            }
        }

        /// <summary>
        /// Gets the amount of comparisons in the last sort.
        /// </summary>
        public ulong Comparisons
        {
            get
            {
                return comparisons;
            }
        }

        /// <summary>
        /// Gets the amount of comparisons in the last sort.
        /// </summary>
        public ulong Itterations
        {
            get
            {
                return itterations;
            }
        }

        public ulong Recursions
        {
            get
            {
                return recursions;
            }
        }

        /// <summary>
        /// Gets the amount of swaps in the last sort.
        /// </summary>
        public ulong Swaps
        {
            get
            {
                return swaps;
            }
        }

        /// <summary>
        /// Resets the analytics counters of comparisons, loops and swap back to 0.
        /// </summary>
        public void ResetCounters()
        {
            comparisons = 0ul;
            itterations = 0ul;
            recursions = 0ul;
            swaps = 0ul;
        }
        #endregion

        /// <summary>
        /// Sorts the array according using the user selected sorting algorithm and order.
        /// </summary>
        /// <param name="source">The Array to be sorted.</param>
        public static void SortArray(float[] source)
        {
            if (source.Length < 2) { return; }

            Instance.ResetCounters();

            int choiceAlg = Utils.InputLoop(SORTING_ALGORITHMNS, SORTING_ALG_I);
            Console.WriteLine();
            int choiceCompare = Utils.InputLoop(SORTING_ORDER, SORTING_ORDER_I);

            Func<float, float, bool> Comparison;
            switch (choiceCompare)
            {
                case ASCENDING_ORDER:
                    Comparison = Descending;
                    break;
                case DESCENDING_ORDER:
                    Comparison = Ascending;
                    break;
                default:
                    Comparison = Descending;
                    break;
            }

            switch (choiceAlg)
            {
                case MERGE_SORT:
                    MergeSort(source, Comparison);
                    break;
                case QUICK_SORT:
                    QuickSort(source, Comparison);
                    break;
                case BUBBLE_SORT:
                    BubbleSort(source, Comparison);
                    break;
                case HEAP_SORT:
                    HeapSort(source, Comparison);
                    break;
                case BOGO_SORT:
                    BogoSort(source, Comparison);
                    break;
                case SLOW_SORT:
                    SlowSort(source, Comparison);
                    break;
                case MATHMATICA_SORT:
                    MathMaticaSort(source, Comparison);
                    break;
                default:
                    MergeSort(source, Comparison);
                    break;
            }
        }

        #region MergeSort
        // Made from the Merge sort pseudo code.
        public static void MergeSort(float[] source, Func<float, float, bool> Comparison)
        {
            Instance.sort = "Merge Sort";

            if (Comparison == Ascending) { Instance.order = "Ascending"; }
            else { Instance.order = "Descending"; }

            if (source.Length < 2) { return; }

            float[] worker = new float[source.Length];
            Utils.CopyArray(source, worker, source.Length);

            // Each 1-element run in A is already "sorted".
            // Make successively longer sorted runs of length 2, 4, 8, 16... until whole array is sorted.
            for (int width = 1; width < source.Length; width = 2 * width)
            {
                Instance.itterations++;

                // Array A is full of runs of length width.
                for (int i = 0; i < source.Length; i = i + 2 * width)
                {
                    Instance.itterations++;

                    // Merge two runs: A[i:i+width-1] and A[i+width:i+2*width-1] to B[]
                    // or copy A[i:n-1] to B[] ( if(i+width >= n) )
                    BottomUpMerge(source, i, Math.Min(i + width, source.Length), Math.Min(i + 2 * width, source.Length), worker, Comparison);
                }
                // Now work array B is full of runs of length 2*width.
                // Copy array B to array A for next iteration.
                // A more efficient implementation would swap the roles of A and B.
                Utils.CopyArray(worker, source, source.Length);
                // Now array A is full of runs of length 2*width.
            }
        }

        //  Left run is source[iLeft :iRight-1].
        // Right run is source[iRight:iEnd-1  ].
        static void BottomUpMerge(float[] source, int iLeft, int iRight, int iEnd, float[] output, Func<float, float, bool> Comparison)
        {
            int i = iLeft, j = iRight;
            // While there are elements in the left or right runs...
            for (int k = iLeft; k < iEnd; k++)
            {
                Instance.itterations++;

                // If left run head exists and is <= existing right run head.
                if (i < iRight && (j >= iEnd || Comparison(source[i], source[j])))
                {
                    output[k] = source[i];
                    i = i + 1;
                }
                else
                {
                    output[k] = source[j];
                    j = j + 1;
                }

                Instance.swaps++;
            }
        }
        #endregion

        #region QuickSort
        // Based of off original Pseudo Code.
        public static void QuickSort(float[] source, Func<float, float, bool> Comparison)
        {
            Instance.sort = "Quick Sort";

            if (Comparison == Ascending) { Instance.order = "Ascending"; }
            else { Instance.order = "Descending"; }

            int low = 0, high = source.Length - 1;
            QuickSort(source, low, high, Comparison);
        }

        private static void QuickSort(float[] source, int low, int high, Func<float, float, bool> Comparison)
        {
            Instance.recursions++;

            if (low < high)
            {
                int p = Partition(source, low, high, Comparison);
                QuickSort(source, low, p, Comparison);
                QuickSort(source, p + 1, high, Comparison);
            }
        }

        /*
        public static void ParallelQuickSort(float[] source, Func<float, float, bool> Comparison)
        {
            int low = 0, high = source.Length - 1;
            ParallelQuickSort(source, low, high, Comparison);
        }

        private static void ParallelQuickSort(float[] source, int low, int high, Func<float, float, bool> Comparison)
        {
            if (low < high)
            {
                int p = Partition(source, low, high, Comparison);

                Parallel.Invoke(() => ParallelQuickSort(source, low, p, Comparison),
                                () => ParallelQuickSort(source, p + 1, high, Comparison));
            }
        }
        */

        private static int Partition(float[] source, int low, int high, Func<float, float, bool> Comparison)
        {
            float pivot = source[low];
            int i = low, j = high;
            while (true)
            {
                Instance.itterations++;

                while (Comparison(source[i], pivot))
                {
                    i++;
                }

                while (Comparison(pivot, source[j]))
                {
                    j--;
                }

                Instance.comparisons++;
                if (i >= j) return j;

                Swap(source, i, j);

                Instance.comparisons++;
                if ((source[i] == pivot) && (source[j] == pivot)) { j--; }
            }
        }
        #endregion

        #region BubbleSort
        // Based of off pseudo code.
        public static void BubbleSort(float[] source, Func<float, float, bool> Comparison)
        {
            Instance.sort = "Bubble Sort";

            if (Comparison == Ascending) { Instance.order = "Ascending"; }
            else { Instance.order = "Descending"; }

            int n = source.Length;
            if (n < 2) { return; }

            bool swapped = true;
            while (swapped)
            {
                Instance.itterations++;

                swapped = false;
                for (int i = 1; i < n; i++)
                {
                    int index = i - 1;
                    if (!Comparison(source[index], source[i]))
                    {
                        Swap(source, index, i);
                        swapped = true;
                    }
                }

                n--;
            }
        }
        #endregion

        #region HeapSort
        // Based of off https://www.geeksforgeeks.org/heap-sort/
        // To heapify a subtree rooted with node i which is
        // an index in arr[]. n is size of heap
        private static void Heapify(float[] source, int n, int i, Func<float, float, bool> Comparison)
        {
            Instance.recursions++;

            int largest = i;  // Initialize largest as root
            int l = (i << 1) + 1;  // left = 2*i + 1
            int r = (i << 1) + 2;  // right = 2*i + 2

            // If left child is larger than root
            if (l < n && !Comparison(source[l], source[largest]))
                largest = l;

            // If right child is larger than largest so far
            if (r < n && !Comparison(source[r], source[largest]))
                largest = r;

            // If largest is not root
            if (largest != i)
            {
                Swap(source, i, largest);

                // Recursively heapify the affected sub-tree
                Heapify(source, n, largest, Comparison);
            }
        }

        // main function to do heap sort
        public static void HeapSort(float[] source, Func<float, float, bool> Comparison)
        {
            Instance.sort = "Heap Sort";

            if (Comparison == Ascending) { Instance.order = "Ascending"; }
            else { Instance.order = "Descending"; }

            int n = source.Length;
            // Build heap (rearrange array)
            for (int i = n / 2 - 1; i >= 0; i--)
            {
                Instance.itterations++;

                Heapify(source, n, i, Comparison);
            }

            // One by one extract an element from heap
            for (int i = n - 1; i >= 0; i--)
            {
                Instance.itterations++;

                // Move current root to end
                Swap(source, 0, i);

                // call max heapify on the reduced heap
                Heapify(source, i, 0, Comparison);
            }
        }
        #endregion

        #region SlowSort
        public static void SlowSort(float[] source, Func<float, float, bool> Comparison)
        {
            Instance.sort = "Slow Sort";

            if (Comparison == Ascending) { Instance.order = "Ascending"; }
            else { Instance.order = "Descending"; }

            if (source.Length < 2) { return; }

            SlowSort(source, 0, source.Length - 1, Comparison);
        }

        private static void SlowSort(float[] source, int i, int j, Func<float, float, bool> Comparison)
        {
            Instance.recursions++;

            if (i >= j) { return; }

            //int m = (i + j) / 2;
            int m = (i + j) >> 1;
            SlowSort(source, i, m, Comparison);
            SlowSort(source, m + 1, j, Comparison);
            if (Comparison(source[j], source[m]))
            {
                Swap(source, j, m);
            }

            SlowSort(source, i, j - 1, Comparison);
        }
        #endregion

        #region BogoSort
        public static void BogoSort(float[] source, Func<float, float, bool> Comparison)
        {
            Instance.sort = "Bogo Sort";

            if (Comparison == Ascending) { Instance.order = "Ascending"; }
            else { Instance.order = "Descending"; }

            if (source.Length < 2) { return; }

            while (!IsSorted(source, Comparison))
            {
                Instance.itterations++;

                BogoShuffle(source);
            }
        }

        private static void BogoShuffle(float[] source)
        {
            Random random = Utils.Random;
            for (int i = 0; i < source.Length; i++)
            {
                Instance.itterations++;

                Swap(source, i, random.Next(i, source.Length));
            }
        }
        #endregion

        #region MathmaticaSort
        public static void MathMaticaSort(float[] source, Func<float, float, bool> Comparison)
        {
            Instance.sort = "Mathmatica Sort";

            if (Comparison == Ascending) { Instance.order = "Ascending"; }
            else { Instance.order = "Descending"; }

            Permute(source, 0, source.Length - 1, Comparison);
        }

        private static bool Permute(float[] source, int recursionDepth, int maxDepth, Func<float, float, bool> Comparison)
        {
            Instance.recursions++;

            if (recursionDepth == maxDepth)
            {
                if (IsSorted(source, Comparison)) { return true; }
                return false;
            }

            for (int i = recursionDepth; i <= maxDepth; i++)
            {
                Swap(source ,recursionDepth, i);
                if (Permute(source, recursionDepth + 1, maxDepth, Comparison)) { return true; }
                Swap(source, recursionDepth, i);
            }

            return false;
        }
        #endregion

        #region Invert
        public static void Invert<T>(T[] source)
        {
            int low = 0, high = source.Length;
            if (high < 2) { return; }

            while (low != high)
            {
                Swap(source, low, high);
                low++;
                if (low == high) { return; }
                high++;
            }
        }
        #endregion

        #region Utils
        public static bool Ascending(float x, float y)
        {
            Instance.comparisons++;
            return x < y ? true : false;
        }

        public static bool Descending(float x, float y)
        {
            Instance.comparisons++;
            return x > y ? true : false;
        }

        private static void Swap<T>(T[] source, int x, int y)
        {
            Instance.swaps++;
            T temp = source[x];
            source[x] = source[y];
            source[y] = temp;
        }

        public static bool IsSorted(float[] source, Func<float, float, bool> Comparison)
        {
            int n = source.Length;
            for (int i = 1; i < n; i++)
            {
                if (Comparison(source[i - 1], source[i]) == false || source[i - 1] == source[i]) { return false; }
            }

            return true;
        }
        #endregion
    }
}
