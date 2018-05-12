using DungeonsAndCodeWizards.Entities.Characters;
using System;
using System.Linq;

namespace DungeonsAndCodeWizards.Entities.Factories
{
    public class CharacterFactory
    {
        public Character CreateCharacter(string faction, string characterType, string name)
        {
            var type = this.GetType()
                .Assembly
                .GetTypes()
                .FirstOrDefault(t => typeof(Character).IsAssignableFrom(t) && !t.IsAbstract && t.Name == characterType);

            if (type == null)
            {
                throw new ArgumentException($"Invalid character type \"{characterType}\"!");
            }

            if (!Enum.TryParse<Faction>(faction, out var parsedFaction))
            {
                throw new ArgumentException($"Invalid faction \"{faction}\"!");
            }

            var character = (Character)Activator.CreateInstance(type, new Object[] { name, (Faction)Enum.Parse(typeof(Faction), faction) });

            return character;
        }
    }
}
