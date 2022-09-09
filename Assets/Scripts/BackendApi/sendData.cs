using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Http;
using System.Text.Json;
using System.Text;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using System.Net;


namespace DataAnalysis
{
    public class DataForm
    {
        public int[] timeOfLevels;
        public DataForm(int[] time) { timeOfLevels = time; }
    }

    public class SendData : MonoBehaviour
    {
        private string server;
        private static readonly HttpClient client = new HttpClient();



        public SendData(string serverUrl)
        {
            server = serverUrl;
        }
        // Start is called before the first frame update
        public async void send(int[] data)
        {
            var values = new DataForm(data);
            var stringContent = new StringContent(JsonConvert.SerializeObject(values), Encoding.UTF8, "application/json");
            var response = await client.PostAsync(server, stringContent);

            if (System.Convert.ToInt32(response.StatusCode) == 200)
                print("success!");
            //print(response.StatusCode);
            var responseString = response.Content.ReadAsStringAsync();
            print(responseString);
        }


    }
}