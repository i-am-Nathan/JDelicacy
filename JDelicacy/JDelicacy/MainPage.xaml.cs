using JDelicacy.Models;
using Newtonsoft.Json;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace JDelicacy
{
	public partial class MainPage : ContentPage
	{
		public MainPage()
		{
			InitializeComponent();
		}

        private async void ShareButtonClicked(object sender, EventArgs e)
        {

        }

        private async void UploadButtonClicked(object sender, EventArgs e)
        {

        }

        private async void TakeButtonClicked(object sender, EventArgs e)
        {
            await CrossMedia.Current.Initialize();

            if (CrossMedia.Current.IsCameraAvailable && CrossMedia.Current.IsTakePhotoSupported)
            {
                var MediaOptions = new Plugin.Media.Abstractions.StoreCameraMediaOptions
                {
                    Directory = "Receipts",
                    Name = $"{DateTime.UtcNow}.jpg"
                };

                MediaFile file = await CrossMedia.Current.TakePhotoAsync(MediaOptions);

                if (file == null) return;

                foodImage.Source = ImageSource.FromStream(() =>
                {
                    return file.GetStream();
                });

                foodLabel.Text = "";
                shareButton.IsVisible = false;
                await MakePredictionRequest(file);

            }

        }

        private byte[] GetImageAsByteArray(MediaFile file)
        {
            var stream = file.GetStream();
            BinaryReader br = new BinaryReader(stream);
            return br.ReadBytes((int)stream.Length);
        }

        private async Task MakePredictionRequest(MediaFile file)
        {
            Contract.Ensures(Contract.Result<Task>() != null);
            var client = new HttpClient();

            client.DefaultRequestHeaders.Add("Prediction-Key", "9b2cd115e674489c892a12aa9b4747f2");

            string url = "https://southcentralus.api.cognitive.microsoft.com/customvision/v1.0/Prediction/9ced7a98-a868-45b5-a7ca-e9112877744a/image?iterationId=decd2fad-4dfe-4c98-a7f3-7b82e1b058e6";
            HttpResponseMessage response;

            byte[] byteData = GetImageAsByteArray(file);

            using (var content = new ByteArrayContent(byteData))
            {

                content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                response = await client.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    var responseString = await response.Content.ReadAsStringAsync();

                    EvaluationModel responseModel = JsonConvert.DeserializeObject<EvaluationModel>(responseString);

                    List<Prediction> predictions = responseModel.Predictions;

                    foreach (Prediction prediction in predictions)
                    {
                        if (prediction.Probability >= 0.8)
                        {
                            foodLabel.Text = prediction.Tag;
                            shareButton.IsVisible = true;
                            file.Dispose();
                            return;
                        }
                    }
                    foodLabel.Text = "What is this? Is this even food?";
                }

                //Get rid of file once we have finished using it
                file.Dispose();
            }
        }
    }
}
