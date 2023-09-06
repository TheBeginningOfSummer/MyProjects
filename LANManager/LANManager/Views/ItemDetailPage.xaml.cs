using LANManager.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace LANManager.Views
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