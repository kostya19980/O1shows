using O1shows.Models;
using O1shows.Services;
using O1shows.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace O1shows.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EpisodePage : ContentPage
    {
        private EpisodeViewModel viewModel;
        public int BottomBarHeight;
        public EpisodePage(EpisodeViewModel model)
        {
            viewModel = model;
            BindingContext = viewModel;
            InitializeComponent();
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            BottomBarHeight = DependencyService.Get<IStatusBarStyleManager>().GetBottomNavBarHeight();
            SendMessage.Margin = new Thickness(0,0,0,46);
        }
        public async void ClosingPage(object sender, PanUpdatedEventArgs e)
        {
            switch (e.StatusType)
            {
                case GestureStatus.Running:
                    if (pageContent.TranslationY + e.TotalY < 0)
                    {
                        pageContent.TranslationY = 0;
                    }
                    else
                    {
                        pageContent.TranslationY += e.TotalY;
                    }
                    break;
                case GestureStatus.Completed:
                    if (pageContent.TranslationY > 250)
                    {
                        await Navigation.PopModalAsync();
                    }
                    else
                    {
                        await pageContent.TranslateTo(0, 0);
                    }
                    break;
            }
        }
        private async void ClosePage_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
        private void ClearSearchEntry(object sender, EventArgs e)
        {
            CommentEntry.Text = "";
        }
        private void OnTextChanged(object sender, FocusEventArgs e)
        {
            if (CommentEntry.Text != "")
            {
                clearSearchButton.IsVisible = true;
            }
            else
            {
                clearSearchButton.IsVisible = false;
            }
        }

        private async void SendComment_Clicked(object sender, EventArgs e)
        {
            string MessageText = CommentEntry.Text;
            Comment comment = await viewModel.ProfileService.AddComment(MessageText, viewModel.Episode.Id);
            if(comment != null)
            {
                comment.UserProfile.ImageSrc = "http://192.168.0.9/" + comment.UserProfile.ImageSrc;
                viewModel.Episode.Comments.Add(comment);
            }
        }
    }
}