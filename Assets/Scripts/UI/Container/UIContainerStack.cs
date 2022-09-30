using UnityEngine;

namespace UI.Container {
    public class UIContainerStack: UIContainerBase {

        public Direction direction;
        public float gap = 10;
        public Vector2 padding = Vector2.zero;
        public float dimension = -1;

        public override void UpdateLayout() {
            var tr = (transform as RectTransform)!;
            var count = tr.childCount;
            var size = tr.rect.size;
            var cur = padding;
            
            for (var i = 0; i < count; i++) {
                var childTransform = (transform.GetChild(i) as RectTransform)!;
                childTransform.anchorMin = new Vector2(0, 1);
                childTransform.anchorMax = new Vector2(0, 1);
                childTransform.pivot = new Vector2(0, 1);
                childTransform.anchoredPosition = cur;
                var oldSizeDelta = childTransform.sizeDelta;
                if (dimension > 0) {
                    if (direction == Direction.Horizontal) {
                        childTransform.sizeDelta = new Vector2() {
                            x = dimension/size.x,
                            y = oldSizeDelta.y
                        };
                        cur.x += dimension + gap;
                    }
                    else {
                        childTransform.sizeDelta = new Vector2() {
                            x = oldSizeDelta.x,
                            y = dimension/size.y
                        };
                        cur.y -= (dimension + gap);
                    }
                }
                else {
                    if (direction == Direction.Horizontal) {
                        cur.x += childTransform.rect.size.x + gap;
                    }
                    else {
                        cur.y -= childTransform.rect.size.y + gap;
                    }
                }
            }
        }
    }
}