using System;
using System.Collections.Generic;
using System.IO;

namespace BankDataSorter
{
    class Program
    {
        #region Constants
        private static readonly string LOAD_DIFFERENT_FILES_SELECTION = @"Would you like to compare any other data sets:
    0 - Yes
    1 - No";

        #region FileLoading
        private static readonly string FILE_LOADING_SELECTION = @"Include this file:
    0 - Single File
    1 - Multiple Files
    2 - Directory";

        public const int SINGLE_FILE = 0,
                            MULTIPLE_FILES = 1,
                            DIRECTORY = 2;

        private static readonly int[] FILE_LOADING_I = new int[] { SINGLE_FILE, MULTIPLE_FILES, DIRECTORY };

        #region SingleFile
        private static readonly string SINGLE_FILE_MESSAGE = @"Please input the path to a file: ";
        #endregion

        #region MultipleFile
        private static readonly string MULTIPLE_FILE_MESSAGE = @"Please input the path to file {0}: ";
        #endregion

        #region Directroy
        private static readonly string DIRECTORY_MESSAGE = @"Please input the path to the directory constaining the files to read: ";
        #endregion
        #endregion

        #region SortingCompare
        private static readonly string SORTING_COMPARE_SELECTION = @"Would you like to compare multiple sorting algoritms performance on the passed data:
    0 - Yes
    1 - No";

        private static readonly string SORTING_AGAIN_SELECTION = @"Would you like to compare another sorting algorithm:
    0 - Yes
    1 - No";
        #endregion

        #region SearchingCompare
        private static readonly string SEARCHING_COMPARE_SELECTION = @"Would you like to compare multiple searching algoritms performance on the passed data:
    0 - Yes
    1 - No";

        private static readonly string SEARCHING_AGAIN_SELECTION = @"Would you like to compare another searching algorithm:
    0 - Yes
    1 - No";
        #endregion

        #region Searching
        private static readonly string KEEP_SEARCHING_SELECTION = @"Would you like to search for another value:
    0 - Yes
    1 - No";
        #endregion

        #region ArrayPrinting
        private static readonly string ARRAY_PRINTING_SELECTION = @"Would you like to print the array with all elements or grouped:
    0 - Normal
    1 - Grouped";

        public const int    NORMAL = 0,
                            GROUPED = 1;

        private static readonly int[] ARRAY_PRINTING_I = new int[] { NORMAL, GROUPED };
        #endregion
        #endregion

        #region Variables
        static int arrayPrintingChoice = 0;
        #endregion

