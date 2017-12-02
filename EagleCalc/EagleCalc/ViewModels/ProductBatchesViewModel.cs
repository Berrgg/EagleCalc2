using System;
using System.Collections.Generic;
using System.Text;
using EagleCalc.Abstractions;
using EagleCalc.Models;

namespace EagleCalc.ViewModels
{
    public class ProductBatchesViewModel : BaseViewModel
    {
        public ProductBatchesViewModel(ProductInfo productInfo)
        {
            Title = productInfo.ProdDescription;
        }
    }
}
