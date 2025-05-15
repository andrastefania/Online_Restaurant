using Newtonsoft.Json;
using System;
using System.IO;
using Menu_Restaurant_2.Model;  

namespace Menu_Restaurant.Json
{
    public class UserService
    {
        private string filePath = "user.json";  

        public void SaveUser(Person user)
        {
            try
            {
                string jsonData = JsonConvert.SerializeObject(user);  
                File.WriteAllText(filePath, jsonData); 
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error saving user data: " + ex.Message);
            }
        }
        public Person LoadUser()
        {
            try
            {
                if (File.Exists(filePath))
                {
                    string jsonData = File.ReadAllText(filePath);
                    return JsonConvert.DeserializeObject<Person>(jsonData); 
                }
                return null; 
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error loading user data: " + ex.Message);
                return null;
            }
        }
        public void ClearUser()
        {
            var emptyUser = new Person { Id = 0, Name = "", Type = "",Email = "",Parola = ""};
            string jsonData = JsonConvert.SerializeObject(emptyUser);
            File.WriteAllText(filePath, jsonData);
        }
    }
}