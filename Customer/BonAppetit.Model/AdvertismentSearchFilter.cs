using System;
using System.Collections.Generic;
using System.Text;

namespace BonAppetit.Model
{
    public class AdvertismentSearchFilter
    {
        public float? MinPrice { get; set; }
        public float? MaxPrice { get; set; }
        public string ProvidenceCode { get; set; }
        public string CityCode { get; set; }
        public string CityPartCode { get; set; }
        public string VillageCode { get; set; }
        public string VillagePartCode { get; set; }
        public string UserId { get; set; }
        public int PageNumber { get; set; }
    }
}
