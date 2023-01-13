using UnityEngine;
using TMPro;

namespace PlayKing.Cor
{
    public class CharacterCanvas : MonoBehaviour
    {
        [SerializeField] Transform targetCharacter;
        [SerializeField] Transform targetMonster;

        Transform _target;
        Transform _transform;

        private void Start()
        {
            _transform = GetComponent<Transform>();
            _transform.parent = null;
            _target = targetCharacter;
        }

        private void LateUpdate()
        {
            transform.position = _target.position;
        }

        public void SetMonsterTarget()
        {
            _target = targetMonster;
        }
    }
}
