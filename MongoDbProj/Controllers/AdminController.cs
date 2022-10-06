using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using MongoDbProj.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using MongoDbProj.AppConfig;

namespace MongoDbProj.Controllers
{
    public class AdminController : Controller
    {
        clsMongoDBDataContext _dbContextAdmini = new clsMongoDBDataContext("admini");

        public IActionResult osnovniPrikazAdminu()
        {
            return View();
        }

        public IActionResult idiNaRegistrujAdmin()
        {
            Admin k = new Admin();

            return View(k);
        }
     
        public IActionResult registrujAdmin(Admin p)
        {
            if (p.Password == null || p.Email == null)
            {
                var a = "Popuni sva polja!";
                TempData["msgPopuni"]= JsonConvert.SerializeObject(a.ToString());

                return  RedirectToAction("idiNaRegistrujAdmin","Admin");
            }
            else
            {
                var nameFilter = Builders<Admin>.Filter.Eq("Password", p.Password) & Builders<Admin>.Filter.Eq("Email", p.Email);
                var admin = this._dbContextAdmini.GetAdmini.Find(nameFilter).FirstOrDefault();

                if (admin == null)
                {
                    p.Id = ObjectId.GenerateNewId();
                    this._dbContextAdmini.GetAdmini.InsertOne(p);

                    return  RedirectToAction("osnovniPrikazAdminu","Admin");
                }
                else
                {
                    TempData["msgPostoji"] = JsonConvert.SerializeObject("Nalog vec postoji!");

                    return RedirectToAction("idiNaRegistrujAdmin","Admin");

                }
            }
        }

        public IActionResult idiNaLogAdmin()
        {
            Admin k = new Admin();
            return View(k);
        }

        public IActionResult logAdmin(Admin k)
        {

            var nameFilter = Builders<Admin>.Filter.Eq("Password", k.Password) & Builders<Admin>.Filter.Eq("Email", k.Email);
            var korisnik = this._dbContextAdmini.GetAdmini.Find(nameFilter).FirstOrDefault();

            if (korisnik != null)
            {
                return RedirectToAction("osnovniPrikazAdminu", "Admin");
            }
            else
            {
                TempData["msgGreska"] = JsonConvert.SerializeObject("Pogresan password ili Email!");

                return RedirectToAction("idiNaLogAdmin","Admin");
            }
        }
    }
}
