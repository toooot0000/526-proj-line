using TMPro;
using UnityEngine;

namespace Core.DisplayArea{
    public class RemainingEnemy : MonoBehaviour{
        public TextMeshProUGUI textMesh;

        public int Number{
            set => textMesh.text = $"Remaining: {value.ToString()}";
        }
    }
}