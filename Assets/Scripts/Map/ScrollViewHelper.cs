using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Map
{
    public class ScrollViewHelper : MonoBehaviour
    {
        private ScrollRect scrollRect;
        public float scrollDuration = 1f;

        private float scrollTime = 0f;
        private float startScrollPos;
        private bool isScrolling = false;

        private void Awake()
        {
            scrollRect = GetComponent<ScrollRect>();
        }

        private void OnEnable()
        {
            scrollRect.vertical = false;
        }

        public IEnumerator SmoothScrollToBottom()
        {
            yield return new WaitForSeconds(1f); // Wait 1 frame to let layout complete

            isScrolling = true;
            scrollTime = 0f;
            startScrollPos = scrollRect.verticalNormalizedPosition;

            while (scrollTime < scrollDuration)
            {
                scrollTime += Time.deltaTime;
                float t = scrollTime / scrollDuration;
                // Optional: ease out
                t = Mathf.SmoothStep(0, 1, t);

                scrollRect.verticalNormalizedPosition = Mathf.Lerp(startScrollPos, 0f, t);
                yield return null;
            }

            scrollRect.verticalNormalizedPosition = 0f;
            isScrolling = false;

            // Re-enable user interaction
            scrollRect.vertical = true;
        }
    }
}
