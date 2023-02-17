using UnityEngine;

/// <summary>
/// Collection of Easing Math Functions.<br />
/// Easing functions specify the rate of change of a parameter over time.<br />
/// In every function, the variable x represents the absolute progress of the animation<br />
/// in the bounds of 0 (beginning of the animation) and 1 (end of animation).<br />
/// Based on Easing Functions Cheat Sheet (https://easings.Net) by Andrey Sitnik and Ivan Solovev.
/// </summary>
public static class Easings
{
    public enum Methods
    {
        EaseInLine,
        EaseInSine,
        EaseOutSine,
        EaseInOutSine,
        EaseInQuad,
        EaseOutQuad,
        EaseInOutQuad,
        EaseInCubic,
        EaseOutCubic,
        EaseInOutCubic,
        EaseInQuart,
        EaseOutQuart,
        EaseInOutQuart,
        EaseInQuint,
        EaseOutQuint,
        EaseInOutQuint,
        EaseInExpo,
        EaseOutExpo,
        EaseInOutExpo,
        EaseInCirc,
        EaseOutCirc,
        EaseInOutCirc,
        EaseInBack,
        EaseOutBack,
        EaseInOutBack,
        EaseInElastic,
        EaseOutElastic,
        EaseInOutElastic,
        EaseInBounce,
        EaseOutBounce,
        EaseInOutBounce
    }

    public static float Perform(float x, Methods method)
    {
        return method switch
        {
            Methods.EaseInLine => EaseInLine(x),
            Methods.EaseInSine => EaseInSine(x),
            Methods.EaseOutSine => EaseOutSine(x),
            Methods.EaseInOutSine => EaseInOutSine(x),
            Methods.EaseInQuad => EaseInQuad(x),
            Methods.EaseOutQuad => EaseOutQuad(x),
            Methods.EaseInOutQuad => EaseInOutQuad(x),
            Methods.EaseInCubic => EaseInCubic(x),
            Methods.EaseOutCubic => EaseOutCubic(x),
            Methods.EaseInOutCubic => EaseInOutCubic(x),
            Methods.EaseInQuart => EaseInQuart(x),
            Methods.EaseOutQuart => EaseOutQuart(x),
            Methods.EaseInOutQuart => EaseInOutQuart(x),
            Methods.EaseInQuint => EaseInQuint(x),
            Methods.EaseOutQuint => EaseOutQuint(x),
            Methods.EaseInOutQuint => EaseInOutQuint(x),
            Methods.EaseInExpo => EaseInExpo(x),
            Methods.EaseOutExpo => EaseOutExpo(x),
            Methods.EaseInOutExpo => EaseInOutExpo(x),
            Methods.EaseInCirc => EaseInCirc(x),
            Methods.EaseOutCirc => EaseOutCirc(x),
            Methods.EaseInOutCirc => EaseInOutCirc(x),
            Methods.EaseInBack => EaseInBack(x),
            Methods.EaseOutBack => EaseOutBack(x),
            Methods.EaseInOutBack => EaseInOutBack(x),
            Methods.EaseInElastic => EaseInElastic(x),
            Methods.EaseOutElastic => EaseOutElastic(x),
            Methods.EaseInOutElastic => EaseInOutElastic(x),
            Methods.EaseInBounce => EaseInBounce(x),
            Methods.EaseOutBounce => EaseOutBounce(x),
            Methods.EaseInOutBounce => EaseInOutBounce(x),
            _ => x,
        };
    }

    #region Linear
    public static float EaseInLine(float x) => x;
    #endregion

    #region Sinusoidal
    public static float EaseInSine(float x) => 1 - Mathf.Cos((x * Mathf.PI) / 2);
    public static float EaseOutSine(float x) => Mathf.Sin((x * Mathf.PI) / 2);
    public static float EaseInOutSine(float x) => -(Mathf.Cos(Mathf.PI * x) - 1) / 2;
    #endregion

    #region Quadratic
    public static float EaseInQuad(float x) => x * x;
    public static float EaseOutQuad(float x) => 1 - (1 - x) * (1 - x);
    public static float EaseInOutQuad(float x) => x < 0.5 ? 2 * x * x : 1 - Mathf.Pow(-2 * x + 2, 2) / 2;
    #endregion

    #region Cubic
    public static float EaseInCubic(float x) => x * x * x;
    public static float EaseOutCubic(float x) => 1 - Mathf.Pow(1 - x, 3);
    public static float EaseInOutCubic(float x) => x < 0.5 ? 4 * x * x * x : 1 - Mathf.Pow(-2 * x + 2, 3) / 2;
    #endregion

