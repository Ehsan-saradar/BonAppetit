using System;
using System.Collections.Generic;
using System.Text;
using AutoFixture;
using BonAppetit.Model.Entities;

namespace BonAppetit.Web.IntegrationTest.Factory
{
    public static class GeoDistanceFactory
    {
        private static readonly AutoFixture.Fixture Fixture = new AutoFixture.Fixture();

        //public static IEnumerable<GeoDistance> Create(int count = 1)
        //    => Fixture
        //        .Build<GeoDistance>()
        //        .With(x => x.Id, 0)
        //        .With(x => x.AppUser)
        //        .CreateMany(count);
    }
}
