using TMPro;
using UnityEngine;

namespace Core.DisplayArea.Stage.Player{
    public class ArmorDisplayer: MonoBehaviour{
        public TextMeshProUGUI mesh;

        public int Number{
            set => mesh.text = $"{value.ToString()}";
        }
    }
}