using BusinessAccess;
using BusinessAccess.Interfaces;
using BusinessAccess.RequestModel;
using BusinessAccess.Services;
using DataAccess.DataModel;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ThinkBridgeAssignment;
using ThinkBridgeAssignment.Controllers;
using Xunit;

namespace ThinkBridgeAssignmentUnitTest.CategoryTestCases
{
    public class CategoryControllerTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;

        public CategoryControllerTests(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task Category_Model_State_Test()
        {
            var client = _factory.CreateClient();
            AddCategoryRequestModel objCat = new AddCategoryRequestModel
            {
                Name = ""
            };
            var response = await client.PostAsync("api/Category/Add", new StringContent(
                JsonConvert.SerializeObject(objCat),
                Encoding.UTF8,
                "application/json"));
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        }

        [Fact]
        public async Task Add_Category_Name_Exists()
        {
            var client = _factory.CreateClient();

            var name = TestHelpers.GetRandomString();


            AddCategoryRequestModel objCat = new AddCategoryRequestModel
            {
                Name = name
            };
            //Adding Random Category 
            await client.PostAsync("api/Category/Add", new StringContent(
                JsonConvert.SerializeObject(objCat),
                Encoding.UTF8,
                "application/json"));

            // getting test result with already created category
            var response = await client.PostAsync("api/Category/Add", new StringContent(
                JsonConvert.SerializeObject(objCat),
                Encoding.UTF8,
                "application/json"));
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        }

        [Fact]
        public async Task Add_Category_Test()
        {
            var client = _factory.CreateClient();

            var name = TestHelpers.GetRandomString();

            AddCategoryRequestModel objCat = new AddCategoryRequestModel
            {
                Name = name
            };
            var response = await client.PostAsync("api/Category/Add", new StringContent(
                JsonConvert.SerializeObject(objCat),
                Encoding.UTF8,
                "application/json"));
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        }

        [Fact]
        public async Task Get_Categories_Test()
        {
            var client = _factory.CreateClient();

            var response = await client.GetAsync("api/Category/GetCategories");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        }

        [Fact]
        //For success need to pass id which is availble in Database
        public async Task Get_Category_Not_Found_Test()
        {
            var client = _factory.CreateClient();

            var response = await client.GetAsync("api/Category/GetCategory/999999");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

        }

        [Fact]
        //Need to Pass that id which is available in Database
        public async Task Get_Category_Test()
        {
            var client = _factory.CreateClient();
            var response = await client.GetAsync("api/Category/GetCategory/1");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        }      

        [Fact]
        //Need to Pass that id which is available in Database
        public async Task Category_Delete_Test()
        {
            var client = _factory.CreateClient();
            var response = await client.DeleteAsync("api/Category/Delete/2");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        }

        [Fact]
        //Need to ensure that category with id  must  exists before delete and must have products
        public async Task Category_Delete_Having_Product_Test()
        {
            var client = _factory.CreateClient();

            //Adding Random product to test
            string name = TestHelpers.GetRandomString();

            AddProductRequestModel objProduct = new AddProductRequestModel
            {
                CategoryId = 1,
                Description = name,
                Name = name,
                Price = 100
            };
            await client.PostAsync("api/Product/Add", new StringContent(
                JsonConvert.SerializeObject(objProduct),
                Encoding.UTF8,
                "application/json"));


            var response = await client.DeleteAsync("api/Category/Delete/1");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

        }

        [Fact]
        //Need to pass that id which available in database
        public async Task Update_Category_Test()
        {
            var client = _factory.CreateClient();
            var name = TestHelpers.GetRandomString();

            AddCategoryRequestModel objCat = new AddCategoryRequestModel
            {
                Name = name
            };
           
            var response = await client.PutAsync("api/Category/Update/1", new StringContent(
                JsonConvert.SerializeObject(objCat),
                Encoding.UTF8,
                "application/json"));
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        }

        [Fact]
        public async Task Update_Category_Not_Found_Test()
        {
            var client = _factory.CreateClient();

            var name = TestHelpers.GetRandomString();

            AddCategoryRequestModel objCat = new AddCategoryRequestModel
            {
                Name = name
            };
            //Here we need to ensure that category is not available in database
            var response = await client.PutAsync("api/Category/Update/999999", new StringContent(
                JsonConvert.SerializeObject(objCat),
                Encoding.UTF8,
                "application/json"));
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

        }
    }
}
