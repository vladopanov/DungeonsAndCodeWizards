using DungeonsAndCodeWizards.Entities.Characters.Contracts;
using DungeonsAndCodeWizards.Entities.Inventories;
using System;
using System.Collections.Generic;
using System.Text;

namespace DungeonsAndCodeWizards.Entities.Characters
{
    public class Cleric : Character, IHealable
    {
        public Cleric(string name, Faction faction) 
            : base(name, 50, 25, 40, new Backpack(), faction)
        {
        }

        public void Heal(Character character)
        {
            if (!this.IsAlive | !character.IsAlive)
            {
                throw new InvalidOperationException("Must be alive to perform this action!");
            }

            if (this.Faction != character.Faction)
            {
                throw new InvalidOperationException("Cannot heal enemy character!");
            }

            double health = character.Health + this.AbilityPoints;
            if(health > character.BaseHealth)
            {
                character.Health = character.BaseHealth;
            }
            else
            {
                character.Health = health;
            }
        }
    }
}
