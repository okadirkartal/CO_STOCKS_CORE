using System;

namespace Application.Infrastructure
{
    public class Users
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public string SurName { get; set; }
        public string Password { get; set; }
        public DateTime? CreationDate { get; set; }
        public DateTime? LastLoginDate { get; set; }
    }
}