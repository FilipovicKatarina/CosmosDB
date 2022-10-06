using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using MongoDB.Bson;

namespace MongoDbProj.Models
{
    public class Proizvod
    {
        public ObjectId Id { get; set; }
        public string Sifra { get; set; }
        public string Naziv { get; set; }
        public float Cena { get; set; }
        public int Kolicina { get;set;}
        public string IdProdavnice { get; set; }
        public int BrojOcena { get; set; }
        public float SrednjaOcena { get; set; }
        public int Ocena { get; set; }//zbir ocena
        public List<Komentar> listakomentara { get; set; }
        public string Kategorija { get; set; }
        public IFormFile Slika { get; set; }
        public string PutanjaSlike { get; set; }
        public ObjectId prodavnica { get; set; }

        public  Proizvod()
        {
            listakomentara = new List<Komentar>();
        }
    }
}
