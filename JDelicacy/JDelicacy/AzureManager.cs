using JDelicacy.Models;
using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace JDelicacy
{
    class AzureManager
    {
        private static AzureManager instance;
        private MobileServiceClient client;
        private IMobileServiceTable<FoodLocationModel> foodLocationTable;

        private AzureManager()
        {
            this.client = new MobileServiceClient("http://yumwhatsthis.azurewebsites.net");
            this.foodLocationTable = this.client.GetTable<FoodLocationModel>();
        }

        public async Task PostFoodLocation(FoodLocationModel model)
        {
            try
            {
                await this.foodLocationTable.InsertAsync(model);
            }
            catch (Exception e)
            {
                Console.WriteLine("{0} Exception caught", e);
            }
        }

        public MobileServiceClient AzureClient
        {
            get { return client; }
        }

        public static AzureManager AzureManagerInstance
        {
            get
            {
                if (instance == null)
                {
                    instance = new AzureManager();
                }
                return instance;
            }
        }

        public async Task<List<FoodLocationModel>> GetFoodLocationInfo()
        {
            return await this.foodLocationTable.ToListAsync();
        }
    }
}
