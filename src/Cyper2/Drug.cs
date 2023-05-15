using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cyper2
{
    public class Drug
    {
        public string Name { get; set; }
        public List<string> Cyps { get; set; }
    }
}
