using TMPro;
using UnityEngine;

namespace UI.Common{
    public class CoinWithNumber: MonoBehaviour{

        public TextMeshProUGUI mesh;
        
        public int Number{
            set => mesh.text = $"x {value.ToString()}";
        }
    }
}