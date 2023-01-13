using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace PlayKing.Cor
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class BotMovement : MonoBehaviour
    {
        NavMeshAgent _agent;
        CharacterStatesAnimation _characterStatesAnimation;

        private void Start()
        {
            _agent = GetComponent<NavMeshAgent>();
            _characterStatesAnimation = GetComponentInChildren<CharacterStatesAnimation>();
        }
    }
}
