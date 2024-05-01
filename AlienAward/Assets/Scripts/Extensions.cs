using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class Extensions
{
    public static void Shuffle<T>(this IList<T> ts)
    {
        var count = ts.Count;
        var last = count - 1;
        for (var i = 0; i < last; ++i)
        {
            var r = UnityEngine.Random.Range(i, count);
            var tmp = ts[i];
            ts[i] = ts[r];
            ts[r] = tmp;
        }
    }

    // Runs the Callback after a given number of seconds, after the Update completes
    public static Coroutine Invoke(this MonoBehaviour self, UnityAction Callback, float seconds)
    {
        return self.StartCoroutine(InSecondsCoroutine(seconds, Callback));
    }

    static IEnumerator InSecondsCoroutine(float seconds, UnityAction Callback)
    {
        yield return new WaitForSeconds(seconds);
        Callback?.Invoke();
    }
}
