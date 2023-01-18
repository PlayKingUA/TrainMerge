using QFSW.MOP2;
using UnityEngine;

namespace _Scripts.Helpers
{
    public class ParticleHandler : MonoBehaviour
    {
        [SerializeField] private string poolName;
        private MasterObjectPooler _masterObjectPooler;

        private void Start()
        {
            _masterObjectPooler = MasterObjectPooler.Instance;
        }

        private void OnParticleSystemStopped()
        {
            _masterObjectPooler.Release(gameObject, poolName);
        }
    }
}