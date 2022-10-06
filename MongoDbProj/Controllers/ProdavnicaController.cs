using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MongoDbProj.Models;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDbProj.AppConfig;

namespace MongoDbProj.Controllers
{
    public class ProdavnicaController : Controller
    {
        clsMongoDBDataContext _dbContext = new clsMongoDBDataContext("prodavnice");
    
        public async Task<ActionResult> prikaziProdavnice()
        {
            IEnumerable<Prodavnica> products = null;
            using (IAsyncCursor<Prodavnica> cursor = await this._dbContext.GetProdavnice.FindAsync(new BsonDocument()))
            {
                while (await cursor.MoveNextAsync())
                {
                    products = cursor.Current;
                }
            }
            return View(products);
        }
    }
}