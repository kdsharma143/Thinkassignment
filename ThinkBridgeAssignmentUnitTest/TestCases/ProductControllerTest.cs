using BusinessAccess.RequestModel;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ThinkBridgeAssignment;
using Xunit;

namespace ThinkBridgeAssignmentUnitTest.TestCases
{
    public class ProductControllerTest : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;

        public ProductControllerTest(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task Product_Model_State_Test()
        {
            var client = _factory.CreateClient();
            AddProductRequestModel objProduct = new AddProductRequestModel
            {
                CategoryId = 0,
                Description = "",
                Name="",
                Price=0
            };
            var response = await client.PostAsync("api/Product/Add", new StringContent(
                JsonConvert.SerializeObject(objProduct),
                Encoding.UTF8,
                "application/json"));
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        }

        [Fact]
        //Pass valid Model
        //Pass categoryid which not avalable in database
        public async Task Product_Category_Not_Found_Test()
        {
            var client = _factory.CreateClient();
            AddProductRequestModel objProduct = new AddProductRequestModel
            {
                CategoryId = 2323,
                Description = "Test",
                Name = "TestProduct",
                Price = 2112
            };
            var response = await client.PostAsync("api/Product/Add", new StringContent(
                JsonConvert.SerializeObject(objProduct),
                Encoding.UTF8,
                "application/json"));
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

        }

        [Fact]
        //Pass valid Model
        //Pass a valid category
        //Pass Product Name which already exists in passing category
        public async Task Same_Product_Name_In_Category_Test()
        {
            var client = _factory.CreateClient();

            string name = TestHelpers.GetRandomString();

            AddProductRequestModel objProduct = new AddProductRequestModel
            {
                CategoryId = 1,
                Description = name,
                Name = name,
                Price = 100
            };
            //Adding Product to check same name
            await client.PostAsync("api/Product/Add", new StringContent(
            JsonConvert.SerializeObject(objProduct),
            Encoding.UTF8,
            "application/json"));

            //Adding Stock in 
            TransactionRequestModel objTransaction = new TransactionRequestModel
            {
                ProductId = 1,
                Quantity = 10,
                Type = "Add"
            };

            await client.PostAsync("api/Transaction/StockTansaction", new StringContent(
                JsonConvert.SerializeObject(objTransaction),
                Encoding.UTF8,
                "application/json"));


            var response = await client.PostAsync("api/Product/Add", new StringContent(
                JsonConvert.SerializeObject(objProduct),
                Encoding.UTF8,
                "application/json"));
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

        }

        [Fact]
        //Pass valid Model
        //Pass a product which not exists in passing category
        public async Task Product_Add_Test()
        {
            var client = _factory.CreateClient();
            string name = TestHelpers.GetRandomString();

            AddProductRequestModel objProduct = new AddProductRequestModel
            {
                CategoryId = 1,
                Description = name,
                Name = name,
                Price = 100
            };
            var response = await client.PostAsync("api/Product/Add", new StringContent(
                JsonConvert.SerializeObject(objProduct),
                Encoding.UTF8,
                "application/json"));
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        }


        [Fact]
        //Pass valid Model
        //Pass a product product id which not available in database
        public async Task Update_Product_Not_Found_Test()
        {
            var client = _factory.CreateClient();
            string name = TestHelpers.GetRandomString();

            AddProductRequestModel objProduct = new AddProductRequestModel
            {
                CategoryId = 1,
                Description = name,
                Name = name,
                Price = 100
            };
            var response = await client.PutAsync("api/Product/Update/676767", new StringContent(
                JsonConvert.SerializeObject(objProduct),
                Encoding.UTF8,
                "application/json"));
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

        }

        [Fact]
        //Pass valid Model
        //Pass a product name which not available in  passing category
        //Pass productid which exist in database
        public async Task Update_Product_Test()
        {
            var client = _factory.CreateClient();
            string name = TestHelpers.GetRandomString();

            AddProductRequestModel objProduct = new AddProductRequestModel
            {
                CategoryId = 1,
                Description = name,
                Name = name,
                Price = 100
            };
            var response = await client.PutAsync("api/Product/Update/1", new StringContent(
                JsonConvert.SerializeObject(objProduct),
                Encoding.UTF8,
                "application/json"));
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        }


        [Fact]
        public async Task Get_Products_Test()
        {
            var client = _factory.CreateClient();

            var response = await client.GetAsync("api/Product/GetProducts");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        }

        [Fact]
        //Pass Product id which is not available in database
        public async Task Get_Product_Not_Found_Test()
        {
            var client = _factory.CreateClient();

            var response = await client.GetAsync("api/Product/GetProduct/999999");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

        }

        [Fact]
        //Pass Product id which is available in database
        public async Task Get_Product_Test()
        {
            var client = _factory.CreateClient();

            var response = await client.GetAsync("api/Product/GetProduct/1");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        }


        [Fact]
        //Pass Product id which is available in database and have some stock
        public async Task Delete_Product_Which_Have_Stock_Test()
        {
            var client = _factory.CreateClient();

            var response = await client.DeleteAsync("api/Product/Delete/1");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

        }

        [Fact]
        //Pass Product id which is available in database and don't have stock 
        public async Task Delete_Product_Test()
        {
            var client = _factory.CreateClient();

            var response = await client.DeleteAsync("api/Product/Delete/2");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        }
    }
}
