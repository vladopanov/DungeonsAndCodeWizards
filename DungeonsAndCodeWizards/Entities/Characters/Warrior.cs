using DungeonsAndCodeWizards.Entities.Characters.Contracts;
using DungeonsAndCodeWizards.Entities.Inventories;
using System;
using System.Collections.Generic;
using System.Text;

namespace DungeonsAndCodeWizards.Entities.Characters
{
    public class Warrior : Character, IAttackable
    {
        public Warrior(string name, Faction faction)
            : base(name, 100, 50, 40, new Satchel(), faction)
        {
        }

        public void Attack(Character character)
        {
            if (!this.IsAlive | !character.IsAlive)
            {
                throw new InvalidOperationException("Must be alive to perform this action!");
            }

            if (this == character)
            {
                throw new InvalidOperationException("Cannot attack self!");
            }

            if (this.Faction == character.Faction)
            {
                throw new ArgumentException($"Friendly fire! Both characters are from {this.Faction} faction!");
            }

            character.TakeDamage(this.AbilityPoints);
        }
    }
}
