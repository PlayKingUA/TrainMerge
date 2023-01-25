using _Scripts.Levels;
using TMPro;
using UnityEngine;
using Zenject;

namespace _Scripts.UI.Displays
{
    public class LevelDescription : MonoBehaviour
    {
        #region text
        [SerializeField] private TextMeshProUGUI levelText;
        [SerializeField] private ZombieTable zombieTable;

        [Inject] private LevelManager _levelManager;
        #endregion

        private void Start()
        {
            _levelManager.OnLevelLoaded += UpdateText;
        }

        private void UpdateText(Level currentLevel)
        {
            levelText.text = "Level " + currentLevel.index;
            zombieTable.UpdatePanel(currentLevel.Zombies);
        }
    }
}