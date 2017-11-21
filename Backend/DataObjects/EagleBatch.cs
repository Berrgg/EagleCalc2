using Microsoft.Azure.Mobile.Server;
using System;

namespace Backend.DataObjects
{
    public class EagleBatch : EntityData
    {
        public string IdBatch { get; set; }
        public string ProductCode { get; set; }
        public string Line { get; set; }
        public string TrayId { get; set; }
        public string PluCode { get; set; }
        public double Weight { get; set; }
        public double TrayCl { get; set; }
        public DateTimeOffset ProductionDate { get; set; }
    }
}