using System.Collections.Generic;

namespace _2021
{
    internal class Cross
    {
        public int Id { get; internal set; }
        public HashSet<Light> In { get; internal set; }
    }
}