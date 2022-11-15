using System;
using System.Linq;
using Model;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.UI;

namespace Core.DisplayArea.Stage.Enemy{
    
    
    public class IntentionDisplayer : MonoBehaviour{
        public Image icon;
        public TextMeshProUGUI number;

        public IntentionPair[] pairs;
        
        public EnemyIntention infoIntention;
        public IntentionInfo _info;
        

        public void UpdateIntention(IntentionInfo info)
        {
            _info = info;
            Debug.Log("UpdateIntention");

            icon.sprite = pairs.First(p => p.intention == info.intention).sprite;
            if (info.number > 0){
                number.enabled = true;
                number.text = info.number.ToString();
            } else{
                number.enabled = false;
            }
            // Debug.Log("show the next move");
            //UIManager.shared.OpenUI("UIMove",info);
        }

        [Serializable]
        public struct IntentionPair{
            public EnemyIntention intention;
            public Sprite sprite;
        }

        public struct IntentionInfo{
            public EnemyIntention intention;
            public int number;
        }

        public void OnClickIcon()
        {
            UIManager.shared.OpenUI("UIMove",new Intention((int)_info.intention));
        }
    }
}