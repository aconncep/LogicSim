using System;

namespace LogicSim
{
    public class RunModes
    {
        public static void RunAuto(bool verbose)
        {
            Console.WriteLine("Running in auto, verbose is " + verbose);
        }

        public static void RunManual(bool verbose)
        {
            Console.WriteLine("Running in manual, verbose is " + verbose);
        }
    }
}