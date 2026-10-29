using TMPro;
using UnityEngine;

namespace CWD.Scripts.Modules.Clicker.View
{
    public class EnergyView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _energyText;

        public void SetEnergyText(string text) => _energyText.SetText(text);
    }
}
