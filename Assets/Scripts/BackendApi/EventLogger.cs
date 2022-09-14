using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using UnityEngine;

namespace BackendApi
{
    public class ClearanceRecord
    {
        public int level;
        public string status;
        public int time;
        public ClearanceRecord(int l, string s, int t) { level = l;status = s;time = t; }
    }
    public class SkillUses
    {
        public int skillId;
        public int uses;
        public SkillUses(int s, int u) { skillId = s; uses = u; }
    }

    public class EventLogger : MonoBehaviour
    {

//     using sample:
//     EventLogger elogger = new EventLogger("http://localhost:8080");
//     ClearanceRecord c = new ClearanceRecord(1,"success",180);
//     SkillUses s = new SkillUses(1, 80);
//     elogger.logClearanceRecord(c);
//     elogger.logSkillUses(s);
        private readonly string _server;
        private static readonly HttpClient Client = new HttpClient();



        public EventLogger(string serverUrl)
        {
            _server = serverUrl;
        }
        // Start is called before the first frame update

        public async void LogClearanceRecord(ClearanceRecord data1)
        {
            var stringContent1 = new StringContent(JsonConvert.SerializeObject(data1), Encoding.UTF8, "application/json");
            var response = await client.PostAsync(server+ "/logClearanceRecord", stringContent1);
            if (System.Convert.ToInt32(response.StatusCode) == 200)
                print("logClearanceRecord success!");
            var responseString = response.Content.ReadAsStringAsync();
            print(responseString);
        }
        public async void LogSkillUses( SkillUses data1)
        {
            var stringContent1 = new StringContent(JsonConvert.SerializeObject(data1), Encoding.UTF8, "application/json");
            var response = await client.PostAsync(server+ "/logSkillUses", stringContent1);
            if (System.Convert.ToInt32(response.StatusCode) == 200)
                print("logClearanceRecord success!");
            var responseString = response.Content.ReadAsStringAsync();
            print(responseString);
        }


    }
}