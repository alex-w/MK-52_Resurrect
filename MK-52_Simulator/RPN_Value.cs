using System;
using System.Collections.Generic;
using System.Text;

namespace MK52Simulator
{
    public class RPN_Value
    {
        public const double integerTruncator = 1e-300;
        public const double upperLimit = 1e13;
        public const double lowerLimit = 1e-1;
        public static string largeFormat = "0.000000000000E+000";
        private bool _varTypeInt = true; 
        private Int64 _valueInt = 0L; // in C++ will be a union
        private double _valueReal = 0.0;

        #region Constructors
        public RPN_Value()
        {
        }
        public RPN_Value(int val)
        {
            this.asInt = (Int64)val;
        }
        public RPN_Value(Int64 val)
        {
            this.asInt = val;
        }
        public RPN_Value(double val)
        {
            this.asReal = val;
        }
        public RPN_Value(string val)
        {
            FromString(val);
        }
        public RPN_Value(RPN_Value val)
        {
            FromRPNValue(val);
        }
        #endregion

        /// <summary>
        /// Returns value as long integer
        /// </summary>
        public Int64 asInt
        {
            get
            {
                if (_varTypeInt) return _valueInt;
                if (_valueReal > Int64.MaxValue) return Int64.MaxValue;
                if (_valueReal < Int64.MinValue) return Int64.MinValue;
                return Convert.ToInt64(_valueReal);
            }
            set
            {
                _varTypeInt = true;
                _valueReal = 0.0;
                _valueInt = value;
            }
        }

        /// <summary>
        /// Returns value as double
        /// </summary>
        public double asReal
        {
            get
            {
                if (!_varTypeInt) return _valueReal;
                return Convert.ToDouble(_valueInt);
            }
            set
            {
                _varTypeInt = false;
                _valueReal = value;
                _valueInt = 0L;
                _checkTruncation();
            }
        }

        /// <summary>
        /// Returns true if the value is an integer
        /// </summary>
        public bool isInt
        {
            get
            {
                return _varTypeInt;
            }
        }

        /// <summary>
        /// Returns fractional part of value
        /// </summary>
        public double frac
        {
            get
            {
                if( _varTypeInt) return 0.0;
                return _valueReal - Math.Truncate(_valueReal);
            }
        }

        /// <summary>
        /// Returns rounding error
        /// </summary>
        public double rounding
        {
            get
            {
                if (_varTypeInt) return 0.0;
                return Math.Abs(_valueReal - Math.Truncate(_valueReal));
            }
        }

        /// <summary>
        /// Returns true if the value is out of integer range
        /// </summary>
        public bool isLarge
        {
            get
            {
                if (_varTypeInt) return false;
                if (_valueReal > Int64.MaxValue) return true;
                if (_valueReal < Int64.MinValue) return true;
                return false;
            }
        }

        /// <summary>
        /// Sets value to zero
        /// </summary>
        public void Clear()
        {
            _varTypeInt = true; 
            _valueInt = 0L;
            _valueReal = 0.0;
        }

        /// <summary>
        /// Negates value
        /// </summary>
        public void Negate()
        {
            _valueInt = -_valueInt;
            _valueReal = -_valueReal;
        }

        /// <summary>
        /// Sets value from a string
        /// </summary>
        private void FromString( string val)
        {
            val = val.Trim();
            try
            {
                if (!val.Contains("."))
                {
                    _varTypeInt = true;
                    _valueInt = Convert.ToInt64(val);
                    return;
                }
                _valueReal = Convert.ToDouble(val);
                _varTypeInt = false;
                _checkTruncation();
            }
            catch
            {
                _varTypeInt = true;
                _valueInt = 0L;
                _valueReal = 0.0;
            }
        }

        /// <summary>
        /// Sets value from another value
        /// </summary>
        public void FromRPNValue(RPN_Value val)
        {
            this._valueInt = val._valueInt;
            this._valueReal = val._valueReal;
            this._varTypeInt = val._varTypeInt;
        }

        /// <summary>
        /// Math functions
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static RPN_Value Add(RPN_Value a, RPN_Value b)
        {
            if (a._varTypeInt && b._varTypeInt)
                return new RPN_Value(a.asInt + b.asInt);
            return new RPN_Value(a.asReal + b.asReal);
        }

        public static RPN_Value Subtract(RPN_Value a, RPN_Value b)
        {
            if (a._varTypeInt && b._varTypeInt)
                return new RPN_Value(a.asInt - b.asInt);
            return new RPN_Value(a.asReal - b.asReal);
        }

        public static RPN_Value Multiply(RPN_Value a, RPN_Value b)
        {
            if (a._varTypeInt && b._varTypeInt)
                return new RPN_Value(a.asInt * b.asInt);
            return new RPN_Value(a.asReal * b.asReal);
        }

        public static RPN_Value Divide(RPN_Value a, RPN_Value b)
        {
            if (a._varTypeInt && b._varTypeInt && a._valueInt % b._valueInt == 0)
                return new RPN_Value(a.asInt / b.asInt);
            return new RPN_Value(a.asReal / b.asReal);
        }

        /// <summary>
        /// Converts to string representation
        /// </summary>
        /// <returns>String value</returns>
        public override string ToString()
        {
            if (_varTypeInt) return _valueInt.ToString();
            double av = Math.Abs(_valueReal);
            if (av < lowerLimit || upperLimit < av) return _valueReal.ToString(largeFormat);
            return _valueReal.ToString();
        }

        private void _checkTruncation()
        {
            if( _varTypeInt) return;
            if (rounding > integerTruncator) return;
            if (_valueReal > Int64.MaxValue) return;
            if (_valueReal < Int64.MinValue) return;
            _valueInt = Convert.ToInt64(Math.Round(_valueReal));
            _valueReal = 0.0;
            _varTypeInt = true;
        }
    }
}
