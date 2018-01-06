using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
namespace CrossZero.Models
{
    public class Hod
    {
        [Key]
        public int Id { get; set; }
        public int Player { get; set; }
        public int Row { get; set; }
        public int Col { get; set; }

        public Game Game { get; set; }

    }
}