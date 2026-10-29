using System;
using UnityEngine;

namespace CWD.Scripts.Modules.Clicker.Model
{
    public class ScoreModel
    {
        public event Action OnScoreChanged;
        public float Score { get; private set; }

        public ScoreModel(float score)
        {
            Score = score;
        }

        public void UpdateScore(float score)
        {

            var newScore = Score + score;

            Score = Mathf.Clamp(newScore, 0, newScore);

            OnScoreChanged?.Invoke();
        }
    }
}