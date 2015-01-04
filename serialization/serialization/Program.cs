using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using Newtonsoft.Json;
using ProtoBuf;

namespace serialization
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                if (args.Length != 2) throw new Exception("Not enough arguments");

                switch (args[0])
                {
                    case "save":
                        Serialize(args[1]);
                        break;

                    case "load":
                        Deserialize(args[1]);
                        break;
                }
            }
            catch (Exception error)
            {
                Console.WriteLine("ERROR: " + error.Message);
            }
        }

        private static void Serialize(String format)
        {
            switch (format)
            {
                case "xml":
                    SerializeToXml();
                    break;

                case "json":
                    SerializeToJson();
                    break;

                case "proto":
                    SerializeToProtobuf();
                    break;
            }
        }

        private static void Deserialize(String format)
        {
            var users = (IEnumerable<User>) null;
            switch (format)
            {
                case "xml":
                    users = DeserializeFromXml();
                    break;

                case "json":
                    users = DeserializeFromJson();
                    break;

                case "proto":
                    users = DeserializeFromProtobuf();
                    break;
            }

            if (users == null) return;

            Console.WriteLine("[");
            foreach (var user in users)
            {
                Console.WriteLine("  {");
                Console.WriteLine("    Id: '{0}'", user.Id);
                Console.WriteLine("    FirstName: '{0}',", user.FirstName);
                Console.WriteLine("    LastName: '{0}',", user.LastName);
                Console.WriteLine("    Username: '{0}'", user.Username);
                Console.WriteLine("  },");
            }
            Console.WriteLine("]");
        }

        private static void SerializeToXml()
        {
            var users = GetUsers();
            var xmlSerializer = new XmlSerializer(users.GetType());
            using (var textWriter = File.CreateText("users.xml"))
            {
                xmlSerializer.Serialize(textWriter, users);
            }
        }

        private static IEnumerable<User> DeserializeFromXml()
        {
            var serializer = new XmlSerializer(typeof(User[]));
            using (var textReader = File.OpenText("users.xml"))
            {
                return (IEnumerable<User>) serializer.Deserialize(textReader);
            }
        }

        private static void SerializeToJson()
        {
            var users = GetUsers();
            var json = JsonConvert.SerializeObject(users, Formatting.Indented);
            File.WriteAllText("users.json", json);
        }

        private static IEnumerable<User> DeserializeFromJson()
        {
            var json = File.ReadAllText("users.json");
            return JsonConvert.DeserializeObject<User[]>(json);
        }

        private static void SerializeToProtobuf()
        {
            var users = GetUsers();
            using (var file = File.Create("users.bin"))
            {
                Serializer.Serialize(file, users);
            }
        }

        private static IEnumerable<User> DeserializeFromProtobuf()
        {
            using (var file = File.OpenRead("users.bin"))
            {
                return Serializer.Deserialize<User[]>(file);
            }
        }

        private static IEnumerable<User> GetUsers()
        {
            return new[]
            {
                new User { Id = 1, FirstName = "Homer", LastName = "Simpson", Username = "hsimpson" },
                new User { Id = 2, FirstName = "Marge", LastName = "Simpson", Username = "msimpson" },
                new User { Id = 3, FirstName = "Bart", LastName = "Simpson", Username = "bsimpson" }
            };
        }
    }
}
