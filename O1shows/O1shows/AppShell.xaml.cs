using O1shows.Views;
using O1shows.Views.Account;
using O1shows.Views.SeriesCatalog;
using Xamarin.Forms;

namespace O1shows
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            SetNavBarIsVisible(this, false);
        }

    }
}
