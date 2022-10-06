using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MongoDbProj.Models;
using MongoDB.Driver;
using MongoDB.Bson;
using Newtonsoft.Json;
using MongoDbProj.AppConfig;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace MongoDbProj.Controllers
{
    public class ProizvodController : Controller
    {
        clsMongoDBDataContext _dbContext = new clsMongoDBDataContext("proizvodi");
        clsMongoDBDataContext _dbContextProdavnica = new clsMongoDBDataContext("prodavnice");
        clsMongoDBDataContext _dbContextKorisnici = new clsMongoDBDataContext("korisnici");
        clsMongoDBDataContext _dbContextKomentari = new clsMongoDBDataContext("komentari");
        clsMongoDBDataContext _dbContextKorpe = new clsMongoDBDataContext("korpe");
        private readonly IWebHostEnvironment webHostEnvironment;

        public ProizvodController( IWebHostEnvironment hostEnvironment)
        {
            webHostEnvironment = hostEnvironment;
        }

        public IActionResult idiNaKreirajProizvod(string Id)
        {
            Proizvod p = new Proizvod();
            
            p.IdProdavnice = JsonConvert.DeserializeObject<string>((string)TempData["idProdavnice"]); ;
            return View(p);
        }

        public IActionResult kreirajPoizvod(Proizvod p)
        {
            var idProd = ObjectId.Parse(p.IdProdavnice);
            var nameFilter = Builders<Prodavnica>.Filter.Eq("_id", idProd);

            var prod = this._dbContextProdavnica.GetProdavnice.Find(nameFilter).FirstOrDefault();

            if(prod!= null)
            {
                p.Id = ObjectId.GenerateNewId();
                p.BrojOcena = 0;
                p.SrednjaOcena = 0;
                prod.proizvodi.Add(p.Id);
                p.prodavnica = prod.Id;

                string putanjaSlike=dodajSLiku(p.Slika, p.Id);
            
                p.PutanjaSlike = putanjaSlike;
                p.Slika = null;
                var update = Builders<Prodavnica>.Update.Set("proizvodi", prod.proizvodi);
                this._dbContextProdavnica.GetProdavnice.UpdateOne(nameFilter, update);
                this._dbContext.GetProizvodi.InsertOne(p);
            }

            return RedirectToAction("prikaziProizvode","Proizvod");
        }
        public Proizvod vratiProizvod(string Id)
        {
            FilterDefinition<Proizvod> filter = Builders<Proizvod>.Filter.Eq("_id", ObjectId.Parse(Id));
            var  proiz= this._dbContext.GetProizvodi.Find(filter).FirstOrDefault();
           
            return proiz;
        }
        public async Task<ActionResult> prikaziProizvode()
        {
            var prod = this._dbContext.GetProizvodi.Find(_ => true).ToList();
            List<Proizvod> k = new List<Proizvod>();

            foreach (Proizvod r in prod.ToArray<Proizvod>())
            {
                k.Add(r);
            }

            var prodd = this._dbContextProdavnica.GetProdavnice.Find(_ => true).FirstOrDefault();
            TempData["idProdavnice"] = JsonConvert.SerializeObject(prodd.Id.ToString());
            
            return View(k);
        }

        public IActionResult obrisiProizvod(string Id)
        {
            var objId = ObjectId.Parse(Id);
            var filter = Builders<Proizvod>.Filter.Eq("_id", objId);
            var proizvod = this._dbContext.GetProizvodi.Find(filter).FirstOrDefault();

            foreach(Komentar k in proizvod.listakomentara)
            {
                var filKomen = Builders<Komentar>.Filter.Eq("_id", k.Id);
                var kom = this._dbContextKomentari.GetKomentari.DeleteOne(filKomen);
            }

            var korpe = this._dbContextKorpe.GetKorpe.Find(_ => true).ToList();

            foreach(Korpa k in korpe)
            {
                List<Proizvod> proizz = new List<Proizvod>();
                foreach (Proizvod p in k.Proizvodi)
                {
                    if(p.Id!=objId)
                    {
                      proizz.Add(p);
                    }
                    else
                    {
                        k.UkupnaCenaKorpe = k.UkupnaCenaKorpe - p.Cena;
                    }
                }
                var fil = Builders<Korpa>.Filter.Eq("_id", k.Id);
                var up = Builders<Korpa>.Update.Set("Proizvodi", proizz).Set("UkupnaCenaKorpe", k.UkupnaCenaKorpe);
                this._dbContextKorpe.GetKorpe.UpdateOne(fil,up);
            }

            //prodavnica
            var idd = ObjectId.Parse(proizvod.IdProdavnice);
            var filter1 = Builders<Prodavnica>.Filter.Eq("_id", idd);
            var prodav = this._dbContextProdavnica.GetProdavnice.Find(filter1).FirstOrDefault();
            prodav.proizvodi.Remove(objId);
            var update = Builders<Prodavnica>.Update.Set("proizvodi", prodav.proizvodi);
            this._dbContextProdavnica.GetProdavnice.UpdateOne(filter1, update);

            //brisanje proizvoda
            var rez = this._dbContext.GetProizvodi.DeleteOne(filter);

            return RedirectToAction("prikaziProizvode", "Proizvod");
        }

        public IActionResult idiNaIzmeniProizvod(string Id)
        {
            var objId = ObjectId.Parse(Id);
            Proizvod p = vratiProizvod(Id);
            
            return View(p);
        }

        public IActionResult izmeniProizvod(Proizvod p,string Id)
        {
            var objId = ObjectId.Parse(Id);
            var nameFilter = Builders<Proizvod>.Filter.Eq("_id", objId);

            var update = Builders<Proizvod>.Update.Set("SrednjaOcena", p.SrednjaOcena).Set("_id", objId)
                .Set("Naziv", p.Naziv).Set("Cena", p.Cena).Set("Kolicina", p.Kolicina).Set("Kategorija", p.Kategorija);

            var rez = this._dbContext.GetProizvodi.UpdateOne(nameFilter, update);
            return RedirectToAction("prikaziProizvode", "Proizvod");

        }
       
        public IActionResult oceniProizvod(string Ocena,string Id)
        {
            var ocena = Int32.Parse(Ocena);
            var proizvod = vratiProizvod(Id);

            proizvod.BrojOcena = proizvod.BrojOcena + 1;
            proizvod.Ocena = proizvod.Ocena + ocena;
            proizvod.SrednjaOcena=(proizvod.Ocena)/ proizvod.BrojOcena;

            var objId = ObjectId.Parse(Id);
            var nameFilter = Builders<Proizvod>.Filter.Eq("_id", objId);

            var update = Builders<Proizvod>.Update.Set("Sifra", proizvod.Sifra).Set("_id", objId)
                .Set("Naziv", proizvod.Naziv).Set("Cena", proizvod.Cena).Set("Kolicina", proizvod.Kolicina)
                .Set("BrojOcena", proizvod.BrojOcena).Set("SrednjaOcena",proizvod.SrednjaOcena).Set("Ocena",proizvod.Ocena);

            var rez = this._dbContext.GetProizvodi.UpdateOne(nameFilter, update);
            var prod = this._dbContextProdavnica.GetProdavnice.Find(_ => true).FirstOrDefault();

            return RedirectToAction("prikaziProizvodeKorisniku", "Korisnik", new { Id = prod.Id });
        }

        public ActionResult prikaziKomentare(string Id)// id proizvoda za kog prikazujemo komenatare
        {
            var proiz = vratiProizvod(Id);
            return View(proiz);
        }

        public ActionResult dodajKomentarProizvodu(string Id,string Email,string TekstKomentara)// id proizvoda za kog prikazujemo komenatare
        {
            var objId = ObjectId.Parse(Id);
            var nameFilter = Builders<Proizvod>.Filter.Eq("_id", objId);
            var proiz = this._dbContext.GetProizvodi.Find(nameFilter).FirstOrDefault();

            //korisnik koji komenatrise
            var nameFilter1 = Builders<Korisnik>.Filter.Eq("Email", Email);
            var korisnik = this._dbContextKorisnici.GetKorisnike.Find(nameFilter1).FirstOrDefault();
            
            //komenatr
            Komentar k = new Komentar();
            k.KorisnikKom = korisnik;
            k.Id = ObjectId.GenerateNewId(); 
            k.TekstKomentara = TekstKomentara;
            k.Sifra = Email;//shared

            //dodavanje komenatra proizovdu
            proiz.listakomentara.Add(k);
            var update = Builders<Proizvod>.Update.Set("listakomentara", proiz.listakomentara);
            this._dbContext.GetProizvodi.UpdateOne(nameFilter, update);

            //dodavanje komentara u listu komentara
            this._dbContextKomentari.GetKomentari.InsertOne(k);

            return RedirectToAction("prikaziKomentare", "Proizvod", new { Id = proiz.Id });
        }

         public string dodajSLiku(IFormFile Slika, ObjectId Id) //id proizvoda
         {
            string uniqueFileName = null;

            if (Slika != null)
            {
                string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + Slika.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    Slika.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }

        public List<Proizvod> najboljeOcenjeniProzivodi()
        {
            List<Proizvod> listaProizvoda = new List<Proizvod>();
            var prod = this._dbContext.GetProizvodi.Find(_ => true).ToList();
            prod.Sort((a, b) => b.SrednjaOcena.CompareTo(a.SrednjaOcena));

            var ind = 0;
            if (prod.Count < 10)
            {
                ind = prod.Count;
            }
            else
                ind = prod.Count;

            for (int i = 0; i < ind; i++)
            {
                listaProizvoda.Add(prod.ElementAt<Proizvod>(i));
            }
            return listaProizvoda;

        }
        public  IActionResult pikaziNajbljeOcenjeneProizvode()
        {
            List<Proizvod> listaProizvoda = najboljeOcenjeniProzivodi();

            return View(listaProizvoda);
        }

        public IActionResult preporukaZaLogKorisnike()
        {
            List<Proizvod> listaProizvoda = najboljeOcenjeniProzivodi();

            return View(listaProizvoda);
        }

        public List<Proizvod> vracaListuProizvodaOdrKateg(string Kategorija)
        {
            List<Proizvod> listaProizvoda = new List<Proizvod>();
            var proizvodi = this._dbContext.GetProizvodi.Find(_ => true).ToList();

            foreach (Proizvod p in proizvodi)
            {
                if (p.Kategorija == Kategorija)
                {
                    listaProizvoda.Add(p);
                }
            }
            return listaProizvoda;
        }

        public IActionResult pikaziProizvodePoKategoriji(string Kategorija)
        {
            List<Proizvod> listaProizvoda = new List<Proizvod>();
            listaProizvoda = vracaListuProizvodaOdrKateg(Kategorija);

            return View(listaProizvoda);
        }
        public IActionResult pikaziProizvodePoKategorijiZaNeLogKor(string Kategorija)
        {
            List<Proizvod> listaProizvoda = new List<Proizvod>();
            listaProizvoda = vracaListuProizvodaOdrKateg(Kategorija);

            return View(listaProizvoda);
        }
        public IActionResult idiNaPrikaziFormuZaPretragu()
        {
            Proizvod p = new Proizvod()
;            return View(p);
        }

        public IActionResult pretraziProizvod(Proizvod p)
        {
            List<Proizvod> proiz = null;
            
            if (p.Naziv!=null && p.Cena!=0 && p.Kategorija!=null && p.SrednjaOcena!=0)
            {

                 var nameFilter = Builders<Proizvod>.Filter.Eq("Naziv", p.Naziv)
                           & Builders<Proizvod>.Filter.Eq("Kategorija", p.Kategorija)
                           & Builders<Proizvod>.Filter.Lt("Cena", p.SrednjaOcena)
                           & Builders<Proizvod>.Filter.Gt("Cena", p.Cena);
                 proiz = this._dbContext.GetProizvodi.Find(nameFilter).ToList();
            }
         
            else if(p.Naziv!=null && p.Kategorija!=null) 
            {
                var nameFilter = Builders<Proizvod>.Filter.Eq("Naziv", p.Naziv)
                           & Builders<Proizvod>.Filter.Eq("Kategorija", p.Kategorija);
                proiz = this._dbContext.GetProizvodi.Find(nameFilter).ToList();


            }
            else if(p.Naziv!=null && p.Cena!=0 && p.SrednjaOcena!=0) 
            {
                var nameFilter = Builders<Proizvod>.Filter.Eq("Naziv", p.Naziv) &
                                 Builders<Proizvod>.Filter.Lt("Cena", p.SrednjaOcena)
                                 & Builders<Proizvod>.Filter.Gt("Cena", p.Cena);
                proiz = this._dbContext.GetProizvodi.Find(nameFilter).ToList();

            }
            else if(p.Kategorija!=null && p.Cena!= 0 && p.SrednjaOcena!= 0) 
            {
                var nameFilter =Builders<Proizvod>.Filter.Eq("Kategorija", p.Kategorija)
                          & Builders<Proizvod>.Filter.Lt("Cena", p.SrednjaOcena)
                          & Builders<Proizvod>.Filter.Gt("Cena", p.Cena);
                proiz = this._dbContext.GetProizvodi.Find(nameFilter).ToList();


            }
            else if (p.Naziv != null) 
            {
                var nameFilter = Builders<Proizvod>.Filter.Eq("Naziv", p.Naziv);
                proiz = this._dbContext.GetProizvodi.Find(nameFilter).ToList();

            }
            else if (p.Kategorija != null) 
            {
                var nameFilter = Builders<Proizvod>.Filter.Eq("Kategorija", p.Kategorija);
                proiz = this._dbContext.GetProizvodi.Find(nameFilter).ToList();
            }
            else if (p.Cena != 0 && p.SrednjaOcena != 0) 
            {
                var nameFilter = Builders<Proizvod>.Filter.Lt("Cena", p.SrednjaOcena)
                           & Builders<Proizvod>.Filter.Gt("Cena", p.Cena);
                proiz = this._dbContext.GetProizvodi.Find(nameFilter).ToList();

            }
            List<Proizvod> lista = new List<Proizvod>();

            foreach (Proizvod pp in proiz)
            {
                lista.Add(pp);
            }

            return View(lista);
        }
    }
}

