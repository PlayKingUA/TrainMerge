using UnityEngine;
using DG.Tweening;

public enum CharacterColorType
{
    Blue,
    Green,
    Yellow
}

namespace PlayKing.Cor
{
    public class Character : MonoBehaviour
    {
        [SerializeField] GameObject crown;
        [SerializeField] CharacterColorType _characterColorType;

        StackBalls _stackBalls;
        CharacterStates _characterStates;
        BallsMonster _ballsMoster;

        private void Start()
        {
            _stackBalls = GetComponent<StackBalls>();
            _characterStates = GetComponentInParent<CharacterStates>();
        }

        public void KnockCharacter()
        {

        }

        public void JumpToMontser()
        {
            transform.DOJump(new Vector3(_ballsMoster.transform.position.x,
                _ballsMoster.transform.position.y + 1f, _ballsMoster.transform.position.z), 4f, 1, 0.5f);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Ball"))
            {
                CollectableBall _ball = other.GetComponent<CollectableBall>();

                if (!_ball.IsTrueCharacter(_characterColorType))
                    return;

                _stackBalls.AddCollectableBall(_ball);
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag("MonsterFields"))
            {
                _ballsMoster = other.GetComponentInParent<BallsMonster>();
                if (_ballsMoster.IsTrueCharacter(_characterColorType))
                {
                    _stackBalls.UnstackCollectablekBalls(_ballsMoster);
                }

                if (_ballsMoster.IsFullMonster())
                {
                    _characterStates.CharacterTransformation(_ballsMoster);
                    _stackBalls.ClearStack();
                }
            }
        }
    }
}
