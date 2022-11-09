using Model.Mechanics;
using UnityEngine;

namespace Core.PlayArea.BlackHole{
    public class BlackHoleView: PlayableObjectViewBase, ICircleableView {
        private Model.Mechanics.PlayableObjects.BlackHole _model;
        public Model.Mechanics.PlayableObjects.BlackHole Model {
            set {
                _model = value;
                
            }
            get => _model;
        }
        public void OnCircled() {
            Model.OnCircled().Execute();
        }
    }
}