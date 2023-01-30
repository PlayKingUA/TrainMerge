using System;
using _Scripts.Helpers;
using QFSW.MOP2;
using UnityEngine;

namespace _Scripts.Projectiles
{
    public class ProjectileParticle : MonoBehaviour
    {
        #region Variables
        private string _poolName;
        private float _speed;
        
        private MasterObjectPooler _masterObjectPooler;
        #endregion

        #region Monobehaviour Callbacks
        private void Awake()
        {
            _masterObjectPooler = MasterObjectPooler.Instance;
        }

        private void Update()
        {
            var transform1 = transform;
            transform1.position += transform1.forward * Time.deltaTime * _speed;
        }

        private void OnTriggerEnter(Collider other)
        {
            ReleaseObject();
        }
        #endregion
        
        public void Init(string poolName, float lifeTime, float speed)
        {
            _speed = speed;
            _poolName = poolName;
            
            Invoke(nameof(ReleaseObject), lifeTime);
        }

        public void ReleaseObject()
        {
            _masterObjectPooler.Release(gameObject, _poolName);
        }
    }
}