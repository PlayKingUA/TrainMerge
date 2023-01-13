using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace PlayKing.Cor
{
    public class StackBalls : MonoBehaviour
    {
        [SerializeField] List<CollectableBall> currencyBalls = new List<CollectableBall>();
        [SerializeField] List<Transform> currencyStackPoints = new List<Transform>(); //rework this solution

        public void AddCollectableBall(CollectableBall _ball)
        {
            foreach (var i in currencyBalls)
            {
                if (_ball == i)
                    return;
            }

            currencyBalls.Add(_ball);
            int indexBall = currencyBalls.IndexOf(_ball);
            _ball.transform.parent = currencyStackPoints[indexBall];
            _ball.transform.position = currencyStackPoints[indexBall].position;
            _ball.BallInStack();
        }

        public void UnstackCollectablekBalls(BallsMonster unstackMonster)
        {
            for (int i = currencyBalls.Count - 1; i >= 0; i--)
            {
                currencyBalls[i].transform.SetParent(null);
                currencyBalls[i].transform.DOMove(transform.position + Vector3.up * 4f, 0.25f);
                currencyBalls[i].transform.DOJump(unstackMonster.transform.position, 4, 1, 0.3f).SetDelay(0.25f);
                StartCoroutine(currencyBalls[i].IE_BallToMonster(unstackMonster));
                currencyBalls.Remove(currencyBalls[i]);
                return;
            }
        }

        public void ClearStack()
        {
            foreach(var i in currencyBalls)
            {
                Destroy(i.gameObject);
            }

            currencyBalls.Clear();
        }
    }
}
