using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace AsyncModals.Models
{
    public class Sheep
    {
        private static Dictionary<int, Sheep> Sheeps = new Dictionary<int, Sheep>();
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Cost { get; set; }
        public List<Shepard> Shepards { get; set; } = new List<Shepard>();
        public static Sheep CreateRandomSheep()
        {
            Sheep sheep = new Sheep()
            {
                Id = Sheeps.Count,
                Name = Guid.NewGuid().ToString("N"),
                Cost = (decimal)GetNumber(100)
            };
            Sheeps.Add(sheep.Id, sheep);
            return sheep;
        }
        private static int GetNumber(int maxNotIncluding)
        {
            byte[] randomNumber = new byte[1];
            RNGCryptoServiceProvider gen = new RNGCryptoServiceProvider();
            gen.GetBytes(randomNumber);
            int rand = Convert.ToInt32(randomNumber[0]);
            return rand * maxNotIncluding / 255;
        }
        public static List<Sheep> List()
        {
            return Sheeps.Select(x => x.Value).ToList();
        }
        public static Sheep FindASheep(int id)
        {
            if (Sheeps.ContainsKey(id))
                return Sheeps[id];
            return null;
        }
        public static Sheep UpdateASheep(Sheep sheep)
        {
            if (Sheeps.ContainsKey(sheep.Id))
                return Sheeps[sheep.Id] = sheep;
            return sheep;
        }
        public static (Sheep, Shepard) FindAShepard(string shepardName)
        {
            if (!string.IsNullOrWhiteSpace(shepardName))
                foreach (KeyValuePair<int, Sheep> kvp in Sheeps)
                    foreach (Shepard shepard in kvp.Value.Shepards)
                        if (shepard.Name == shepardName)
                            return (kvp.Value, shepard);
            return (null, null);
        }
    }
}
