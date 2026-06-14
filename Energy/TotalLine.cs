using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Energy {
    internal class TotalLine {
        private CultureInfo mCulture = new CultureInfo("nl-NL");

        MonthTotal mMonthTotal;

        public string xYear {
            get {
                return mMonthTotal.xYear.ToString();
            }   
        }

        public string xMonth {
            get {
                return mMonthTotal.xMonth.ToString();
            }
        }

        public string xConsumed {
            get {
                return mMonthTotal.xConsumed.ToString("###,##0.000", mCulture);
            }
        }

        public string xConsumedNet {
            get {
                return mMonthTotal.xConsumedNet.ToString("###,##0.000", mCulture);
            }
        }

        public string xProduced {
            get {
                return mMonthTotal.xProduced.ToString("###,##0.000", mCulture);
            }
        }

        public string xProducedNet {
            get {
                return mMonthTotal.xProducedNet.ToString("###,##0.000", mCulture);
            }
        }

        public string xConsumedPrice {
            get {
                return mMonthTotal.xConsumedPrice.ToString("###,##0.000", mCulture);
            }
        }

        public string xProducedPrice {
            get {
                return mMonthTotal.xProducedPrice.ToString("###,##0.000", mCulture);
            }
        }

        public string xResort {
            get {
                return (DateTime.DaysInMonth(mMonthTotal.xYear, mMonthTotal.xMonth) * Data.getInstance.xResort).ToString("###,##0.000", mCulture);
            }
        }

        public string xTotalPrice {
            get {
                return (mMonthTotal.xConsumedPrice - mMonthTotal.xProducedPrice + (DateTime.DaysInMonth(mMonthTotal.xYear, mMonthTotal.xMonth) * Data.getInstance.xResort)).ToString("###,##0.000", mCulture);
            }
        }
        internal TotalLine(MonthTotal pMonthTotal) {
            mMonthTotal = pMonthTotal;
        }
    }
}
