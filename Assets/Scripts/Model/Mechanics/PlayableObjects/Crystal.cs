using UnityEngine;

namespace Model.Mechanics.PlayableObjects {

    public interface ICrystalEffect : IExecutable {
        public void Reset();
    }
    
    public class Crystal: GameModel, IPlayableObject, ISliceable {
        protected Crystal(GameModel parent) : base(parent) {
        }

        public RectInt InitGridPosition { get; set; }
        public IExecutable OnSliced() {
            throw new System.NotImplementedException();
        }
    }
}