using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace AnimalShelter.Models
{
    public class Animal
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string Gender { get; set; }
        public string DateAdmitted { get; set; }
        public string Breed { get; set; }

        public int Id { get; set; }

        // public Animal(string description)
        // {
        //     Description = description;
        // }

        public Animal(string name, string type, string gender, string dateAdmitted, string breed, int id)
        {
            Name = name;
            Type = type;
            Gender = gender;
            DateAdmitted = dateAdmitted;
            Breed = breed;
            Id = id;
        }

        public override bool Equals(System.Object otherAnimal)
        {
            if (!(otherAnimal is Animal))
            {
                return false;
            }
            else
            {
                Animal newAnimal = (Animal)otherAnimal;
                bool idEquality = (this.Id == newAnimal.Id);
                bool nameEquality = (this.Name == newAnimal.Name);
                bool typeEquality = (this.Type == newAnimal.Type);
                bool genderEquality = (this.Gender == newAnimal.Gender);
                bool dateAdmittedEquality = (this.DateAdmitted == newAnimal.DateAdmitted);
                bool breedEquality = (this.Breed == newAnimal.Breed);
                return (idEquality && nameEquality && typeEquality && genderEquality && dateAdmittedEquality && breedEquality);
            }
        }

        public static List<Animal> GetAll()
        {
            List<Animal> allAnimals = new List<Animal> { };
            MySqlConnection conn = DB.Connection();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM animals;";
            MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
            while (rdr.Read())
            {
                int animalId = rdr.GetInt32(0);
                string animalName = rdr.GetString(1);
                string animalType = rdr.GetString(2);
                string animalGender = rdr.GetString(3);
                string animalBreed = rdr.GetString(4);
                string animalDateAdmitted = rdr.GetString(5);
                Animal newAnimal = new Animal(animalName, animalType, animalGender, animalBreed, animalDateAdmitted, animalId);
                allAnimals.Add(newAnimal);
            }
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return allAnimals;
        }

        public static void ClearAll()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"DELETE FROM animals;";
            cmd.ExecuteNonQuery();
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

        public static Animal Find(int searchId)
        {
            // We open a connection.
            MySqlConnection conn = DB.Connection();
            conn.Open();

            // We create MySqlCommand object and add a query to its CommandText property. We always need to do this to make a SQL query.
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM `animals` WHERE id = @thisId;";

            // We have to use parameter placeholders (@thisId) and a `MySqlParameter` object to prevent SQL injection attacks. This is only necessary when we are passing parameters into a query. We also did this with our Save() method.
            MySqlParameter thisId = new MySqlParameter();
            thisId.ParameterName = "@thisId";
            thisId.Value = id;
            cmd.Parameters.Add(thisId);

            // We use the ExecuteReader() method because our query will be returning results and we need this method to read these results. This is in contrast to the ExecuteNonQuery() method, which we use for SQL commands that don't return results like our Save() method.
            var rdr = cmd.ExecuteReader() as MySqlDataReader;
            int animalId = 0;
            string animalName = "";
            string animalType = "";
            string animalGender = "";
            string animalBreed = "";
            string animalDateAdmitted = "";
            while (rdr.Read())
            {
                animalId = rdr.GetInt32(0);
                animalName = rdr.GetString(1);
                animalType = rdr.GetString(2);
                animalGender = rdr.GetString(3);
                animalBreed = rdr.GetString(4);
                animalDateAdmitted = rdr.GetString(5);
            }
            Animal foundAnimal = new Animal(animalName, animalType, animalGender, animalBreed, animalDateAdmitted, animalId);

            // We close the connection.
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return foundAnimal;
        }

        public void Save()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;

            // Begin new code

            cmd.CommandText = @"INSERT INTO animals (name) VALUES (@AnimalName);";
            cmd.CommandText = @"INSERT INTO animals (type) VALUES (@AnimalType);";
            cmd.CommandText = @"INSERT INTO animals (gender) VALUES (@AnimalGender);";
            cmd.CommandText = @"INSERT INTO animals (breed) VALUES (@AnimalBreed);";
            cmd.CommandText = @"INSERT INTO animals (dateAdmitted) VALUES (@AnimalName);";
            MySqlParameter name = new MySqlParameter();
            name.ParameterName = "@AnimalName";
            MySqlParameter type = new MySqlParameter();
            type.ParameterName = "@AnimalType";
            MySqlParameter gender = new MySqlParameter();
            gender.ParameterName = "@AnimalType";
            MySqlParameter breed = new MySqlParameter();
            breed.ParameterName = "@AnimalBreed";
            MySqlParameter dateAdmitted = new MySqlParameter();
            dateAdmitted.ParameterName = "@AnimalDateAdmitted";
            name.Value = this.Name;
            type.Value = this.Type;
            gender.Value = this.Gender;
            breed.Value = this.Breed;

            cmd.Parameters.Add(name);
            cmd.Parameters.Add(type);
            cmd.Parameters.Add(gender);
            cmd.Parameters.Add(breed);
            cmd.Parameters.Add(dateAdmitted);
            cmd.ExecuteNonQuery();
            Id = (int)cmd.LastInsertedId;

            // End new code

            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

    }
}