using System;
using System.Data;
using UI;
using UnityEngine;
using Utility.Loader;

namespace Model{

    public class EventChoice{
        public string desc;
        public string[] effectToken;
        public int[] values;

        public EventChoice(string fromString){
            var result = EventChoice.ParseArgs(fromString);
            desc = result[0] as string;
            effectToken = result[1] as string[];
            values = result[2] as int[];
        }
        
        private static object[] ParseArgs(string arg){
            if (arg == "__empty") return null;

            var args = new object[3];
            var temp = arg.Split(';');
            var num = temp.Length / 2;
            var tempArr = new string[num];
            var tempArr2 = new int[num];
            for (var i = 1; i < temp.Length; i++)
                if (i % 2 == 1)
                    tempArr[i / 2] = temp[i];
                else
                    tempArr2[i / 2 - 1] = int.Parse(temp[i]);
            args[0] = temp[0];
            args[1] = tempArr;
            args[2] = tempArr2;
            return args;
        }
    }
    
    public class Event{
        public string[] answers ={ "ans1", "ans2", "ans3" };
        public readonly string[] argsArray = new string[3];
        public readonly string desc;
        public int id;
        public string name;
        public string type;
        public readonly EventChoice[] choices;


        public Event(int id){
            var event1 = CsvLoader.TryToLoad("Configs/events", id);
            if (event1 == null){
                Debug.LogError("Event not found");
                return;
            }

            this.id = id;
            name = event1["name"] as string;
            desc = event1["desc"] as string;
            type = event1["type"] as string;
            argsArray[0] = event1["arg1"] as string;
            argsArray[1] = event1["arg2"] as string;
            argsArray[2] = event1["arg3"] as string;
            choices = new[]{
                new EventChoice(argsArray[0]),
                new EventChoice(argsArray[1]),
                new EventChoice(argsArray[2])
            };
        }

        public string GetQuestion(){
            return desc;
        }

        public string GetAnswer(int index){
            return choices[index].desc;
        }


        public string ExplainArgsToAction(int index){
            if (index >= choices.Length){
                Debug.Log("no data in datatable");
                return null;
            }

            var effect = choices[index].effectToken;
            var effectValue = choices[index].values;
            var result = "";

            for (var i = 0; i < effect.Length; i++){
                switch (effect[i]){
                    case "GetLife":
                        GameManager.shared.game.player.CurrentHp += effectValue[i];
                        if (effectValue[i] > 0)
                            result += "You got " + effectValue[i] + " HP\n";
                        else
                            result += "You lost " + effectValue[i] + " HP\n";
                        break;
                    case "GetGear":
                        var player = GameManager.shared.game.player;
                        if (player.gears.Find(g => g.id == effectValue[i]) == null){
                            player.AddGear(new Gear(player, effectValue[i]));
                        }
                        break;
                    case "GetCoin":
                        GameManager.shared.game.player.Coin += effectValue[i];
                        if (effectValue[i] > 0)
                            result += "You got " + effectValue[i] + " coins\n";
                        else
                            result += "You lost " + effectValue[i] + " coins\n";
                        Debug.Log("GetCoin" + effectValue);
                        break;
                    case "GetArtifact":
                        Debug.Log("GetArtifact" + effectValue[i]);
                        break;
                    case "Nothing":
                        result = "Nothing happened";
                        Debug.Log("Nothing");
                        break;
                    default:
                        Debug.Log("No such effect");
                        break;
                }
            }
            return result;
        }
    }
}