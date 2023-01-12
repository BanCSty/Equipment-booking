using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Test.Models
{
    public class ServiceObject
    {

        public string ID { get; set; }
        public int Amount { get; set; }
        public string Name { get; set; }
    }

    public class Order
    {
        [Key]
        public int idMain { get; set; }
        public string ID { get; set; }
        public int AmountOrder { get; set; }
        public string NameOrder { get; set; }
    }
}
