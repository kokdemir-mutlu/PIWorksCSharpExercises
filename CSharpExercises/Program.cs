using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace CSharpExercises
{
    class Program
    {

        public static void Main(string[] args)
        {
            //ProcessDirectory(@"C:\Program Files\dotnet\sdk\5.0.301");
            Exercise2();

            //Console.WriteLine("Hello World!");
        }

        public static void ProcessDirectory(string path)
        {
            var queryResult = Directory.GetFiles(path)
                .Where(path => Path.GetFileName(path).StartsWith('M'))
                .Select(file => Path.GetExtension(file))
                .GroupBy(g => g, (fileExt, countExt) =>
                new
                {
                    Extension = fileExt,
                    Count = countExt.Count()
                })
                .OrderByDescending(res => res.Count);

            foreach (var result in queryResult)
            {
                Console.WriteLine("{0} : {1}", result.Extension, result.Count);
                // ProcessFile(fileName);
            }

            string[] subdirectoryEntries = Directory.GetDirectories(path);
            foreach (string subdirectoryEntry in subdirectoryEntries)
            {
                ProcessDirectory(subdirectoryEntry);
            }
        }

        private static void ProcessFile(string fileName)
        {
            Console.WriteLine("Processed file '{0}'.", fileName);
        }


        private static void Exercise2()
        {
            var urls = new string[]
            {
                "https://docs.microsoft.com",
                "https://google.com",
                "https://piworks.net",
                "https://piworks.net/insight/events",
                "https://piworks.net/insight/news",
                "https://piworks.net/products"
            };

            long totalTextLengt = 0;
            string[] contents = new string[urls.Length];
            for (int i = 0; i < urls.Length; ++i)
            {
                contents[i] = GetUrlContent(urls[i]).Result;
            }

            for(int i = 0; i < urls.Length; ++i)
            {
                Console.WriteLine("string length for url({0}) : {1}.", urls[i], contents[i].Length);
                totalTextLengt += contents[i].Length;
            }

            Console.WriteLine("Total text length : {0}.", totalTextLengt);
        }

        private static async Task<string> GetUrlContent(string url)
        {
            var client = new HttpClient();

            string str = await client.GetStringAsync(url);

            return str;
        }

    }
}
