using System;
using System.Collections.Generic;
using Core.DisplayArea.Stage;
using UnityEngine;
using Utility.Extensions;

namespace Core.PlayArea.Mine{
    public class MineManager: MonoBehaviour{

        public GameObject minePrefab;
        public PlayAreaManager playAreaManager;
        public StageManager stageManager;
        
        private List<MineView> _mineViews;

        public MineView PlaceMine(Model.Obstacles.Mine mine){
            var newMine = _mineViews.FirstNotActiveOrNew(GenerateMineView);
            newMine.model = mine;
            newMine.stageManager = stageManager;
            var rect = playAreaManager.GridRectToRect(mine.RectInt);
            newMine.transform.position = rect.position;
            return newMine;
        }

        public bool RemoveMine(MineView mine){
            
            return false;
        }

        private MineView GenerateMineView(){
            var ret = Instantiate(minePrefab, transform).GetComponent<MineView>();
            if (ret == null) throw new Exception();
            return ret;
        }

    }
}