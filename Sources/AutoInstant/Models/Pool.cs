using System.Collections.Generic;

namespace AutoInstant.Models
{
    public class Pool
    {
        public Pool()
        {
            Matches = new List<Match>();
        }

        public List<Match> Matches { get; set; }
    }
}