using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace Utility.Loader{
    using Table = Dictionary<int, Dictionary<string, object>>;
    using TableInfo = Dictionary<string, object>;

    public static class CsvLoader{
        private static readonly Dictionary<string, Table> PrevLoaded = new();
        public const string EmptyString = "__empty";
        public static Table Load(string filename) {
            if (PrevLoaded.ContainsKey(filename)) return PrevLoaded[filename];
            var file = Resources.Load<TextAsset>(filename);
            if (file == null){
                Debug.LogError($"Can't find resources with name {filename}");
                return null;
            }
            
            var lines = ParseLine(file.text, '\n');
            if (lines.Length < 3){
                Debug.LogError($"Table contents error, less than 3 lines!");
                return null;
            }
            Dictionary<int, Dictionary<string, object>> ret = new();
            var keys = ParseLine(lines[0], ',');
            keys = keys.Select((str => str.ToLower())).ToArray();
            var idInd = 0;
            while (idInd < keys.Length && !keys[idInd].ToLower().Equals("id")) idInd++;
            if (idInd >= keys.Length){
                Debug.LogError("Table doesn't have field 'id'!");
                return null;
            }
            var types = ParseLine(lines[2], ',');
            for(var i = 3; i<lines.Length; i++){
                var line = ParseLine(lines[i], ',');
                var id = int.Parse(line[idInd]);
                ret[id] = new();
                for (var j = 0; j < line.Length; j++){
                    var key = keys[j];
                    var type = types[j];
                    ret[id][key] = ParseValue(type, line[j]);
                }
            }

            PrevLoaded[filename] = ret;
            return ret;
        }

        private static object ParseValue(string typename, string val) => typename.ToLower() switch{
            "int" => int.Parse(val),
            "string" => val.Equals(EmptyString)? "": val,
            "float" => float.Parse(val),
            _ => val
        };

        private static string[] ParseLine(string lines, char deli){
            lines = lines.Replace("\r", "");
            if (!lines.EndsWith(deli)) lines += deli;
            List<string> ret = new();
            var inQuote = false;
            var lastInd = 0;
            for (int i = 0; i < lines.Length; i++){
                switch (lines[i]){
                    case var x when x == deli && !inQuote:
                        ret.Add(lines.Substring(lastInd, i - lastInd));
                        lastInd = i + 1;
                        break;
                    case '\'':
                    case '\"':
                        inQuote = !inQuote;
                        break;
                }
            }
            return ret.ToArray();
        }

        public static TableInfo TryToLoad(string filename, int id) {
            var table = Load(filename);
            if (table == null) return null;
            if (!table.ContainsKey(id)) {
                Debug.LogError($"Wrong id in table '{filename}', id = {id}");
                return null;
            }
            return table[id];
        }
    }
}