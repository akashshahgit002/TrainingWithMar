using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainingWithAutomation.Helpers
{
    public class ApiHelper
    {
        private readonly HttpClient httpClient;
        public ApiHelper(HttpClient client)
        {
            httpClient = client;
        }

        //---Get Request
        public async Task<string> GetAsync(string url)
        {
            try
            {
                var reponse = await httpClient.GetAsync(url);
                reponse.EnsureSuccessStatusCode();
                return await reponse.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error occurred while making GET requesst to url {url}: {ex.Message}",ex);
            }
        }

        //--- Post Request
        public async Task<string> PostAsync(string url, string payload)
        {
            var content = new StringContent(payload, Encoding.UTF8,"application/json");
            var reponse = await httpClient.PostAsync(url, content);
            reponse.EnsureSuccessStatusCode();
            return await reponse.Content.ReadAsStringAsync();
        }

        //---Post Request using json file
        public async Task<string> PostFromFileAsync(string url, string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"The specified file does not exists: {filePath}");
            }
            var payload = await File.ReadAllTextAsync(filePath);
            return await PostAsync(url, payload);
        }

        //--Put Request
        public async Task<string> PutAsync(string url, string payload)
        {
            var content = new StringContent(payload,Encoding.UTF8,"application/json");
            var response = await httpClient.PutAsync(url,content);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> DeleteAsync(string url)
        {
            var reponse = await httpClient.DeleteAsync(url);
            reponse.EnsureSuccessStatusCode();
            return await reponse.Content.ReadAsStringAsync();
        }

        //--
        public void SetAuthorization(string token)
        {
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        }
    }
}
