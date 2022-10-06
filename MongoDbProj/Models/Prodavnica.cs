using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;

namespace MongoDbProj.Models
{
    public class Prodavnica
    {
       public ObjectId Id { get; set; }
       public int IdProdavnice { get; set; }
       public string Ime { get; set; }
       public string Adresa { get; set; }
       public string Sifra { get; set; }
       public List<ObjectId> proizvodi { get; set; }

       public Prodavnica()
       {
           proizvodi = new List<ObjectId>();
       }
    }
}
