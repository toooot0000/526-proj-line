using UnityEngine;

namespace Model.Mechanics{

    public interface IExecutable{
        void Execute();
    }
    
    public interface IPlayableObject{
        RectInt InitGridPosition{ get; set; }
    }

    public interface IMovable{
        float Velocity{ get; set; }
        float VelocityMultiplier{ get; set; }
    }

    public interface ISliceable{
        IExecutable OnSliced();
    }

    public interface ICircleable{
        IExecutable OnCircled();
    }
}