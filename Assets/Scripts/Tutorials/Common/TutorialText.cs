using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Tutorials.Common{
    public class TutorialText: MonoBehaviour{
        public TextMeshProUGUI textMesh;
        public SpriteRenderer bg;

        public string Text{
            set => textMesh.text = value;
            get => textMesh.text;
        }

        public bool Enable{
            set{
                enabled = value;
                textMesh.enabled = value;
                bg.enabled = value;
            }
            get => enabled;
        }
    }
}