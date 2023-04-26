﻿using MongoDB.Driver;
using MongoDB.Bson;
using System.Security.Cryptography;
using System.Text;

namespace GameToBeNamed.server
{
    internal sealed class GameDataBaseAccess : IDisposable
    {
        readonly MongoClient client;
        public GameDataBaseAccess()
        {
            MongoClientSettings settings = MongoClientSettings.FromConnectionString("mongodb+srv://GameAccess:bfHY0pSTcDAHnrYp@discgameserver.svtoxdh.mongodb.net/?retryWrites=false&w=majority");
            settings.ServerApi = new(ServerApiVersion.V1);
            client = new(settings);
        }

        public async Task<share.ErrorOrData> LoginAsync(string username, string password)
        {
            try
            {
                IMongoCollection<BsonDocument> collection = client.GetDatabase("DiscGameServer").GetCollection<BsonDocument>("Game_Accounts");
                return new(await collection.Find(Builders<BsonDocument>.Filter.And(
                                                                         Builders<BsonDocument>.Filter.Eq("UserName", username),
                                                                         Builders<BsonDocument>.Filter.Eq("UserPassword", password))).AnyAsync());
            }
            catch (Exception ex)
            {
                return new(ex.Message);
            }
        }

        public void Dispose()
        {
            // Method intentionally left empty.
        }
    }
}