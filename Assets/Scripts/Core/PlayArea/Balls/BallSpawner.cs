using UnityEngine;

namespace Core.PlayArea.Balls{
    public class BallSpawner: MonoBehaviour{
        public GameObject ballPrefab;
        public void Spawn(Model.Ball ball){
            var newBallObject = Instantiate(ballPrefab, transform, false);
            var newBallConfig = newBallObject.GetComponent<BallConfig>();
            newBallConfig.modelBall = ball;
            newBallObject.transform.localPosition = GenerateRandomLocalPosition();
        }
        private Vector3 GenerateRandomLocalPosition(){
            return Vector3.zero; // TODO
        }
    }
}