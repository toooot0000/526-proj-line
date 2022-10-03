using System;

namespace BackendApi {
    public abstract class LoggableEvent {
        public virtual string URLPath => throw new NotImplementedException();
        public string uuid = GameManager.shared.uuid.ToString();
    }

    public class EventClearanceRecord: LoggableEvent{
        public int level;
        public string status; // "success" || "fail"
        public int time;
        public override string URLPath => "logClearanceRecord";
    }
    public class EventSkillUses: LoggableEvent{
        public int skillId;
        public int uses;
        public override string URLPath => "logSkillUses";
    }

    public class EventItemInteract : LoggableEvent{
        public int itemId;
        public string status;
        public int count;
        public override string URLPath => "logItemsInteract";
    }
}