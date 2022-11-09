using System;
using System.Collections.Generic;
using Model.Mechanics.PlayableObjects;
using UnityEngine;
using Utility.Extensions;

namespace Core.PlayArea.BlackHoles{
    public class BlackHoleManager: MonoBehaviour{

        public GameObject minePrefab;
        public PlayAreaManager playAreaManager;

        private readonly List<BlackHoleView> _mineViews = new();

        public BlackHoleView PlaceBlackHole(BlackHole blackHole){
            var newMine = _mineViews.FirstNotActiveOrNew(GenerateMineView);
            newMine.Model = blackHole;
            playAreaManager.SetPlayableObjectPosition(newMine, blackHole);
            return newMine;
        }

        public bool RemoveMine(BlackHoleView blackHole){
            blackHole.gameObject.SetActive(false);
            return _mineViews.Remove(blackHole);
        }

        private BlackHoleView GenerateMineView(){
            var ret = Instantiate(minePrefab, transform).GetComponent<BlackHoleView>();
            if (ret == null) throw new Exception();
            return ret;
        }

        // private void Update(){
        //     if (Input.GetKeyUp(KeyCode.S)){
        //         
        //     }
        // }
    }
}