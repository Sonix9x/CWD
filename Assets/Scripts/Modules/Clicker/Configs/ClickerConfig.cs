using UnityEngine;

namespace CWD.Scripts.Modules.Clicker.Config
{
    [CreateAssetMenu(fileName = "ClickerSettings", menuName = "Clicker/Create ClickerSettings")]
    public class ClickerConfig : ScriptableObject
    {
        [SerializeField] private int _clickReward;
        [SerializeField] private int _autoClickDelay;
        [SerializeField] private int _energyPriceForClick;
        [SerializeField] private int _energyPriceForAutoClick;
        [SerializeField] private int _energyAutoRefill;
        [SerializeField] private int _energyAutoRefillDelay;
        [SerializeField] private int _maxEnergy;

        public int ClickReward => _clickReward;
        public int AutoClickDelay => _autoClickDelay;
        public int EnergyPriceForClick => _energyPriceForClick;
        public int EnergyPriceForAutoClick => _energyPriceForAutoClick;
        public int EnergyAutoRefill => _energyAutoRefill;
        public int EnergyAutoRefillDelay => _energyAutoRefillDelay;
        public int MaxEnergy => _maxEnergy;
    }
}
