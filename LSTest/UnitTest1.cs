using System.IO;
using NUnit.Framework;

namespace LSTest
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void PrintHDLTest1()
        {
            string[] lines = File.ReadAllLines("../../../../TestHDLFiles/test1.txt");
            LogicSim.RunModes.GenerateCircuit(lines);
            string expected = "A B | w1 w2 w3\n0 0 |  0  0  1\n0 1 |  0  0  1\n1 0 |  0  0  1\n1 1 |  1  1  0\n";

            Assert.AreEqual(expected, LogicSim.CircuitGroup.mainCircuit.ToString());
        }
    }
}