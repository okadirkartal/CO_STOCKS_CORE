using System;
using Newtonsoft.Json;

namespace Application.Infrastructure
{
    public class Users
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        
        [JsonProperty(PropertyName = "username")]
        public string UserName { get; set; }
        
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        
        [JsonProperty(PropertyName = "surname")]
        public string SurName { get; set; }
        
        [JsonProperty(PropertyName = "password")]
        public string Password { get; set; }
        
        [JsonProperty(PropertyName = "creationdate")]
        public DateTime? CreationDate { get; set; }
        
        [JsonProperty(PropertyName = "lastlogindate")]
        public DateTime? LastLoginDate { get; set; }
    }
}