    #region Quartic
    public static float EaseInQuart(float x) => x * x * x * x;
    public static float EaseOutQuart(float x) => 1 - Mathf.Pow(1 - x, 4);
    public static float EaseInOutQuart(float x) => x < 0.5 ? 8 * x * x * x * x : 1 - Mathf.Pow(-2 * x + 2, 4) / 2;
    #endregion

    #region Quintic
    public static float EaseInQuint(float x) => x * x * x * x * x;
    public static float EaseOutQuint(float x) => 1 - Mathf.Pow(1 - x, 5);
    public static float EaseInOutQuint(float x) => x < 0.5 ? 16 * x * x * x * x * x : 1 - Mathf.Pow(-2 * x + 2, 5) / 2;
    #endregion

    #region Exponential
    public static float EaseInExpo(float x) => x == 0 ? 0 : Mathf.Pow(2, 10 * x - 10);
    public static float EaseOutExpo(float x) => x == 1 ? 1 : 1 - Mathf.Pow(2, -10 * x);
    public static float EaseInOutExpo(float x)
    {
        return x == 0 ? 0 : x == 1 ? 1 : x < 0.5 ? Mathf.Pow(2, 20 * x - 10) / 2 : (2 - Mathf.Pow(2, -20 * x + 10)) / 2;
    }
    #endregion

    #region Circular
    public static float EaseInCirc(float x) => 1 - Mathf.Sqrt(1 - Mathf.Pow(x, 2));
    public static float EaseOutCirc(float x) => Mathf.Sqrt(1 - Mathf.Pow(x - 1, 2));
    public static float EaseInOutCirc(float x)
    {
        return x < 0.5 ? (1 - Mathf.Sqrt(1 - Mathf.Pow(2 * x, 2))) / 2 : (Mathf.Sqrt(1 - Mathf.Pow(-2 * x + 2, 2)) + 1) / 2;
    }
    #endregion

    #region Back
    public static float EaseInBack(float x)
    {
        float c0 = 1.70158f;
        float c1 = c0 + 1;
        return (c1 * x * x * x) - (c0 * x * x);
    }
    public static float EaseOutBack(float x)
    {
        float c0 = 1.70158f;
        float c1 = c0 + 1;
        return 1 + (c1 * Mathf.Pow(x - 1, 3)) + (c0 * Mathf.Pow(x - 1, 2));
    }
    public static float EaseInOutBack(float x)
    {
        float c0 = 1.70158f;
        float c1 = c0 * 1.525f;
        return x < 0.5
            ? Mathf.Pow(2 * x, 2) * (((c1 + 1) * 2 * x) - c1) / 2
            : ((Mathf.Pow((2 * x) - 2, 2) * (((c1 + 1) * ((x * 2) - 2)) + c1)) + 2) / 2;
    }
    #endregion

    #region Elastic
    public static float EaseInElastic(float x)
    {
        float c = 2f * Mathf.PI / 3f;
        return x == 0 ? 0 : x == 1 ? 1 : -Mathf.Pow(2, 10 * x - 10) * Mathf.Sin((x * 10 - 10.75f) * c);
    }

    public static float EaseOutElastic(float x)
    {
        float c = 2f * Mathf.PI / 3f;
        return x == 0 ? 0 : x == 1 ? 1 : Mathf.Pow(2, -10 * x) * Mathf.Sin((x * 10 - 0.75f) * c) + 1;
    }

    public static float EaseInOutElastic(float x)
    {
        float c = 2f * Mathf.PI / 4.5f;
        return x == 0 ? 0 : x == 1 ? 1 : x < 0.5 
            ? -(Mathf.Pow(2, (20 * x) - 10) * Mathf.Sin(((20 * x) - 11.125f) * c)) / 2
            : (Mathf.Pow(2, (-20 * x) + 10) * Mathf.Sin(((20 * x) - 11.125f) * c) / 2) + 1;
    }
    #endregion

    #region Bounce
    public static float EaseInBounce(float x) => 1 - EaseOutBounce(1 - x);
    public static float EaseOutBounce(float x)
    {
        float n1 = 7.5625f;
        float d1 = 2.75f;
        if (x < 1 / d1) return n1 * x * x;
        else if (x < 2 / d1) return n1 * (x -= 1.5f / d1) * x + 0.75f;
        else if (x < 2.5 / d1) return n1 * (x -= 2.25f / d1) * x + 0.9375f;
        else return n1 * (x -= 2.625f / d1) * x + 0.984375f;
    }
    public static float EaseInOutBounce(float x) => x < 0.5 ? (1 - EaseOutBounce(1 - 2 * x)) / 2 : (1 + EaseOutBounce(2 * x - 1)) / 2;
    #endregion
}
