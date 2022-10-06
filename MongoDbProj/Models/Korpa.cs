using System.Collections.Generic; 
using MongoDB.Bson; 

namespace MongoDbProj.Models
{
    public class Korpa
    {
        public ObjectId Id { get; set; }
        public int IdKorpe { get; set; }
        public ObjectId Korisnik { get; set; }
        public List<Proizvod> Proizvodi { get; set; }
        public float UkupnaCenaKorpe { get; set; }
        public Korpa()
        {
            Proizvodi = new List<Proizvod>();
        }
    }
}
