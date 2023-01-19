using _Scripts.Money_Logic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace _Scripts.UI.Displays
{
    public class HpDisplay : MonoBehaviour
    {
        #region Variables
        [SerializeField] private TextMeshProUGUI hpText;
        [SerializeField] private Slider slider;
        [Inject] private Train.Train _train;
        #endregion
        
        #region Monobehaviour Callbacks
        private void Awake()
        {
            _train.HpChanged += Display;
        }
        #endregion
        
        private void Display()
        {
            hpText.text = _train.CurrentHealth + "/" + _train.MaxHealth;
            slider.value = _train.CurrentHealth / _train.MaxHealth;
        }
    }
}