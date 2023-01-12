using System.Linq;
using _Scripts.Weapons;
using UnityEngine;

namespace _Scripts.Platforms
{
    public class PlatformsManager : Singleton<PlatformsManager>
    {
        [SerializeField] private Weapon firstLevelWeapon;
        private Platform[] _platforms;

        public bool HasFreePlace() => _platforms.Any(platform => platform.IsEmpty);

        public void CreateNewWeapon()
        {
            var emptyPlatform = _platforms.FirstOrDefault(platform => platform.IsEmpty);
            emptyPlatform.CreateWeapon(firstLevelWeapon);
        }
        
        private void Start()
        {
            _platforms = transform.GetComponentsInChildren<Platform>();
        }
    }
}