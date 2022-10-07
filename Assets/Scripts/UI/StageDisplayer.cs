using System.Collections;
using System.Collections.Generic;
using Model;
using UI.Common;
using UnityEngine;

namespace UI
{
    public class StageDisplayer : MonoBehaviour
    {
        public StageWithNumber stageid;
        private void Start(){
            GameManager.shared.game.player.OnStageChanged += (game, model) => stageid.Number = ((Player)model).StageId;
            stageid.Number = GameManager.shared.game.currentStage.id;
        }
    }

}
