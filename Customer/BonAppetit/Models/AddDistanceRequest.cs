using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BonAppetit.Model.Entities;

namespace BonAppetit.Web.Models
{
    public class AddDistanceRequest
    {
        public GeoPoint Start { get; set; }
        public  GeoPoint End { get; set; }

        //public GeoDistance GetGeoDistance()
        //{
        //    return new GeoDistance()
        //    {
        //        StartLat = Start.Lat,
        //        StartLongt = Start.Longt,
        //        EndLat = End.Lat,
        //        EndLogt = End.Longt,
        //    };
        //}
    }
}
