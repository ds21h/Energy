using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Energy {
    internal class MonthTotalLine {
        private CultureInfo mCulture = new CultureInfo("nl-NL");

        MonthTotal mMonthTotal;

        public string xYear {
            get {
                return mMonthTotal.xYear.ToString();
            }   
        }

        public string xMonth {
            get {
                string lResult;

                if (mMonthTotal.xIsTotal) {
                   lResult = "Totaal";
                } else {
                    lResult = mMonthTotal.xMonth.ToString();    
                }
                return lResult;
            }
        }

        public string xDaysInMonth {
            get {
                return mMonthTotal.xDaysInMonth.ToString();
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

        private double sTariff {
            get {
                double lResult;
                if (Data.getInstance.xProviders.xSelectedProvider.xPeriod == Provider.TariffPeriod.Month) {
                    lResult = Data.getInstance.xProviders.xSelectedProvider.xTariff;
                } else {
                    lResult = Data.getInstance.xProviders.xSelectedProvider.xTariff * mMonthTotal.xDaysInMonth;
                }
                return lResult;
            }
        }

        public string xTariff {
            get {
                return sTariff.ToString("###,##0.000", mCulture);
            }
        }

        private double sTaxDiscount {
            get {
                double lTaxDiscount;

                if (Data.getInstance.xSituation.xBusiness) {
                    lTaxDiscount = 0.00;
                } else {
                    lTaxDiscount = (mMonthTotal.xDaysInMonth) * Data.getInstance.xTaxes.xReturn;
                }
                return lTaxDiscount;
            }
        }

        public string xTaxDiscount {
            get {
                return sTaxDiscount.ToString("###,##0.000", mCulture);
            }
        }

        private double sResort {
            get {
                return (mMonthTotal.xDaysInMonth) * Data.getInstance.xResort.xSelectedResortTariff.xPriceDay;
            }
        }

        public string xResort {
            get {
                return sResort.ToString("###,##0.000", mCulture);
            }
        }

        public string xTotalPriceExTVA {
            get {
                return (mMonthTotal.xConsumedPrice + sResort + sTariff + sTaxDiscount - mMonthTotal.xProducedPrice).ToString("###,##0.000", mCulture);
            }
        }

        public string xTotalPriceInTVA {
            get {
                return (((mMonthTotal.xConsumedPrice + sResort + sTariff + sTaxDiscount) * 1.21) - mMonthTotal.xProducedPrice).ToString("###,##0.000", mCulture);
            }
        }

        public bool xIsTotal {
            get {
                return mMonthTotal.xIsTotal;
            }
        }

        internal MonthTotalLine(MonthTotal pMonthTotal) {
            mMonthTotal = pMonthTotal;
        }
    }
}
