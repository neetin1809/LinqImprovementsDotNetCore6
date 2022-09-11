using System.Linq;

namespace Linq_DotNet_Core6 
{
    internal class Program
    {
        IEnumerable<IEnumerable<T>> ChunkBy<T>(IEnumerable<T> source, int chunkSize)
        {
            return source
                .Select((x,i) => new {Index = i, Value = x})
                .GroupBy(x => x.Index/ chunkSize)
                .Select(x => x.Select(v => v.Value));
        }

        static void Main(string[] args)
        {
            var usersName = new string[] { "Amit", "Neetin", "Anup", "Dinesh", "Mayur", "Kartik" };
            #region Chunk
            //the old way
            var userNames1st = usersName.Take(3);
            var userNames2nd = usersName.Skip(3).Take(3);

            //the new way
            IEnumerable<string[]> clusterNames = usersName.Chunk(3);
            // Print each cluster.
            foreach (var name in clusterNames)
            {
                Console.WriteLine($"Cluster of {string.Join(", ", name)}");
            }
            //uisng Parallel Linq, we can speed up the processing
            Console.BackgroundColor = ConsoleColor.Green;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Use Of Parallel Linq");
            Console.BackgroundColor = ConsoleColor.Black;
            foreach (var name in clusterNames)
            {
                //Observe the Difference in sequencing of Names Displayed each time
                Parallel.ForEach(name, (item) =>
                {
                    //Do something Parallel here. 
                    Console.WriteLine(item);
                });
            }
            Console.ForegroundColor = ConsoleColor.White;

            var list = Enumerable.Range(1, 100);
            var chunkSize = 10;
            foreach (var chunk in list.Chunk(chunkSize)) //Returns a chunk with the correct size. 
            {
                foreach (var item in chunk)
                {
                    Console.WriteLine(item);
                }
            }

            foreach (var chunk in list.Chunk(chunkSize)) //Returns a chunk with the correct size. 
            {
                Parallel.ForEach(chunk, (item) =>
                {
                    //Do something Parallel here. 
                    Console.WriteLine(item);
                });
            }
            #endregion

            #region Try Get Non Enumerated Method

                var canGetCountDirectly = usersName.TryGetNonEnumeratedCount(out int theCount);
                Console.WriteLine($"UserNames One count can be returned directly = {canGetCountDirectly}");
                Console.WriteLine($"The count for UserNames = {theCount}");
            #endregion

            #region Zip Method
            int[] numbers = { 1, 2, 3, 4 };
            string[] words = { "one", "two", "three" };
            //Combining integer and number through zip )awesome feature
            var numbersAndWords = numbers.Zip(words, (first, second) => string.Concat(first, ":- ", second));
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.WriteLine("Using Zip Method:-");
            foreach (var item in numbersAndWords)
            Console.WriteLine(item);
            Console.BackgroundColor = ConsoleColor.Black;

            //Back to userNames example
            int[] ages = { 33, 30, 29 };
            var combineAges = usersName.Zip(ages);
            Console.WriteLine("Back to users name example");
            foreach (var item in combineAges)
                Console.WriteLine(item);
            #endregion

            #region Min, MaxBy
            Console.WriteLine("Min By, Max By feature");
            var youngestMember = combineAges.OrderBy(x => x.Second).First();
            var seniorMember = combineAges.OrderBy(x => x.Second).First();
            //New way
            youngestMember = combineAges.MinBy(x => x.Second);
            seniorMember = combineAges.MaxBy(x => x.Second);
            Console.WriteLine($"Youngest Member Name: {youngestMember.First}");
            Console.WriteLine($"Eldest Member Name:{seniorMember.First}");
            #endregion

            #region Indices and range
            Console.WriteLine("Using Indices Operator");
            Console.WriteLine($"Last second operator: {usersName.ElementAt(^2)}");


            string[] fourUsers = usersName[1..4];
            string[] firstTwoUsers = usersName[^2..^0];
            string[] allUsers = usersName[..]; // contains all through it.
            string[] firstFourUsers = usersName[..4]; // contains The first four users
            string[] lastPhrase = usersName[6..]; // contains the last three from 6th
            #endregion
        }
    }
}