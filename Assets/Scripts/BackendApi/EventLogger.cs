using System;
using System.Net;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.InputSystem.Controls;

namespace BackendApi{
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
    
    /**
     * 
//     using sample:
//     EventLogger elogger = new EventLogger("http://localhost:8080");
//     ClearanceRecord c = new ClearanceRecord(1,"success",180);
//     SkillUses s = new SkillUses(1, 80);
//     elogger.logClearanceRecord(c);
//     elogger.logSkillUses(s);
     */
    public class EventLogger
    {
        private static readonly HttpClient Client = new();
        public static string serverURL = "http://localhost:8080/";

        private static EventLogger _shared;
        public static EventLogger Shared => _shared ?? new EventLogger();
        
        public async void Log<T>(T loggableEvent)where T: LoggableEvent{
            var stringContent = new StringContent(JsonConvert.SerializeObject(loggableEvent), Encoding.UTF8,
                "application/json");
            var response = await Client.PostAsync($"{serverURL}{loggableEvent.URLPath}", stringContent);
            if (response.StatusCode == HttpStatusCode.OK){
                Debug.Log("Logging event succeed!");
            }
            var responseStr = response.Content.ReadAsStringAsync();
            Debug.Log(responseStr);
        }
    }
}