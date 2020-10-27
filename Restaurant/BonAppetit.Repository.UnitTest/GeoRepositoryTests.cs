using System;
using System.Collections.Generic;
using System.Text;
using BonAppetit.Model.Entities;
using BonAppetit.Repository.Implementations;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace BonAppetit.Repository.UnitTest
{
    public class GeoRepositoryTests
    {
        //[Fact]
        //public void CreateGeoDistance_WithValidObject()
        //{
        //    //Arrange                        
        //    var geoDistance = new GeoDistance()
        //    {
        //        StartLat = (float)1.01,
        //        StartLongt = (float)4.76,
        //        EndLat = (float)2.23,
        //        EndLogt = (float)2.323,
        //        RequestDateTime = DateTime.Now
        //    };
        //    var sut = CreateSUT();
        //    //Act
        //    sut.AddGeoDistance(geoDistance);

        //    //Assert
        //    Assert.True(geoDistance.Id >= 0);
        //}

        //private GeoDistanceRepository CreateSUT()
        //{
        //    var dbOptions = new DbContextOptionsBuilder<BonAppetitDbContext>()
        //        .UseInMemoryDatabase(databaseName: "GeoDb")
        //        .Options;

        //    var context = new BonAppetitDbContext(dbOptions);

        //    return new GeoDistanceRepository(context);
        //}
    }
}
