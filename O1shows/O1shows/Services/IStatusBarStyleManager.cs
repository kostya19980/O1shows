using System;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;

namespace O1shows.Services
{
    public interface IStatusBarStyleManager
    {
        void SetTransparent();
        void SetDefault();
        int GetHeight();
        int GetBottomNavBarHeight();
    }
}
