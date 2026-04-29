using System;
using System.Collections.Generic;
using System.Text;

namespace Energy {
    internal class ChartLine {
        private double mValue;

        public double xValue {
            get {
                return mValue;
            }
            set {
                mValue = value;
            }
        }

        internal ChartLine(double pValue) {
            mValue = pValue;
        }
    }
}
