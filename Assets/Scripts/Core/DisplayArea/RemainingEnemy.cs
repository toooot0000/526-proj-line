using System;
using TMPro;
using UnityEngine;

namespace Core.DisplayArea{
    public class RemainingEnemy: MonoBehaviour{
        private TextMeshProUGUI _textMesh;

        private void Start(){
            _textMesh = GetComponent<TextMeshProUGUI>();
        }

        public int Number{
            set => _textMesh.text = $"Remaining: {value.ToString()}";
        }
    }
}