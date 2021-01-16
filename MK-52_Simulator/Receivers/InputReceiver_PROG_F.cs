using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using MK52Simulator.Displays;

namespace MK52Simulator.Receivers
{
    //
    // Implements a receiver for F-modified buttons
    //
    public class InputReceiver_PROG_F: RPN_InputReceiver
    {
        public InputReceiver_PROG_F(MK52_Host parent)
            : base( parent)
        {
            Moniker = "PROG_F";
            _displayName = " F ";
        }

        public override string tick(RPN_Button button)
        {
            if (_parent.Program.Counter.isActive)
            {

                if (_parent.Program.Counter.onButton(button, false))
                {
                    _parent.Program.AppendCounterString();
                    _parent.Program.Counter.Increment();
                    _parent.setReceiver("PROG_N");
                }
                return "Nothing";
            }
            switch (button.Moniker)
            {
                case "Func K":
                    _parent.setReceiver("PROG_K");
                    return "Nothing";
                case "Mode":
                    _parent.Program.SetCurrentLine("DEG");
                    _parent.Program.Counter.Increment();
                    _parent.setReceiver("PROG_N");
                    return "Nothing";
                case "Func A":
                    _parent.setReceiver("PROG_A");
                    return "Nothing";

                // Column 1
                case "->":
                    _parent.Program.SetCurrentLine("IFNOT X<0 GOTO ");
                    _parent.Program.Counter.ActivateEntry();
                    return "Nothing";
                case "<-":
                    _parent.Program.SetCurrentLine("IFNOT X==0 GOTO ");
                    _parent.Program.Counter.ActivateEntry();
                    return "Nothing";
                case "B/O":
                    _parent.Program.SetCurrentLine("IFNOT X>=0 GOTO ");
                    _parent.Program.Counter.ActivateEntry();
                    return "Nothing";
                case "S/P":
                    _parent.Program.SetCurrentLine("IFNOT X!=0 GOTO ");
                    _parent.Program.Counter.ActivateEntry();
                    return "Nothing";

                // Column 2
                case "M->X":
                    _parent.Program.SetCurrentLine("WHILE L0>0 GOTO ");
                    _parent.Program.Counter.ActivateEntry();
                    return "Nothing";
                case "X->M":
                    _parent.Program.SetCurrentLine("WHILE L1>0 GOTO ");
                    _parent.Program.Counter.ActivateEntry();
                    return "Nothing";
                case "GOTO":
                    _parent.Program.SetCurrentLine("WHILE L2>0 GOTO ");
                    _parent.Program.Counter.ActivateEntry();
                    return "Nothing";
                case "GOSUB":
                    _parent.Program.SetCurrentLine("WHILE L3>0 GOTO ");
                    _parent.Program.Counter.ActivateEntry();
                    return "Nothing";

                // Column 3
                case "7":
                    _parent.Program.SetCurrentLine("SIN");
                    _parent.Program.Counter.Increment();
                    _parent.setReceiver("PROG_N");
                    return "Nothing";
                case "4":
                    _parent.Program.SetCurrentLine("arcSIN");
                    _parent.Program.Counter.Increment();
                    _parent.setReceiver("PROG_N");
                    return "Nothing";
                case "1":
                    _parent.Program.SetCurrentLine("EXP");
                    _parent.Program.Counter.Increment();
                    _parent.setReceiver("PROG_N");
                    return "Nothing";
                case "0":
                    _parent.Program.SetCurrentLine("10^X");
                    _parent.Program.Counter.Increment();
                    _parent.setReceiver("PROG_N");
                    return "Nothing";

                // Column 4
                case "8":
                    _parent.Program.SetCurrentLine("COS");
                    _parent.Program.Counter.Increment();
                    _parent.setReceiver("PROG_N");
                    return "Nothing";
                case "5":
                    _parent.Program.SetCurrentLine("arcCOS");
                    _parent.Program.Counter.Increment();
                    _parent.setReceiver("PROG_N");
                    return "Nothing";
                case "2":
                    _parent.Program.SetCurrentLine("LG");
                    _parent.Program.Counter.Increment();
                    _parent.setReceiver("PROG_N");
                    return "Nothing";
                case ".":
                    _parent.Program.SetCurrentLine("Rotate");
                    _parent.Program.Counter.Increment();
                    _parent.setReceiver("PROG_N");
                    return "Nothing";

                // Column 5
                case "9":
                    _parent.Program.SetCurrentLine("TG");
                    _parent.Program.Counter.Increment();
                    return "Nothing";
                case "6":
                    _parent.Program.SetCurrentLine("arcTG");
                    _parent.Program.Counter.Increment();
                    _parent.setReceiver("PROG_N");
                    return "Nothing";
                case "3":
                    _parent.Program.SetCurrentLine("LN");
                    _parent.Program.Counter.Increment();
                    _parent.setReceiver("PROG_N");
                    return "Nothing";
                case "/-/":
                    _parent.setReceiver("AUTO_N");
                    return "Nothing";

                // Column 6
                case "-":
                    _parent.Program.SetCurrentLine("SQRT");
                    _parent.Program.Counter.Increment();
                    _parent.setReceiver("PROG_N");
                    return "Nothing";
                case "+":
                    _parent.Program.SetCurrentLine("pi");
                    _parent.Program.Counter.Increment();
                    _parent.setReceiver("PROG_N");
                    return "Nothing";
                case "Swap":
                    _parent.Program.SetCurrentLine("X^Y");
                    _parent.Program.Counter.Increment();
                    _parent.setReceiver("PROG_N");
                    return "Nothing";
                case "EE":
                    _parent.setReceiver("PROG_N");
                    return "Nothing";

                // Column 7
                case "/":
                    _parent.Program.SetCurrentLine("1/X");
                    _parent.Program.Counter.Increment();
                    _parent.setReceiver("PROG_N");
                    return "Nothing";
                case "*":
                    _parent.Program.SetCurrentLine("X^2");
                    _parent.Program.Counter.Increment();
                    _parent.setReceiver("PROG_N");
                    return "Nothing";
                case "Enter":
                    _parent.Program.SetCurrentLine("Bx");
                    _parent.Program.Counter.Increment();
                    _parent.setReceiver("PROG_N");
                    return "Nothing";
                case "Cx":
                    _parent.setReceiver("PROG_N");
                    return "Nothing";
                default:
                    return "Nothing";
            }                
        }
    }
}
