using CoreGraphics;
using O1shows.Elements;
using O1shows.iOS.Elements;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(WatchProgressBar), typeof(WatchProgressBarRenderer))]
namespace O1shows.iOS.Elements
{
    public class WatchProgressBarRenderer : ProgressBarRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<ProgressBar> e)
        {
            base.OnElementChanged(e);

            Control.ProgressTintColor = Color.FromRgb(182, 231, 233).ToUIColor();// Color..FromHex("#254f5e").ToUIColor();
            Control.TrackTintColor = Color.FromRgb(188, 203, 219).ToUIColor();
        }


        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            var X = 1.0f;
            var Y = 7.0f;

            CGAffineTransform transform = CGAffineTransform.MakeScale(X, Y);
            this.Transform = transform;



            this.ClipsToBounds = true;
            this.Layer.MasksToBounds = true;
            this.Layer.CornerRadius = 5; // This is for rounded corners.
        }
    }
}