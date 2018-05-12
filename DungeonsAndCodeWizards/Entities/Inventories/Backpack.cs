using System;
using System.Collections.Generic;
using System.Text;

namespace DungeonsAndCodeWizards.Entities.Inventories
{
    public class Backpack : Bag
    {
        public Backpack() : base(capacity: 100)
        {
        }
    }
}
