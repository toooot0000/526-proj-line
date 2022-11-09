using System;
using System.Collections.Generic;
using Model.Mechanics.PlayableObjects;
using UnityEngine;
using Utility.Extensions;

namespace Core.PlayArea.BlackHoles{
    public class BlackHoleManager: MonoBehaviour, IPlayableViewManager{

        public GameObject minePrefab;
        public PlayAreaManager playAreaManager;

        private readonly List<BlackHoleView> _blackHoleViews = new();

        public BlackHoleView PlaceBlackHole(BlackHole blackHole){
            var newMine = _blackHoleViews.FirstNotActiveOrNew(GenerateMineView);
            newMine.Model = blackHole;
            playAreaManager.SetPlayableViewPosition(newMine, blackHole);
            return newMine;
        }

        public bool RemoveMine(BlackHoleView blackHole){
            blackHole.gameObject.SetActive(false);
            return _blackHoleViews.Remove(blackHole);
        }

        private BlackHoleView GenerateMineView(){
            var ret = Instantiate(minePrefab, transform).GetComponent<BlackHoleView>();
            if (ret == null) throw new Exception();
            return ret;
        }


        public IEnumerable<PlayableObjectViewBase> GetAllViews() => _blackHoleViews;
    }
}