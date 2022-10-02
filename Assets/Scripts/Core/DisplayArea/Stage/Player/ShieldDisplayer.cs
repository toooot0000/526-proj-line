using TMPro;
using UnityEngine;

namespace Core.DisplayArea.Stage.Player{
    public class ShieldDisplayer: MonoBehaviour{
        public TextMeshPro mesh;

        public int Number{
            set => mesh.text = $"{value.ToString()}";
        }
    }
}