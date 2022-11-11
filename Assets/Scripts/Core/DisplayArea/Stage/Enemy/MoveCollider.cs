using System;
using Model;
using UI;
using UnityEngine;

namespace Core.DisplayArea.Stage.Enemy
{
    public class MoveCollider:MonoBehaviour
    {

        public IntentionDisplayer.IntentionInfo info;

        public void SetInfo(IntentionDisplayer.IntentionInfo _info)
        {
            info = _info;
        }

        private void OnMouseDown()
        {
            // info.intention = EnemyIntention.Defend;
            // info.number = 688;
            Debug.Log("OnMouseDown");
            UIManager.shared.OpenUI("UIMove",info);
        }
    }
}