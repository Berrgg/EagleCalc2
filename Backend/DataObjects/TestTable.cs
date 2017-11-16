using Microsoft.Azure.Mobile.Server;

namespace Backend.DataObjects
{
    public class TestTable : EntityData
    {
        public string TestName { get; set; }
        public int Number { get; set; }
    }
}