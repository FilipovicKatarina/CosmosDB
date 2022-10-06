using System;
using MongoDB.Bson;

namespace MongoDbProj.Models
{
    public class Admin
    {
        public ObjectId Id { get; set; }
        public int idAdmina{ get; set; }
        public String Email { get; set; }
        public String Password { get; set; }
    }
}
