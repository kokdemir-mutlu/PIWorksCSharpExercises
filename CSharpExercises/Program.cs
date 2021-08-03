using System;
using System.Collections.Generic;
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
            Exercise1();
            //Exercise2();
        }

        private static void Exercise1()
        {
            Dictionary<string, int> dicExtCount = new Dictionary<string, int>();
            ProcessDirectory(dicExtCount, @"C:\Program Files\dotnet\sdk\5.0.301");
            var extList = dicExtCount.ToList();
            extList.Sort((ext1, ext2) => ext2.Value.CompareTo(ext1.Value));
            foreach(KeyValuePair<string,int> element in extList)
            {
                Console.WriteLine("{0} : {1}.", element.Key, element.Value);
            }
        }
        private static void ProcessDirectory(Dictionary<string,int> dic, string path)
        {
            var queryResult = Directory.GetFiles(path)
                .Where(path => Path.GetFileName(path).StartsWith('M'))
                .Select(file => Path.GetExtension(file))
                .GroupBy(g => g, (fileExt, countExt) =>
                new
                {
                    Extension = fileExt,
                    Count = countExt.Count()
                });
            

            foreach (var result in queryResult)
            {
                if(!dic.ContainsKey(result.Extension))
                    dic.Add(result.Extension, result.Count);
                else
                    dic[result.Extension] += result.Count;
                //Console.WriteLine("{0} : {1}", result.Extension, result.Count);
                // ProcessFile(fileName);
            }

            string[] subdirectoryEntries = Directory.GetDirectories(path);
            foreach (string subdirectoryEntry in subdirectoryEntries)
            {
                ProcessDirectory(dic,subdirectoryEntry);
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

            // --------------
            int totalTextLength2 = 0;
            var watch = new System.Diagnostics.Stopwatch();
            watch.Start();
            string[] contents2 = (string[])GetUrlContents(urls).Result;
            watch.Stop();


            for(int i = 0; i < contents2.Length; ++i)
            {
                Console.WriteLine("string length for url({0}) : {1}.", urls[i], contents2[i].Length);
                totalTextLength2 += contents2[i].Length;
            }
            Console.WriteLine("Total text length : {0}.", totalTextLength2);
            Console.WriteLine("{0} miliseconds spent.", watch.ElapsedMilliseconds);

        }

        private static async Task<IEnumerable<string>> GetUrlContents(string[] urls)
        {
            var getStringTasks = new List<Task<string>>();
            var client = new HttpClient();
            foreach(string url in urls)
            {
                getStringTasks.Add(client.GetStringAsync(url));
            }

            return await Task.WhenAll(getStringTasks);
        }

        private static async Task<string> GetUrlContent(string url)
        {
            var client = new HttpClient();

            string str = await client.GetStringAsync(url);

            return str;
        }

    }
}
