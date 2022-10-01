using System;

namespace Utility {
    public static class IntUtility {
        public static int ParseString(string s) => (int)Math.Round(float.Parse(s));
    }
}