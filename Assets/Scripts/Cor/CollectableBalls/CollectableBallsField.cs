using System.Collections.Generic;
using UnityEngine;

namespace PlayKing.Cor
{
    public class CollectableBallsField : MonoBehaviour
    {
        [System.Serializable]
        public class SpawnedBall
        {
            public CollectableBall collectableBall;
            public Vector3 spawnPosition;
            public bool isBallRemoved;
        }

        [Header("BallPrefabs")]
        [SerializeField] List<GameObject> ballPrefabs = new List<GameObject>();

        [Header("FieldPlacement")]
        [SerializeField] private int length;
        [SerializeField] private int line;
        [SerializeField] private int xOrder;
        [SerializeField] private float xPosition;
        [SerializeField] private float zPosition;

        [Header("TimerResetBall")]
        [SerializeField] private float timeToResetBall;
        [SerializeField] private float timer;

        private Vector3 startPoint;
        private Vector3 position;

        public List<SpawnedBall> spawnedBalls = new List<SpawnedBall>();

        void Start()
        {
            startPoint = transform.position;
            zPosition = transform.position.z;
            xPosition = transform.position.x;

            BallsPlacement();
        }

        private void Update()
        {
            timer += Time.deltaTime;

            if(timer >= timeToResetBall)
            {
                foreach (var i in spawnedBalls)
                {
                    if (i.isBallRemoved)
                    {
                        GenerateRemovedBall(i);
                        break;
                    }
                }
                timer = 0f;
            }
        }

        public void RemoveBall(CollectableBall collectableBall)
        {
            foreach (var i in spawnedBalls)
            {
                if (collectableBall == i.collectableBall)
                {
                    i.collectableBall = null;
                    i.isBallRemoved = true;
                    break;
                }
            }
        }

        private void BallsPlacement()
        {
            for (int i = 0; i < length; i++)
            {
                xOrder += 1;

                if (i % line == 0)
                {
                    zPosition -= 1f;
                    xOrder = 0;
                    position = new Vector3(xPosition, startPoint.y, zPosition);
                }
                else
                {
                    position = new Vector3(xPosition + xOrder, startPoint.y, zPosition);
                }

                GameObject newCollectableBall = Instantiate(ballPrefabs[Random.Range(0, ballPrefabs.Count)], 
                    position, ballPrefabs[Random.Range(0, ballPrefabs.Count)].transform.rotation);

                SpawnedBall spawnedBall = new SpawnedBall();
                spawnedBall.collectableBall = newCollectableBall.GetComponent<CollectableBall>();
                spawnedBall.isBallRemoved = false;
                spawnedBall.spawnPosition = position;
                spawnedBalls.Add(spawnedBall);
            }
        }

        private void GenerateRemovedBall(SpawnedBall spawnedBall)
        {
            GameObject createdBall = Instantiate(ballPrefabs[Random.Range(0, ballPrefabs.Count)], spawnedBall.spawnPosition,
                Quaternion.identity);

            createdBall.transform.position = spawnedBall.spawnPosition;
            spawnedBall.collectableBall = createdBall.GetComponent<CollectableBall>();
            spawnedBall.isBallRemoved = false;
        }
    }
}
