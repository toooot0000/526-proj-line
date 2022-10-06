using UnityEngine;

namespace Model{
    /// <summary>
    ///     对数据负责，不对流程负责！
    /// </summary>
    public class GameModel{
        public Game currentGame;
        public GameObject gameObject = null;
        public GameModel parent;

        protected GameModel(GameModel parent){
            if (parent == null) return;
            this.parent = parent;
            currentGame = parent.currentGame;
        }
    }
}