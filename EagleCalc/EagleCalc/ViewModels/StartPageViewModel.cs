using System;
using System.Collections.Generic;
using System.Text;
using EagleCalc.Abstractions;

namespace EagleCalc.ViewModels
{
    public class StartPageViewModel : BaseViewModel
    {
        public StartPageViewModel()
        {
            Title = "Start page";

        }

        public string AppService { get; set; }

    }
}
