using _Scripts.Game_States;
using _Scripts.Input_Logic;
using _Scripts.Levels;
using _Scripts.Money_Logic;
using _Scripts.Slot_Logic;
using _Scripts.UI;
using _Scripts.Units;
using _Scripts.Weapons;
using UnityEngine;
using Zenject;

namespace _Scripts.Resources
{
    public class ServicesInstaller : MonoInstaller
    {
        [SerializeField] private SlotManager slotManager;
        [SerializeField] private MoneyWallet moneyWallet;
        [SerializeField] private WeaponManager weaponManager;
        [SerializeField] private InputHandler inputHandler;
        [SerializeField] private DragManager dragManager;
        [SerializeField] private WindowsManager windowsManager;
        [SerializeField] private ZombieManager zombieManager;
        [SerializeField] private GameStateManager gameStateManager;
        [SerializeField] private LevelManager levelManager;
        
        public override void InstallBindings()
        {
            Container.Bind<SlotManager>().FromInstance(slotManager).AsSingle().NonLazy();
            Container.Bind<MoneyWallet>().FromInstance(moneyWallet).AsSingle().NonLazy();
            Container.Bind<WeaponManager>().FromInstance(weaponManager).AsSingle().NonLazy();
            Container.Bind<InputHandler>().FromInstance(inputHandler).AsSingle().NonLazy();
            Container.Bind<DragManager>().FromInstance(dragManager).AsSingle().NonLazy();
            Container.Bind<WindowsManager>().FromInstance(windowsManager).AsSingle().NonLazy();
            Container.Bind<ZombieManager>().FromInstance(zombieManager).AsSingle().NonLazy();
            Container.Bind<GameStateManager>().FromInstance(gameStateManager).AsSingle().NonLazy();
            Container.Bind<LevelManager>().FromInstance(levelManager).AsSingle().NonLazy();
        }
    }
}