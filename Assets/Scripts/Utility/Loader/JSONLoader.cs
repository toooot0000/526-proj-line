using UnityEngine;

namespace Utility.Loader
{
    public class JSONLoader
    {
        public static T Load<T>(string path)
        {
            TextAsset textAsset = Resources.Load<TextAsset>(path);
            return JsonUtility.FromJson<T>(textAsset.text);
        }
    }
}