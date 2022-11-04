using System;
using System.Collections.Generic;
using Core.DisplayArea.Stage;
using Core.PlayArea.TouchTracking;
using UnityEngine;
using Utility.Extensions;

namespace Core.PlayArea.Mine{
    public class MineManager: MonoBehaviour{

        public GameObject minePrefab;
        public PlayAreaManager playAreaManager;
        public StageManager stageManager;
        public TouchTracker tracker;

        private readonly List<MineView> _mineViews = new();

        public MineView PlaceMine(Model.Obstacles.Mine mine){
            var newMine = _mineViews.FirstNotActiveOrNew(GenerateMineView);
            newMine.Init();
            newMine.model = mine;
            newMine.stageManager = stageManager;
            var rect = playAreaManager.GridRectToRect(mine.RectInt);
            var rectTrans = newMine.transform;
            ((RectTransform)rectTrans).anchoredPosition = rect.position;
            ((RectTransform)rectTrans).sizeDelta = rect.size;
            newMine.tracker = tracker;
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

    }
}