using DG.Tweening;
using UnityEngine;

namespace _Scripts.Helpers
{
    public class ScaleAnimation : MonoBehaviour
    {
        [SerializeField] private float maxScale;
        [SerializeField] private float animationSpeed;
        
        private void Start()
        {
            var maxSize = transform.localScale * maxScale;
            transform.DOScale(maxSize, animationSpeed / 2f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
        }
    }
}
