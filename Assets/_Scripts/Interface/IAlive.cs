namespace _Scripts.Interface
{
    public interface IAlive
    {
        bool IsDead { get; }

        void GetDamage(int damagePoint);
        void Die();
    }
}