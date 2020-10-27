using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace BonAppetit.Web.Models
{
    public class AddDistanceResponse:AddDistanceRequest
    {
        public double Distance { get; set; }

        public AddDistanceResponse()
        {

        }

        public double CalcDist()
        {
            double rlat1 = Math.PI * Start.Lat / 180;
            double rlat2 = Math.PI * End.Lat / 180;
            double theta = Start.Longt - End.Longt;
            double rtheta = Math.PI * theta / 180;
            double dist =
                Math.Sin(rlat1) * Math.Sin(rlat2) + Math.Cos(rlat1) *
                Math.Cos(rlat2) * Math.Cos(rtheta);
            dist = Math.Acos(dist);
            dist = dist * 180 / Math.PI;
            dist = dist * 60 * 1.1515;
            this.Distance = dist * 1.609344;
            return Distance;

        }

        public AddDistanceResponse(AddDistanceRequest addDistanceRequest)
        {
            this.Start = addDistanceRequest.Start;
            this.End = addDistanceRequest.End;
        }
    }
}
