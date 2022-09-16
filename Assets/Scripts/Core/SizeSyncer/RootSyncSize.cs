using Extensions;
using UnityEngine;

namespace Core.SizeSyncer{
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

            foreach (var syncer in GetComponentsInChildren<SpriteSizeSyncer>()){
                syncer.SyncSize();
            }

            foreach (var syncer in GetComponentsInChildren<BaseColliderSizeSyncer>()){
                syncer.SyncSize();
            }
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
