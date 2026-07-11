using System;
using System.Collections.Generic;
using System.Text;

namespace Energy {
    internal class MonthTotal {
        private const int cMonthTotal = 99;

        private int mYear;
        private int mMonth;
        private int mLastDay;
        private int mDaysInMonth;
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

        internal int xDaysInMonth {
            get {
                return mDaysInMonth;
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
                return mMonth == cMonthTotal;
            }
        }

        internal MonthTotal(int pYear) {
            mYear = pYear;
            mMonth = cMonthTotal;
            mLastDay = 0;
            mDaysInMonth = 0;
            mConsumed = 0;
            mConsumedNet = 0;
            mConsumedPrice = 0;
            mProduced = 0;
            mProducedNet = 0;
            mProducedPrice = 0;
        }

        internal MonthTotal(int pYear, int pMonth) {
            mYear = pYear;
            mMonth = pMonth;
            mLastDay = 0;
            mDaysInMonth = 0;
            mConsumed = 0;
            mConsumedNet = 0;
            mConsumedPrice = 0;
            mProduced = 0;
            mProducedNet = 0;
            mProducedPrice = 0;
        }

        internal bool xIsCurrent(int pYear) {
            return mYear == pYear && mMonth == cMonthTotal;
        }

        internal bool xIsCurrent(int pYear, int pMonth) {
            return mYear == pYear && mMonth == pMonth;
        }

        internal void xAddLine(DisplayLine pLine) {
            if (pLine.xTimeStampLocal.Day != mLastDay) {
                mLastDay = pLine.xTimeStampLocal.Day;
                mDaysInMonth++;
            }
            mConsumed += pLine.xConsumed;
            mConsumedNet += pLine.xNetConsumed;
            mConsumedPrice += pLine.xConsumedPrice;
            mProduced += pLine.xProduced;
            mProducedNet += pLine.xNetProduced;
            mProducedPrice += pLine.xProducedPrice;
        }
    }
}
