using My_app_backend.Models;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using System;

namespace My_app_backend.Services
{
    public class UserService
    {
        private readonly IMongoCollection<User> _users;

        public UserService(IUserstoreDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _users = database.GetCollection<User>(settings.UsersCollectionName);
        }

        public List<UserDto> Get() 
        {
             var user = from b in _users.AsQueryable<User>()
                select new UserDto()
                {
                    Id = b.Id,
                    Name = b.Name,
                    Email = b.Email,
                    Admin = b.Admin
                };
            return user.Select( x => x ).ToList();    
        }

        public User GetByName(string Name) =>
            _users.Find<User>(user => user.Name == Name).FirstOrDefault();
 

        public UserDto Get(string id) 
        {

           var user =_users.Find<User>(user => user.Id == id).FirstOrDefault();
           if(user == null ) {
               return null;
           } else {
            return new UserDto() {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                    Admin = user.Admin
                };
           }    
        } 
        
        public User Create(User user)
        {
            _users.InsertOne(user);
            return user;
        }

        public void Update(string id, User userIn) =>
            _users.ReplaceOne(user => user.Id == id, userIn);

        public void Remove(User userIn) =>
            _users.DeleteOne(user => user.Id == userIn.Id);

        public void Remove(string id) => 
            _users.DeleteOne(user => user.Id == id);
    }
}