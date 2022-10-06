using MongoDB.Bson; 

namespace MongoDbProj.Models
{
    public class Komentar
    {
        public ObjectId Id { get; set; }
        public string TekstKomentara { get; set; }
        public Korisnik KorisnikKom { get; set; }
        public string Sifra { get; set; } 
    }
}
