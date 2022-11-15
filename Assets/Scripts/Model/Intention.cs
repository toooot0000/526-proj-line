using UnityEngine;
using Utility.Loader;

namespace Model
{
    public class Intention
    {
        public string name;
        public string desc;
        public string imgPth;
        
        public Intention(int id)
        {
            var intention = CsvLoader.TryToLoad("Configs/intentions", id);
            if (intention == null){
                Debug.LogError("Intention not found");
                return;
            }
            name = intention["name"] as string;
            desc = intention["desc"] as string;
            imgPth = intention["img_path"] as string;
        }
    }
}