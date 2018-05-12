using DungeonsAndCodeWizards.Entities.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DungeonsAndCodeWizards.Entities.Inventories
{
    public abstract class Bag
    {
        private readonly List<Item> items;

        protected Bag(int capacity)
        {
            this.Capacity = capacity;
            this.items = new List<Item>();
        }

        public int Capacity { get; }

        public int Load => this.items.Sum(item => item.Weight);

        public IReadOnlyCollection<Item> Items => this.items.AsReadOnly();

        public void AddItem(Item item)
        {
            if (this.Load + item.Weight > this.Capacity)
            {
                throw new InvalidOperationException("Bag is full!");
            }

            this.items.Add(item);
        }

        public Item GetItem(string name)
        {
            if (this.items.Count <= 0)
            {
                throw new InvalidOperationException("Bag is empty!");
            }

            if (!this.items.Any(item => item.GetType().Name == name))
            {
                throw new ArgumentException($"No item with name {name} in bag!");
            }

            Item itemToRemove = this.items.FirstOrDefault(item => item.GetType().Name == name);
            this.items.Remove(itemToRemove);

            return itemToRemove;
        }
    }
}
