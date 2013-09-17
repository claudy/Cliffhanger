using System;

namespace Cliffhanger
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (CliffhangerGame game = new CliffhangerGame())
            {
                game.Run();
            }
        }
    }
#endif
}

