using Android.Content;
using Android.Graphics.Drawables;
using Android.Runtime;
using Android.Widget;
using AndroidX.Core.Content;
using O1shows.Droid.Elements;
using O1shows.Elements;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(SearchEntry), typeof(SearchEntryAndroid))]
namespace O1shows.Droid.Elements
{
    class SearchEntryAndroid : EntryRenderer
    {
        public SearchEntryAndroid(Context context) : base(context)
        {
            //this.SetBackgroundColor(Color.FromHex("#373942").ToAndroid());
            //var gradientDrawable = new GradientDrawable();
            //gradientDrawable.SetCornerRadius(60f);
            //gradientDrawable.SetStroke(1, Android.Graphics.Color.Transparent);
            //gradientDrawable.SetColor(Color.FromHex("#373942").ToAndroid());
            //Control.SetBackground(gradientDrawable);
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);
            if (e.OldElement == null)
            {
                int borderHeight = 2;
                var gradientDrawable = new GradientDrawable();
                //gradientDrawable.SetCornerRadius(30f);
                gradientDrawable.SetStroke(borderHeight, Android.Graphics.Color.Transparent);
                gradientDrawable.SetColor(Color.FromHex("#444447").ToAndroid());
                Control.SetBackground(gradientDrawable);
                Control.SetPadding(40, Control.PaddingTop, 20, Control.PaddingBottom);
                //e.NewElement.Unfocused += (sender, evt) =>
                //{
                //    gradientDrawable.SetStroke(borderHeight, Android.Graphics.Color.Transparent);
                //    Control.SetBackground(gradientDrawable);
                //};
                //e.NewElement.Focused += (sender, evt) =>
                //{
                //    gradientDrawable.SetStroke(borderHeight, Color.FromHex("#AB47F2").ToAndroid());
                //    Control.SetBackground(gradientDrawable);
                //};
                //IntPtr IntPtrtextViewClass = JNIEnv.FindClass(typeof(TextView));
                //IntPtr mCursorDrawableResProperty = JNIEnv.GetFieldID(IntPtrtextViewClass, "mCursorDrawableRes", "I");
                //JNIEnv.SetField(Control.Handle, mCursorDrawableResProperty, 0);
                //MainActivity.Instance.Window
            }
        }
    }
}