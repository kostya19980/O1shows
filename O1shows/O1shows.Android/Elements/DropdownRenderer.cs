using System.ComponentModel;
using Android.Content;
using Android.Graphics;
using Android.Widget;
using AndroidX.AppCompat.Widget;
using AndroidX.Core.Graphics;
using O1shows.Droid.Elements;
using O1shows.Elements.Dropdown;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Color = Xamarin.Forms.Color;

[assembly: ExportRenderer(typeof(Dropdown), typeof(DropdownRenderer))]
namespace O1shows.Droid.Elements
{
    public class DropdownRenderer : ViewRenderer<Dropdown, AppCompatSpinner>
    {
        AppCompatSpinner spinner;
        public DropdownRenderer(Context context) : base(context)
        {

        }

        protected override void OnElementChanged(ElementChangedEventArgs<Dropdown> e)
        {
            base.OnElementChanged(e);

            if (Control == null)
            {
                //Color backgroundColor = (Color)Application.Current.Resources["grey-100"];
                spinner = new AppCompatSpinner(Context);
                spinner.Background.SetColorFilter(BlendModeColorFilterCompat.CreateBlendModeColorFilterCompat(Color.White.ToAndroid(), BlendModeCompat.SrcAtop));
                //spinner.SetPopupBackgroundDrawable(new ColorDrawable(backgroundColor.ToAndroid()));
                SetNativeControl(spinner);
            }
            if (e.OldElement != null)
            {
                Control.ItemSelected -= OnItemSelected;
            }
            if (e.NewElement != null)
            {
                var view = e.NewElement;
                ArrayAdapter adapter = new ArrayAdapter(Context, Android.Resource.Layout.SimpleListItem1, view.ItemsSource);
                Control.Adapter = adapter;

                if (view.SelectedIndex != -1)
                {
                    Control.SetSelection(view.SelectedIndex);
                }

                Control.ItemSelected += OnItemSelected;
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var view = Element;
            Control.DropDownVerticalOffset = 55;
            if (e.PropertyName == Dropdown.ItemsSourceProperty.PropertyName)
            {
                ArrayAdapter adapter = new ArrayAdapter(Context, Android.Resource.Layout.SimpleListItem1, view.ItemsSource);
                Control.Adapter = adapter;
            }
            if (e.PropertyName == Dropdown.SelectedIndexProperty.PropertyName)
            {
                Control.SetSelection(view.SelectedIndex);
            }
            base.OnElementPropertyChanged(sender, e);
        }

        private void OnItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            var view = Element;
            if (view != null)
            {
                view.SelectedIndex = e.Position;
                view.OnItemSelected(e.Position);
                TextView textView = (TextView)spinner.SelectedView;
                if(view.TextColor != null)
                {
                    textView.SetTextColor(view.TextColor.ToAndroid());
                    textView.TextSize = (float)view.FontSize;
                }
                //textView.SetBackgroundColor(Color.Red.ToAndroid());
                if (!view.IsArrowVisible)
                {
                    textView.SetPadding(0, 0, 0, 0);
                    textView.SetPaddingRelative(0, 0, 0, 0);
                    spinner.SetPadding(4, 4, 4, 4);
                    spinner.Background = null;
                    textView.Gravity = Android.Views.GravityFlags.CenterHorizontal;
                }
            }
        }
    }
}