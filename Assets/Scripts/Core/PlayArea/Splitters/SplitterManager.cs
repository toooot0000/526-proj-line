using Model.Mechanics.PlayableObjects;
using UnityEngine;

namespace Core.PlayArea.Splitters{
    public class SplitterManager: PlayableViewManager<Splitter>{
        public GameObject splitterPrefab;
        protected override PlayableObjectViewWithModel<Splitter> GenerateNewObject(){
            return Instantiate(splitterPrefab, transform).GetComponent<SplitterView>();
        }
    }
}