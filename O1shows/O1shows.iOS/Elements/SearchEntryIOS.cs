using CoreGraphics;
using O1shows.Elements;
using O1shows.iOS.Elements;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
[assembly: ExportRenderer(typeof(SearchEntry), typeof(SearchEntryIOS))]
namespace O1shows.iOS.Elements
{
    class SearchEntryIOS : EntryRenderer
    {
        public SearchEntryIOS()
        {
        }
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);
            if (e.OldElement == null)
            {
                Control.Layer.BackgroundColor = Color.FromHex("#444447").ToCGColor();
                Control.Layer.CornerRadius = 8;
                Control.BorderStyle = UITextBorderStyle.None;
                Control.LeftView = new UIView(new CGRect(0, 0, 10, 0));
                Control.LeftViewMode = UITextFieldViewMode.Always;
                Control.TintColor = UIColor.White;
            }
        }
    }
}