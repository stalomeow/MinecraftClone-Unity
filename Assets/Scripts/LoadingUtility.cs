using ToaruUnity.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Minecraft
{
    public static class LoadingUtility
    {
        private const string LoadingMenuKey = "Loading Menu";

        public static void LoadSceneAsync(int index)
        {
            AsyncOperation operation = SceneManager.LoadSceneAsync(index, LoadSceneMode.Single);
            IUIManager manager = Object.FindObjectOfType<UIManager>();

            if (manager != null)
            {
                while (manager.ViewCount > 0)
                {
                    manager.CloseActiveView();
                }

                manager.SwitchView(LoadingMenuKey, SwitchViewMode.NavigateOrOpenNew, new SwitchViewParameters
                {
                    OpenOrNavigateViewParam = operation
                });
            }
        }
    }
}