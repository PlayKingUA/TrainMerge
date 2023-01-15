using DG.Tweening;
using UnityEngine;

namespace _Scripts.UI
{
    public class WindowsManager : MonoBehaviour
    {
        #region Variables
        [SerializeField] private CanvasGroup[] windows;
        [SerializeField] private WindowType currentWindow;
        #endregion
        
        #region Monobehaviour Callbacks
        private void Start()
        {
            SwapWindow(WindowType.PrepareToLevel);
        }
        #endregion
        
        public void SwapWindow(WindowType window)
        {
            currentWindow = window;
            foreach (var item in windows)
            {
                CanvasGroupSwap(item, 0, false);
            }

            CanvasGroupSwap(windows[(int)currentWindow], 1, true);
        }
        
        private static void CanvasGroupSwap(CanvasGroup canvasGroup, float alpha, bool isEnabled)
        {
            canvasGroup.DOFade(alpha, 0.25f);

            canvasGroup.interactable = isEnabled;
            canvasGroup.blocksRaycasts = isEnabled;
        }
    }
}