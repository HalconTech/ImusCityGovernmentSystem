using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImusCityGovernmentSystem.General.Customer.Model
{
    public class IdentificationCardModel
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public bool IsSelected { get; set; }
        public string CardNumber { get; set; }
    }
}
