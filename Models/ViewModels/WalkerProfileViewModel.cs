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
        public Walk Walk { get; set; }
        public Dog Dog { get; set; }
        //public List<Walker> Walkers { get; set; }
    }
}
