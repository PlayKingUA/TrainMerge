using System.Collections;
using QFSW.MOP2;
using UnityEngine;

namespace _Scripts.Helpers
{
    public class PoolElement : MonoBehaviour
    {
        [SerializeField] protected ObjectPool pool;
        [SerializeField] private float lifeTime;

        protected MasterObjectPooler MasterObjectPooler;
        private WaitForSeconds _wait;

        private void Awake()
        {
            MasterObjectPooler = MasterObjectPooler.Instance;
            _wait = new WaitForSeconds(lifeTime);
        }

        private void OnEnable()
        {
            StartCoroutine(ReturnToPool());
        }

        private IEnumerator ReturnToPool()
        {
            yield return _wait;
            MasterObjectPooler.Release(gameObject, pool.PoolName);
        }
    }
}