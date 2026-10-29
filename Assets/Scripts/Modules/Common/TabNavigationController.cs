using UnityEngine;

namespace CWD.Scripts.Modules.Common.Controller
{
    public class TabNavigationController : MonoBehaviour
    {
        [SerializeField] private TabControllerBase[] _tabs;
        [SerializeField] private int _initialTabIndex;

        private int _currentTabIndex = -1;

        private void Start()
        {
            SelectTab(_initialTabIndex);
        }

        public void SelectTab(int index)
        {
            if (index < 0 || index >= _tabs.Length)
            {
                return;
            }

            if (_currentTabIndex == index)
            {
                return;
            }

            if (_currentTabIndex >= 0)
            {
                var previousTab = _tabs[_currentTabIndex];

                previousTab.OnTabClosed();
                previousTab.SetTabVisible(false);
            }

            _currentTabIndex = index;

            var currentTab = _tabs[index];

            currentTab.SetTabVisible(true);
            currentTab.OnTabOpened();
        }

        public void OnClickerToggleChanged(bool isOn)
        {
            if (isOn)
            {
                SelectTab(0);
            }
        }

        public void OnDogsToggleChanged(bool isOn)
        {
            if (isOn)
            {
                SelectTab(2);
            }
        }

        public void OnWeatherToggleChanged(bool isOn)
        {
            if (isOn)
            {
                SelectTab(1);
            }
        }
    }
}