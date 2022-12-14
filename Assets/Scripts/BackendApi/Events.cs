using System;

namespace BackendApi{
    public abstract class LoggableEvent{
        public string userId = GameManager.shared.uuid.ToString();
        public virtual string URLPath => throw new NotImplementedException();
    }

    public class EventPeopleEnterSuccesses : LoggableEvent 
    {
        public int level;
        public string status; //"enter" or "success"
        public override string URLPath => "logPeopleEnterSuccesses";
    }
    
    public class EventGearShows : LoggableEvent
    {
        public int gearId;
        public override string URLPath => "logGearShows";
    }
    
    public class EventGearObtains : LoggableEvent
    {
        public int gearId;
        public override string URLPath => "logGearObtains";
    }
    
    public class EventGearUses : LoggableEvent
    {
        public int gearId;
        public string status;// "plain use" or "charge" or "combo"
        public override string URLPath => "logGearUses";
    }

    public class EventHpofEnemies : LoggableEvent
    {
        public int enemyId;
        public int hp;
        public override string URLPath => "logHpofEnemies";
    }

    public class EventHitofBalls : LoggableEvent
    {
        public int ballId;
        public int hitCount;
        public override string URLPath => "logHitofBalls";
    }
}