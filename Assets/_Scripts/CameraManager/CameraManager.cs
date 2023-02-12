using _Scripts.Game_States;
using Cinemachine;
using UnityEngine;
using Zenject;

namespace _Scripts.CameraManager
{
    public class CameraManager : MonoBehaviour
    {
        #region Variables
        [Inject] private GameStateManager _gameStateManager;
        [SerializeField] private CinemachineVirtualCamera[] cameras;
        [SerializeField] private CameraType currentCameraType;

        private CinemachineVirtualCamera _currentCamera;
        #endregion

        private void Start()
        {
            _gameStateManager.AttackStarted += () =>
            {
                ChangeCameraPriority(CameraType.Attack);
            };
        }

        #region Change Camera
        private void ChangeCameraPriority(CameraType cameraType)
        {
            currentCameraType = cameraType;

            foreach (var virtualCamera in cameras)
            {
                virtualCamera.m_Priority = 0;
            }
            _currentCamera = cameras[(int)currentCameraType];
            _currentCamera.m_Priority = 100;
        }

        #endregion`
    }
}
