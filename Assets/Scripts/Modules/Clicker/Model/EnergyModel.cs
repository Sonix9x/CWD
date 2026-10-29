using System;
using UnityEngine;

namespace CWD.Scripts.Modules.Clicker.Model
{
    public class EnergyModel
    {
        public event Action OnEnergyChanged;

        public float CurrentEnergy { get; private set; }
        public float MaxEnergy { get; private set; }

        public EnergyModel(float currentEnergy, float maxEnergy)
        {
            CurrentEnergy = currentEnergy;
            MaxEnergy = maxEnergy;
        }

        public void UpdateEnergy(float score)
        {
            var oldEnergy = CurrentEnergy;

            CurrentEnergy = Mathf.Clamp(CurrentEnergy + score, 0, MaxEnergy);

            if (oldEnergy != CurrentEnergy)
            {
                OnEnergyChanged?.Invoke();
            }
        }
    }
}