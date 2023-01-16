namespace _Scripts.Interface
{
    public interface IMove
    {
        void Move();
        void StopMove();
        void SetSpeed(float targetSpeed);
    }
}