using Microsoft.AspNetCore.Mvc;
using System.Data;
using MySqlConnector;
using Dapper;

namespace UserDbService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserDatabaseController : ControllerBase
    {

        private IDbConnection CatalogDatabaseConnection = new MySqlConnection("Server=catalogDatabase;Database=catalog-database;Uid=add-bruger;Pwd=C@ch3d1v2;");


        public UserDatabaseController()
        {
        }

        [HttpPost]
        public string Post(string NameFromParam, int LevelFromParam)
        {
            CatalogDatabaseConnection.Open();
            var tables = CatalogDatabaseConnection.Query<string>("SHOW TABLES LIKE 'Users'");
            if (!tables.Any())
            {
                CatalogDatabaseConnection.Execute("CREATE TABLE Users (Name TEXT, Level int)");

            }
            CatalogDatabaseConnection.Execute("INSERT INTO Users (Name, Level) VALUES (@Name, @Level)", new { Name = NameFromParam, Level = LevelFromParam});
            return NameFromParam + "inserted";
        }

        [HttpGet]
        public IEnumerable<string> Get()
        {
            IEnumerable<string> resultList = CatalogDatabaseConnection.Query<string>("SELECT Name from Users");
            resultList.Append(CatalogDatabaseConnection.Query<string>("SELECT Level from Users").ToString());
            foreach (var item in resultList)
            {
                Console.WriteLine(item.ToString());
            }
            return resultList;

        }
    }
}