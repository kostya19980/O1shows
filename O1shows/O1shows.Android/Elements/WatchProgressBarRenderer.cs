using Android.Content;
using O1shows.Droid.Elements;
using O1shows.Elements;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(WatchProgressBar), typeof(WatchProgressBarRenderer))]
namespace O1shows.Droid.Elements
{
    public class WatchProgressBarRenderer : ProgressBarRenderer
    {
        public WatchProgressBarRenderer(Context context) : base(context)
        {
        }
        protected override void OnElementChanged(ElementChangedEventArgs<ProgressBar> e)
        {
            base.OnElementChanged(e);
            if (Control != null)
            {
                var drawFinished = Context.GetDrawable(Resource.Drawable.watch_progressbar_finished);
                var draw = Context.GetDrawable(Resource.Drawable.watch_progressbar);
                if (e.NewElement.Progress >= 0.95)
                {
                    Control.ProgressDrawable = drawFinished;
                }
                else
                {
                    Control.ProgressDrawable = draw;
                }
            }
        }
    }
}