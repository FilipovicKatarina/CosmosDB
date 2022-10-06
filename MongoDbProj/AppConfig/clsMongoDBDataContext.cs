using System;
using MongoDbProj.Models;
using MongoDB.Driver;
namespace MongoDbProj.AppConfig
{
    public class clsMongoDBDataContext
    {
        private string _connectionStrings = string.Empty;
        private string _databaseName = string.Empty;
        private string _collectionName = string.Empty;
        private readonly IMongoClient _client;
        private readonly IMongoDatabase _database;

        public clsMongoDBDataContext(string strCollectionName)
        {
            this._collectionName = strCollectionName;
            this._connectionStrings = AppConfiguration.GetConfiguration("ServerName");
            this._databaseName = AppConfiguration.GetConfiguration("DatabaseName");
            this._client = new MongoClient(_connectionStrings);
            this._database = _client.GetDatabase(_databaseName);
        }

        public IMongoClient Client
        {
            get { return _client; }
        }

        public IMongoDatabase Database
        {
            get { return _database; }
        }

        public IMongoCollection<Prodavnica> GetProdavnice
        {
            get { return _database.GetCollection<Prodavnica>(_collectionName); }
        }

        public IMongoCollection<Proizvod> GetProizvodi
        {
            get { return _database.GetCollection<Proizvod>(_collectionName); }
        }

        public IMongoCollection<Korisnik> GetKorisnike
        {
            get { return _database.GetCollection<Korisnik>(_collectionName); }
        }

        public IMongoCollection<Korpa> GetKorpe
        {
            get { return _database.GetCollection<Korpa>(_collectionName); }
        }

        public IMongoCollection<Admin> GetAdmini
        {
            get { return _database.GetCollection<Admin>(_collectionName); }
        }
        public IMongoCollection<Komentar> GetKomentari
        {
            get { return _database.GetCollection<Komentar>(_collectionName); }
        }
     
    }
}
