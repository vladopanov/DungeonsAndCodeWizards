using System;
using System.Collections.Generic;
using System.Text;

namespace DungeonsAndCodeWizards.Entities.Characters.Contracts
{
    public interface IHealable
    {
        void Heal(Character character);
    }
}
