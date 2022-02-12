using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace _2021
{
    class Program
    {
        static void Main(string[] args)
        {
            ProblemParams mapParams;
            Dictionary<string, Street> streets;
            Dictionary<int, CarPath> carPaths;
            Dictionary<int, Cross> crosses;
            Parse(
                args,
                out mapParams,
                out streets,
                out carPaths,
                out crosses);

            mapParams.DefaultTimeForLight = int.Parse(args[1]);

            foreach (var cross in crosses)
            {
                foreach (var light in cross.Value.In)
                {
                    light.Time = mapParams.DefaultTimeForLight;
                }
            }

            // Generating output file
            var outputLines = new List<string>();
            var greenCrosses = crosses.Where(x => x.Value.In.Any(x => x.Time != 0));


            outputLines.Add(greenCrosses.Count().ToString());

            foreach (var cross in greenCrosses)
            {
                outputLines.Add(cross.Key.ToString());

                var greenLights = cross.Value.In.Where(x => x.Time != 0);
                outputLines.Add(greenLights.Count().ToString());

                foreach (var light in greenLights)
                {
                    var str = light.Street.Name + " " + light.Time.ToString();
                    outputLines.Add(str);
                }
            }

            File.WriteAllLinesAsync(args[0]+"-out.txt", outputLines);
        }

        private static void Parse(string[] args,
                                  out ProblemParams mapParams,
                                  out Dictionary<string, Street> streets,
                                  out Dictionary<int, CarPath> carPaths,
                                  out Dictionary<int, Cross> crosses)
        {
            var filename = args[0];
            string[] lines = System.IO.File.ReadAllLines(filename);

            var mapParamRaw = lines[0].Split(" ").Select(x => Int16.Parse(x)).ToList();

            mapParams = new ProblemParams
            {
                AllTime = mapParamRaw[0],
                IntersecCount = mapParamRaw[1],
                StreetCount = mapParamRaw[2],
                CarCount = mapParamRaw[3],
                Bonus = mapParamRaw[4]
            };

            streets = lines.Skip(1).Take(mapParams.StreetCount).Select(x =>
            {
                var raw = x.Split(" ");
                return new Street
                {
                    Start = int.Parse(raw[0]),
                    End = int.Parse(raw[1]),
                    Name = raw[2],
                    Time = int.Parse(raw[3])
                };
            }).ToDictionary(x => x.Name);

            carPaths = lines.Skip(1 + mapParams.StreetCount)
                               .Take(mapParams.CarCount)
                               .Select((x, i) =>
                               {
                                   var raw = x.Split(" ");

                                   return new CarPath
                                   {
                                       CarId = i + 1,
                                       Path = raw.Skip(1).ToList(),
                                       PathLength = raw[0]
                                   };
                               })
                               .ToDictionary(x => x.CarId);

            crosses = new Dictionary<int, Cross>();
            foreach (var item in streets)
            {
                if (!crosses.TryGetValue(item.Value.End, out Cross value))
                {
                    crosses[item.Value.End] = new Cross
                    {
                        Id = item.Value.End,
                        In = new HashSet<Light>
                        {
                            new Light{
                                Street = item.Value,
                                Time = 0
                            }
                        }
                    };

                    continue;
                }

                value.In.Add(new Light
                {
                    Street = item.Value,
                    Time = 0
                });
            }
        }
    }
}
