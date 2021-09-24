using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DogGo.Models
{
    public class Walker
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Hmmm... You should really add a Name...")]
        [MaxLength(35)]
        public string Name { get; set; }

        [Required]
        [DisplayName("Neighborhood")]
        public int NeighborhoodId { get; set; }

        [Required(ErrorMessage = "Please include your selfie as an avatar...")]
        [DisplayName("Image URL")]
        public string ImageUrl { get; set; }
        public Neighborhood Neighborhood { get; set; }
        public List<Walk> Walks { get; set; }
        public Walk Walk { get; set; }
        public Owner Owner { get; set; }
        public Dog Dog { get; set; }
    }
}

