using UnityEngine;

namespace _Scripts.Units
{
    public class ZombieAttack : MonoBehaviour
    {
        private Zombie _zombie;
        
        private void Awake()
        {
            _zombie = GetComponentInParent<Zombie>();
        }
        
        public void Attack()
        {
            _zombie.Attack();
        }
    }
}