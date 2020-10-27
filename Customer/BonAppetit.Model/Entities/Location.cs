using System;
using System.Collections.Generic;
using System.Text;

namespace BonAppetit.Model.Entities
{
    public class Location
    {
        public long Id { get; set; }
        public string ProvidenceName { get; set; }
        public string ProvidenceCode { get; set; }
        public string CityName { get; set; }
        public string CityCode { get; set; }
        public string CityPartName { get; set; }
        public string CityPartCode { get; set; }
        public string VillageName { get; set; }
        public string VillageCode { get; set; }
        public string VillagePartName { get; set; }
        public string VillagePartCode { get; set; }
    }
}
