using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OneToManyMVC.Models
{
    public class Student
    {
        [Key]
        public int Id { get; set; }
        public string StudentName { get; set; }

        [ForeignKey("Grade")]
        public int CurrentGradeID { get; set; }
        [ForeignKey("Ambion")]
        public int CurrentAmbionID { get; set; }
        public Ambion Ambion { get; set; }
        public Grade Grade { get; set; }
    }
}
