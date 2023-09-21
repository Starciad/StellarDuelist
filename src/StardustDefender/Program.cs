using System;

namespace StardustDefender
{
    internal static class Program
    {
        [STAThread]
        private static void Main()
        {
            using SGame game = new();
            game.Run();
        }
    }
}