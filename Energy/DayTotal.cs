using System;
using System.Collections.Generic;
using System.Text;

namespace Energy {
    internal class DayTotal :Total{
        private const int cDayTotal = 99;

        private int mDay;

        internal int xDay {
            get {
                return mDay;
            }
        }

        internal bool xIsTotal {
            get {
                return mDay == cDayTotal;
            }
        }

        internal DayTotal(int pYear, int pMonth) :base(pYear, pMonth){
            mDay = cDayTotal;
        }

        internal DayTotal(int pYear, int pMonth, int pDay) :base(pYear, pMonth) {
            mDay = pDay;
        }

        internal bool xIsCurrent(int pYear, int pMonth) {
            return mYear == pYear && mMonth == pMonth && mDay == cDayTotal;
        }

        internal bool xIsCurrent(int pYear, int pMonth, int pDay) {
            return mYear == pYear && mMonth == pMonth && mDay == pDay;
        }
    }
}
