using System.Collections;
using UnityEngine;

namespace PlayKing.Cor
{
    public class CharacterStates : MonoBehaviour
    {
        [SerializeField] Character _character;
        [SerializeField] CharacterMonster _characterMonster;
        [SerializeField] CharacterStatesAnimation _characterStatesAnimation;
        [SerializeField] CharacterCanvas _characterCanvas;
        [SerializeField] PlayerMovement _playerMovement;
        private bool isMonsterStage;

        BallsMonster monster;

        public void CharacterTransformation(BallsMonster ballsMonster)
        {
            if (isMonsterStage)
                return;

            monster = ballsMonster;

            if (_playerMovement != null)
                _playerMovement.LockControll(true);

            _characterStatesAnimation.JumpAnimation();
            _characterStatesAnimation.IsMonsterStage(true);
            _characterCanvas.SetMonsterTarget();
            StartCoroutine(IE_CharacterInMonster());
            StartCoroutine(IE_ActivetedMonster());
            isMonsterStage = true;
        }

        private IEnumerator IE_CharacterInMonster()
        {
            yield return new WaitForSeconds(0.5f);

            _playerMovement.MovementToTarget(monster.transform);
            _character.JumpToMontser();
        }

        private IEnumerator IE_ActivetedMonster()
        {
            yield return new WaitForSeconds(0.9f);

            monster.gameObject.SetActive(false);
            _character.gameObject.SetActive(false);
            _characterMonster.gameObject.SetActive(true);
           
            if (_playerMovement != null)
                _playerMovement.LockControll(false);
        }
    }
}
