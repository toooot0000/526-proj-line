using System;
using System.Collections.Generic;
using Model.Mechanics.PlayableObjects;
using UnityEngine;
using Utility.Extensions;

namespace Core.PlayArea.BlackHoles{
    public class BlackHoleManager: PlayableViewManager<BlackHole>, IPlayableViewManager{

        public GameObject minePrefab;
        public PlayAreaManager playAreaManager;

        private readonly List<BlackHoleView> _blackHoleViews = new();

        public BlackHoleView PlaceBlackHole(BlackHole blackHole){
            var newMine = _blackHoleViews.FirstNotActiveOrNew(GenerateMineView);
            newMine.Model = blackHole;
            playAreaManager.SetPlayableViewPosition(newMine, blackHole);
            return newMine;
        }

        private BlackHoleView GenerateMineView(){
            var ret = Instantiate(minePrefab, transform).GetComponent<BlackHoleView>();
            if (ret == null) throw new Exception();
            return ret;
        }
        
        public override PlayableObjectViewWithModel<BlackHole> Place(BlackHole model) => PlaceBlackHole(model);

        public override void Remove(PlayableObjectViewWithModel<BlackHole> view) =>
            _blackHoleViews.Remove(view as BlackHoleView);

        protected override PlayableObjectViewWithModel<BlackHole> GenerateNewObject() => GenerateMineView();

        public override IEnumerable<PlayableObjectViewBase> GetAllViews() => _blackHoleViews;
    }
}