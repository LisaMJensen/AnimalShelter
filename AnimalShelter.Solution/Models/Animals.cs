using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace AnimalShelter.Models
{
    public class Animal
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string Gender { get; set; }
        public int DateAdmitted { get; set; }
        public string Breed { get; set; }

        public int Id { get; set; }

        public Animal(string description)
        {
            Description = description;
        }

        public Animal(string description, int id)
        {
            Description = description;
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
                bool descriptionEquality = (this.Description == newAnimal.Description);
                return (idEquality && descriptionEquality);
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
                int animslId = rdr.GetInt32(0);
                string animslDescription = rdr.GetString(1);
                Animal newAnimal = new Animal(animalDescription, animalId);
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

        public static Animal Find(int id)
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
            string animalDescription = "";
            while (rdr.Read())
            {
                animalId = rdr.GetInt32(0);
                animalDescription = rdr.GetString(1);
            }
            Animal foundAnimal = new Animal(animalDescription, animalId);

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

            cmd.CommandText = @"INSERT INTO animals (description) VALUES (@AnimalDescription);";
            MySqlParameter description = new MySqlParameter();
            description.ParameterName = "@AnimalDescription";
            description.Value = this.Description;
            cmd.Parameters.Add(description);
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