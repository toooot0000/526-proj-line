using Model.Mechanics.PlayableObjects;
using UnityEngine;

namespace Core.PlayArea.DirectionChangers{
    public class DirectionChangerManager: PlayableViewManager<DirectionChanger>{
        public GameObject directionChanger;
        protected override PlayableObjectViewWithModel<DirectionChanger> GenerateNewObject(){
            return Instantiate(directionChanger, transform)
                .GetComponent<DirectionChangerView>();
        }
    }
}