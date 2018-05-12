using System;
using System.Collections.Generic;
using System.Text;

namespace DungeonsAndCodeWizards.Entities.Characters.Contracts
{
    public interface IAttackable
    {
        void Attack(Character character);
    }
}
