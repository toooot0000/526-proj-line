using System.Collections.Generic;
using UnityEngine;

namespace Utility.Loader{
    public class CsvLoader{
        public static Dictionary<int, Dictionary<string, object>> Load(string filename){
            var file = Resources.Load<TextAsset>(filename);
            if (file == null){
                Debug.LogError($"Can't find resources with name {filename}");
            }
            var lines = file.text.Split("\n");
            if (lines.Length < 3){
                Debug.LogError($"Table contents error, less than 3 lines!");
            }
            Dictionary<int, Dictionary<string, object>> ret = new();
            var keys = ParseHeader(lines[0]);
            var types = ParseType(lines[1]);
            for(var i = 3; i<lines.Length; i++){
                ret[i] = new();
                var line = lines[i].Split(",");
                for (var j = 0; j < line.Length; j++){
                    var key = keys[j];
                    var type = types[j];
                    ret[i][key] = ParseValue(type, line[j]);
                }
            }
            return ret;
        }

        private static string[] ParseHeader(string line) => line.Split("\n");

        private static string[] ParseType(string line) => line.Split("\n");

        private static object ParseValue(string typename, string str) => typename.ToLower() switch{
            "int" => int.Parse(str),
            "string" => str,
            "float" => float.Parse(str),
            _ => str
        };
    }
}