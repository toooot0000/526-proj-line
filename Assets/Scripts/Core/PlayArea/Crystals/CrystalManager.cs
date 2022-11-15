using System;
using Model.Mechanics.PlayableObjects;
using UnityEngine;

namespace Core.PlayArea.Crystals{
    public class CrystalManager: PlayableViewManager<Crystal>{
        public GameObject crystalPrefab;
        protected override PlayableObjectViewWithModel<Crystal> GenerateNewObject(){
            var ret = Instantiate(crystalPrefab, transform).GetComponent<PlayableObjectViewWithModel<Crystal>>();
            if (ret == null)  throw new Exception();
            return ret;
        }
    }
}