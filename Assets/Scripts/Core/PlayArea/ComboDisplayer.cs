using TMPro;
using UnityEngine;
using Utility;

namespace Core.PlayArea{
    public class ComboDisplayer: MonoBehaviour{

        public SpriteRenderer bg;
        public TextMeshProUGUI textMesh;
        public AnimationCurve curve;
        public float time = 0.5f;

        private Color _startColor;
        private Color _endColor;

        private int ComboNum{
            set{
                if (value <= 1) return;
                textMesh.text = value.ToString();
                PlayAnimation();
            }
        }

        private void Start(){
            _startColor = bg.color;
            _endColor = new Color(_startColor.r, _startColor.g, _startColor.b, 0);
            bg.color = _endColor;
            textMesh.color = _endColor;
        }

        private void PlayAnimation(){
            StartCoroutine(TweenUtility.Lerp(
                seconds: time,
                begin: null,
                update: i => {
                    var color = Color.Lerp(_startColor, _endColor, curve.Evaluate(i));
                    bg.color = color;
                    textMesh.color = color;
                },
                complete: null
            )());
        }

        public void Show(int number, Vector3 worldPosition){
            transform.position = new Vector3(worldPosition.x, worldPosition.y, transform.position.z);
            ComboNum = number;
        }
    }
}