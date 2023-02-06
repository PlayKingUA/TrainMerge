using _Scripts.Game_States;
using _Scripts.Weapons;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(ParticleSystem))]
public class SpeedLineSettings : MonoBehaviour
{
    [SerializeField, Range(1f, 10f)] private float maxMultiplier = 1.7f;
    
    private ParticleSystem _lines;
    private ParticleSystem.EmissionModule _linesEmission;
    private float _startMultiplier;

    [Inject] private GameStateManager _gameStateManager;
    [Inject] private SpeedUpLogic _speedUpLogic;
    
    private void Awake()
    {
        _lines = GetComponent<ParticleSystem>();
        _linesEmission = _lines.emission;
        
        _startMultiplier = _linesEmission.rateOverTimeMultiplier;
    }

    private void Start()
    {
        _gameStateManager.AttackStarted += () =>
        {
            _lines.Play();
        };

        _gameStateManager.Fail += () =>
        {
            _lines.Stop();
        };

        _speedUpLogic.OnTapCountChanged += () =>
        {
            _linesEmission.rateOverTimeMultiplier = _startMultiplier 
                                                    + _startMultiplier * (maxMultiplier - 1) * _speedUpLogic.EffectPower;
        };
    }
}
