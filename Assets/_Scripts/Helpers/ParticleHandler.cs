
namespace _Scripts.Helpers
{
    public class ParticleHandler : PoolElement
    {
        private void OnParticleSystemStopped()
        {
            MasterObjectPooler.Release(gameObject, pool.PoolName);
        }
    }
}