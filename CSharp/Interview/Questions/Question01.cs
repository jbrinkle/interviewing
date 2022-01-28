using System;
using System.IO;
using System.Text.Json;
using System.Linq;

namespace Interview.Questions
{
    /// <summary>
    /// Given an array of numbers, detect the number of times a successive number
    /// is greater than the previous number.
    ///
    /// Modify to consider the sum of a rolling window of numbers before doing
    /// the comparison.
    /// </summary>
    public class Question01
    {
        public Question01()
        {
        }

        public int[] ReadData()
        {
            var json = File.ReadAllText("data01.json");
            var data = JsonSerializer.Deserialize<int[]>(json);
            return data;
        }

        public void Part1(int[] data)
        {
            var count = 0;
            for (var idx = 1; idx < data.Length; idx++)
            {
                if (data[idx] > data[idx - 1])
                {
                    count++;
                }
            }

            Console.WriteLine(count);
        }

        public void Part2(int[] data)
        {
            var count = 0;
            var lastWindow = data[0] + data[1] + data[2];

            for (var idx = 3; idx < data.Length; idx++)
            {
                var window = data[idx] + data[idx - 1] + data[idx - 2];
                if (window > lastWindow)
                {
                    count++;
                }
                lastWindow = window;
            }

            Console.WriteLine(count);
        }
    }
}
