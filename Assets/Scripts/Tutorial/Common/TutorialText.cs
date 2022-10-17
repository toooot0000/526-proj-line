using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Tutorial.Common{
    public class TutorialText: MonoBehaviour{
        public TextMeshProUGUI textMesh;
        public SpriteRenderer bg;
        public Image imgBg;

        private void Awake(){
            Enabled = false;
        }

        public string Text{
            set => textMesh.text = value;
            get => textMesh.text;
        }

        public bool Enabled{
            set{
                enabled = value;
                textMesh.enabled = value;
                if (bg != null){
                    bg.enabled = value;
                } else{
                    imgBg.enabled = value;
                }
            }
            get => enabled;
        }
    }
}