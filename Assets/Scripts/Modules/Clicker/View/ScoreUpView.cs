using CWD.Scripts.Modules.Clicker.Model;
using CWD.Scripts.Modules.Clicker.Pool;
using DG.Tweening;
using TMPro;
using UnityEngine;
using Zenject;

namespace CWD.Scripts.Modules.Clicker.View
{
    public class ScoreUpView : MonoBehaviour
    {
        [SerializeField] private RectTransform _scoreUp;
        [SerializeField] private TextMeshProUGUI _scoreText;

        private ScoreUpPool _pool;
        private Sequence _appearanceSequence;

        [Inject]
        public void Construct(ScoreUpPool pool)
        {
            _pool = pool;
        }

        public void Play(ScoreUpModel model)
        {
            _scoreText.text = $"+{model.ScoreForClick}";

            KillAnimation();

            _appearanceSequence = DOTween.Sequence();

            _scoreUp.localScale = Vector3.one;
            _scoreUp.localPosition = Vector3.zero;
            
            _appearanceSequence.Append(_scoreUp.DOLocalMoveY(270f, 1f))
                               .Join(_scoreUp.DOScale(Vector3.zero, 1f));
            
            _appearanceSequence.OnComplete(() =>
                               {
                                   _pool.Despawn(this);
                               });
        }

        private void OnDisable()
        {
            KillAnimation();
        }

        private void KillAnimation()
        {
            if (_appearanceSequence != null)
            {
                if (_appearanceSequence.IsActive())
                {
                    _appearanceSequence.Kill();
                }
                _appearanceSequence = null;
            }
        }
    }
}