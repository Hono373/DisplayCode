using UnityEngine;

public static class CanvasGroupExtensions
{
    public static void Ray(this CanvasGroup cg, bool clickAble)
    {
        cg.blocksRaycasts = clickAble;
    }
    public static void Alpha(this CanvasGroup cg, bool show)
    {
        if (show) cg.alpha = 1;
        else cg.alpha = 0;
    }
    public static CanvasGroup GetCanvasGroup(this MonoBehaviour target)
    {
        if (!target.TryGetComponent(out CanvasGroup canvasGroup))
        {
            canvasGroup = target.gameObject.AddComponent<CanvasGroup>();
        }
        return canvasGroup;
    }
}
