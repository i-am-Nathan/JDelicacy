using JDelicacy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace JDelicacy
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class FoodLocationPage : ContentPage
	{
        Geocoder geoCoder;

        public FoodLocationPage ()
		{
			InitializeComponent();
            geoCoder = new Geocoder();
		}

        private async void UpdateButtonClicked(object sender, EventArgs e)
        {

            List<FoodLocationModel> foodLocationInfo = await AzureManager.AzureManagerInstance.GetFoodLocationInfo();
            FoodList.ItemsSource = foodLocationInfo;
                
            
        }

    }
}