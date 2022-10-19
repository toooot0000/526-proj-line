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
        public static string serverURL = "https://test526.wn.r.appspot.com/";
        private const bool IsActive = false;

        private static EventLogger _shared;

        public static EventLogger Shared => _shared ??= new EventLogger();

        public void init()
        {
            Debug.Log("Init EventLogger");
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

            GameManager.shared.game.currentStage.OnStageBeaten += (game1, stage) =>
            {
                var id = ((Stage)stage).id;
                this.Log(new EventPeopleEnterSuccesses()
                {
                    level = id,
                    status = "success"
                });
            };
        }

        private void LogStageBeaten(Game g, GameModel m){
            
        }

        private EventLogger(){ }

        public async void Log<T>(T loggableEvent) where T : LoggableEvent{
            if (!IsActive) return;
            var stringContent = new StringContent(JsonConvert.SerializeObject(loggableEvent), Encoding.UTF8,
                "application/json");
            var response = await Client.PostAsync($"{serverURL}{loggableEvent.URLPath}", stringContent);
            if (response.StatusCode == HttpStatusCode.OK) Debug.Log("Logging event succeed!");
            var responseStr = response.Content.ReadAsStringAsync();
            Debug.Log(responseStr);
        }
    }
}