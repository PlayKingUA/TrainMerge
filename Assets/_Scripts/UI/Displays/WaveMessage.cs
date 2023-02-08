using System.Collections;
using System.Collections.Generic;
using _Scripts.Units;
using UnityEngine;
using Zenject;

public class WaveMessage : MonoBehaviour
{
    [Inject] private ZombieManager _zombieManager;
    
    private void Start()
    {
        gameObject.SetActive(false);
        _zombieManager.HugeWaveMessage += () => { gameObject.SetActive(true); };
        _zombieManager.LastWaveStarted += () => { gameObject.SetActive(false); };
    }

}
