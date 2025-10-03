using System;
using UnityEngine;

namespace Helpers
{
    public static class GeneralHelper
    {
        public static int ClampAndWarn(int actual, int limit, string label)
        {
            if (actual > limit)
                Debug.LogWarning($"{label} Character Limit exceeded; only first {limit} will be used.");
            return Math.Min(actual, limit);
        }
    }
}