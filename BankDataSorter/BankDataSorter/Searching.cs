using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankDataSorter
{
    struct SearchingStatistics
    {
        private string _search;
        private float _value;
        private int _comparisons;
        private int _itterations;

        public SearchingStatistics(string search, float value, int comparisons, int itterations)
        {
            _search = search;
            _value = value;
            _comparisons = comparisons;
            _itterations = itterations;
        }

        /// <summary>
        /// Gets the searching algorithm which provided these statistics.
        /// </summary>
        public string Search
        {
            get
            {
                return _search;
            }
        }

        /// <summary>
        /// Gets the value that was searched for.
        /// </summary>
        public float Value
        {
            get
            {
                return _value;
            }
        }

        /// <summary>
        /// Gets the amount of comparisons in the last search.
        /// </summary>
        public int Comparisons
        {
            get
            {
                return _comparisons;
            }
        }

        /// <summary>
        /// Gets the amount of comparisons in the last sort.
        /// </summary>
        public int Itterations
        {
            get
            {
                return _itterations;
            }
        }
    }

    class Searching
    {
        #region Singleton
        private static Searching instance = null;

        private Searching()
        {

        }

        /// <summary>
        /// Returns the singleton instance of the sorting class.
        /// </summary>
        public static Searching Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Searching();
                }
                return instance;
            }
        }
        #endregion

        #region Constants
        public const int NOT_FOUND = -1;

        #region Searching
        private static readonly string SEARCH_ALGORITHMS = @"Searching Algoritms:
    0 - Linear Search           - O(n)
    1 - Binary Search           - O(log(n))
    2 - Interpolation Search    - O(n)          - Warning: Bounds search and Nearest search is unimplemented.";

        private static readonly string SEARCH_MESSAGE = @"Input the value to search for: ";

        public const int LINEAR_SEARCH = 0,
                            BINARY_SEARCH = 1,
                            INTERP_SEARCH = 2;

        private static readonly int[] SEARCH_ALG_I = new int[] { LINEAR_SEARCH, BINARY_SEARCH, INTERP_SEARCH };
        #endregion

        #region SearchType
        private static readonly string SEARCH_TYPE = @"Searching Algoritms:
    0 - First Found
    1 - Bounds
    2 - Nearest";

        public const int FIRST_FOUND = 0,
                            BOUNDS = 1,
                            NEAREST = 2;

        private static readonly int[] SEARCH_TYPE_I = new int[] { FIRST_FOUND, BOUNDS, NEAREST };
        #endregion
        #endregion

        #region Variables
        private string search = "";
        private float value = 0.0f;
        private int comparisons = 0;
        private int itterations = 0;

        public SearchingStatistics GetStatistics
        {
            get
            {
                return new SearchingStatistics(search, value, comparisons, itterations);
            }
        }

        /// <summary>
        /// Gets the searching algorithm used in the last search.
        /// </summary>
        public string Search
        {
            get
            {
                return search;
            }
        }

        /// <summary>
        /// Gets the value that was searched for last.
        /// </summary>
        public float Value
        {
            get
            {
                return value;
            }
        }

        /// <summary>
        /// Gets the amount of comparisons in the last search.
        /// </summary>
        public int Comparisons
        {
            get
            {
                return comparisons;
            }
        }

        /// <summary>
        /// Gets the amount of comparisons in the last sort.
        /// </summary>
        public int Itterations
        {
            get
            {
                return itterations;
            }
        }

        /// <summary>
        /// Resets the analytics counters of comparisons and loops back to 0.
        /// </summary>
        public void ResetCounters()
        {
            comparisons = 0;
            itterations = 0;
        }
        #endregion

        public static int[] SearchArray(float[] source)
        {
            int choiseSearcher = Utils.InputLoop(SEARCH_ALGORITHMS, SEARCH_ALG_I);
            Console.WriteLine();
            int choiceSearchType = Utils.InputLoop(SEARCH_TYPE, SEARCH_TYPE_I);
            Console.WriteLine();
            float valueToFind = Utils.InputLoopFloat(SEARCH_MESSAGE);

            switch (choiseSearcher)
            {
                case LINEAR_SEARCH:
                    switch (choiceSearchType)
                    {
                        case FIRST_FOUND:
                            return new int[] { LinearSearch(source, valueToFind) };
                        case BOUNDS:
                            return LinearSearchBounds(source, valueToFind);
                        case NEAREST:
                            return new int[] { LinearSearchNearest(source, valueToFind) };
                        default:
                            return new int[] { NOT_FOUND };
                    }
                case BINARY_SEARCH:
                    switch (choiceSearchType)
                    {
                        case FIRST_FOUND:
                            return new int[] { BinarySearch(source, valueToFind) };
                        case BOUNDS:
                            return BinarySearchBounds(source, valueToFind);
                        case NEAREST:
                            return new int[] { BinarySearchNearest(source, valueToFind) };
                        default:
                            return new int[] { NOT_FOUND };
                    }
                case INTERP_SEARCH:
                    switch (choiceSearchType)
                    {
                        case FIRST_FOUND:
                            return new int[] { InterpSearch(source, valueToFind) };
                        case BOUNDS:
                            return InterpSearchBounds(source, valueToFind);
                        case NEAREST:
                            return new int[] { InterpSearchNearest(source, valueToFind) };
                        default:
                            return new int[] { NOT_FOUND };
                    }
                default:
                    return new int[] { NOT_FOUND };
            }
        }

        #region LinearSearch
        public static int LinearSearch(float[] source, float value)
        {
            Instance.search = "Linear Search";
            Instance.value = value;

            for (int i = 0; i < source.Length; i++)
            {
                Instance.itterations++;
                Instance.comparisons++;

                if (source[i] == value) { return i; }
            }

            return NOT_FOUND;
        }

        public static int[] LinearSearchBounds(float[] source, float value)
        {
            Instance.search = "Linear Search Bounds";
            Instance.value = value;

            int lowerBound = NOT_FOUND;
            for (int i = 0; i < source.Length; i++)
            {
                Instance.itterations++;
                Instance.comparisons++;

                if (source[i] == value)
                {
                    lowerBound = i;
                    break;
                }
            }

            if (lowerBound == NOT_FOUND) { return new int[] { NOT_FOUND, NOT_FOUND }; }

            int upperBound = lowerBound + 1;
            for (int i = upperBound; i < source.Length; i++)
            {
                Instance.itterations++;
                Instance.comparisons++;

                if (source[i] != value)
                {
                    upperBound = i - 1;
                    break;
                }
            }

            return new int[] { lowerBound, upperBound };
        }

        public static int LinearSearchNearest(float[] source, float value)
        {
            Instance.search = "Linear Search Nearest";
            Instance.value = value;

            if (source[0] == value) { return 0; }

            int nearestIndex = 0;
            float nearest = Math.Abs(value - source[0]);
            for (int i = 1; i < source.Length; i++)
            {
                Instance.itterations++;
                Instance.comparisons++;

                if (source[i] == value) { return i; }

                Instance.comparisons++;

                float newNearest = Math.Abs(value - source[i]);
                if (newNearest < nearest)
                {
                    nearestIndex = i;
                    nearest = newNearest;
                }
            }

            return nearestIndex;
        }
        #endregion

        #region BinarySearch
        public static int BinarySearch(float[] source, float value)
        {
            Instance.search = "Binary Search";
            Instance.value = value;

            int low = 0, high = source.Length - 1;
            while (low <= high)
            {
                Instance.itterations++;
                Instance.comparisons++;

                int mid = (low + high) / 2;
                if (source[mid] > value)
                    high = mid - 1;
                else if (source[mid] < value)
                {
                    Instance.comparisons++;
                    low = mid + 1;
                }
                else
                    return mid;
            }

            return NOT_FOUND;
        }

        public static int BinarySearchNearest(float[] source, float value)
        {
            Instance.search = "Binary Search Nearest";
            Instance.value = value;

            int low = 0, high = source.Length - 1;
            while (low <= high)
            {
                Instance.itterations++;
                Instance.comparisons++;

                int mid = (low + high) / 2;
                if (source[mid] > value)
                    high = mid - 1;
                else if (source[mid] < value)
                {
                    Instance.comparisons++;
                    low = mid + 1;
                }
                else
                    return mid;
            }

            if (low >= source.Length) { return high; }
            return source[low] - value < value - source[high] ? low : high;
        }

        public static int[] BinarySearchBounds(float[] source, float value)
        {
            Instance.search = "Binary Search Bounds";
            Instance.value = value;

            int low = 0, high = source.Length - 1;
            int lowBound = BinarySearchLow(source, value, low, high);

            if (lowBound == NOT_FOUND) return new int[] { NOT_FOUND, NOT_FOUND };

            int upperBound = BinarySearchHigh(source, value, lowBound, high);

            int[] indexs = new int[] { lowBound, upperBound };

            return indexs;
        }

        private static int BinarySearchLow(float[] source, float value, int low, int high)
        {
            while (low <= high)
            {
                Instance.itterations++;
                Instance.comparisons++;

                int mid = (low + high) / 2;
                if (source[mid] >= value)
                    high = mid - 1;
                else if (source[mid] < value)
                {
                    Instance.comparisons++;
                    low = mid + 1;
                }
                else
                    return mid;
            }

            return source[low] == value ? low : NOT_FOUND;
        }

        private static int BinarySearchHigh(float[] source, float value, int low, int high)
        {
            while (low <= high)
            {
                Instance.itterations++;
                Instance.comparisons++;

                int mid = (low + high + 1) / 2;
                if (source[mid] > value)
                    high = mid - 1;
                else if (source[mid] <= value)
                {
                    Instance.comparisons++;
                    low = mid + 1;
                }
            }

            return source[low] == value ? low : high;
        }
        #endregion

        #region InterpSearch
        public static int InterpSearch(float[] source, float value)
        {
            int low = 0, high = source.Length - 1, mid;
            while (source[low] != source[high] && (value >= source[low]) && (value <= source[high]))
            {
                mid = (int)(low + ((value - source[low]) * (high - low) / (source[high] - source[low])));

                if (value > source[mid])
                    low = mid + 1;
                else if (value < source[mid])
                    high = mid - 1;
                else
                    return mid;
            }

            if (value == source[low]) { return low; }

            return NOT_FOUND;
        }

        public static int InterpSearchNearest(float[] source, float value)
        {
            int low = 0, high = source.Length - 1, mid;
            while (source[low] != source[high] && (value >= source[low]) && (value <= source[high]))
            {
                mid = (int)(low + ((value - source[low]) * (high - low) / (source[high] - source[low])));

                if (value > source[mid])
                    low = mid + 1;
                else if (value < source[mid])
                    high = mid - 1;
                else
                    return mid;
            }

            return source[low] - value < value - source[high] ? low : high;
        }

        public static int[] InterpSearchBounds(float[] source, float value)
        {
            int low = 0, high = source.Length - 1;
            int lowBound = InterpSearchLow(source, value, low, high);

            if (lowBound == NOT_FOUND) return new int[] { NOT_FOUND, NOT_FOUND };

            int upperBound = InterpSearchHigh(source, value, lowBound, high);

            int bound = upperBound - lowBound + 1;
            int[] indexs = new int[] { lowBound, upperBound };

            return indexs;
        }

        private static int InterpSearchLow(float[] source, float value, int low, int high)
        {
            int mid;
            while (source[low] != source[high] && (value >= source[low]) && (value <= source[high]))
            {
                mid = (int)(low + ((value - source[low]) * (high - low) / (source[high] - source[low])));

                if (value > source[mid])
                    low = mid + 1;
                else if (value <= source[mid])
                    high = mid - 1;
                else
                    return mid;
            }

            if (value == source[low]) { return low; }

            return NOT_FOUND;
        }

        private static int InterpSearchHigh(float[] source, float value, int low, int high)
        {
            int mid;
            while (low != high && (value >= source[low]) && (value <= source[high]))
            {
                mid = (int)(low + ((value - source[low]) * (high - low) / (source[high] - source[low])));

                if (value > source[mid])
                    low = mid + 1;
                else if (value < source[mid])
                    high = mid - 1;
                else
                    return mid;
            }

            if (value == source[low]) { return low; }

            return NOT_FOUND;
        }
        #endregion
    }
}
