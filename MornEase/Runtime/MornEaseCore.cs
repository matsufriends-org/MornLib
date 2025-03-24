using System;
using UnityEngine;

namespace MornEase
{
    public static class MornEaseCore
    {
        public static float Ease(this float x, MornEaseType easeType)
        {
            if (x <= 0) return 0;

            if (x >= 1) return 1;

            const float c1 = 1.70158f;
            const float c2 = c1 * 1.525f;
            const float c3 = c1 + 1f;
            const float c4 = 2 * Mathf.PI / 3;
            const float c5 = 2 * Mathf.PI / 4.5f;
            const float n1 = 7.5625f;
            const float d1 = 2.75f;
            switch (easeType)
            {
                case MornEaseType.EaseInSine:
                    return 1 - Mathf.Cos(x * Mathf.PI / 2);
                case MornEaseType.EaseOutSine:
                    return Mathf.Sin(x * Mathf.PI / 2);
                case MornEaseType.EaseInOutSine:
                    return -(Mathf.Cos(Mathf.PI * x) - 1) / 2;
                case MornEaseType.EaseInQuad:
                    return x * x;
                case MornEaseType.EaseOutQuad:
                    return 1 - (1 - x) * (1 - x);
                case MornEaseType.EaseInOutQuad:
                    return x < 0.5 ? 2 * x * x : 1 - Mathf.Pow(-2 * x + 2, 2) / 2;
                case MornEaseType.EaseInCubic:
                    return x * x * x;
                case MornEaseType.EaseOutCubic:
                    return 1 - Mathf.Pow(1 - x, 3);
                case MornEaseType.EaseInOutCubic:
                    return x < 0.5 ? 4 * x * x * x : 1 - Mathf.Pow(-2 * x + 2, 3) / 2;
                case MornEaseType.EaseInQuart:
                    return x * x * x * x;
                case MornEaseType.EaseOutQuart:
                    return 1 - Mathf.Pow(1 - x, 4);
                case MornEaseType.EaseInOutQuart:
                    return x < 0.5 ? 8 * x * x * x * x : 1 - Mathf.Pow(-2 * x + 2, 4) / 2;
                case MornEaseType.EaseInQuint:
                    return x * x * x * x * x;
                case MornEaseType.EaseOutQuint:
                    return 1 - Mathf.Pow(1 - x, 5);
                case MornEaseType.EaseInOutQuint:
                    return x < 0.5 ? 16 * x * x * x * x * x : 1 - Mathf.Pow(-2 * x + 2, 5) / 2;
                case MornEaseType.EaseInExpo:
                    return Mathf.Pow(2, 10 * x - 10);
                case MornEaseType.EaseOutExpo:
                    return 1 - Mathf.Pow(2, -10 * x);
                case MornEaseType.EaseInOutExpo:
                    return x < 0.5 ? Mathf.Pow(2, 20 * x - 10) / 2 : (2 - Mathf.Pow(2, -20 * x + 10)) / 2;
                case MornEaseType.EaseInCirc:
                    return 1 - Mathf.Sqrt(1 - Mathf.Pow(x, 2));
                case MornEaseType.EaseOutCirc:
                    return Mathf.Sqrt(1 - Mathf.Pow(x - 1, 2));
                case MornEaseType.EaseInOutCirc:
                    return x < 0.5
                        ? (1 - Mathf.Sqrt(1 - Mathf.Pow(2 * x, 2))) / 2
                        : (Mathf.Sqrt(1 - Mathf.Pow(-2 * x + 2, 2)) + 1) / 2;
                case MornEaseType.EaseInBack:
                    return c3 * x * x * x - c1 * x * x;
                case MornEaseType.EaseOutBack:
                    return 1 + c3 * Mathf.Pow(x - 1, 3) + c1 * Mathf.Pow(x - 1, 2);
                case MornEaseType.EaseInOutBack:
                    return x < 0.5
                        ? Mathf.Pow(2 * x, 2) * ((c2 + 1) * 2 * x - c2) / 2
                        : (Mathf.Pow(2 * x - 2, 2) * ((c2 + 1) * (x * 2 - 2) + c2) + 2) / 2;
                case MornEaseType.EaseInElastic:
                    return -Mathf.Pow(2, 10 * x - 10) * Mathf.Sin((x * 10 - 10.75f) * c4);
                case MornEaseType.EaseOutElastic:
                    return Mathf.Pow(2, -10 * x) * Mathf.Sin((x * 10 - 0.75f) * c4) + 1;
                case MornEaseType.EaseInOutElastic:
                    return x < 0.5
                        ? -(Mathf.Pow(2, 20 * x - 10) * Mathf.Sin((20 * x - 11.125f) * c5)) / 2
                        : Mathf.Pow(2, -20 * x + 10) * Mathf.Sin((20 * x - 11.125f) * c5) / 2 + 1;
                case MornEaseType.EaseInBounce:
                    return 1 - Ease(1 - x, MornEaseType.EaseOutBounce);
                case MornEaseType.EaseOutBounce:
                    return x switch
                    {
                        < 1 / d1 => n1 * x * x,
                        < 2 / d1 => n1 * (x -= 1.5f / d1) * x + 0.75f,
                        < 2.5f / d1 => n1 * (x -= 2.25f / d1) * x + 0.9375f,
                        _ => n1 * (x -= 2.625f / d1) * x + 0.984375f
                    };
                case MornEaseType.EaseInOutBounce:
                    return x < 0.5
                        ? (1 - Ease(1 - 2 * x, MornEaseType.EaseOutBounce)) / 2
                        : (1 + Ease(2 * x - 1, MornEaseType.EaseOutBounce)) / 2;
                case MornEaseType.Linear:
                    return x;
                default:
                    throw new ArgumentOutOfRangeException(nameof(easeType), easeType, null);
            }
        }
    }
}