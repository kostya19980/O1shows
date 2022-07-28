using O1shows.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace O1shows.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}