using TMPro;
using UnityEngine;

namespace _Scripts.Helpers
{
    public class DamageText : PoolElement
    {
        [SerializeField] private TextMeshPro text;

        public void SetText(string targetText)
        {
            text.text = targetText;
        }
    }
}