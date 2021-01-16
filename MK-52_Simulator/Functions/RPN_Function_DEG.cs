using System;
using System.Collections.Generic;
using System.Text;
using MK52Simulator;

namespace MK52Simulator.Functions
{
    public class RPN_Function_DEG : RPN_Function
    {
        public RPN_Function_DEG( MK52_Host parent):
            base(parent)
        {
            Keyword = "DEG";
            Description = "Set trig mode to Degrees";
        }

        public override void execute(string code)
        {
            _parent.dMode = MK52_Host.dMode_Degrees; 
        }
    }
}
