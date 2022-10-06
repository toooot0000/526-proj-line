using System;

namespace Utility{
    public static class IntUtility{
        public static int ParseString(string s){
            return (int)Math.Round(float.Parse(s));
        }
    }
}