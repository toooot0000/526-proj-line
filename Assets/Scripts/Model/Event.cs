using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Runtime.CompilerServices;
using UnityEngine;
using Utility.Loader;

namespace Model
{
    public class Event
    {
        public int ID;
        public string Name;
        public string Desc;
        public string Type;
        public string[] answers = {"ans1", "ans2", "ans3"};
        public string[] argsArray = new string[3];
        public DataTable dt = new DataTable();


        public Event(int id)
        {
            var event1  = CsvLoader.TryToLoad("Configs/events", id);
            if (event1 == null)
            {
                Debug.LogError("Event not found");
                return;
            }
            this.ID = id;
            Name = event1["name"] as string;
            Desc = event1["desc"] as string;
            Type = event1["type"] as string;
            argsArray[0] = event1["arg1"] as string;
            argsArray[1] = event1["arg2"] as string;
            argsArray[2] = event1["arg3"] as string;
            dt.Columns.Add("desc", typeof(String));
            dt.Columns.Add("effect", typeof(String[]));
            dt.Columns.Add("value", typeof(Int32[]));
            dt.Rows.Add(parseArgs(argsArray[0]));
            dt.Rows.Add(parseArgs(argsArray[1]));
            dt.Rows.Add(parseArgs(argsArray[2]));

        }
        
        public string getQuestion()
        {
            return Desc;
        }
        
        public string getAnswer(int index)
        {
            return dt.Rows[index]["desc"].ToString();
        }
        
        
        public object[] parseArgs(string arg)
        {
            
            if (arg == "__empty")
            {
                return null;
            }

            object[] args = new object[3];
            var temp = arg.Split(';');
            int num = temp.Length / 2;
            string[] tempArr = new string[num];
            int[] tempArr2 = new int[num];
            for (int i = 1; i < temp.Length; i++)
            {
                if (i % 2 == 1)
                {
                    tempArr[i / 2] = temp[i];
                }
                else
                {
                    tempArr2[i / 2 - 1] = int.Parse(temp[i]);
                }
            }
            args[0] = temp[0];
            args[1] = tempArr;
            args[2] = tempArr2;
            return args;
        }

        
        public void explainArgs(int index)
        {
            if(dt.Rows[index] == null)
            {
                Debug.Log("no data in datatable");
                return;
            }
            
            String[] effect = dt.Rows[index]["effect"] as String[];
            int[] effectValue = dt.Rows[index]["value"] as int[];

            for (int i = 0; i < effect.Length; i++)
            {
                switch (effect[i])
                {
                    case "GetLife":
                        GameManager.shared.game.player.GetLife(effectValue[i]);
                        //Debug.Log("GetLife" + (effectValue).ToString());
                        break;
                    case "GetGear":
                        GameManager.shared.game.player.GetGear(effectValue[i]);
                        //Debug.Log("GetGear" + (effectValue).ToString());
                        break;
                    case "GetCoin":
                        GameManager.shared.game.player.GetCoin(effectValue[i]);
                        Debug.Log("GetCoin" + effectValue);
                        break;
                    case "GetArtifact":
                        //GameManager.shared.game.player.GetArtifact(effectValue);
                        Debug.Log("GetArtifact" + effectValue[i]);
                        break;
                    case "Nothing":
                        Debug.Log("Nothing");
                        break;
                    default:
                        Debug.Log("No such effect");
                        break;
                }
            }
        }
    }
    
    
}
