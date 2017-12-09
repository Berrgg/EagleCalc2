using System;

namespace EagleCalc.Helpers
{
    public class BarCodeSplit
    {
        private string _barCode;
        public string PluCode { get; set; }
        public double Weight { get; set; }
        public string TrayId { get; set; }
        public double TrayCl { get; set; }

        public BarCodeSplit(string barCode)
        {
            _barCode = barCode;
            PluCode = _barCode.Substring(0, 5);
            TrayId = _barCode.Substring(5, 16);
            Weight = Convert.ToDouble(_barCode.Substring(21, 6).Insert(4, "."));
            TrayCl = Convert.ToDouble(_barCode.Substring(27, 4).Insert(2, "."));
        }
    }
}
