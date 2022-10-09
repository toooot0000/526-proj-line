using System.Net;
using System.Net.Http;
using System.Text;
using Model;
using Newtonsoft.Json;
using UnityEngine;

namespace BackendApi{
    /**
     * * 
     * //     using sample:
     * //     EventLogger elogger = new EventLogger("http://localhost:8080");
     * //     ClearanceRecord c = new ClearanceRecord(1,"success",180);
     * //     SkillUses s = new SkillUses(1, 80);
     * //     elogger.logClearanceRecord(c);
     * //     elogger.logSkillUses(s);
     */
    public class EventLogger{
        private static readonly HttpClient Client = new();
        public static string serverURL = "http://localhost:8080/";

        private static EventLogger _shared;

        public static EventLogger Shared
        {
            get
            {
                if (_shared == null) _shared = new EventLogger();
                return _shared;
            }
        }

        public void init()
        {
            
        }

        private EventLogger()
        {
            Debug.Log("construct EventLogger");
            GameManager.shared.game.OnStageLoaded += game =>
            {
                var stage = game.currentStage;
                var id = stage.id;
                this.Log(new EventPeopleEnterSuccesses()
                {
                    level = id,
                    status = "enter"
                });
            };
            GameManager.shared.game.OnStageBeaten += (game1, stage) =>
            {
                var id = ((Stage)stage).id;
                this.Log(new EventPeopleEnterSuccesses()
                {
                    level = id,
                    status = "success"
                });
            };
        }

        public async void Log<T>(T loggableEvent) where T : LoggableEvent{
            var stringContent = new StringContent(JsonConvert.SerializeObject(loggableEvent), Encoding.UTF8,
                "application/json");
            var response = await Client.PostAsync($"{serverURL}{loggableEvent.URLPath}", stringContent);
            if (response.StatusCode == HttpStatusCode.OK) Debug.Log("Logging event succeed!");
            var responseStr = response.Content.ReadAsStringAsync();
            Debug.Log(responseStr);
        }
    }
}