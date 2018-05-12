using DungeonsAndCodeWizards.Entities.Items;
using System;
using System.Linq;

namespace DungeonsAndCodeWizards.Entities.Factories
{
    public class ItemFactory
    {
        public Item CreateItem(string itemName)
        {
            var type = this.GetType()
                .Assembly
                .GetTypes()
                .FirstOrDefault(t => typeof(Item).IsAssignableFrom(t) && !t.IsAbstract && t.Name == itemName);

            if (type == null)
            {
                throw new ArgumentException($"Invalid item \"{itemName}\"!");
            }

            var item = (Item)Activator.CreateInstance(type);

            return item;
        }
    }
}
