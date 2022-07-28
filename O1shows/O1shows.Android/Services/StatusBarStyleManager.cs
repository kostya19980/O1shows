using Android.Content.Res;
using Android.Views;
using O1shows.Droid.Services;
using O1shows.Services;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Color = Xamarin.Forms.Color;

[assembly: Dependency(typeof(StatusBarStyleManager))]
namespace O1shows.Droid.Services
{
    public class StatusBarStyleManager : IStatusBarStyleManager
    {
        private MainActivity activity;
        public StatusBarStyleManager()
        {
            activity = MainActivity.MainActivityInstance;
        }
        public void SetTransparent()
        {
            activity.Window.AddFlags(WindowManagerFlags.TranslucentNavigation);
            activity.Window.SetStatusBarColor(Color.Transparent.ToAndroid());
        }
        public void SetDefault()
        {
            Color backgroundColor = (Color)Application.Current.Resources["color-bg"];
            activity.Window.SetStatusBarColor(backgroundColor.ToAndroid());
            activity.Window.SetNavigationBarColor(backgroundColor.ToAndroid());
        }
        public int GetHeight()
        {
            int statusBarHeight = -1;
            int resourceId = activity.Resources.GetIdentifier("status_bar_height", "dimen", "android");
            if (resourceId > 0)
            {
                statusBarHeight = (int)(activity.Resources.GetDimensionPixelSize(resourceId) / activity.Resources.DisplayMetrics.Density);
            }
            return statusBarHeight;
        }
        public int GetBottomNavBarHeight()
        {
            TypedArray styledAttributes = activity.Theme.ObtainStyledAttributes(new int[] { Android.Resource.Attribute.ActionBarSize });
            int actionbarHeight = (int)styledAttributes.GetDimension(0, 0);
            styledAttributes.Recycle();
            return actionbarHeight;
        }
    }
}