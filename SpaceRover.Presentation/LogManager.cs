using System;

namespace SpaceRovers
{
    internal static class LogManager
    {
        /// <summary>
        /// Log kuyruğuna yeni bir log kaydı ekler.
        /// </summary>
        /// <param name="log"></param>
        public static void AddLogToQueue(string log)
        {
            Console.WriteLine(log);
        }

        public static void AddSystemLogToQueue(string log)
        {
            Console.WriteLine($"[Sistem Log]: {log}");
        }

        public static void AddSystemLogToQueue(string log, string error)
        {
            Console.WriteLine($"[Sistem Log]: {log} [Seyir Defteri]: {error}");
        }
    }
}
