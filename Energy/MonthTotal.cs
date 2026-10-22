using System;
using System.Collections.Generic;
using System.Text;

namespace Energy {
    internal class MonthTotal :Total {
        private const int cMonthTotal = 99;

        private int mLastDay;
        private int mDaysInMonth;

        internal int xDaysInMonth {
            get {
                return mDaysInMonth;
            }
        }

        internal bool xIsTotal {
            get {
                return mMonth == cMonthTotal;
            }
        }

        internal MonthTotal(int pYear) :base(pYear, cMonthTotal) {
            mLastDay = 0;
            mDaysInMonth = 0;
        }

        internal MonthTotal(int pYear, int pMonth) :base(pYear, pMonth) {
            mLastDay = 0;
            mDaysInMonth = 0;
        }

        internal bool xIsCurrent(int pYear) {
            return mYear == pYear && mMonth == cMonthTotal;
        }

        internal bool xIsCurrent(int pYear, int pMonth) {
            return mYear == pYear && mMonth == pMonth;
        }

        new internal void xAddLine(DisplayLine pLine) {
            if (pLine.xTimeStampLocal.Day != mLastDay) {
                mLastDay = pLine.xTimeStampLocal.Day;
                mDaysInMonth++;
            }
            base.xAddLine(pLine);
        }
    }
}
