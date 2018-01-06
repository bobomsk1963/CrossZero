using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
namespace CrossZero.Models
{
    public class Game
    {
        [Key]
        public int Id { get; set; }
        // Дата и время
        public DateTime dt { get; set; }
        public int Victory { get; set; }  // 0 - ничья  1 - первый 2 - второй

        public virtual List<Hod> Hods { get; set; }
    }
}