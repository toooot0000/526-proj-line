using System;
using System.Collections.Generic;
using System.Linq;
using Tutorials;
using Utility.Loader;

namespace Utility.Extensions{
    public static class StringExtension{
        public static object[] ParseAsParams(this string str, string[] types){
            return str.Split(";").Select((t, i) => CsvLoader.ParseValue(types[i], t)).ToArray();
        }

        public static T[] ParseAsParams<T>(this string str, Func<string, T> parser){
            return str.Split(";").Select(parser).ToArray();
        }
    }
}