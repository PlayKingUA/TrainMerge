using UnityEngine;
using Random = UnityEngine.Random;

namespace _Scripts.Helpers
{
    public class RandomChild : MonoBehaviour
    {
        private void Awake()
        {
            var targetChild = Random.Range(0, transform.childCount);
            for (var i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(i == targetChild);
            }
        }
    }
}
