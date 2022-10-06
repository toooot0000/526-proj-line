using UnityEngine;

namespace Utility.Loader{
    public class JSONLoader{
        public static T Load<T>(string path){
            var textAsset = Resources.Load<TextAsset>(path);
            return JsonUtility.FromJson<T>(textAsset.text);
        }
    }
}