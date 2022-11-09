using System;
using System.Collections.Generic;
using Core.DisplayArea.Stage;
using UnityEngine;
using Utility.Extensions;

namespace Core.PlayArea.Mines{
    public class MineManager: MonoBehaviour, IPlayableViewManager{

        public GameObject minePrefab;
        public PlayAreaManager playAreaManager;
        public StageManager stageManager;

        private readonly List<MineView> _mineViews = new();

        public MineView PlaceMine(Model.Mechanics.PlayableObjects.Mine mine){
            var newMine = _mineViews.FirstNotActiveOrNew(GenerateMineView);
            newMine.Init();
            newMine.Model = mine;
            newMine.stageManager = stageManager;
            var rect = playAreaManager.GridRectToRect(mine.InitGridPosition);
            var rectTrans = newMine.transform;
            ((RectTransform)rectTrans).anchoredPosition = rect.position;
            ((RectTransform)rectTrans).sizeDelta = rect.size;
            return newMine;
        }

        public bool RemoveMine(MineView mine){
            mine.gameObject.SetActive(false);
            return _mineViews.Remove(mine);
        }

        private MineView GenerateMineView(){
            var ret = Instantiate(minePrefab, transform).GetComponent<MineView>();
            if (ret == null) throw new Exception();
            return ret;
        }

        public IEnumerable<PlayableObjectViewBase> GetAllViews() {
            return _mineViews;
        }
    }
}