using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace O1shows.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public class SquarePage : ContentView
    {
        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);
            HeightRequest = Width;
        }
    }
}