using System.Net;
using System.Net.Http;
using System.Text;
using Model;
using Newtonsoft.Json;
using UI;
using UI.Interfaces;
using UI.Interfaces.SelectGear;
using UI.Interfaces.ShopSystem;
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
            // stage enter
            GameManager.shared.game.OnStageLoaded += game =>
            {
                Debug.Log("OnStageLoaded!!");
                var stage = game.currentStage;
                var id = stage.id;
                this.Log(new EventPeopleEnterSuccesses()
                {
                    level = id,
                    status = "enter"
                });
            };

            //stage success 
            GameManager.shared.game.currentStage.OnStageBeaten += (game1, stage) =>
            {
                Debug.Log("OnStageBeaten!!");
                var id = ((Stage)stage).id;
                this.Log(new EventPeopleEnterSuccesses()
                {
                    level = id,
                    status = "success"
                });
            };
            
            //Gear show
            UIManager.shared.OnOpenUI += (ui) =>
            {
                if (ui is UISelectGear)
                {
                    Debug.Log("OnGearShow!!");
                    var newUI = (UISelectGear)ui;
                    for (var i = 0; i < newUI.items.Length; i++)
                    {
                        this.Log(new EventGearShows()
                        {
                            gearId = newUI.items[i].id
                        });
                    }
                }
                if (ui is UIShopSystem)
                {
                    Debug.Log("OnGearShow!!");
                    var newUI = (UIShopSystem)ui;
                    for (var i = 0; i < newUI._items.Count; i++)
                    {
                        this.Log(new EventGearShows()
                        {
                            gearId = newUI._items[i].id
                        });
                    }
                }
            };
            
            //gear obtain
            GameManager.shared.game.player.OnGearAdded += (game, gear) =>
            {
                Gear newGear = (Gear)gear;
                this.Log(new EventGearObtains()
                {
                    gearId = newGear.id
                });
            };
            //gear use
            GameManager.shared.OnPlayerAttack += () =>
            {
                foreach (var gear in GameManager.shared.game.player.gears)
                {
                    if (gear.IsComboIng())
                    {
                        this.Log(new EventGearUses()
                        {
                            gearId = gear.id,
                            status = "combo"
                        });
                    }
                    else if (gear.IsCharged())
                    {
                        this.Log(new EventGearUses()
                        {
                            gearId = gear.id,
                            status = "charge"
                        });
                    }
                    else
                    {
                        this.Log(new EventGearUses()
                        {
                            gearId = gear.id,
                            status = "plain use"
                        });
                    }
                }
            };
            //dmg of enemies
            GameManager.shared.game.player.OnTakeDamage += (game, damage) =>
            {
                if((damage).finalDamagePoint>0)
                    this.Log(new EventHpofEnemies()
                    {
                        enemyId = GameManager.shared.game.CurrentEnemy.id,
                        hp = (damage).finalDamagePoint
                    });
            };
            //hit of balls
            GameManager.shared.game.player.OnHitBall += (g, b) =>
            {
                this.Log(new EventHitofBalls()
                {
                    ballId = (b as Ball).id,
                    hitCount = 1
                });
            };

            GameManager.shared.game.player.OnCircledBall += (g, b) =>
            {
                this.Log(new EventHitofBalls()
                {
                    ballId = (b as Ball).id,
                    hitCount = 1
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