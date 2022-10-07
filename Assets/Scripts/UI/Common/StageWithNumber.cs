using TMPro;
using UnityEngine;

namespace UI.Common
{
    public class StageWithNumber : MonoBehaviour
    {
        public TextMeshProUGUI mesh;

        public int Number{
            set => mesh.text = $" {value.ToString()}";
        }
    }

}
