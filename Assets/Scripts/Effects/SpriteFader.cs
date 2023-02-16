using System;
using System.Collections;
using UnityEngine;

public static class SpriteFader
{
    public static void FadeIn(SpriteRenderer spriteRenderer, float fadeSeconds = 3f, Action callback = null)
        => GameManager.Instance.StartCoroutine(Fade(fadeIn: true, spriteRenderer, fadeSeconds, callback));

    public static void FadeOut(SpriteRenderer spriteRenderer, float fadeSeconds = 3f, Action callback = null)
        => GameManager.Instance.StartCoroutine(Fade(fadeIn: false, spriteRenderer, fadeSeconds, callback));

    private static IEnumerator Fade(bool fadeIn, SpriteRenderer spriteRenderer, float fadeSeconds, Action callback)
    {
        Color initialColor = spriteRenderer.color;
        Color finalColor = spriteRenderer.color;
        finalColor.a = fadeIn ? 1f : 0f;
        fadeSeconds = Mathf.Clamp(fadeSeconds, 0f, Mathf.Infinity);
        Func<bool> CheckFinalColorNotReached() => fadeIn ?
            () => spriteRenderer.color.a < finalColor.a :
            () => spriteRenderer.color.a > finalColor.a;
        Func<bool> FinalColorNotReached = CheckFinalColorNotReached();
        float t = 0f;
        while (FinalColorNotReached())
        {
            yield return new WaitForEndOfFrame();
            t += Time.deltaTime / fadeSeconds;
            spriteRenderer.color = Color.Lerp(initialColor, finalColor, t);
        }
        callback?.Invoke();
    }
}
