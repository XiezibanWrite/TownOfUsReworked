namespace TownOfUsReworked.Modules
{
    [Serializable]
    public struct HSBColor
    {
        public float h;
        public float s;
        public float b;
        public float a;

        public HSBColor(float h, float s, float b, float a)
        {
            this.h = h;
            this.s = s;
            this.b = b;
            this.a = a;
        }

        public HSBColor(float h, float s, float b)
        {
            this.h = h;
            this.s = s;
            this.b = b;
            a = 1f;
        }

        public HSBColor(Color col)
        {
            var temp = FromColor(col);
            h = temp.h;
            s = temp.s;
            b = temp.b;
            a = temp.a;
        }

        public static HSBColor FromColor(Color color)
        {
            var ret = new HSBColor(0f, 0f, 0f, color.a);

            var r = color.r;
            var g = color.g;
            var b = color.b;

            var max = Mathf.Max(r, Mathf.Max(g, b));

            if (max <= 0)
                return ret;

            var min = Mathf.Min(r, Mathf.Min(g, b));
            var dif = max - min;

            if (max > min)
            {
                if (g == max)
                    ret.h = ((b - r) / dif * 60f) + 120f;
                else if (b == max)
                    ret.h = ((r - g) / dif * 60f) + 240f;
                else if (b > g)
                    ret.h = ((g - b) / dif * 60f) + 360f;
                else
                    ret.h = (g - b) / dif * 60f;

                if (ret.h < 0)
                    ret.h += 360f;
            }
            else
                ret.h = 0;

            ret.h *= 1f / 360f;
            ret.s = dif / max * 1f;
            ret.b = max;

            return ret;
        }

        public static Color ToColor(HSBColor hsbColor)
        {
            var r = hsbColor.b;
            var g = hsbColor.b;
            var b = hsbColor.b;

            if (hsbColor.s != 0)
            {
                var max = hsbColor.b;
                var dif = hsbColor.b * hsbColor.s;
                var min = hsbColor.b - dif;

                var h = hsbColor.h * 360f;

                if (h < 60f)
                {
                    r = max;
                    g = (h * dif / 60f) + min;
                    b = min;
                }
                else if (h < 120f)
                {
                    r = (-(h - 120f) * dif / 60f) + min;
                    g = max;
                    b = min;
                }
                else if (h < 180f)
                {
                    r = min;
                    g = max;
                    b = ((h - 120f) * dif / 60f) + min;
                }
                else if (h < 240f)
                {
                    r = min;
                    g = (-(h - 240f) * dif / 60f) + min;
                    b = max;
                }
                else if (h < 300f)
                {
                    r = ((h - 240f) * dif / 60f) + min;
                    g = min;
                    b = max;
                }
                else if (h <= 360f)
                {
                    r = max;
                    g = min;
                    b = (-(h - 360f) * dif / 60) + min;
                }
                else
                {
                    r = 0;
                    g = 0;
                    b = 0;
                }
            }

            return new(Mathf.Clamp01(r), Mathf.Clamp01(g), Mathf.Clamp01(b), hsbColor.a);
        }

        public readonly Color ToColor() => ToColor(this);

        public override readonly string ToString() => $"H: {h} S: {s} B: {b}";

        public static HSBColor Lerp(HSBColor a, HSBColor b, float t)
        {
            float h, s;

            //Check special case black (color.b==0): interpolate neither hue nor saturation!
            //Check special case grey (color.s==0): don't interpolate hue!
            if (a.b == 0)
            {
                h = b.h;
                s = b.s;
            }
            else if (b.b == 0)
            {
                h = a.h;
                s = a.s;
            }
            else
            {
                if (a.s == 0)
                    h = b.h;
                else if (b.s == 0)
                    h = a.h;
                else
                {
                    var angle = Mathf.LerpAngle(a.h * 360f, b.h * 360f, t);

                    while (angle < 0f)
                        angle += 360f;

                    while (angle > 360f)
                        angle -= 360f;

                    h = angle / 360f;
                }

                s = Mathf.Lerp(a.s, b.s, t);
            }

            return new(h, s, Mathf.Lerp(a.b, b.b, t), Mathf.Lerp(a.a, b.a, t));
        }

        public static void Test()
        {
            var color = new HSBColor(Color.red);
            Debug.Log("red: " + color);

            color = new(Color.green);
            Debug.Log("green: " + color);

            color = new(Color.blue);
            Debug.Log("blue: " + color);

            color = new(Color.grey);
            Debug.Log("grey: " + color);

            color = new(Color.white);
            Debug.Log("white: " + color);

            color = new(new(0.4f, 1f, 0.84f, 1f));
            Debug.Log("0.4, 1f, 0.84: " + color);

            Debug.Log("164,82,84   .... 0.643137f, 0.321568f, 0.329411f  :" + ToColor(new(new(0.643137f, 0.321568f, 0.329411f))));
        }
    }
}