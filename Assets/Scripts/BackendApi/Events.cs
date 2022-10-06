using System;

namespace BackendApi{
    public abstract class LoggableEvent{
        public string uuid = GameManager.shared.uuid.ToString();
        public virtual string URLPath => throw new NotImplementedException();
    }

    public class EventClearanceRecord : LoggableEvent{
        public int level;
        public string status; // "success" || "fail"
        public int time;
        public override string URLPath => "logClearanceRecord";
    }

    public class EventSkillUses : LoggableEvent{
        public int skillId;
        public int uses;
        public override string URLPath => "logSkillUses";
    }

    public class EventItemInteract : LoggableEvent{
        public int count;
        public int itemId;
        public string status;
        public override string URLPath => "logItemsInteract";
    }
}