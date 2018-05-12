using DungeonsAndCodeWizards.Entities.Inventories;
using DungeonsAndCodeWizards.Entities.Items;
using System;
using System.Collections.Generic;
using System.Text;

namespace DungeonsAndCodeWizards.Entities.Characters
{
    public enum Faction
    {
        CSharp,
        Java
    }

    public abstract class Character
    {
        private string name;

        protected Character(string name, double health, double armor, double abilityPoints, Bag bag, Faction faction)
        {
            this.Name = name;
            this.BaseHealth = health;
            this.Health = health;
            this.BaseArmor = armor;
            this.Armor = armor;
            this.AbilityPoints = abilityPoints;
            this.Bag = bag;
            this.Faction = faction;
            this.IsAlive = true;
            this.RestHealMultiplier = 0.2;
        }

        public string Name
        {
            get
            {
                return this.name;
            }
            private set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Name cannot be null or whitespace!");
                }

                this.name = value;
            }
        }

        public double BaseHealth { get; private set; }

        public double Health { get; set; }

        public double BaseArmor { get; private set; }

        public double Armor { get; set; }

        public double AbilityPoints { get; private set; }

        public Bag Bag { get; }

        public Faction Faction { get; }

        public bool IsAlive { get; set; }

        public double RestHealMultiplier { get; set; }

        public void TakeDamage(double hitPoints)
        {
            if (!this.IsAlive)
            {
                return;
            }

            this.Armor -= hitPoints;
            if (this.Armor < 0)
            {
                hitPoints = Math.Abs(this.Armor);
                this.Armor = 0;
                this.Health -= hitPoints;
                if (this.Health <= 0)
                {
                    this.Health = 0;
                    this.IsAlive = false;
                }
            }
        }

        public void Rest()
        {
            if (!this.IsAlive)
            {
                return;
            }

            var health = this.Health + (this.BaseHealth * this.RestHealMultiplier);
            if (health > this.BaseHealth)
            {
                this.Health = this.BaseHealth;
            }
            else
            {
                this.Health = health;
            }
        }

        public void UseItem(Item item)
        {
            if (!this.IsAlive)
            {
                return;
            }

            item.AffectCharacter(this);
        }

        public void UseItemOn(Item item, Character character)
        {
            if (!this.IsAlive & !character.IsAlive)
            {
                return;
            }

            item.AffectCharacter(character);
        }

        public void GiveCharacterItem(Item item, Character character)
        {
            if (!this.IsAlive & !character.IsAlive)
            {
                return;
            }

            character.Bag.AddItem(item);
        }

        public void ReceiveItem(Item item)
        {
            if (!this.IsAlive)
            {
                return;
            }

            this.Bag.AddItem(item);
        }
    }
}
