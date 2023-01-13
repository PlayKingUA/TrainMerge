using System.Collections;
using UnityEngine;

namespace PlayKing.Cor
{
    public class CharacterStatesAnimation : MonoBehaviour
    {
        [SerializeField] Animator _animCharacter;
        [SerializeField] Animator _animMonster;
        private bool isMonsterStage;

        public void RunAnimation(bool isRun)
        {
            if(isMonsterStage)
            {
                _animMonster.SetBool("Run", isRun);
                return;
            }

            _animCharacter.SetBool("Run", isRun);
        }

        public void JumpAnimation()
        {
            _animCharacter.SetTrigger("Jump");
        }

        public void IsMonsterStage(bool monsterStage)
        {
            isMonsterStage = monsterStage;
        }
    }
}