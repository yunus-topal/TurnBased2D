using System.Collections;
using UnityEngine;

namespace Helpers
{
    public static class SystemHelper
    {
        public static IEnumerator InvokeLambda(System.Action action, float delay)
        {
            yield return new WaitForSeconds(delay);
            action?.Invoke();
        }

    }
}