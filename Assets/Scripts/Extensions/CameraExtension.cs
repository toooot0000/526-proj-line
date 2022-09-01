using UnityEngine;

namespace Extensions{
    public static class CameraExtension{
        public static Vector2 ScreenSizeInWorldUnit(this Camera cmr){
            var screenSize = new Vector2(Screen.width, Screen.height);
            var cmrPosition = (Vector2)cmr.transform.position;
            var cmrNearClip = cmr.nearClipPlane;
            var bottomLeft = cmrPosition - screenSize / 2;
            var topRight = cmrPosition + screenSize / 2;
            var worldBottomLeft = cmr.ScreenToWorldPoint(new Vector3(bottomLeft.x, bottomLeft.y, cmrNearClip));
            var worldTopRight = cmr.ScreenToWorldPoint(new Vector3(topRight.x, topRight.y, cmrNearClip));
            return worldTopRight - worldBottomLeft;
        }
    }
}