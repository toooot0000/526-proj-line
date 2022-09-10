using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using UnityEngine;

namespace BackendApi
{
    public class DataForm
    {
        public int[] timeOfLevels;
        public DataForm(int[] time) { timeOfLevels = time; }
    }

    public class EventLogger : MonoBehaviour
    {
        private readonly string _server;
        private static readonly HttpClient Client = new HttpClient();



        public EventLogger(string serverUrl)
        {
            _server = serverUrl;
        }
        // Start is called before the first frame update
        public async void Log(int[] data)
        {
            var values = new DataForm(data);
            var stringContent = new StringContent(JsonConvert.SerializeObject(values), Encoding.UTF8, "application/json");
            var response = await Client.PostAsync(_server, stringContent);

            if (System.Convert.ToInt32(response.StatusCode) == 200)
                print("success!");
            //print(response.StatusCode);
            var responseString = response.Content.ReadAsStringAsync();
            print(responseString);
        }


    }
}