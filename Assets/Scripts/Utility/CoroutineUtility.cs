using System;
using System.Collections;

namespace Utility{
    public class CoroutineUtility{
        public static IEnumerator Delayed(Action action){
            yield return null;
            action();
        }
    }
}