using System;
using System.Collections.Generic;
using System.Text;
using MK52Simulator;

namespace MK52Simulator.Functions
{
    public class RPN_Function_LblT : RPN_Function
    {
        public RPN_Function_LblT( MK52_Host parent):
            base(parent)
        {
            Keyword = "LT>";
            Description = "Puts value into T Label";
        }

        public override void execute(string code)
        {
            string tmp = code.Substring(Keyword.Length).Trim();
            _parent.CalcStack.T_Label = (tmp.Length==0)? "T:" : tmp;
        }
    }
}
