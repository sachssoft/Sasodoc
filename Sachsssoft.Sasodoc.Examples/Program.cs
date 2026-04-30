using Sachsssoft.Sasodoc.Examples.Animals;

namespace Sachsssoft.Sasodoc.Examples
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var owner = new AnimalOwner
            {
                FirstName = "Aaron",
                LastName = "Miller"
            };

            var animals = new List<Animal>
            {
                new Animal
                {
                    Name = "Simba",
                    Owner = owner,
                    Age = 8,
                    Type = AnimalType.Cat
                },
                new Animal
                {
                    Name = "Pluto",
                    Owner = owner,
                    Age = 5,
                    Type = AnimalType.Dog
                },
                new Animal
                {
                    Name = "Micky",
                    Owner = owner,
                    Age = 1,
                    Type = AnimalType.Mouse
                }
            };

            foreach (var animal in animals)
            {
                Console.WriteLine("Start: {0}", animal.Name);
                Dump(animal);

                using (var ms = new MemoryStream())
                {
                    animal.Save(ms);

                    // Stream-Inhalt als String anzeigen
                    var json = System.Text.Encoding.UTF8.GetString(ms.ToArray());
                    Console.WriteLine("View JSON: {0}", json);

                    // Wichtig: Stream zurücksetzen
                    ms.Position = 0;

                    Console.WriteLine("Loading Stream");

                    var loadedAnimal = Animal.Load(ms);
                    Dump(loadedAnimal);
                }

                Console.WriteLine("-------");
            }

            Console.ReadKey();
        }

        static void Dump(Animal animal)
        {
            Console.WriteLine(
                "Animal Name: {0}, Age: {1}, Type: {2}, Owner: {3}",
                animal.Name,
                animal.Age,
                animal.Type,
                animal.Owner?.ToString()
            );
        }
    }
}