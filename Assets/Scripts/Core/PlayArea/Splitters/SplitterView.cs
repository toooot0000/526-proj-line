using Model.Mechanics.PlayableObjects;

namespace Core.PlayArea.Splitters{
    public class SplitterView: PlayableObjectViewWithModel<Splitter>{
        public override Splitter Model{ get; set; }
    }
}