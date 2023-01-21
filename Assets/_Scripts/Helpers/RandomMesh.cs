using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Scripts.Helpers
{
    public class RandomMesh : MonoBehaviour
    {
        private void Awake()
        {
            var targetMesh = Random.Range(0, transform.childCount);
            for (var i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(i == targetMesh);
            }
        }
    }
}
