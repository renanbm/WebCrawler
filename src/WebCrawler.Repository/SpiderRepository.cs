using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;
using WebCrawler.CrossCutting;
using WebCrawler.Domain.Interfaces;
using WebCrawler.Domain.Results;
using WebCrawler.Repository.Models;

namespace WebCrawler.Repository
{
    public class SpiderRepository : ISpiderRepository
    {
        private const string DbName = "web_crawler";
        private readonly Credentials _cloudantCredentials;


        public SpiderRepository(Credentials credentials)
        {
            _cloudantCredentials = credentials;
        }

        public async Task CreateAsync(SpiderData item)
        {
            using (var client = CloudantClient())
            {
                var response = await client.PostAsync(DbName, CreateJsonContent(item));
                if (response.IsSuccessStatusCode)
                {
                    var responseJson = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<UpdateResult>(responseJson);
                    item.id = result.id;
                    item.rev = result.rev;
                }
            }
        }

        public async Task<Rootobject> GetAllAsync()
        {
            using (var client = CloudantClient())
            {
                var response = await client.GetAsync(DbName + "/_all_docs?include_docs=true");
                if (!response.IsSuccessStatusCode) return null;
                var responseJson = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Rootobject>(responseJson);
            }
        }

        public async Task UpdateAsync(SpiderData item)
        {
            using (var client = CloudantClient())
            {
                var response = await client.PutAsync($"{DbName}/{HttpUtility.UrlEncode(item.id)}?rev={HttpUtility.UrlEncode(item.rev)}", CreateJsonContent(item));
                if (response.IsSuccessStatusCode)
                {
                    var responseJson = await response.Content.ReadAsStringAsync();
                    var responseReturn = JsonConvert.DeserializeObject<UpdateResult>(responseJson);

                    lock (item)
                    {
                        item.id = responseReturn.id;
                        item.rev = responseReturn.rev;
                    }
                }
            }
        }

        private static JsonContent CreateJsonContent(SpiderData item)
        {
            lock (item)
                return new JsonContent(item);
        }

        private HttpClient CloudantClient()
        {
            if (_cloudantCredentials.username == null || _cloudantCredentials.password == null || _cloudantCredentials.host == null)
            {
                throw new Exception("Missing Cloudant NoSQL DB service credentials");
            }

            var auth = Convert.ToBase64String(Encoding.ASCII.GetBytes(_cloudantCredentials.username + ":" + _cloudantCredentials.password));

            var client = new HttpClient { BaseAddress = new Uri("https://" + _cloudantCredentials.host) };
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", auth);
            return client;
        }
    }
}