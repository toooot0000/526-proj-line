using System;
using UnityEngine;

namespace UI.Container{
    public class UIContainerFlexBox: UIContainerBase{
        public Vector2 padding = new(0, 0);
        public float gap = 10;
        public Direction direction = Direction.Horizontal;

        private void Start(){
            UpdateLayout();
        }

        public override void UpdateLayout(){
            var t = (RectTransform)transform;
            var rect = t.rect;
            var cur = padding;
            var childCount = t.childCount;
            var size = new Vector2(){
                x = direction == Direction.Horizontal
                    ? (rect.width - Math.Max(childCount - 1, 0) * gap - padding.x * 2) / childCount
                    : rect.width - padding.x * 2,
                y = direction == Direction.Vertical
                    ? (rect.height - Math.Max(childCount - 1, 0) * gap - padding.y * 2) / childCount
                    : rect.height - padding.y * 2,
            };
            for (int i = 0; i < transform.childCount; i++){
                var childTransform = transform.GetChild(i) as RectTransform;
                if (childTransform != null){
                    childTransform.anchoredPosition = Vector2.zero;
                    childTransform.sizeDelta = Vector2.one;
                    childTransform.anchorMin = new(cur.x/rect.width, cur.y/rect.height);
                    childTransform.anchorMax = new((cur.x + size.x) / rect.width, (cur.y + size.y) / rect.height);
                    cur.x += direction == Direction.Horizontal ? size.x + gap : 0;
                    cur.y += direction == Direction.Vertical ? size.y + gap : 0;
                }
            }
        }
    }

    public enum Direction{
        Vertical,
        Horizontal
    }
}