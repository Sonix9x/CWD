using UnityEngine;

namespace CWD.Scripts.Modules.Common.Controller
{
    public abstract class TabControllerBase : MonoBehaviour
    {
        [SerializeField] private GameObject _tabRoot;

        public void SetTabVisible(bool visible)
        {
            if (_tabRoot != null)
            {
                _tabRoot.SetActive(visible);
            }
        }

        public abstract void OnTabOpened();
        public abstract void OnTabClosed();
    }
}