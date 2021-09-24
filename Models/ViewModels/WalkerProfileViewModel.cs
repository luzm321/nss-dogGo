using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DogGo.Models.ViewModels
{
    public class WalkerProfileViewModel
    {
        public Walker Walker { get; set; }
        public List<Walk> Walks { get; set; }
        //public Walk Walk { get; set; }
        //public Dog Dog { get; set; }
        //public Owner Owner { get; set; }
        public string TotalDurationTime
        {
            get
            {
                // Sum() method is an IEnumerable<int> extension that computes and returns
                // the sum of a sequence of int values:
                int totalWalkTime = Walks.Select(walk => walk.Duration).Sum();
                int timeInHours = totalWalkTime / 3600;
                // totalWalkTime in seconds %modulo operator evenly divisible by 3600 (removes hours equivalent from totalWalktime)
                // then the seconds left over divided by 60 seconds will result in the total minutes from totalWalkTime:
                int timeInMinutes = totalWalkTime % 3600 / 60;
                return $"{timeInHours}hr {timeInMinutes}min";
            }
        }
    }
}
