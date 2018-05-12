using DungeonsAndCodeWizards.Entities.Characters;
using DungeonsAndCodeWizards.Entities.Characters.Contracts;
using DungeonsAndCodeWizards.Entities.Factories;
using DungeonsAndCodeWizards.Entities.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DungeonsAndCodeWizards.Core
{
    public class DungeonMaster
    {
        private readonly IList<Character> party;
        private readonly IList<Item> itemPool;
        private int rounds;

        private readonly CharacterFactory characterFactory;
        private readonly ItemFactory itemFactory;

        public DungeonMaster()
        {
            this.party = new List<Character>();
            this.itemPool = new List<Item>();
            this.rounds = 2;

            this.characterFactory = new CharacterFactory();
            this.itemFactory = new ItemFactory();
        }

        public string JoinParty(string[] args)
        {
            string faction = args[0];
            string characterType = args[1];
            string name = args[2];

            if (!Enum.IsDefined(typeof(Faction), faction))
            {
                throw new ArgumentException($"Invalid faction \"{faction}\"!");
            }

            var character = this.characterFactory.CreateCharacter(faction, characterType, name);
            this.party.Add(character);

            return $"{name} joined the party!";
        }

        public string AddItemToPool(string[] args)
        {
            string itemName = args[0];

            var item = this.itemFactory.CreateItem(itemName);
            this.itemPool.Add(item);

            return $"{itemName} added to pool.";
        }

        public string PickUpItem(string[] args)
        {
            string characterName = args[0];
            Character character = this.GetCharacterByName(characterName);

            if (!this.itemPool.Any())
            {
                throw new InvalidOperationException("No items left in pool!");
            }

            var item = this.itemPool.LastOrDefault();
            this.itemPool.Remove(item);
            character.Bag.AddItem(item);

            return $"{characterName} picked up {item.GetType().Name}!";
        }

        public string UseItem(string[] args)
        {
            string characterName = args[0];
            string itemName = args[1];

            Character character = this.GetCharacterByName(characterName);
            Item item = character.Bag.GetItem(itemName);

            character.UseItem(item);

            return $"{character.Name} used {itemName}.";
        }

        public string UseItemOn(string[] args)
        {
            string giverName = args[0];
            string receiverName = args[1];
            string itemName = args[2];

            Character giver = this.GetCharacterByName(giverName);
            Character receiver = this.GetCharacterByName(receiverName);
            Item item = giver.Bag.Items.FirstOrDefault(i => i.GetType().Name == itemName);

            receiver.UseItem(item);

            return $"{giverName} used {itemName} on {receiverName}.";
        }

        public string GiveCharacterItem(string[] args)
        {
            string giverName = args[0];
            string receiverName = args[1];
            string itemName = args[2];

            Character giver = this.GetCharacterByName(giverName);
            Character receiver = this.GetCharacterByName(receiverName);
            Item item = giver.Bag.Items.FirstOrDefault(i => i.GetType().Name == itemName);

            giver.Bag.GetItem(item.GetType().Name);
            receiver.Bag.AddItem(item);

            return $"{giverName} gave {receiverName} {itemName}.";
        }

        public string GetStats()
        {
            var sb = new StringBuilder();
            var orderedCharacters = this.party.OrderByDescending(c => c.IsAlive).ThenByDescending(c => c.Health);
            sb.AppendLine("Final stats:");
            foreach(var c in orderedCharacters)
            {
                string status = c.IsAlive ? "Alive" : "Dead";
                sb.AppendLine($"{c.Name} - HP: {c.Health}/{c.BaseHealth}, AP: {c.Armor}/{c.BaseArmor}, Status: {status}");
            }

            return sb.ToString();
        }

        public string Attack(string[] args)
        {
            string attackerName = args[0];
            string receiverName = args[1];

            var attacker = this.GetCharacterByName(attackerName) as IAttackable;
            double attackerAbilityPoints = this.GetCharacterByName(attackerName).AbilityPoints;
            Character receiver = this.GetCharacterByName(receiverName);

            if (attacker.GetType().GetMethod("Attack") == null)
            {
                throw new ArgumentException($"{attackerName} cannot attack!");
            }

            attacker.Attack(receiver);

            var sb = new StringBuilder();
            sb.AppendLine($"{attackerName} attacks {receiverName} for {attackerAbilityPoints} hit points! {receiverName} has " +
                $"{receiver.Health}/{receiver.BaseHealth} HP and {receiver.Armor}/{receiver.BaseArmor} AP left!");

            if (!receiver.IsAlive)
            {
                sb.AppendLine($"{receiver.Name} is dead!");
            }

            return sb.ToString().TrimEnd('\r', '\n');
        }

        public string Heal(string[] args)
        {
            string healerName = args[0];
            string healingReceiverName = args[1];

            IHealable healer = this.GetCharacterByName(healerName) as IHealable;
            double abilityPoints = this.GetCharacterByName(healingReceiverName).AbilityPoints;
            Character receiver = this.GetCharacterByName(healingReceiverName);

            if (healer.GetType().GetMethod("Heal") == null)
            {
                throw new ArgumentException($"{healerName} cannot heal!");
            }

            healer.Heal(receiver);

            return $"{healerName} heals {receiver.Name} for {abilityPoints}! {receiver.Name} has {receiver.Health} health now!";
        }

        public string EndTurn()
        {
            var aliveCharacters = this.party.Where(c => c.IsAlive);

            var sb = new StringBuilder();
            foreach (var ac in aliveCharacters)
            {
                double healthBeforeRest = ac.Health;
                ac.Rest();
                sb.AppendLine($"{ac.Name} rests ({healthBeforeRest} => {ac.Health})");
            }

            if (aliveCharacters.Count() <= 1)
            {
                this.rounds -= 1;
            }

            return sb.ToString().TrimEnd('\r', '\n');
        }

        public bool IsGameOver()
        {
            return this.rounds <= 0;
        }

        private Character GetCharacterByName(string characterName)
        {
            var character = this.party.FirstOrDefault(c => c.Name == characterName);

            if (character == null)
            {
                throw new ArgumentException($"Character {characterName} not found!");
            }

            return character;
        }
    }
}
