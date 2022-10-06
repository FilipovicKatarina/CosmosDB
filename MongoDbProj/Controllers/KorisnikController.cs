using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using MongoDbProj.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using MongoDbProj.AppConfig;

namespace MongoDbProj.Controllers
{
    public class KorisnikController : Controller
    {
        clsMongoDBDataContext _dbContextKorisnici = new clsMongoDBDataContext("korisnici");
        clsMongoDBDataContext _dbContextProdavnica = new clsMongoDBDataContext("prodavnice");
        clsMongoDBDataContext _dbContextKorpe = new clsMongoDBDataContext("korpe");
        clsMongoDBDataContext _dbContextProizvodi = new clsMongoDBDataContext("proizvodi");

        public IActionResult kreirajKorisnika()  
        {
            Korisnik k = new Korisnik();
            return View(k);
        }

        public IActionResult kreirajKorisnikaa(Korisnik k) 
        {
            if (k.Password == null || k.Email == null)
            {
                var a = "Popuni sva polja!";
                TempData["msgPopuni"] = JsonConvert.SerializeObject(a.ToString());
                return RedirectToAction("kreirajKorisnika", "Korisnik");
            }
            else
            {
                var nameFilter1 = Builders<Korisnik>.Filter.Eq("Password", k.Password) & Builders<Korisnik>.Filter.Eq("Email", k.Email);
                var kor = this._dbContextKorisnici.GetKorisnike.Find(nameFilter1).FirstOrDefault();

                if (kor == null)
                {
                    //generisali kljuc korsisnika
                    k.Id = ObjectId.GenerateNewId();
                    k.Sifra = k.Email;
                    
                    Korpa korpa = kreirajKorpu(k.Id.ToString());
                    k.Korpa = korpa.Id;

                    this._dbContextKorisnici.GetKorisnike.InsertOne(k);

                    TempData["idKorisnika"] = JsonConvert.SerializeObject(k.Id.ToString());
                    TempData["idKorpe"] = JsonConvert.SerializeObject(k.Korpa.ToString());

                    var prod = this._dbContextProdavnica.GetProdavnice.Find(_ => true).FirstOrDefault();
                    return RedirectToAction("prikaziProizvodeKorisniku", "Korisnik", new { Id = prod.Id });
                }
                else
                {
                    TempData["msgPostoji"] = JsonConvert.SerializeObject("Nalog vec postoji!");

                    return RedirectToAction("kreirajKorisnika", "Korisnik");
                }
            }    
        }

        public IActionResult idiNaLogKorisnika() 
        {
            return View();
        }

        public IActionResult logKorisnika(Korisnik k)
        {
            var nameFilter = Builders<Korisnik>.Filter.Eq("Password", k.Password) & Builders<Korisnik>.Filter.Eq("Email", k.Email);
            var korisnik = this._dbContextKorisnici.GetKorisnike.Find(nameFilter).FirstOrDefault();

            if (korisnik != null)
            {
                TempData["idKorisnika"] = JsonConvert.SerializeObject(korisnik.Id.ToString());
                TempData["idKorpe"] = JsonConvert.SerializeObject(korisnik.Korpa.ToString());
                var prod = this._dbContextProdavnica.GetProdavnice.Find(_ => true).FirstOrDefault();
               
                return RedirectToAction("prikaziProizvodeKorisniku", "Korisnik", new { Id=prod.Id});
            }
            else 
            {
                TempData["msgGreska"] = JsonConvert.SerializeObject("Pogresan password ili Email!");

                return RedirectToAction("idiNaLogKorisnika","Korisnik");
            }
        }

        public Korpa kreirajKorpu(string Id)//id korisnika
        {
           
            Korpa k = new Korpa();
            k.Id = ObjectId.GenerateNewId();
            k.Korisnik = ObjectId.Parse(Id);
            this._dbContextKorpe.GetKorpe.InsertOne(k);

            return k;
        }

        public IActionResult dodajProizvodUkorpu(string Id)//id proizvoda
        {
            string idkorisnika = JsonConvert.DeserializeObject<string>(TempData["idKorisnika"] as string);
            TempData["idKorisnika"] = JsonConvert.SerializeObject(idkorisnika);

            //proizvod
            var idProiz = ObjectId.Parse(Id);
            var filter = Builders<Proizvod>.Filter.Eq("_id", idProiz);
            var proizvod = this._dbContextProizvodi.GetProizvodi.Find(filter).FirstOrDefault();
           
            //korisnik 
            var objId = ObjectId.Parse(idkorisnika);
            var nameFilter = Builders<Korisnik>.Filter.Eq("_id", objId);
            var korisnik = this._dbContextKorisnici.GetKorisnike.Find(nameFilter).FirstOrDefault();

           //korpa
            ObjectId idKorpe = korisnik.Korpa;
            var fil = Builders<Korpa>.Filter.Eq("_id", idKorpe);
            var korpa = this._dbContextKorpe.GetKorpe.Find(fil).FirstOrDefault();
         
            if (proizvod.Kolicina>0)
            {
                int novaKolicina=proizvod.Kolicina-1;
                var update = Builders<Proizvod>.Update.Set("Kolicina", novaKolicina);
                this._dbContextProizvodi.GetProizvodi.UpdateOne(filter, update);

                korpa.Proizvodi.Add(proizvod);
                korpa.UkupnaCenaKorpe = korpa.UkupnaCenaKorpe + proizvod.Cena;
                var up = Builders<Korpa>.Update.Set("Proizvodi", korpa.Proizvodi).Set("UkupnaCenaKorpe", korpa.UkupnaCenaKorpe);
                this._dbContextKorpe.GetKorpe.UpdateOne(fil, up);

            }
            var prod = this._dbContextProdavnica.GetProdavnice.Find(_ => true).FirstOrDefault();

            return RedirectToAction("prikaziProizvodeKorisniku", "Korisnik", new { Id = prod.Id });
        }

