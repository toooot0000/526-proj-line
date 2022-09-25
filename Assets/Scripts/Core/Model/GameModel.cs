using UnityEngine;

namespace Core.Model{
    public class GameModel{
        public Game currentGame;
        public GameModel parent;
        public GameObject gameObject = null;
        protected GameModel(GameModel parent){
            if (parent == null) return;
            this.parent = parent;
            currentGame = parent.currentGame;
        }
    }
}