using Microsoft.AspNetCore.Mvc;
using System.Data;
using MySqlConnector;
using Dapper;
namespace CatalogDbService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CatalogDbController : ControllerBase
    {

        private IDbConnection CatalogDatabaseConnection = new MySqlConnection("Server=catalogDatabase;Database=catalog-database;Uid=add-bruger;Pwd=C@ch3d1v2;");


        public CatalogDbController()
        {

        }

        [HttpPost]
        public string Post(string titleFromParam, int lengthFromParam, string reviewFromParam)
        {
            CatalogDatabaseConnection.Open();
            var tables = CatalogDatabaseConnection.Query<string>("SHOW TABLES LIKE 'Movies'");
            if (!tables.Any())
            {
                CatalogDatabaseConnection.Execute("CREATE TABLE Movies (Title TEXT, length int, review TEXT)");

            }
            CatalogDatabaseConnection.Execute("INSERT INTO Movies (Title,length,review) VALUES (@title, @length, @review)", new { title = titleFromParam, length = lengthFromParam, review = reviewFromParam });
            return titleFromParam + "inserted";
        }

        [HttpGet]
        public IEnumerable<string> Get()
        {
            IEnumerable<string> resultList = CatalogDatabaseConnection.Query<string>("SELECT title from Movies");
            resultList.Append(CatalogDatabaseConnection.Query<string>("SELECT length from Movies").ToString());
            foreach (var item in resultList)
            {
                Console.WriteLine(item.ToString());
            }
            return resultList;

        }
    }
}
