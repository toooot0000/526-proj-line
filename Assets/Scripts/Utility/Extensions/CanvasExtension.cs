using UnityEngine;

namespace Utility.Extensions{
    public static class CanvasExtensions{
        /// <summary>
        /// Output an anchoredPosition works only for center-anchored components.
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="worldPosition"></param>
        /// <param name="camera"></param>
        /// <returns></returns>
        public static Vector2 WorldToCanvas(this Canvas canvas, Vector3 worldPosition, Camera camera = null){
            if (camera == null){
                camera = Camera.main;
            }

            var viewportPosition = camera.WorldToViewportPoint(worldPosition);
            var canvasRect = canvas.GetComponent<RectTransform>();

            var sizeDelta = canvasRect.sizeDelta;
            return new Vector2((viewportPosition.x * sizeDelta.x) - (sizeDelta.x * 0.5f),
                (viewportPosition.y * sizeDelta.y) - (sizeDelta.y * 0.5f));
        }
    }
}