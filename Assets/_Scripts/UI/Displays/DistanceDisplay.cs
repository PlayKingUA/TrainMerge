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
        #endregion

        private void Start()
        {
            _train.DistanceChanged += Display;
        }


        private void Display(float meters)
        {
            distanceText.text = meters.ToString("F0");
        }
    }
}