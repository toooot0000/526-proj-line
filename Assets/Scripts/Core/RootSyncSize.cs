using System;
using Extensions;
using UnityEditor;
using UnityEngine;

namespace Core{
    public class RootSyncSize : MonoBehaviour
    {
        // Start is called before the first frame update
        [SerializeField]
        public Camera currentCmr;
        
        void Start(){
            var size = currentCmr.ScreenSizeInWorldUnit();
            var rectTrans = GetComponent<RectTransform>();
            rectTrans.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, size.x);
            rectTrans.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, size.y);
            rectTrans.SetPositionAndRotation(Vector3.zero, Quaternion.Euler(0, 0, 0));

            foreach (var syncer in GetComponentsInChildren<SpriteSyncSizeWithRectTransform>()){
                syncer.SyncSize();
            }

            foreach (var syncer in GetComponentsInChildren<BoxCollider2DSyncSizeWithRectTransform>()){
                syncer.SyncSize();
            }
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
