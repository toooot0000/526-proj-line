using System;
using Utility.Loader;

namespace Model{
    [Serializable]
    public class Artifact : GameModel
    {
        public int id;
        public String desc;
        public String name;
        public int rarity;

        public Artifact(GameModel parent) : base(parent) { }

        public Artifact(GameModel parent, int id) : base(parent)
        {
            var artifact = CsvLoader.TryToLoad("Configs/artifacts", id);
            if (artifact == null) return;
            this.id = id;
            name = artifact["name"] as string;
            desc = artifact["desc"] as string;
            rarity = (int)artifact["rarity"];
        }

    }
}