        public Korpa vratiKorpu(string Id)
        {
            var objId = ObjectId.Parse(Id);
            var fil = Builders<Korpa>.Filter.Eq("_id", objId);
            var korpa = this._dbContextKorpe.GetKorpe.Find(fil).FirstOrDefault();

            return korpa;
        }

        public IActionResult prikaziSadrzajKorpe()
        {
            string Id = JsonConvert.DeserializeObject<string>(TempData["idKorpe"] as string);
            TempData["idKorpe"] = JsonConvert.SerializeObject(Id);
            var korpa = vratiKorpu(Id);

            return View(korpa);
        }

        public IActionResult obrisiProizvodIzKorpe(string Id,string  IdKorpe)
        {
            var korpa = vratiKorpu(IdKorpe);
            var objId = ObjectId.Parse(Id);
            var filt = Builders<Proizvod>.Filter.Eq("_id", objId);
            var proiz = this._dbContextProizvodi.GetProizvodi.Find(filt).FirstOrDefault();

            foreach(Proizvod p in korpa.Proizvodi)
            {
                if(p.Id==proiz.Id)
                {
                    //povecavamo kolicinu proizvoda, jer ga brisemo iz korpe 
                    proiz.Kolicina = proiz.Kolicina + 1;
                    var update = Builders<Proizvod>.Update.Set("Kolicina", proiz.Kolicina);
                    this._dbContextProizvodi.GetProizvodi.UpdateOne(filt, update);

                    //cuvamo izmene korpe
                    korpa.Proizvodi.Remove(p);
                    korpa.UkupnaCenaKorpe = korpa.UkupnaCenaKorpe - proiz.Cena;
                    var fil = Builders<Korpa>.Filter.Eq("_id", korpa.Id);
                    var up = Builders<Korpa>.Update.Set("Proizvodi", korpa.Proizvodi).Set("UkupnaCenaKorpe", korpa.UkupnaCenaKorpe);
                    this._dbContextKorpe.GetKorpe.UpdateOne(fil, up);
                    return RedirectToAction("prikaziSadrzajKorpe");
                }
            }
            return RedirectToAction("prikaziSadrzajKorpe");
        }

        public IActionResult prikaziProizvodeKorisniku(string Id) //id prodavnice
        {
            var prod = this._dbContextProdavnica.GetProdavnice.Find(_ => true).FirstOrDefault();
            List<Proizvod> lista = new List<Proizvod>();

            foreach (ObjectId idd in prod.proizvodi)
            {
                var filt = Builders<Proizvod>.Filter.Eq("_id", idd);
                var proiz = this._dbContextProizvodi.GetProizvodi.Find(filt).FirstOrDefault();
                lista.Add(proiz);
            }

            return View(lista);
        }

        public IActionResult prikaziKorisnike()
        {
            var prod = this._dbContextKorisnici.GetKorisnike.Find(_ => true).ToList();
            List<Korisnik> k = new List<Korisnik>();

            foreach (Korisnik r in prod.ToArray<Korisnik>())
            {
                k.Add(r);
            }
            return View(k);
        }
        
        public IActionResult obrisiKorisnika(string Id)
        {
            var objId = ObjectId.Parse(Id);
            var filter = Builders<Korisnik>.Filter.Eq("_id", objId);
            var korisnik = this._dbContextKorisnici.GetKorisnike.Find(filter).FirstOrDefault();

            var filter1 = Builders<Korpa>.Filter.Eq("_id", korisnik.Korpa);
            this._dbContextKorpe.GetKorpe.DeleteOne(filter1);

            var rez = this._dbContextKorisnici.GetKorisnike.DeleteOne(filter);

            return RedirectToAction("prikaziKorisnike");
        }

        public IActionResult idiNaIzmeniKorisnik(string Id)
        {
            var objId = ObjectId.Parse(Id);
            var nameFilter = Builders<Korisnik>.Filter.Eq("_id", objId);
            var k = this._dbContextKorisnici.GetKorisnike.Find(nameFilter).FirstOrDefault();

            return View(k);
        }

        public IActionResult izmeniKorisnika(Korisnik k,string Id)
        {
            var objId = ObjectId.Parse(Id);
            k.Id = objId;
            var nameFilter = Builders<Korisnik>.Filter.Eq("_id", objId);

            if(k.Password!=null)
            {
                var update = Builders<Korisnik>.Update.Set("Email", k.Email).Set("Password", k.Password);
                var rez = this._dbContextKorisnici.GetKorisnike.UpdateOne(nameFilter, update);
            }
            else
            {
                var update = Builders<Korisnik>.Update.Set("Email", k.Email);
                var rez = this._dbContextKorisnici.GetKorisnike.UpdateOne(nameFilter, update);
            }

            return RedirectToAction("prikaziKorisnike");
        }

        public IActionResult idiNaKupi(string Id)//id korpe
        {
            var objId = ObjectId.Parse(Id);
            var filter = Builders<Korpa>.Filter.Eq("_id", objId);
            var korpa = this._dbContextKorpe.GetKorpe.Find(filter).FirstOrDefault();

            korpa.Proizvodi.Clear();
            korpa.UkupnaCenaKorpe = 0;
            var update = Builders<Korpa>.Update.Set("Proizvodi", korpa.Proizvodi).Set("UkupnaCenaKorpe", korpa.UkupnaCenaKorpe);
            var rez = this._dbContextKorpe.GetKorpe.UpdateOne(filter, update);
          
            return RedirectToAction("prikaziSadrzajKorpe");
        }
    }
}
