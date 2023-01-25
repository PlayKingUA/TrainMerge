using TMPro;
using UnityEngine;

namespace _Scripts.UI.Displays
{
    public class ZombieDisplay : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI countText;

        public void UpdateCount(int count)
        {
            gameObject.SetActive(count != 0);
            countText.text = count.ToString();
        }
    }
}