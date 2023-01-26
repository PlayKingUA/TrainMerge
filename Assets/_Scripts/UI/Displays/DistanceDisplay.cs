using TMPro;
using UnityEngine;
using Zenject;

namespace _Scripts.UI.Displays
{
    public class DistanceDisplay : MonoBehaviour
    {
        #region Variables
        [SerializeField] private TextMeshProUGUI distanceText;
        
        [Inject] private Train.Train _train;

        private const string Format = "F1";
        #endregion

        private void Start()
        {
            _train.DistanceChanged += Display;
        }

        private void Display(float value)
        {
            var result = value.ToString("F0") + 'm';
            if (value > 1e3)
            {
                result = (value / 1e3).ToString(Format) + "km";
            }
            distanceText.text = result;
        }
    }
}