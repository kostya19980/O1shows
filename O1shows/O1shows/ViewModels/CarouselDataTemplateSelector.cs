using O1shows.Views;
using O1shows.Views.SeriesCatalog;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace O1shows.ViewModels
{
    public class CarouselDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate Tab1 { get; set; }
        public DataTemplate Tab2 { get; set; }

        public CarouselDataTemplateSelector()
        {
            Tab1 = new DataTemplate(typeof(SeriesListView));
            Tab2 = new DataTemplate(typeof(RecommendationsView));
        }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            var position = $"{item}";

            if (position == "1")
            {
                return Tab1;
            }
            return Tab2;
        }
    }
}
