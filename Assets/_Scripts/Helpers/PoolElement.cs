using System.Collections;
using QFSW.MOP2;
using UnityEngine;

namespace _Scripts.Helpers
{
    public class PoolElement : MonoBehaviour
    {
        [SerializeField] private ObjectPool pool;
        [SerializeField] private float lifeTime;
        
        private MasterObjectPooler _masterObjectPooler;
        private WaitForSeconds _wait;

        private void Awake()
        {
            _masterObjectPooler = MasterObjectPooler.Instance;
            _wait = new WaitForSeconds(lifeTime);
        }

        private void OnEnable()
        {
            StartCoroutine(ReturnToPool());
        }

        private IEnumerator ReturnToPool()
        {
            yield return _wait;
            _masterObjectPooler.Release(gameObject, pool.PoolName);
        }
    }
}