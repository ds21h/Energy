using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Energy {
    internal class DayTotalLine {
        private CultureInfo mCulture = new CultureInfo("nl-NL");

        DayTotal mDayTotal;

        public string xYear {
            get {
                return mDayTotal.xYear.ToString();
            }
        }

        public string xMonth {
            get {
                return mDayTotal.xMonth.ToString();
            }
        }

        public string xDay {
            get {
                return mDayTotal.xDay.ToString();
            }
        }

        public string xConsumed {
            get {
                return mDayTotal.xConsumed.ToString("###,##0.000", mCulture);
            }
        }

        public string xConsumedNet {
            get {
                return mDayTotal.xConsumedNet.ToString("###,##0.000", mCulture);
            }
        }

        public string xProduced {
            get {
                return mDayTotal.xProduced.ToString("###,##0.000", mCulture);
            }
        }

        public string xProducedNet {
            get {
                return mDayTotal.xProducedNet.ToString("###,##0.000", mCulture);
            }
        }

        public string xConsumedPrice {
            get {
                return mDayTotal.xConsumedPrice.ToString("###,##0.000", mCulture);
            }
        }

        public string xProducedPrice {
            get {
                return mDayTotal.xProducedPrice.ToString("###,##0.000", mCulture);
            }
        }

        public string xTotalPriceExVAT {
            get {
                return (mDayTotal.xConsumedPrice - mDayTotal.xProducedPrice).ToString("###,##0.000", mCulture);
            }
        }

        private double sTotalPriceInVAT {
            get {
                return (mDayTotal.xConsumedPrice * 1.21) - mDayTotal.xProducedPrice;
                ;
            }
        }

        public string xTotalPriceInVAT {
            get {
                return sTotalPriceInVAT.ToString("###,##0.000", mCulture);
            }
        }

        public string xPriceKWh {
            get {
                return (sTotalPriceInVAT / (mDayTotal.xConsumedNet - mDayTotal.xProducedNet)).ToString("###,##0.000", mCulture);
            }
        }

        internal DayTotalLine(DayTotal pDayTotal) {
            mDayTotal = pDayTotal;
        }
    }
}
