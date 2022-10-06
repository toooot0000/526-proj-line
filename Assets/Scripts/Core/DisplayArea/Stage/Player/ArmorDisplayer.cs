using TMPro;
using UnityEngine;

namespace Core.DisplayArea.Stage.Player{
    public class ArmorDisplayer : MonoBehaviour{
        public TextMeshProUGUI mesh;

        public int Number{
            set{
                if (value <= 0){
                    gameObject.SetActive(false);
                } else{
                    gameObject.SetActive(true);
                    mesh.text = $"{value.ToString()}";
                }
            }
        }
    }
}