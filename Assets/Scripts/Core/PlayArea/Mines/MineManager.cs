using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Core.DisplayArea.Stage;
using Model.Mechanics.PlayableObjects;
using UnityEngine;
using Utility.Extensions;

namespace Core.PlayArea.Mines{
    public class MineManager: PlayableViewManager<Mine>{

        public GameObject minePrefab;
        public StageManager stageManager;


        public override PlayableObjectViewWithModel<Mine> Place(Mine model){
            var ret = base.Place(model) as MineView;
            ret!.Init();
            ret!.stageManager = stageManager;
            return ret;
        }

        private MineView GenerateMineView(){
            var ret = Instantiate(minePrefab, transform).GetComponent<MineView>();
            if (ret == null) throw new Exception();
            return ret;
        }

        protected override PlayableObjectViewWithModel<Mine> GenerateNewObject(){
            return GenerateMineView();
        }
    }
}