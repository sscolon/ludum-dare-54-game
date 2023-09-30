using System;
using UnityEngine;

namespace DDCore
{
    public static class Easing
    {
        public static float InElastic(float time) => 1 - OutElastic(1 - time);
        public static float OutElastic(float time)
        {
            float power = 0.3f;
            return (float)Math.Pow(2, -10 * time) * (float)Math.Sin((time - power / 4) * (2 * Math.PI) / power) + 1;
        }

        public static float InOutElastic(float time)
        {
            if (time < 0.5) return InElastic(time * 2) / 2;
            return 1 - InElastic((1 - time) * 2) / 2;
        }
        public static float InBounce(float time) => 1 - OutBounce(1 - time);
        public static float OutBounce(float time)
        {
            float div = 2.75f;
            float mult = 7.5625f;

            if (time < 1 / div)
            {
                return mult * time * time;
            }
            else if (time < 2 / div)
            {
                time -= 1.5f / div;
                return mult * time * time + 0.75f;
            }
            else if (time < 2.5 / div)
            {
                time -= 2.25f / div;
                return mult * time * time + 0.9375f;
            }
            else
            {
                time -= 2.625f / div;
                return mult * time * time + 0.984375f;
            }
        }
        public static float InOutBounce(float time)
        {
            if (time < 0.5) return InBounce(time * 2) / 2;
            return 1 - InBounce((1 - time) * 2) / 2;
        }
        public static float InSine(float time) => (float)-Math.Cos(time * Math.PI / 2);
        public static float OutSine(float time) => (float)Math.Sin(time * Math.PI / 2);
        public static float InOutSine(float time) => (float)(Math.Cos(time * Math.PI) - 1) / -2;
        public static float InCubic(float t) => t * t * t;
        public static float OutCubic(float t)
        {
            return 1 - (float)Math.Pow(1 - t, 3);
        }

        public static float InOutCubic(float t)
        {
            if (t < 0.5) return InCubic(t * 2) / 2;
            return 1 - InCubic((1 - t) * 2) / 2;
        }
        private static float Flip(float time)
        {
            return 1 - time;
        }

        public static float InSquare(float time)
        {
            return time * time;
        }

        public static float OutSquare(float time)
        {
            return Flip(InSquare(Flip(time)));
        }
        public static float OutExpo(float time)
        {
            return time == 1 ? 1 : 1 - Mathf.Pow(2, -10 * time);
        }

        public static float OutCirc(float time)
        {
            return Mathf.Sqrt(1 - Mathf.Pow(time - 1, 2));
        }

        public static float InQuint(float time)
        {
            return time * time * time * time * time;
        }

        public static float OutQuint(float time)
        {
            return 1 - Mathf.Pow(1 - time, 5);
        }

        public static float InOutQuint(float time)
        {
            return time < 0.5 ? 16 * time * time * time * time * time : 1 - Mathf.Pow(-2 * time + 2, 5) / 2;
        }

        public static float InBack(float time)
        {
            const float C1 = 1.70158f;
            const float C3 = C1 + 1;
            return C3 * time * time * time - C1 * time * time;
        }

        public static float OutBack(float time)
        {
            const float C1 = 1.70158f;
            const float C3 = C1 + 1;
            return 1 + C3 * Mathf.Pow(time - 1, 3) + C1 * Mathf.Pow(time - 1, 2);
        }

        public static float InOutBack(float time)
        {
            const float C1 = 1.70158f;
            const float C2 = C1 * 1.525f;
            return time < 0.5f
               ? (Mathf.Pow(2 * time, 2) * ((C2 + 1) * 2 * time - C2)) / 2
               : (Mathf.Pow(2 * time - 2, 2) * ((C2 + 1) * (time * 2 - 2) + C2) + 2) / 2;
        }

        public static Vector3 Calculate(Vector3 start, Vector3 end, float time, EaseType easeType)
        {
            float easedTime = Calculate(time, easeType);
            float scaleX = Mathf.Lerp(start.x, end.x, easedTime);
            float scaleY = Mathf.Lerp(start.y, end.y, easedTime);
            return new Vector3(scaleX, scaleY);
        }

        public static float Calculate(float time, EaseType easeType)
        {
            switch (easeType)
            {
                default:
                case EaseType.In_Elastic:
                    return InElastic(time);
                case EaseType.Out_Elastic:
                    return OutElastic(time);
                case EaseType.In_Out_Elastic:
                    return InOutElastic(time);
                case EaseType.In_Bounce:
                    return InBounce(time);
                case EaseType.Out_Bounce:
                    return OutBounce(time);
                case EaseType.In_Out_Bounce:
                    return InOutBounce(time);
                case EaseType.In_Sine:
                    return InSine(time);
                case EaseType.Out_Sine:
                    return OutSine(time);
                case EaseType.In_Out_Sine:
                    return Mathf.Lerp(InSine(time), OutSine(time), time);
                case EaseType.In_Square:
                    return InSquare(time);
                case EaseType.Out_Square:
                    return OutSquare(time);
                case EaseType.In_Out_Square:
                    return Mathf.Lerp(InSquare(time), OutSquare(time), time);
                case EaseType.In_Cubic:
                    return InCubic(time);
                case EaseType.Out_Cubic:
                    return OutCubic(time);
                case EaseType.In_Out_Cubic:
                    return InOutCubic(time);
                case EaseType.In_Quint:
                    return InQuint(time);
                case EaseType.Out_Quint:
                    return OutQuint(time);
                case EaseType.In_Out_Quint:
                    return InOutQuint(time);
                case EaseType.Out_Circ:
                    return OutCirc(time);
                case EaseType.In_Back:
                    return InBack(time);
                case EaseType.Out_Back:
                    return OutBack(time);
                case EaseType.In_Out_Back:
                    return InOutBack(time);
            }
        }


        public static float Calculate(float start, float end, float time, EaseType easeType)
        {
            float easedTime = Calculate(time, easeType);
            float f = Mathf.Lerp(start, end, easedTime);
            return f;
        }
    }
}
