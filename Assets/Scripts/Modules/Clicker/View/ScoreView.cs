using TMPro;
using UnityEngine;

namespace CWD.Scripts.Modules.Clicker.View
{
    public class ScoreView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _scoreText;

        public void SetScoreText(string text) => _scoreText.SetText(text);
    }
}
