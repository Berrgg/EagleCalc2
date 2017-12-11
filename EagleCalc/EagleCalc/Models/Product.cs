using EagleCalc.Abstractions;

namespace EagleCalc.Models
{
    public class Product : TableData
    {
        public string ProductCode { get; set; }
        public string Description { get; set; }
        public string Factory { get; set; }
        public string CustomerName { get; set; }
    }
}
