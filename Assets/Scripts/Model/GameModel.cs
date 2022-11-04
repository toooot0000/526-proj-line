using UnityEngine;

namespace Model{
    
    
    public delegate void ModelEvent(Game game, GameModel model);

    public delegate void ModelEvent<in T>(Game game, T target) where T : GameModel;

    public delegate void SimpleModelEvent(Game game);
    
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