        static void Main(string[] args)
        {
            bool isInNoneStandardMode = false;
            for (int i = 0; i < args.Length; i++)
            {
                switch (args[i])
                {
                    case "-benchmark":
                        isInNoneStandardMode = true;
                        Benchmark();
                        break;
                    case "-testSearching":
                        isInNoneStandardMode = true;
                        TestSearching();
                        break;
                    case "-testSlowSort":
                        isInNoneStandardMode = true;
                        TestSlowSort();
                        break;
                    case "-testSorting":
                        isInNoneStandardMode = true;
                        TestSorting();
                        break;
                    case "-testSorting2":
                        isInNoneStandardMode = true;
                        TestSorting2();
                        break;
                }
            }

            if (isInNoneStandardMode == false)
            {
                while (true)
                {
                    string[] files = GetFiles();
                    for (int i = 0; i < files.Length; i++)
                    {
                        Console.Clear();
                        Console.WriteLine("Reading File: {0}", Path.GetFileNameWithoutExtension(files[i]));
                        float[] values = Utils.ParseFile(files[i]);
                        if (Utils.InputLoop(@"Would you like to merge multiple files for this search:
    0 - Yes
    1 - No"))
                        {
                            while(true)
                            {
                                List<int> indexs = new List<int>();
                                string message = string.Format("Which files would you like to merge with {0}:", Path.GetFileNameWithoutExtension(files[i]));
                                for (int x = i + 1; x < files.Length; x++)
                                {
                                    indexs.Add(x);
                                    message += string.Format("\n\t{0} - {1}", x, Path.GetFileNameWithoutExtension(files[x]));
                                }
                                int choice = Utils.InputLoop(message, indexs.ToArray());
                                values = Utils.ParseMergeFiles(values, files[choice]);

                                if (!Utils.InputLoop(@"Would you like to merge another file:
    0 - Yes
    1 - No")) { break; }
                            }
                        }
                        
                        Utils.ConsoleHang();
                        Console.Clear();

                        arrayPrintingChoice = Utils.InputLoop(ARRAY_PRINTING_SELECTION, ARRAY_PRINTING_I);

                        #region Sort
                        float[] sortingValues = new float[values.Length];
                        int sortingComparisonsChoice = Utils.InputLoop(SORTING_COMPARE_SELECTION, Utils.YES_NO_I);
                        switch (sortingComparisonsChoice)
                        {
                            case Utils.YES:
                                List<SortingStatistics> sortingStatistics = new List<SortingStatistics>();
                                while (true)
                                {
                                    Utils.CopyArray(values, sortingValues, values.Length);
                                    Sorting.SortArray(sortingValues);
                                    sortingStatistics.Add(Sorting.Instance.GetStatistics);

                                    ArrayPrinting(sortingValues, arrayPrintingChoice, Path.GetFileNameWithoutExtension(files[i]));
                                    Console.WriteLine();
                                    Utils.WriteLastStatisticsSort();

                                    Utils.ConsoleHang();
                                    Console.Clear();

                                    if (!Utils.InputLoop(SORTING_AGAIN_SELECTION))
                                    { break; }
                                }

                                // Compare Sorts
                                foreach (SortingStatistics stats in sortingStatistics)
                                {
                                    Utils.WriteLastStatisticsSort();
                                    Console.WriteLine();
                                }

                                Utils.ConsoleHang();
                                Console.Clear();
                                break;
                            case Utils.NO:
                                Utils.CopyArray(values, sortingValues, values.Length);
                                Sorting.SortArray(sortingValues);

                                ArrayPrinting(sortingValues, arrayPrintingChoice, Path.GetFileNameWithoutExtension(files[i]));
                                Console.WriteLine();
                                Utils.WriteLastStatisticsSort();

                                Utils.ConsoleHang();
                                Console.Clear();
                                break;
                            default:
                                break;
                        }

                        Utils.ConsoleHang();
                        Console.Clear();
                        #endregion

                        #region SearchArray
                        int searchComparisonsChoice = Utils.InputLoop(SEARCHING_COMPARE_SELECTION, Utils.YES_NO_I);
                        switch (searchComparisonsChoice)
                        {
                            case Utils.YES:
                                List<SearchingStatistics> searchingStatistics = new List<SearchingStatistics>();
                                while (true)
                                {
                                    Searching.SearchArray(sortingValues);
                                    Utils.WriteLastStatisticsSearch();

                                    searchingStatistics.Add(Searching.Instance.GetStatistics);

                                    Utils.ConsoleHang();
                                    Console.Clear();

                                    if (Utils.InputLoop(KEEP_SEARCHING_SELECTION))
                                    { break; }
                                }

                                Console.Clear();

                                foreach (SearchingStatistics stats in searchingStatistics)
                                {
                                    Utils.WriteStatisticsSearch(stats);
                                    Console.WriteLine();
                                }

                                Console.Clear();
                                break;
                            case Utils.NO:
                                while (true)
                                {
                                    Searching.SearchArray(sortingValues);
                                    Utils.WriteLastStatisticsSearch();

                                    Utils.ConsoleHang();
                                    Console.Clear();

                                    if (Utils.InputLoop(KEEP_SEARCHING_SELECTION))
                                    { break; }
                                }
                                break;
                            default:
                                break;
                        }

                        Utils.ConsoleHang();
                        Console.Clear();
                        #endregion

                        if (Utils.InputLoop(LOAD_DIFFERENT_FILES_SELECTION)) { break; }
                    }
                }
            }

            // Make the console hang till user input.
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

        public static void Benchmark()
        {
            string[] files = Utils.ParseDirectory(Utils.InputLoopDirPath(DIRECTORY_MESSAGE));
            List<string> fileNamesSort = new List<string>();
            List<string> fileNamesSearch = new List<string>();
            List<int[]> searchIndexs = new List<int[]>();
            List<SortingStatistics> sortingStatistics = new List<SortingStatistics>();
            List<SearchingStatistics> searchingStatistics = new List<SearchingStatistics>();
            for (int i = 0; i < files.Length; i++)
            {
                Console.WriteLine(Path.GetFileName(files[i]));

                float[] values = Utils.ParseFile(files[i]);
                float[] workerValues = new float[values.Length];

                #region Sort
                #region MergeSort
                Console.WriteLine("MergeSort");
                fileNamesSort.Add(Path.GetFileName(files[i]));
                Utils.CopyArray(values, workerValues, values.Length);
                Sorting.Instance.ResetCounters();
                Sorting.MergeSort(workerValues, Sorting.Ascending);
                sortingStatistics.Add(Sorting.Instance.GetStatistics);

                fileNamesSort.Add(Path.GetFileName(files[i]));
                Utils.CopyArray(values, workerValues, values.Length);
                Sorting.Instance.ResetCounters();
                Sorting.MergeSort(workerValues, Sorting.Descending);
                sortingStatistics.Add(Sorting.Instance.GetStatistics);
                #endregion

                #region QuickSort
                Console.WriteLine("QuickSort");
                fileNamesSort.Add(Path.GetFileName(files[i]));
                Utils.CopyArray(values, workerValues, values.Length);
                Sorting.Instance.ResetCounters();
                Sorting.QuickSort(workerValues, Sorting.Ascending);
                sortingStatistics.Add(Sorting.Instance.GetStatistics);

                fileNamesSort.Add(Path.GetFileName(files[i]));
                Utils.CopyArray(values, workerValues, values.Length);
                Sorting.Instance.ResetCounters();
                Sorting.QuickSort(workerValues, Sorting.Descending);
                sortingStatistics.Add(Sorting.Instance.GetStatistics);
                #endregion

                #region BubbleSort
                Console.WriteLine("BubbleSort");
                fileNamesSort.Add(Path.GetFileName(files[i]));
                Utils.CopyArray(values, workerValues, values.Length);
                Sorting.Instance.ResetCounters();
                Sorting.BubbleSort(workerValues, Sorting.Ascending);
                sortingStatistics.Add(Sorting.Instance.GetStatistics);

                fileNamesSort.Add(Path.GetFileName(files[i]));
                Utils.CopyArray(values, workerValues, values.Length);
                Sorting.Instance.ResetCounters();
                Sorting.BubbleSort(workerValues, Sorting.Descending);
                sortingStatistics.Add(Sorting.Instance.GetStatistics);
                #endregion

                #region HeapSort
                Console.WriteLine("HeapSort");
                fileNamesSort.Add(Path.GetFileName(files[i]));
                Utils.CopyArray(values, workerValues, values.Length);
                Sorting.Instance.ResetCounters();
                Sorting.HeapSort(workerValues, Sorting.Ascending);
                sortingStatistics.Add(Sorting.Instance.GetStatistics);

                fileNamesSort.Add(Path.GetFileName(files[i]));
                Utils.CopyArray(values, workerValues, values.Length);
                Sorting.Instance.ResetCounters();
                Sorting.HeapSort(workerValues, Sorting.Descending);
                sortingStatistics.Add(Sorting.Instance.GetStatistics);
                #endregion

                #region SlowSort
                if (workerValues.Length <= 256)
                {
                    Console.WriteLine("SlowSort");
                    fileNamesSort.Add(Path.GetFileName(files[i]));
                    Utils.CopyArray(values, workerValues, values.Length);
                    Sorting.Instance.ResetCounters();
                    Sorting.SlowSort(workerValues, Sorting.Ascending);
                    sortingStatistics.Add(Sorting.Instance.GetStatistics);

                    fileNamesSort.Add(Path.GetFileName(files[i]));
                    Utils.CopyArray(values, workerValues, values.Length);
                    Sorting.Instance.ResetCounters();
                    Sorting.SlowSort(workerValues, Sorting.Descending);
                    sortingStatistics.Add(Sorting.Instance.GetStatistics);
                }
                #endregion

                #region MathMaticaSort
                if (workerValues.Length <= 64)
                {
                    Console.WriteLine("MathMaticaSort");
                    fileNamesSort.Add(Path.GetFileName(files[i]));
                    Utils.CopyArray(values, workerValues, values.Length);
                    Sorting.Instance.ResetCounters();
                    Sorting.MathMaticaSort(workerValues, Sorting.Ascending);
                    sortingStatistics.Add(Sorting.Instance.GetStatistics);

                    fileNamesSort.Add(Path.GetFileName(files[i]));
                    Utils.CopyArray(values, workerValues, values.Length);
                    Sorting.Instance.ResetCounters();
                    Sorting.MathMaticaSort(workerValues, Sorting.Descending);
                    sortingStatistics.Add(Sorting.Instance.GetStatistics);
                }
                #endregion
                #endregion

                #region Search
                Utils.CopyArray(values, workerValues, values.Length);
                Sorting.Instance.ResetCounters();
                Sorting.MergeSort(workerValues, Sorting.Ascending);

                float firstSearchValue = workerValues[Utils.Instance.Random.Next(0, workerValues.Length)];
                float boundSearchValue = Utils.MostCommonValue(workerValues);
                float nearestSearchValue = ((workerValues[Utils.Instance.Random.Next(0, workerValues.Length)] + workerValues[Utils.Instance.Random.Next(0, workerValues.Length)]) / 2) + ((float)Utils.Instance.Random.NextDouble() * workerValues[Utils.Instance.Random.Next(0, workerValues.Length)]);

                #region LinearSearch
                Console.WriteLine("LinearSearch");
                fileNamesSearch.Add(Path.GetFileName(files[i]));
                Searching.Instance.ResetCounters();
                searchIndexs.Add(new int[] { Searching.LinearSearch(workerValues, firstSearchValue) });
                searchingStatistics.Add(Searching.Instance.GetStatistics);

                fileNamesSearch.Add(Path.GetFileName(files[i]));
                Searching.Instance.ResetCounters();
                searchIndexs.Add(Searching.LinearSearchBounds(workerValues, boundSearchValue));
                searchingStatistics.Add(Searching.Instance.GetStatistics);

                fileNamesSearch.Add(Path.GetFileName(files[i]));
                Searching.Instance.ResetCounters();
                searchIndexs.Add(new int[] { Searching.LinearSearchNearest(workerValues, nearestSearchValue) });
                searchingStatistics.Add(Searching.Instance.GetStatistics);
                #endregion

                #region BinarySearch
                Console.WriteLine("BinarySearch");
                fileNamesSearch.Add(Path.GetFileName(files[i]));
                Searching.Instance.ResetCounters();
                searchIndexs.Add(new int[] { Searching.BinarySearch(workerValues, firstSearchValue) });
                searchingStatistics.Add(Searching.Instance.GetStatistics);

                fileNamesSearch.Add(Path.GetFileName(files[i]));
                Searching.Instance.ResetCounters();
                searchIndexs.Add(Searching.BinarySearchBounds(workerValues, boundSearchValue));
                searchingStatistics.Add(Searching.Instance.GetStatistics);

                fileNamesSearch.Add(Path.GetFileName(files[i]));
                Searching.Instance.ResetCounters();
                searchIndexs.Add(new int[] { Searching.BinarySearchNearest(workerValues, nearestSearchValue) });
                searchingStatistics.Add(Searching.Instance.GetStatistics);
                #endregion
                #endregion

                Console.WriteLine();
            }

            int count = 0;
            StreamWriter file = new StreamWriter(@"benchmark.txt");
            foreach (SortingStatistics stats in sortingStatistics)
            {
                file.WriteLine(fileNamesSort[count++]);
                file.WriteLine(stats.Sort);
                file.WriteLine(stats.Order);
                file.WriteLine(stats.Comparisons);
                file.WriteLine(stats.Itterations);
                file.WriteLine(stats.Recursions);
                file.WriteLine(stats.Swaps);
                file.WriteLine("");
            }
            count = 0;
            foreach (SearchingStatistics stats in searchingStatistics)
            {
                file.WriteLine(fileNamesSearch[count]);
                file.WriteLine(stats.Search);
                file.WriteLine(stats.Value);
                file.WriteLine(searchIndexs[count].Length < 2 ? searchIndexs[count][0].ToString() : string.Format("{0}, {1}", searchIndexs[count][0], searchIndexs[count][1]));
                file.WriteLine(stats.Comparisons);
                file.WriteLine(stats.Itterations);
                file.WriteLine("");
                count++;
            }
            file.Close();
        }

        public static void TestSlowSort()
        {
            while (true)
            {
                int size = Utils.InputLoopInt("Input the size of the test array: ");
                float[] source = Utils.RandomArray(size);
                Sorting.SlowSort(source, Sorting.Ascending);
                Utils.WriteArray(source);

                Utils.ConsoleHang();
            }
        }

        public static string[] GetFiles()
        {
            string[] output;
            int choice = Utils.InputLoop(FILE_LOADING_SELECTION, FILE_LOADING_I);
            switch (choice)
            {
                case SINGLE_FILE:
                    output = new string[] { Utils.InputLoopFilePath(SINGLE_FILE_MESSAGE) };
                    break;
                case MULTIPLE_FILES:
                    output = Utils.InputLoopMultiFilePath(MULTIPLE_FILE_MESSAGE);
                    break;
                case DIRECTORY:
                    output = Utils.ParseDirectory(Utils.InputLoopDirPath(DIRECTORY_MESSAGE));
                    break;
                default:
                    output = new string[0];
                    break;
            }

            return output;
        }

        public static void ArrayPrinting(float[] source, int choice, string name)
        {
            switch (choice)
            {
                case NORMAL:
                    Utils.WriteArray(source, name);
                    break;
                case GROUPED:
                    Utils.WriteArrayTree(source, name);
                    break;
                default:
                    Utils.WriteArray(source, name);
                    break;
            }
        }
        
        public static void TestSorting()
        {
            float[] testArray = Utils.RandomArray(100);

            float[] sortedTestArray = new float[100];
            Utils.CopyArray(testArray, sortedTestArray, testArray.Length);

            Sorting.SortArray(sortedTestArray);

            Console.Clear();
            Utils.WriteArray(testArray, "Unsorted", 7);
            Console.WriteLine();
            Utils.WriteArray(sortedTestArray, "Sorted", 7);
        }

        public static void TestSorting2()
        {
            float[] testArray = Utils.ParseFile(Utils.InputLoopFilePath("Input some files path:"));

            Sorting.SortArray(testArray);
            
            Console.WriteLine();
            Utils.WriteArray(testArray, "Sorted", 7);
            Utils.WriteArrayTree(testArray, "Sorted", 5);
            Console.WriteLine("Comparisons: {0}", Sorting.Instance.Comparisons);
            Console.WriteLine("Swaps: {0}", Sorting.Instance.Swaps);
        }

        public static void TestSearching()
        {
            float[] testArray = new float[] { 0.0f, 1.0f, 1.0f, 1.0f, 3.5f, 3.5f, 7.8f, 12.1f, 12.2f, 12.2f, 12.2f, 12.7f, 30.0f, 100.0f, 122.0f };
            Utils.WriteArray(testArray, "Test Array", 7);

            int[] indexs = Searching.SearchArray(testArray);
            Utils.WriteArray(indexs, "Indexs");
        }
    }
}
