using System;

namespace _Scripts.Interface
{
    public interface IAlive
    {
        bool IsDead { get; }
        
        event Action<IAlive> DeadEvent;
        event Action DamageEvent;
        
        void GetDamage(int damagePoint);
        void Die();
    }
}