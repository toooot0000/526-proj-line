using System;
using System.Linq;
using Model;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Core.DisplayArea.Stage.Enemy{
    public class IntentionDisplayer : MonoBehaviour{
        public Image icon;
        public TextMeshProUGUI number;

        public IntentionPair[] pairs;

        public void UpdateIntention(IntentionInfo info){
            icon.sprite = pairs.First(p => p.intention == info.intention).sprite;
            if (info.number > 0){
                number.enabled = true;
                number.text = info.number.ToString();
            } else{
                number.enabled = false;
            }
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
    }
}