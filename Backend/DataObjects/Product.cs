using Microsoft.Azure.Mobile.Server;

namespace Backend.DataObjects
{
    public class Product : EntityData
    {
        public string ProductCode { get; set; }
        public string Description { get; set; }
        public string Factory { get; set; }
    }
}