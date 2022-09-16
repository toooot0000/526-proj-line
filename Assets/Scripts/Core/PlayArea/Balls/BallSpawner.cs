using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Core.PlayArea.Balls{
    public class BallSpawner: MonoBehaviour{
        public GameObject ballPrefab;
        public int randomSpawnNumber = 5;
        private readonly List<Model.Ball> _ballModels = new();


        private void Start()
        {
            GameManager.shared.game.player.OnHitBall += (game, model) =>
            {
                _ballModels.Remove((Model.Ball)model);
                if (_ballModels.Count == 0)
                {
                    SpawnRandom();
                }
            };
            GameManager.shared.game.player.OnCircledBall += (game, model) =>
            {
                _ballModels.Remove((Model.Ball)model);
                if (_ballModels.Count == 0)
                {
                    SpawnRandom();
                }
            };
            SpawnRandom();
        }

        public void Spawn(Model.Ball ball){
            var newBallObject = Instantiate(ballPrefab, transform, false);
            var newBallConfig = newBallObject.GetComponent<BallConfig>();
            newBallConfig.modelBall = ball;
            newBallObject.transform.localPosition = GenerateRandomLocalPosition();
            _ballModels.Add(ball);
        }
        
        public void SpawnRandom()
        {
            for (int i = 0; i < randomSpawnNumber; i++)
            {
                var model = new Model.Ball(GameManager.shared.game)
                {
                    point = Random.Range(1, 10),
                    size = Random.Range(1, 3) / 2.0f,
                    speed = Random.Range(1, 10) / 2.0f
                };
                Spawn(model);
            }
        }

        private Vector3 GenerateRandomLocalPosition(){
            return Vector3.zero; // TODO
        }
    }
}