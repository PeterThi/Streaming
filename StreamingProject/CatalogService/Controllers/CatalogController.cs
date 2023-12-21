using Microsoft.AspNetCore.Mvc;

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
            var client = new HttpClient();
            client.BaseAddress = new Uri("http://catalogdbservice/CatalogDb");

            var request = new HttpRequestMessage(HttpMethod.Post, client.BaseAddress + "?titleFromParam=" + Title + "&lengthFromParam=" + Length + "&reviewFromParam=" + Review);
            var response = client.SendAsync(request);

            Console.WriteLine("Posted new Movie");
            string result = response.Result.Content.ReadAsStringAsync().Result;
            return result;


        }
    }
}