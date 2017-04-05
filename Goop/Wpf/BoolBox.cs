namespace Goop.Wpf
{
    public static class BoolBox
    {
        public static readonly object True = true;
        public static readonly object False = false;
        public static object Box(bool value) => value ? True : False;
    }
}
