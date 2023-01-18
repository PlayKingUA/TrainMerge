using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Scripts.Helpers
{
    public class RandomMesh : MonoBehaviour
    {
        [SerializeField] private List<Mesh> meshes;
        [SerializeField] private MeshFilter meshFilter;
    
        private void Awake()
        {
            if (meshes.Count > 0)
            {
                meshFilter.mesh = meshes[Random.Range(0, meshes.Count)];
            }
        }
    }
}
