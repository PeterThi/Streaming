using Microsoft.AspNetCore.Mvc;
using Polly;
using Polly.Retry;

namespace CatalogService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CatalogController : ControllerBase
    {

        public CatalogController()
        {

        }

        [HttpPost]
        public string Post(string Title, int Length, string Review)
        {
            var retryPolicy = Policy.Handle<Exception>()
                .WaitAndRetry(3,
                  retryAttempt => TimeSpan.FromSeconds(2),
                  (exception, TimeSpan, retryCount, context) =>
                  {
                      Console.WriteLine("Failed to post, retrying nr. : " + retryCount);
                  });

            retryPolicy.Execute(() =>
            {
                var client = new HttpClient();
                client.BaseAddress = new Uri("http://catalogdbservice/CatalogDb");

                var request = new HttpRequestMessage(HttpMethod.Post, client.BaseAddress + "?titleFromParam=" + Title + "&lengthFromParam=" + Length + "&reviewFromParam=" + Review);
                var response = client.SendAsync(request);

                Console.WriteLine("Posted new Movie");
                string result = response.Result.Content.ReadAsStringAsync().Result;
                return result;
            });

            return "Nothing posted!";
            


        }

        [HttpGet]
        public string Get()
        {

            var retryPolicy = Policy.Handle<Exception>()
     .WaitAndRetry(3,
       retryAttempt => TimeSpan.FromSeconds(2),
       (exception, TimeSpan, retryCount, context) =>
       {
           Console.WriteLine("Failed to post, retrying nr. : " + retryCount);
       });

            var bresult = retryPolicy.Execute(()  =>
            {
                var client = new HttpClient();
                client.BaseAddress = new Uri("http://catalogdbservice/CatalogDb");

                var request = new HttpRequestMessage(HttpMethod.Get, client.BaseAddress);
                var response = client.SendAsync(request);
                var result = response.Result.Content.ReadAsStringAsync().Result;
                Console.WriteLine("from catalog controller" + result);
                return result;
            });

            //this goes through no matter what for some reason
            
            return bresult;
        }
    }
}