using UnityEngine;

namespace Core.Model{
    public class GameModel{
        public Game currentGame;
        public GameObject gameObject = null;
        protected GameModel(GameModel parent){
            if (parent == null) return;
            currentGame = parent.currentGame;
        }
    }
}