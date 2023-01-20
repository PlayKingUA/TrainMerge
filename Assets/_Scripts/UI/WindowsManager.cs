using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

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
                CanvasGroupSwap(item, false);
            }

            CanvasGroupSwap(windows[(int)currentWindow], true);
        }
        
        public async void SwapWindow(WindowType window, int delay)
        {
            currentWindow = window;
            foreach (var item in windows)
            {
                CanvasGroupSwap(item, false);
            }

            await Task.Delay(delay);
            CanvasGroupSwap(windows[(int)currentWindow], true);
        }
        
        public static void CanvasGroupSwap(CanvasGroup canvasGroup, bool isEnabled)
        {
            canvasGroup.DOFade(isEnabled? 1 : 0, 0.25f);

            canvasGroup.interactable = isEnabled;
            canvasGroup.blocksRaycasts = isEnabled;
        }
    }
}