namespace Dairy.Api;

public static class Extensions
{
    public static T Also<T>(this T o, Action<T> action)
    {
        action(o);
        return o;
    }
}