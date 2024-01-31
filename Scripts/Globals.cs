using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;
using System.Runtime.InteropServices;
using TweenSharp;
using System.Linq.Expressions;
using TweenSharp.Factory;
using MonoGame.Extended.Tweening;
using System.Reflection;
using Newtonsoft.Json.Linq;
using TweenSharp.Animation;

namespace RhythmGalaxy
{
    public static class Globals
    {
        public static string currentScene = "";
        public static Dictionary<string, Scene> scenes = new Dictionary<string, Scene>();
        public static Random random = new Random();
        //public static TweenHandler? tweenHandler;
        public static float timeDelta { get
            {
                return 1f / 60f;
            }
        }
        public static string resPath
        {
            get { return workingDir + @"\res\"; }
        }
        public static string levelsPath
        {
            get { return workingDir + @"\levels\"; }
        }
        public static string workingDir
        {
            get
            {
                unsafe
                {
                    return new string(GetWorkingDirectory()) + @"\res\";
                }
            }
        }
        public static Color LerpColor(Color a, Color b, float t)
        {
            return Vector4.Lerp(a.ToVector(), b.ToVector(), t).ToColor();
        }
        public static Vector4 ToVector(this Color color)
        {
            return new Vector4(
                color.r,
                color.g,
                color.b,
                color.a
                );
        }
        public static Color ToColor(this Vector4 vect)
        {
            return new Color(
                (byte)vect.X,
                (byte)vect.Y,
                (byte)vect.Z,
                (byte)vect.W
                );
        }
        public static Linecast ToLinecast(this Raycast raycast)
        {
            var linecast = new Linecast();
            linecast.startPos = raycast.startPos;
            linecast.endPos = raycast.startPos + (raycast.direction * raycast.distance);
            linecast.range = raycast.range;
            return linecast;
        }
        public static Raycast ToRaycast(this Linecast linecast)
        {
            var raycast = new Raycast();
            raycast.startPos = linecast.startPos;
            raycast.distance = (linecast.endPos - linecast.startPos).Length();
            raycast.direction = (linecast.endPos - linecast.startPos) / raycast.distance;
            raycast.range = linecast.range;
            return raycast;
        }
        public static Vector2 Absolute(this Vector2 vect)
        {
            return new Vector2(MathF.Abs(vect.X), MathF.Abs(vect.Y));
        }
        public static Vector2 Normalize(this Vector2 vect)
        {
            float c = MathF.Sqrt(MathF.Abs(vect.X) + MathF.Abs(vect.Y));
            return new Vector2(vect.X / c, vect.Y / c);
        }
        public static int FindEmptySlot(this List<Object> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i] == null)
                    return i;
            }
            return -1;
        }
        public static void DrawLineTo(this Vector2 vect, Vector2 vect2, Color color)
        {
            Raylib.DrawLine((int)vect.X, (int)vect.Y, (int)vect2.X, (int)vect2.Y, color);
        }
        public static void DrawLine(this Linecast cast, Color color)
        {
            cast.startPos.DrawLineTo(cast.endPos, color);
        }

        public static unsafe byte[] ConvertStruct<T>(ref T str) where T : struct
        {
            int size = Marshal.SizeOf(str);
            var arr = new byte[size];

            fixed (byte* arrPtr = arr)
            {
                Marshal.StructureToPtr(str, (IntPtr)arrPtr, true);
            }

            return arr;
        }
        /*public static TweenTarget<T, double> Tween<T>(this T target, Expression<Func<T, double>> expression)
        {
            var memberExpression = expression.Body as MemberExpression;

            if (memberExpression == null)
            {
                // If it's not a MemberExpression, check if it's a UnaryExpression (might be due to implicit conversion)
                var unaryExpression = expression.Body as UnaryExpression;

                if (unaryExpression != null && unaryExpression.Operand is MemberExpression)
                {
                    memberExpression = (MemberExpression)unaryExpression.Operand;
                }
            }

            if (memberExpression != null)
            {
                //Console.WriteLine(memberExpression.Member.Name); // This should output "hp"
            }
            else
            {
                Console.WriteLine("Expression is not of type MemberExpression");
            }
            return new TweenTargetModified<T, double>(target, memberExpression, ArithmeticProgressFunction);
        }*/
        private static double ArithmeticProgressFunction(double start, double end, double progress)
        {
            return start + (end - start) * progress;
        }
        

        /*public Tween<TMember> TweenTo<TTarget, TMember>(this Tweener tweener, TTarget target, Expression<Func<TTarget, TMember>> expression, TMember toValue, float duration, float delay = 0f) where TTarget : class where TMember : struct
        {
            MemberInfo member = ((MemberExpression)expression.Body).Member;
            TweenMember<TMember> member2 = GetMember<TMember>(target, member.Name);
            tweener.FindTween(target, member2.Name)?.Cancel();
            tweener.AllocationCount++;
            Tween<TMember> tween = new Tween<TMember>(target, duration, delay, member2, toValue);
            tweener._activeTweens.Add(tween);
            return tween;
        }*/
    }
}
