using System;
using UnityEngine;

namespace UI.Container{
    public class UIContainerStack : UIContainerBase{
        public Direction direction;
        public float gap = 10;
        public Vector2 padding = Vector2.zero;
        public float dimension = -1;

        public override void UpdateLayout(){
            var tr = (transform as RectTransform)!;
            var count = tr.childCount;
            var size = tr.rect.size;
            var cur = padding;
            var maxFixed = -1f;
            var totalGrowth = 0f;

            for (var i = 0; i < count; i++){
                var childTransform = (transform.GetChild(i) as RectTransform)!;
                childTransform.anchorMin = new Vector2(0, 1);
                childTransform.anchorMax = new Vector2(0, 1);
                childTransform.pivot = new Vector2(0, 1);
                childTransform.anchoredPosition = cur;
                var childSize = childTransform.rect.size;
                var oldSizeDelta = childTransform.sizeDelta;
                if (dimension > 0){
                    if (direction == Direction.Horizontal){
                        childTransform.sizeDelta = new Vector2{
                            x = dimension / size.x,
                            y = oldSizeDelta.y
                        };
                        cur.x += dimension + gap;
                    } else{
                        childTransform.sizeDelta = new Vector2{
                            x = oldSizeDelta.x,
                            y = dimension / size.y
                        };
                        cur.y -= dimension + gap;
                    }
                } else{
                    if (direction == Direction.Horizontal)
                        cur.x += childSize.x + gap;
                    else
                        cur.y -= childSize.y + gap;
                }

                // handle parent container size;
                maxFixed = direction switch{
                    Direction.Vertical => Mathf.Max(maxFixed, childSize.x),
                    Direction.Horizontal => Mathf.Max(maxFixed, childSize.y)
                };
                totalGrowth += direction switch{
                    Direction.Vertical => childSize.y,
                    Direction.Horizontal => childSize.x
                } + gap;
            }

            totalGrowth += (direction == Direction.Horizontal ? padding.x : padding.y) * 2 - gap;
            maxFixed += (direction == Direction.Horizontal ? padding.y : padding.x) * 2;
            
            ((RectTransform)transform).sizeDelta = new Vector2(){
                x = (direction == Direction.Horizontal ? totalGrowth : maxFixed),
                y = (direction == Direction.Horizontal ? maxFixed : totalGrowth),
            };
        }
    }
}