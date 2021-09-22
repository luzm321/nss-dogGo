using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DogGo.Models
{
    public class Walk
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int WalkerId { get; set; }
        public int DogId { get; set; }
        public int Duration { get; set; }
        // may need to make a list of duration times to calculate the total walk time of each walker   
    }
}
