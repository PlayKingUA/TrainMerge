using _Scripts.Managers;
using _Scripts.Money_Logic;
using UnityEngine;
using Zenject;

namespace _Scripts.Resources
{
    public class ServicesInstaller : MonoInstaller
    {
        [SerializeField] private SlotManager slotManager;
        [SerializeField] private MoneyWallet moneyWallet;
        [SerializeField] private WeaponManager weaponManager;
    
        public override void InstallBindings()
        {
            Container.Bind<SlotManager>().FromInstance(slotManager).AsSingle().NonLazy();
            Container.Bind<MoneyWallet>().FromInstance(moneyWallet).AsSingle().NonLazy();
            Container.Bind<WeaponManager>().FromInstance(weaponManager).AsSingle().NonLazy();
        }
    }
}