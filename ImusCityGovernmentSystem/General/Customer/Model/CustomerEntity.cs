using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImusCityGovernmentSystem.General.Customer.Model
{
    public class CustomerEntity
    {
        public int CustomerId { get; set; }
        public string Name { get; set; }
        public string DateAdded { get; set; }
        public string Birthdate { get; set; }
    }
}
