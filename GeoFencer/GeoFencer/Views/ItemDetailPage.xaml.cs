using GeoFencer.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace GeoFencer.Views
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