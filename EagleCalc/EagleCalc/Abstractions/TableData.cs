using System;

namespace EagleCalc.Abstractions
{
    public abstract class TableData
    {
        public string Id { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }
        public DateTimeOffset? CreatedAt { get; set; }
        public byte[] Version { get; set; }
        public string CustomerName { get; set; }
        public string Line { get; set; }
        public DateTimeOffset ProductionDate { get; set; }
    }
}
