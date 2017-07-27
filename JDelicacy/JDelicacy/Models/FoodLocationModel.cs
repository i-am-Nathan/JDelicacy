using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace JDelicacy.Models
{
    class FoodLocationModel
    {
        [JsonProperty(PropertyName = "Id")]
        public string ID { get; set; }

        [JsonProperty(PropertyName = "Title")]
        public string Title { get; set; }

        [JsonProperty(PropertyName = "Longitude")]
        public float Longitude { get; set; }

        [JsonProperty(PropertyName = "Latitude")]
        public float Latitude { get; set; }

        public string City { get; set; }
    }
}
