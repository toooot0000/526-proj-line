using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace Utility.Loader{
    public static class CsvLoader{
        public static Dictionary<int, Dictionary<string, object>> Load(string filename){
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
            return ret;
        }

        private static object ParseValue(string typename, string val) => typename.ToLower() switch{
            "int" => int.Parse(val),
            "string" => val,
            "float" => float.Parse(val),
            _ => val
        };

        private static string[] ParseLine(string lines, char deli){
            lines = lines.Replace("\r", "");
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
        
        public static DataTable LoadCsv(string fileName)
        {
            DataTable dt = new DataTable();
            FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            StreamReader sr = new StreamReader(fs, new System.Text.UnicodeEncoding());
            //记录每次读取的一行记录
            string strLine = "";
            //记录每行记录中的各字段内容
            string[] aryLine;
            //标示列数
            int columnCount = 0;
            //标示是否是读取的第一行
            bool IsFirst = true;
        
            //逐行读取CSV中的数据
            while ((strLine = sr.ReadLine()) != null)
            {
                aryLine = strLine.Split(',');
                if (IsFirst == true)
                {
                    IsFirst = false;
                    columnCount = aryLine.Length;
                    Console.Out.WriteLine(columnCount);
                    //创建列
                    for (int i = 0; i < columnCount; i++)
                    {
                        DataColumn dc = new DataColumn(aryLine[i]);
                        dt.Columns.Add(dc);
                            
                    }
                }
                else
                {
                    DataRow dr = dt.NewRow();
                    for (int j = 0; j < columnCount; j++)
                    {
                        dr[j] = aryLine[j];
                    }
                    dt.Rows.Add(dr);
                }
            }
        
            sr.Close();
            fs.Close();
            return dt;
        }
    }
}