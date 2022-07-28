using O1shows.iOS.Services;
using O1shows.Services;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(StatusBarStyleManager))]
namespace O1shows.iOS.Services
{
    public class StatusBarStyleManager : IStatusBarStyleManager
    {
        public void SetTransparent()
        {

        }
        public void SetDefault()
        {

        }
        public int GetHeight()
        {
            return (int)UIApplication.SharedApplication.StatusBarFrame.Height;
        }
    }
}