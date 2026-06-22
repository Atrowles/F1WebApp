using F1WebAPI.Controllers;
using F1WebAPI.DTOs;
using F1WebAPI.Models;
using F1WebAPI.Repositories;
using F1WebAPI.Services;
using F1WebAPI.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web.Http;

namespace UnitTestF1API
{
    [TestClass]
    public class UnitTest1
    {

        [TestMethod]
        public async Task TestUpdate()
        {
            var updatedDriver = new Driver {Id=1, Name = "Updated Driver", Championships=2, CurrentPoints=12, Poles=1, RaceWins=1, TeamID=1 };

            List<Driver> drivers = new List<Driver>();

            drivers.Add(updatedDriver);


            var mockRepoDriver = new Mock<IRepositoryBase<Driver>>();
            mockRepoDriver
                .Setup(m => m.Update(updatedDriver))
                .Returns(Task.FromResult(updatedDriver));

            drivers.AsEnumerable<Driver>();

            mockRepoDriver
                .Setup(m=>m.GetWhereNoTracking(It.IsAny<Expression<Func<Driver, bool>>>()))
                .Returns(Task.FromResult(drivers.AsEnumerable<Driver>()));


            var mockRepoTeam = new Mock<IRepositoryBase<Team>>();

            var service = new DriverService(mockRepoDriver.Object, mockRepoTeam.Object);

            Driver result = await service.UpdateDriver(updatedDriver);

            Assert.AreEqual("Updated Driver", result.Name);
        }

        [TestMethod]
        public async Task TestInsert()
        {
            List<Driver> drivers = new List<Driver>();

            var newDriver = new Driver { Id = 99, Name = "New Driver", Championships = 1, CurrentPoints = 1, Poles = 6, RaceWins =0, TeamID = 1 };

            var mockRepoDriver = new Mock<IRepositoryBase<Driver>>();
            mockRepoDriver
                .Setup(m => m.Add(newDriver))
                .Returns(Task.FromResult(newDriver))
                .Callback((Driver d)=>drivers.Add(d));

            var mockRepoTeam = new Mock<IRepositoryBase<Team>>();

            var service = new DriverService(mockRepoDriver.Object, mockRepoTeam.Object);

            Driver result = await service.SaveDriver(newDriver);

            Assert.AreEqual("New Driver", result.Name);


        }


        [TestMethod]
        public async Task TestDelete()
        {
            var deletedDriver = new Driver { Id = 99, Name = "Deleted Driver", Championships = 1, CurrentPoints = 1, Poles = 6, RaceWins = 0, TeamID = 1 };
            List<Driver> drivers = new List<Driver>();

            drivers.Add(deletedDriver);

            var mockRepoDriver = new Mock<IRepositoryBase<Driver>>();
            mockRepoDriver
                .Setup(m => m.Delete(It.IsAny<long>()))
                .Returns(Task.FromResult(deletedDriver))
                .Callback((long id) => drivers.RemoveAll(x=>x.Id==id));

            var mockRepoTeam = new Mock<IRepositoryBase<Team>>();

            var service = new DriverService(mockRepoDriver.Object, mockRepoTeam.Object);

            Driver result = await service.DeleteDriver(99);

            Assert.AreEqual("Deleted Driver", result.Name);
            mockRepoDriver.Verify(r => r.Delete(99));
        }

        [TestMethod]
        public async Task TestGet()
        {
            //set up the mock repo

            FilterPageVM filter = new FilterPageVM();
            filter.SortStr = "";
            filter.SearchStr = "";

            var mockRepoDriver = new Mock<IRepositoryBase<Driver>>();
            var mockRepoTeam = new Mock<IRepositoryBase<Team>>();

            mockRepoDriver.Setup(repo => repo.GetAll())
                .Returns(Task.FromResult(GetTestDrivers()));

            mockRepoTeam.Setup(repo => repo.GetAll())
                .Returns(Task.FromResult(GetTestTeams()));

            var service = new DriverService(mockRepoDriver.Object, mockRepoTeam.Object);

            ResponseDTO actionResult = await service.GetDrivers(filter);
            
            //if it was an object use reflection to get value
           // PropertyInfo property = actionResult.GetType().GetProperty("TotalRecords"); 
            
           // var recordCount =  property.GetValue(actionResult, null);

            Assert.AreEqual(2, actionResult.TotalRecords);
        }

        private IEnumerable<Team> GetTestTeams()
        {

            return new List<Team>()
            {
                new Team()
                {
                    Id = 1,
                    Name = "John"
                    
                },
                new Team()
                {
                    Id = 2,
                    Name = "Doe"
                  

                }
            };

        }

        private IEnumerable<Driver> GetTestDrivers()
        {
  
           return new List<Driver>()
            {
                new Driver()
                {
                    Id = 1,
                    TeamID=1,
                    Name = "John"
                },
                new Driver()
                {
                    Id = 2,
                    Name = "Doe",
                    TeamID=2

                }
            };

        }
    }
}
