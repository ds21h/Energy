using System;
using System.Collections.Generic;
using System.Text;

namespace Energy {
    internal class DayTotal {
        private const int cDayTotal = 99;

        private int mYear;
        private int mMonth;
        private int mDay;
        private double mConsumed;
        private double mConsumedNet;
        private double mConsumedPrice;
        private double mProduced;
        private double mProducedNet;
        private double mProducedPrice;

        internal int xYear {
            get {
                return mYear;
            }
        }

        internal int xMonth {
            get {
                return mMonth;
            }
        }

        internal int xDay {
            get {
                return mDay;
            }
        }

        internal double xConsumed {
            get {
                return mConsumed;
            }
        }

        internal double xConsumedNet {
            get {
                return mConsumedNet;
            }
        }

        internal double xProduced {
            get {
                return mProduced;
            }
        }

        internal double xProducedNet {
            get {
                return mProducedNet;
            }
        }

        internal double xConsumedPrice {
            get {
                return mConsumedPrice;
            }
        }

        internal double xProducedPrice {
            get {
                return mProducedPrice;
            }
        }

        internal bool xIsTotal {
            get {
                return mDay == cDayTotal;
            }
        }

        internal DayTotal(int pYear, int pMonth) {
            mYear = pYear;
            mMonth = pMonth;
            mDay = cDayTotal;
            mConsumed = 0;
            mConsumedNet = 0;
            mConsumedPrice = 0;
            mProduced = 0;
            mProducedNet = 0;
            mProducedPrice = 0;
        }

        internal DayTotal(int pYear, int pMonth, int pDay) {
            mYear = pYear;
            mMonth = pMonth;
            mDay = pDay;
            mConsumed = 0;
            mConsumedNet = 0;
            mConsumedPrice = 0;
            mProduced = 0;
            mProducedNet = 0;
            mProducedPrice = 0;
        }

        internal bool xIsCurrent(int pYear, int pMonth) {
            return mYear == pYear && mMonth == pMonth && mDay == cDayTotal;
        }

        internal bool xIsCurrent(int pYear, int pMonth, int pDay) {
            return mYear == pYear && mMonth == pMonth && mDay == pDay;
        }

        internal void xAddLine(DisplayLine pLine) {
            mConsumed += pLine.xConsumed;
            mConsumedNet += pLine.xNetConsumed;
            mConsumedPrice += pLine.xConsumedPrice;
            mProduced += pLine.xProduced;
            mProducedNet += pLine.xNetProduced;
            mProducedPrice += pLine.xProducedPrice;
        }
    }
}
