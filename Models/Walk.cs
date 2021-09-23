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
        public string ParsedDateTime
        {
            get
            {
                return Date.ToShortDateString();
            }
        }
        public int WalkerId { get; set; }
        public Walker Walker { get; set; }
        public Dog Dog { get; set; }
        public int DogId { get; set; }
        public int Duration { get; set; }
        public int DurationInMinutes
        {
            get
            {
                return Duration / 60;
            }
        }

        public int TotalDurationTime
        {
            get
            {
                return Duration += Duration / 120;
            }
        }
        // may need to make a list of duration times to calculate the total walk time of each walker   
    }
}
