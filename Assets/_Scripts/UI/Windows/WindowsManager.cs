using System.Collections;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace _Scripts.UI.Windows
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
        
        public void SwapWindow(WindowType window, float delay)
        {
            StartCoroutine(SwapWithDelay(window, delay));
        }

        private IEnumerator SwapWithDelay(WindowType window, float delay)
        {
            currentWindow = window;
            foreach (var item in windows)
            {
                CanvasGroupSwap(item, false);
            }

            yield return new WaitForSecondsRealtime(delay);
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