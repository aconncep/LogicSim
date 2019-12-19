using System.Collections.Generic;
using System.IO;
using LogicSim;
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
            RunModes.GenerateCircuit(lines);
            string expected = "A B | w1 w2 w3\n0 0 |  0  0  1\n0 1 |  0  0  1\n1 0 |  0  1  0\n1 1 |  1  1  0\n";

            Assert.AreEqual(expected, CircuitGroup.mainCircuit.ToString());
            CircuitGroup.mainCircuit.ClearCombinations();
        }
        
        [Test]
        public void PrintHDLTest2()
        {
            string[] lines = File.ReadAllLines("../../../../TestHDLFiles/test2.txt");
            RunModes.GenerateCircuit(lines);
            string expected = "A B C | w1 w2 w4\n0 0 0 |  0  0  0\n0 0 1 |  1  0  1\n0 1 0 |  0  0  0\n0 1 1 |  1  0  1\n1 0 0 |  1  0  1\n1 0 1 |  1  0  0\n" +
                              "1 1 0 |  1  0  1\n1 1 1 |  1  0  0\n";

            Assert.AreEqual(expected, CircuitGroup.mainCircuit.ToString());
            CircuitGroup.mainCircuit.ClearCombinations();

        }

        [Test]

        public void CheckCombinationEquality()
        {
            Variable v1 = new Variable("A", 1, VariableType.INPUT);
            Variable v2 = new Variable("B", 0, VariableType.INPUT);
            Variable v3 = new Variable("C", 0, VariableType.INPUT);
            List<Variable> inputs1 = new List<Variable>();
            inputs1.Add(v1);
            inputs1.Add(v2);
            inputs1.Add(v3);
            
            Variable v4 = new Variable("w1", 1, VariableType.LOCAL);
            Variable v5 = new Variable("w2", 1, VariableType.LOCAL);
            Variable v6 = new Variable("w3", 1, VariableType.LOCAL);
            List<Variable> locals1 = new List<Variable>();
            locals1.Add(v4);
            locals1.Add(v5);
            locals1.Add(v6);
            
            Combination c1 = new Combination(inputs1, locals1);
            
            Variable x1 = new Variable("A", 1, VariableType.INPUT);
            Variable x2 = new Variable("B", 0, VariableType.INPUT);
            Variable x3 = new Variable("C", 0, VariableType.INPUT);
            List<Variable> inputs2 = new List<Variable>();
            inputs2.Add(x1);
            inputs2.Add(x2);
            inputs2.Add(x3);
            
            Variable z4 = new Variable("w1", 1, VariableType.LOCAL);
            Variable z5 = new Variable("w2", 1, VariableType.LOCAL);
            Variable z6 = new Variable("w3", 1, VariableType.LOCAL);
            List<Variable> locals2 = new List<Variable>();
            locals2.Add(z4);
            locals2.Add(z5);
            locals2.Add(z6);
            
            Combination c2 = new Combination(inputs2, locals2);
            
            Assert.AreEqual(c1,c2);
            
            
            
            

        }
        
        
    }
}