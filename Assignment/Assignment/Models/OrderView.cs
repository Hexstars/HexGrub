using System.ComponentModel.DataAnnotations.Schema;

namespace Assignment.Models
{
    [NotMapped]
    public class OrderView
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public List<string> ProductName { get; set; }
        public List<string> Image {  get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public decimal Total => UnitPrice * Quantity;
    }
}
