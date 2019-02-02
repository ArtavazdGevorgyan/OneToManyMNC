using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OneToManyMVC.Models
{
    public class Ambion
    {
        [Key]
        public int AmbionId { get; set; }
        public string AmbionName { get; set; }

        public ICollection<Student> Students { get; set; }

        [NotMapped]
        public SelectList Ambionner { get; set; }
    }
}
