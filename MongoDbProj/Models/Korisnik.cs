using System;
using MongoDB.Bson;

namespace MongoDbProj.Models
{
    public class Korisnik
    {
        public ObjectId Id { get; set; }
        public int idKorisnika { get; set; }
        public String Email { get; set; }
        public String Password { get; set; }
        public String Sifra { get; set; }
        public ObjectId Korpa { get; set; }

    }
}
