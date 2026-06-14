using System;
using System.Collections.Generic;
using System.Text;

namespace Energy {
    internal class MonthTotal {
        private int mYear;
        private int mMonth;
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

        internal MonthTotal(int pYear, int pMonth) {
            mYear = pYear;
            mMonth = pMonth;
            if (pYear == 0 || pMonth == 0) {
                mDaysInMonth = 0;
            } else {
                mDaysInMonth = DateTime.DaysInMonth(pYear, pMonth);
            }
            mConsumed = 0;
            mConsumedNet = 0;
            mConsumedPrice = 0;
            mProduced = 0;
            mProducedNet = 0;
            mProducedPrice = 0;
        }

        internal bool xIsCurrent(int pYear, int pMonth) {
            return mYear == pYear && mMonth == pMonth;
        }

        internal void xAddLine(ProviderLine pLine) {
            mConsumed += pLine.xConsumed;
            mConsumedNet += pLine.xNetConsumed;
            mConsumedPrice += pLine.xConsumedPrice;
            mProduced += pLine.xProduced;
            mProducedNet += pLine.xNetProduced;
            mProducedPrice += pLine.xProducedPrice;
        }
    }
}
