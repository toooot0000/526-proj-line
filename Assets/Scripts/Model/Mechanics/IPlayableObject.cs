using UnityEngine;

namespace Model.Mechanics{
    public interface IPlayableObject{
        RectInt InitGridPosition{ get; set; }
    }
}