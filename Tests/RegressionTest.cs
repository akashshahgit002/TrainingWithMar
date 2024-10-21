using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainingWithAutomation.Helpers;

namespace TrainingWithAutomation.Tests
{
    public class RegressionTest
    {
        private ApiHelper apiHelper;
        private readonly string baseUrl = "https://jsonplaceholder.typicode.com/";

        [SetUp]
        public void Setup()
        {
            var client = new HttpClient
            {
                BaseAddress = new Uri(baseUrl)
            };
            apiHelper = new ApiHelper(client);
        }

        [Test]
        public async Task getRequest()
        {
            string reponseData = await apiHelper.GetAsync("/posts/1");
            Console.WriteLine("Get Api Response Data is:- " + reponseData);
        }

        [Test]
        public async Task PostRequest()
        {
            var postData = new
            {
                userId = 1,
                title = "selenium training with Team",
                body= "This is our body"
            };
            string jsonContent = JsonConvert.SerializeObject(postData);
            string responseData = await apiHelper.PostAsync("posts", jsonContent);
            Console.WriteLine("Post request response:- "+responseData);
        }

        [Test]
        public async Task PutRequest()
        {
            var putData = new
            {
                userId = 1,
                title = "updated title",
                body = "updated body"
            };
            string jsonContent = JsonConvert.SerializeObject(putData);
            string reponseData = await apiHelper.PutAsync("/posts/1", jsonContent);
            Console.WriteLine("Put Response"+ reponseData);
        }

        [Test]
        public async Task DeleteRequest()
        {
            string reponseData = await apiHelper.DeleteAsync("/posts/1");
            Console.WriteLine("Delete Response" + reponseData);
        }

        [Test]
        public async Task getRequestForRapidAPI()
        {
            string reponseData = await apiHelper.GetAsync("https://rapidapi.com");
            Console.WriteLine("Get Api Response Data is:- " + reponseData);
        }

    }
}
