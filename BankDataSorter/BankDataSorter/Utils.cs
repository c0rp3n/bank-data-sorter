using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankDataSorter
{
    class Utils
    {
        #region Singleton
        private static readonly Utils instance = new Utils();

        private Utils() { }

        public static Utils Instance
        {
            get
            {
                return instance;
            }
        }
        #endregion

        #region Constants
        private static readonly string CONTINUE_MESSAGE = @"
Press any key to continue...";

        public const int    YES = 0,
                            NO = 1;

        #region MultipleFileSelection
        private static readonly string MULTIPLE_FILE_SELECTION = @"Would you like to include another file?:
    0 - Yes
    1 - No";
        #endregion

        #region DirectorySelection
        private static readonly string DIRECTORY_SELECTION = @"Include this file:
    0 - Yes
    1 - No";

        public static readonly int[] YES_NO_I = new int[] { YES, NO };
        #endregion
        #endregion

        #region Console
        public static void ConsoleHang()
        {
            Console.WriteLine(CONTINUE_MESSAGE);
            Console.ReadKey();
        }
        #endregion

        #region Random
        private Random random = null;
        public Random Random
        {
            get
            {
                if (random == null) { random = new Random(); }

                return random;
            }
        }

        /// <summary>
        /// Creates an array of random floating point values.
        /// </summary>
        /// <param name="n">The size of the array</param>
        /// <returns></returns>
        public static float[] RandomArray(int n)
        {
            float[] output = new float[n];
            if (n < 1) { return output; }

            Random randomInstance = Utils.Instance.Random;
            for (int i = 0; i < n; i++)
            {
                output[i] = (float)randomInstance.NextDouble();
            }

            return output;
        }
        #endregion

        #region InputLoop
        public static bool InputLoop(string message)
        {
            Console.WriteLine(message);

            int input;
            while (true)
            {
                if (int.TryParse(Console.ReadLine(), out input))
                {
                    if (input == YES) { return true; }
                    else if (input == NO) { return false; }
                }

                Console.WriteLine("Please input a valid response.");
            }
        }

        /// <summary>
        /// An itterative input loops that repeats until the user passes a correct value.
        /// </summary>
        /// <param name="message">Message to print to the user at the start.</param>
        /// <param name="correctValues">Array of correct values that the loop should check for before exiting.</param>
        /// <returns></returns>
        public static int InputLoop(string message, int[] correctValues)
        {
            Console.WriteLine(message);

            int input;
            int n = correctValues.Length;
            while (true)
            {
                if (int.TryParse(Console.ReadLine(), out input))
                {
                    for (int i = 0; i < n; i++)
                    {
                        if (input == correctValues[i]) { return input; }
                    }
                }

                Console.WriteLine("Please input a valid response.");
            }
        }

        /// <summary>
        /// An itterative input loops that repeats until the user passes a correct value.
        /// </summary>
        /// <param name="message">Message to print to the user at the start.</param>
        /// <returns>The inputted integer value.</returns>
        public static int InputLoopInt(string message)
        {
            Console.WriteLine(message);
            int input;
            while (true)
            {
                if (int.TryParse(Console.ReadLine(), out input)) { return input; }

                Console.WriteLine("Please input a valid response.");
            }
        }

        /// <summary>
        /// An itterative input loops that repeats until the user passes a correct value.
        /// </summary>
        /// <param name="message">Message to print to the user at the start.</param>
        /// <returns>The inputted floating point number.</returns>
        public static float InputLoopFloat(string message)
        {
            Console.WriteLine(message);
            float input;
            while (true)
            {
                if (float.TryParse(Console.ReadLine(), out input)) { return input; }

                Console.WriteLine("Please input a valid response.");
            }
        }

        /// <summary>
        /// An itterative input loops that repeats until the user passes a correct value.
        /// </summary>
        /// <param name="message">Message to print to the user at the start.</param>
        /// <returns>The inputted file path.</returns>
        public static string InputLoopFilePath(string message)
        {
            Console.WriteLine(message);
            string input;
            while (true)
            {
                input = Console.ReadLine().Trim(new char[] { '"' });
                if (File.Exists(input)) { return input; }

                Console.WriteLine("Please input a valid response.");
            }
        }

        /// <summary>
        /// An itterative input loops that repeats until the user passes a correct value.
        /// </summary>
        /// <param name="message">Message to print to the user at the start.</param>
        /// <returns>The inputted file path.</returns>
        public static string[] InputLoopMultiFilePath(string message)
        {
            Console.WriteLine(message);
            List<string> output = new List<string>();
            string input;
            int count = 0;
            while (true)
            {
                int choice = InputLoop(string.Format(MULTIPLE_FILE_SELECTION, count++), YES_NO_I);
                if (choice == NO)
                {
                    break;
                }

                while (true)
                {
                    input = Console.ReadLine().Trim(new char[] { '"' });
                    if (File.Exists(input))
                    {
                        output.Add(input);
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Please input a valid response.");
                    }
                }
            }

            return output.ToArray();
        }

        /// <summary>
        /// An itterative input loops that repeats until the user passes a correct value.
        /// </summary>
        /// <param name="message">Message to print to the user at the start.</param>
        /// <returns>The inputted directory path.</returns>
        public static string InputLoopDirPath(string message)
        {
            Console.WriteLine(message);
            string input;
            while (true)
            {
                input = Console.ReadLine().Trim(new char[] { '"' });
                if (Directory.Exists(input)) { return input; }

                Console.WriteLine("Please input a valid response.");
            }
        }
        #endregion

        #region WriteArray
        /// <summary>
        /// Output the content of an array of anytype to the console in a structured manner.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">The source array to be outputed.</param>
        public static void WriteArray<T>(T[] source)
        {
            WriteArray(source, "Array", 7);
        }

        /// <summary>
        /// Output the content of an array of anytype to the console in a structured manner.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">The source array to be outputed.</param>
        /// <param name="name">The name to be outputted for the array.</param>
        public static void WriteArray<T>(T[] source, string name)
        {
            WriteArray(source, name, 7);
        }

        /// <summary>
        /// Output the contents of an array of anytype to the console in a structured manner.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">The source array to be outputed.</param>
        /// <param name="name">The name to be outputted for the array.</param>
        /// <param name="columns">The ammount of columns to print on one line in the console.</param>
        public static void WriteArray<T>(T[] source, string name, int columns)
        {
            int n = source.Length;
            if (n < 1) { return; }

            Console.WriteLine("{0}: size - {1}", name, n);
            Console.WriteLine("Data:");

            int maxSize = 0;
            string[] writes = new string[n];
            for (int i = 0; i < n; i++)
            {
                writes[i] = source[i].ToString();
                maxSize = Math.Max(writes[i].Length, maxSize);
            }

            int lastWriteSize = writes[0].Length;
            Console.Write("[ {0}", writes[0]);
            for (int i = 1; i < n; i++)
            {
                Console.Write(",".PadRight(Math.Max(maxSize - lastWriteSize, 0) + 1));
                if (i % columns == 0) { Console.Write("\n  "); }
                else { Console.Write("  "); }

                lastWriteSize = writes[i].Length;
                Console.Write(writes[i]);
            }
            Console.WriteLine(" ]");
        }
        #endregion

        #region WriteArrayTree
        /// <summary>
        /// Output the contents of an array displaying the occurunce of each unique value to the console in a structured manner.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">The source array to be outputed.</param>
        public static void WriteArrayTree<T>(T[] source)
        {
            WriteArrayTree(source, "Array");
        }

        /// <summary>
        /// Output the contents of an array displaying the occurunce of each unique value to the console in a structured manner.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">The source array to be outputed.</param>
        /// <param name="name">The name to be outputted for the array.</param>
        public static void WriteArrayTree<T>(T[] source, string name)
        {
            WriteArrayTree(source, name, 7);
        }

        /// <summary>
        /// Output the contents of an array displaying the occurunce of each unique value to the console in a structured manner.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">The source array to be outputed.</param>
        /// <param name="name">The name to be outputted for the array.</param>
        /// <param name="columns">The ammount of columns to print on one line in the console.</param>
        public static void WriteArrayTree<T>(T[] source, string name, int columns)
        {
            int n = source.Length;
            if (n < 1) { return; }

            Dictionary<T, int> arrayDictionary = new Dictionary<T, int>();
            for (int i = 0; i < n; i++)
            {
                if (arrayDictionary.ContainsKey(source[i]))
                {
                    arrayDictionary[source[i]]++;
                }
                else
                {
                    arrayDictionary.Add(source[i], 1);
                }
            }

            Console.WriteLine("{0}: size - {1}", name, n);
            Console.WriteLine("Data:");

            KeyValuePair<T, int> keyValuePair;
            int maxSize = 0;
            string[] writes = new string[arrayDictionary.Count];
            for (int i = 0; i < arrayDictionary.Count; i++)
            {
                keyValuePair = arrayDictionary.ElementAt(i);
                writes[i] = string.Format("{0} * {1}", keyValuePair.Key, keyValuePair.Value);
                maxSize = Math.Max(writes[i].Length, maxSize);
            }

            int lastWriteSize = writes[0].Length;
            Console.Write("[ {0}", writes[0]);
            for (int i = 1; i < arrayDictionary.Count; i++)
            {
                Console.Write(",".PadRight(Math.Max(maxSize - lastWriteSize, 0)));

                if (i % columns == 0) { Console.Write("\n  "); }
                else { Console.Write("    "); }

                keyValuePair = arrayDictionary.ElementAt(i);
                lastWriteSize = writes[i].Length;

                Console.Write(writes[i]);
            }
            Console.WriteLine(" ]");
        }
        #endregion

        #region WriteStatistics
        public static void WriteStatisticsSearch(SearchingStatistics statistics)
        {
            Console.WriteLine(@"Statistics for the {0} of {1} are as follows.
    Comparisons:    {2}
    Itterations:    {3}", statistics.Search, statistics.Value, statistics.Comparisons, statistics.Itterations);
        }

        public static void WriteLastStatisticsSearch()
        {
            Console.WriteLine(@"Statistics for the last search are as follows.
    Comparisons:    {0}
    Itterations:    {1}", Searching.Instance.Comparisons, Sorting.Instance.Itterations);
        }

        public static void WriteStatisticsSort(SortingStatistics statistics)
        {
            Console.WriteLine(@"Statistics from the {0} in {1} order are as follows.
    Comparisons:    {2}
    Itterations:    {3}
    Recursions:     {4}
    Swaps:          {5}", statistics.Sort, statistics.Order, statistics.Comparisons, statistics.Itterations, statistics.Recursions, statistics.Swaps);
        }

        public static void WriteLastStatisticsSort()
        {
            Console.WriteLine(@"Statistics for the last sort are as follows.
    Comparisons:    {0}
    Itterations:    {1}
    Recursions:     {2}
    Swaps:          {3}", Sorting.Instance.Comparisons, Sorting.Instance.Itterations, Sorting.Instance.Recursions, Sorting.Instance.Swaps);
        }
        #endregion

        #region CopyArray
        /// <summary>
        /// Copies the source array into the output array.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">The array to be copied.</param>
        /// <param name="output">The array to be copied too.</param>
        /// <param name="n">The size of the new array.</param>
        public static void CopyArray<T>(T[] source, T[] output, int n)
        {
            CopyArray<T>(source, output, 0, n);
        }

        /// <summary>
        /// Copies the source array into the output array.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">The array to be copied.</param>
        /// <param name="output">The array to be copied too.</param>
        /// <param name="start">The index to start copying at.</param>
        /// <param name="end">The index to finish copying at.</param>
        public static void CopyArray<T>(T[] source, T[] output, int start, int end)
        {
            for (int i = 0; i < end - start; i++)
                output[start + i] = source[i];
        }
        #endregion

        #region ApendArray
        /// <summary>
        /// Merges to arrays.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">The source array.</param>
        /// <param name="append">The array to be appended.</param>
        /// <returns>The reference to the new array.</returns>
        public static T[] ApendArray<T>(T[] source, T[] append)
        {
            T[] output = null;
            ApendArray(source, append, output);

            return output;
        }

        /// <summary>
        /// Merges to arrays.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">The first array to be appended.</param>
        /// <param name="append">The array to be appended.</param>
        /// <param name="output">The array to be written too.</param>
        public static void ApendArray<T>(T[] source, T[] append, T[] output)
        {
            output = new T[source.Length + append.Length];
            CopyArray(source, output, source.Length);
            CopyArray(append, output, source.Length, output.Length);
        }
        #endregion

        #region ParseFile
        /// <summary>
        /// Parses the text file contained at the given location into an array of floating point values.
        /// </summary>
        /// <param name="filePath">The path to the text file to be read.</param>
        /// <returns></returns>
        public static float[] ParseFile(string filePath)
        {
            if (!File.Exists(filePath)) { return new float[0]; }

            string[] source = File.ReadAllLines(filePath);
            int n = source.Length;
            if (n < 1) { return new float[0]; }

            float[] output = new float[n];
            for (int i = 0; i < n; i++)
            {
                output[i] = float.Parse(source[i]);
            }

            return output;
        }

        /// <summary>
        /// Parses the text file contained at the given location into an existing array of floating point values.
        /// </summary>
        /// <param name="source">An array storing your current values.</param>
        /// <param name="pathAppend">The path to the text file to be read.</param>
        /// <returns></returns>
        public static float[] ParseMergeFiles(float[] source, string pathAppend)
        {
            float[] appendSource = ParseFile(pathAppend);

            if (appendSource.Length < 1) { return source; }

            float[] output = new float[source.Length + appendSource.Length];
            CopyArray(source, output, source.Length);
            CopyArray(appendSource, output, source.Length, output.Length);

            return output;
        }

        /// <summary>
        /// Parses the text files contained at the given locations into an array of floating point values.
        /// </summary>
        /// <param name="filePaths">The paths to the text files to be read.</param>
        /// <returns></returns>
        public static float[] ParseFiles(string[] filePaths)
        {
            float[] output = new float[0];
            for (int i = 0; i < filePaths.Length; i++)
            {
                output = ParseMergeFiles(output, filePaths[i]);
            }

            return output;
        }
        #endregion

        #region ParseDirectory
        public static string[] ParseDirectory(string directoryPath)
        {
            if (!Directory.Exists(directoryPath)) { return new string[0]; }

            string[] files = Directory.GetFiles(directoryPath);
            List<string> output = new List<string>();
            for (int i = 0; i < files.Length; i++)
            {
                Console.WriteLine("File: {0}", Path.GetFileName(files[i]));

                int choice = InputLoop(DIRECTORY_SELECTION, YES_NO_I);
                switch (choice)
                {
                    case YES:
                        output.Add(files[i]);
                        break;
                    case NO:
                        break;
                }

                Console.WriteLine();
            }

            return output.ToArray<string>();
        }
        #endregion

        #region WriteToFile
        /// <summary>
        /// Outputs the given floating point array to a file to be saved at the passed location.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">The array to be outputted.</param>
        /// <param name="filePath">The file to be created and then written too.</param>
        private static void WriteToFile<T>(T[] source, string filePath)
        {

        }
        #endregion

        #region MostCommonValue
        public static T MostCommonValue<T>(T[] source)
        {
            Dictionary<T, int> arrayDictionary = new Dictionary<T, int>();
            for (int i = 0; i < source.Length; i++)
            {
                if (arrayDictionary.ContainsKey(source[i]))
                {
                    arrayDictionary[source[i]]++;
                }
                else
                {
                    arrayDictionary.Add(source[i], 1);
                }
            }

            int maxValueIndex = -1;
            int maxValueCount = -1;
            KeyValuePair<T, int> keyValuePair;
            for (int i = 0; i < arrayDictionary.Count; i++)
            {
                keyValuePair = arrayDictionary.ElementAt(i);
                if (keyValuePair.Value > maxValueCount)
                {
                    maxValueIndex = i;
                    maxValueCount = keyValuePair.Value;
                }
            }

            return source[maxValueIndex];
        }
        #endregion
    }
}
