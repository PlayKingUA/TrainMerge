using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.UI.Displays
{
    public class MultiplierBar : MonoBehaviour
    {
        #region Variables
        [SerializeField] private TextInt[] multipliers;
        [SerializeField] private float oneSideDuration;
        [SerializeField] private Slider slider;

        private Tweener _tween;

        private int _previousIndex = -1;
        
        public event Action OnValueChanged;
        #endregion
        
        #region Monobehaviour Callbacks
        private void Start()
        {
            _tween = slider.DOValue(1f, oneSideDuration).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
            slider.onValueChanged.AddListener(delegate { SliderValueChanged(); });
        }

        private void OnValidate()
        {
            foreach (var multiplier in multipliers)
            {
                multiplier.textMeshPro.text = multiplier.value + "x";
            }
        }
        #endregion
        
        public void StopPointer()
        {
            _tween.Kill();
        }

        public float GetMultiplayer()
        {
            return multipliers[CurrentIndex].value;
        }
        
        private void SliderValueChanged()
        {
            if (CurrentIndex == _previousIndex) 
                return;
            _previousIndex = CurrentIndex;
            OnValueChanged?.Invoke();
        }
        
        private int CurrentIndex => (int) (slider.value / 1f * multipliers.Length);
    }

    [Serializable]
    public class TextInt
    {
        public TextMeshProUGUI textMeshPro;
        public float value;
    }
}