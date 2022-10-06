using TMPro;
using UnityEngine;

namespace Core.DisplayArea{
    public class RemainingEnemy : MonoBehaviour{
        private TextMeshProUGUI _textMesh;

        public int Number{
            set => _textMesh.text = $"Remaining: {value.ToString()}";
        }

        private void Start(){
            _textMesh = GetComponent<TextMeshProUGUI>();
        }
    }
}