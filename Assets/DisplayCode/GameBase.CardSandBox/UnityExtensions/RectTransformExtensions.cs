using UnityEngine;

public static class RectTransformExtensions
{
    public static RectTransform GetRectTransform(this MonoBehaviour target)
    {
        if (!target.TryGetComponent(out RectTransform canvasGroup))
        {
            canvasGroup = target.gameObject.AddComponent<RectTransform>();
        }
        return canvasGroup;
    }
}
