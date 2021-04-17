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
    public class TransactionControllerTest : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;

        public TransactionControllerTest(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        //Pass invalid model 
        public async Task Transaction_Model_State_Test()
        {
            var client = _factory.CreateClient();
            TransactionRequestModel objTransaction = new TransactionRequestModel
            {
                ProductId = 0,
                Quantity = 0
            };
            var response = await client.PostAsync("api/Transaction/StockTansaction", new StringContent(
                JsonConvert.SerializeObject(objTransaction),
                Encoding.UTF8,
                "application/json"));
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        }

        [Fact]
        //Pass valid model
        //Pass product id which is not stock table
        public async Task Transaction_Product_Not_Found_Test()
        {
            var client = _factory.CreateClient();
            TransactionRequestModel objTransaction = new TransactionRequestModel
            {
                ProductId = 99999,
                Quantity = 10,
                Type="Add"
            };
            var response = await client.PostAsync("api/Transaction/StockTansaction", new StringContent(
                JsonConvert.SerializeObject(objTransaction),
                Encoding.UTF8,
                "application/json"));
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

        }

        [Fact]
        //Pass valid model
        //Pass Add in Type 
        //Pass quantity more than 0
        public async Task Transaction_Stock_Add_Test()
        {
            var client = _factory.CreateClient();
            TransactionRequestModel objTransaction = new TransactionRequestModel
            {
                ProductId = 1,
                Quantity = 10,
                Type = "Add"
            };
            var response = await client.PostAsync("api/Transaction/StockTansaction", new StringContent(
                JsonConvert.SerializeObject(objTransaction),
                Encoding.UTF8,
                "application/json"));
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        }


        [Fact]
        //Pass valid model
        //Pass Sale in Type 
        //Pass quantity more than available quantity in stock
        public async Task Transaction_Stock_Remove_Quantity_Not_Found_Test()
        {
            var client = _factory.CreateClient();
            TransactionRequestModel objTransaction = new TransactionRequestModel
            {
                ProductId = 1,
                Quantity = 55,
                Type = "Sale"
            };
            var response = await client.PostAsync("api/Transaction/StockTansaction", new StringContent(
                JsonConvert.SerializeObject(objTransaction),
                Encoding.UTF8,
                "application/json"));
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

        }


        [Fact]
        //Pass valid model
        //Pass Sale in Type 
        //Pass quantity less than available quantity in stock
        public async Task Transaction_Stock_Sale_Quantity_Test()
        {
            var client = _factory.CreateClient();
            TransactionRequestModel objTransaction = new TransactionRequestModel
            {
                ProductId = 1,
                Quantity = 5,
                Type = "Sale"
            };
            var response = await client.PostAsync("api/Transaction/StockTansaction", new StringContent(
                JsonConvert.SerializeObject(objTransaction),
                Encoding.UTF8,
                "application/json"));
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        }


        [Fact]     
        public async Task Transaction_Get_Transactions_Test()
        {
            var client = _factory.CreateClient();
            var response = await client.GetAsync("api/Transaction/Gettransactions");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        }

        [Fact]
        public async Task Transaction_Get_Stock_Report_Test()
        {
            var client = _factory.CreateClient();
            var response = await client.GetAsync("api/Transaction/Getstockrepot");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        }
    }
}
