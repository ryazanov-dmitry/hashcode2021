using System;
using System.Linq;

namespace _2021
{
    class Program
    {
        static void Main(string[] args)
        {
            var filename = args[0];
            string[] lines = System.IO.File.ReadAllLines(filename);

            var mapParamRaw = lines[0].Split(" ").Select(x => Int16.Parse(x)).ToList();


            var mapParams = new ProblemParams
            {
                AllTime = mapParamRaw[0],
                IntersecCount = mapParamRaw[1],
                StreetCount = mapParamRaw[2],
                CarCount = mapParamRaw[3],
                Bonus = mapParamRaw[4]
            };

            var streets = lines.Skip(1).Take(mapParams.StreetCount).Select(x =>
            {
                var raw = x.Split(" ");
                return new Street
                {
                    Start = int.Parse(raw[0]),
                    End = int.Parse(raw[1]),
                    Name = raw[2],
                    Time = int.Parse(raw[3])
                };
            }).ToDictionary(x=>x.Name);


            Console.WriteLine(streets["rue-de-rome"].Time);
        }
    }
}
