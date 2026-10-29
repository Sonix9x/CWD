using CWD.Scripts.Modules.Clicker.Config;
using CWD.Scripts.Modules.Clicker.Enum;
using CWD.Scripts.Modules.Clicker.Model;

namespace CWD.Scripts.Modules.Clicker.Helper
{
    public class CanClickChecker
    {
        private readonly ClickerConfig _clickerConfig;
        private readonly EnergyModel _energyModel;

        public CanClickChecker (ClickerConfig clickerConfig, EnergyModel energyModel)
        {
            _clickerConfig = clickerConfig;
            _energyModel = energyModel;
        }

        public bool CanClick(ClickReason clickReason)
        {
            switch (clickReason)
            {
                case ClickReason.Manual:
                    return _energyModel.CurrentEnergy >= _clickerConfig.EnergyPriceForClick;
                case ClickReason.Auto:
                    return _energyModel.CurrentEnergy >= _clickerConfig.EnergyPriceForAutoClick;
                default:
                    return true;
            }
        }
    }
}