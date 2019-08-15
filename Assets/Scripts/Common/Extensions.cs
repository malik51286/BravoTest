using UnityEngine;
using System.Collections;
using System;
public static class Extensions 
{
    public static void PerformActionWithDelay(this MonoBehaviour mono, float delay, Action action)
    {
        mono.StartCoroutine(mono.ExecuteDelayedAction(delay, action));
    }
    private static IEnumerator ExecuteDelayedAction(this MonoBehaviour ienum, float delay, Action action)
    {
        yield return new WaitForSeconds(delay);

        action();
    }
}
