using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Congratulations.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public DateTime NearestBirthday { get; set; }

        public override string ToString()
        {
            return $"{Name} {Date.ToString("d")}";
        }
    }
}
