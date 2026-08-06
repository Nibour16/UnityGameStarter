namespace UnityGameStarter.StringLibrary 
{
    public class StringLibrary
    {
        public static object Parse(string value)
        {
            if (int.TryParse(value, out var i))
                return i;

            if (float.TryParse(value, out var f))
                return f;

            if (bool.TryParse(value, out var b))
                return b;

            return value;
        }
    }
}