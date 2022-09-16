using System;
using System.Net;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using UnityEngine;

namespace BackendApi
{

    public interface ILoggableEvent {
        public string URLPath{ get; }
    }
    public struct ClearanceRecord: ILoggableEvent{
        public int level;
        public string status;
        public int time;
        public string URLPath => "logClearanceRecord";
    }
    public struct SkillUses: ILoggableEvent{
        public int skillId;
        public int uses;
        public string URLPath => "logSkillUses";
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
    public class EventLogger : MonoBehaviour
    {
        private readonly string _server;
        private static readonly HttpClient Client = new HttpClient();
        public static string serverURL = "http://localhost:8080";
        
        public EventLogger(){
            _server = EventLogger.serverURL;
        }
        
        public async void Log(ILoggableEvent logLoggableEvent){
            var stringContent = new StringContent(JsonConvert.SerializeObject((object)logLoggableEvent), Encoding.UTF8,
                "application/json");
            var response = await Client.PostAsync($"{_server}{logLoggableEvent.URLPath}", stringContent);
            if (response.StatusCode == HttpStatusCode.OK){
                Debug.Log("Logging event succeed!");
            }
            var responseStr = response.Content.ReadAsStringAsync();
            Debug.Log(responseStr);
        }
    }
}