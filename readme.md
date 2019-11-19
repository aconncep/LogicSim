[![Build Status](https://austincepalia.visualstudio.com/LogicSim/_apis/build/status/LogicSim-.NET%20Desktop-CI?branchName=master)](https://austincepalia.visualstudio.com/LogicSim/_build/latest?definitionId=1&branchName=master)


# What is LogicSim?

LogicSim is an educational tool used to simulate combinational logic circuits. The software uses a simplified HDL (Hardware Description Language) to interpret circuits composed of various logic gates. LogicSim is written in .NET Core for cross-platform compatibility.

LogicSim is primarily used in the classroom as a tool to quickly simulate student-created logic circuits. It's been used to teach basic robotics engineering to middle-school summer camp students at Rochester Institute of Technology. It's also been used it as an example of object-oriented programming.


# Usage
LogicSim [path_to_file] [auto/manual] [verbose/simple]

# Features

**Modes**:
- *Automatic*: In this mode, the software simulates every possible input combination automatically. For example, if the HDL specifies two inputs, the software simulates 00, 01, 10, and 11, displaying 0 or 1 for each output. 

- *Manual*: In this mode, the user enters a binary value for each input manually, and then the software simulates the circuit with those inputs and displays the output. This runs indefinitely until the user quits with 'x'.

- The software also supports a simplified or verbose output, which is achieved by inputting [*simple/verbose*] for the last program argument. Simple mode draws a table for the output, whereas verbose mode includes information about the variable types (input/output). Manual simple mode is not supported - the software will automatically switch to manual verbose instead.

# HDL File
The Hardware Design Language (HDL) file is simply a text file that contains the instructions LogicSim uses to interpret the circuit. It supports seven logic operations:

- NOT
- AND
- OR
- XOR
- XNOR
- NOR
- NAND

Each operation is called a command and is called as if it were a function. Every command takes two input arguments, with the exception of NOT which only takes one. 

Each file requires the presence of an INPUTS and an OUTPUT line (LogicSim will warn you if one is missing). INPUTS defines the input variables, and OUTPUT defines the variables that should be displayed as output. Comments are also supported with #, either on their own line or inline (like Python).

The program now supports commands nested within one another. For example, `AND(NOR(1,0),XOR(0,1))`. You can also store the output of each command in a new variable (one per line).

# Example Usage

    # Test program circuit.txt
    INPUTS A,B  
    w1 = AND(A,B)  
    w2 = NOT(w1) # inline comment
    w3 = NAND(w2,B)
    OUTPUT w1,w2

Running with ./LogicSim circuit.txt auto simple

    Warning: Local variable [w3] is assigned but never used
    Simulating circuit...
    A B  | w1 w2 
    0 0  | 0  1   
    0 1  | 0  1   
    1 0  | 0  1   
    1 1  | 1  0   

Running with ./LogicSim circuit.txt manual verbose

    Warning: Local variable [w3] is assigned but never used
    Enter value for input variable A [or x to quit]:  0
    Enter value for input variable B [or x to quit]:  1
    
    Output variable w1: 0  Output variable w2: 1  
    Enter value for input variable A [or x to quit]:  x
    Exiting...


 
  

