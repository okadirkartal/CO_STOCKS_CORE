using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace Application.Infrastructure
{
    public class Users
    {
        [BsonId,BsonElement]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        
        [BsonElement("UserName")]
        public string UserName { get; set; }
        
        [BsonElement("Name")]
        public string Name { get; set; }
        
        [BsonElement("SurName")]
        public string SurName { get; set; }
        
        [BsonElement("Password")]
        public string Password { get; set; }
        
        [BsonElement("CreationDate")]
        public DateTime? CreationDate { get; set; }
        
        [BsonElement("LastLoginDate")]
        public DateTime? LastLoginDate { get; set; }
    }
}