using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Shapes;

namespace Energy {
    internal class DisplayData {
        private List<DisplayLine> mDisplayLines = new List<DisplayLine>();
        private List<MonthTotal> mMonthTotals = new List<MonthTotal>();
        private List<DayTotal> mDayTotals = new List<DayTotal>();

        internal List<DisplayLine> xDisplayLines {
            get {
                return mDisplayLines;
            }
        }

        internal List<MonthTotal> xMonthTotals {
            get {
                return mMonthTotals;
            }
        }

        internal List<DayTotal> xDayTotals {
            get {
                return mDayTotals;
            }
        }

        internal DisplayData() {
            sProcessLines();
        }

        private void sProcessLines() {
            List<DataLine> lDataLines;
            int lYear;
            int lMonth;
            int lDay;
            MonthTotal lMonthTotal = new MonthTotal(0, 0);
            MonthTotal lMonthTotalYear;
            DayTotal lDayTotal = new DayTotal(0, 0, 0);
            DayTotal lDayTotalMonth;
            DisplayLine lDisplayLine;
            double lLastBattery;
            int lMaxBattery;

            lDataLines = Data.getInstance.xExtData.xLines;
            lLastBattery = 0;
            lMaxBattery = Data.getInstance.xSituations.xSelectedSituation.xBattery;
            mMonthTotals.Clear();
            mDayTotals.Clear();
            if (lDataLines.Count > 0) {
                lYear = lDataLines[0].xTimeStampLocal.Year;
                lMonth = lDataLines[0].xTimeStampLocal.Month;
                lMonthTotalYear = new MonthTotal(lYear);    
                lDayTotalMonth = new DayTotal(lYear, lMonth);
                mDisplayLines = new List<DisplayLine>(lDataLines.Count);
                foreach (DataLine lDataLine in lDataLines) {
                    lYear = lDataLine.xTimeStampLocal.Year;
                    lMonth = lDataLine.xTimeStampLocal.Month;
                    lDay = lDataLine.xTimeStampLocal.Day;
                    if (!lMonthTotal.xIsCurrent(lYear, lMonth)) {
                        if (!lMonthTotalYear.xIsCurrent(lYear)) {
                            mMonthTotals.Add(lMonthTotal);
                            lMonthTotal = new MonthTotal(lYear);
                        }
                        lMonthTotal = new MonthTotal(lYear, lMonth);
                        mMonthTotals.Add(lMonthTotal);
                    }
                    if (!lDayTotal.xIsCurrent(lYear, lMonth, lDay)) {
                        if (!lDayTotalMonth.xIsCurrent(lYear, lMonth)) {
                            mDayTotals.Add(lDayTotalMonth);
                            lDayTotalMonth = new DayTotal(lYear, lMonth);
                        }
                        lDayTotal = new DayTotal(lYear, lMonth, lDay);
                        mDayTotals.Add(lDayTotal);
                    }
                    lDisplayLine = new DisplayLine(lDataLine, lMaxBattery, lLastBattery);
                    lLastBattery = lDisplayLine.xBattery;
                    mDisplayLines.Add(lDisplayLine);
                    lMonthTotal.xAddLine(lDisplayLine);
                    lMonthTotalYear.xAddLine(lDisplayLine);
                    lDayTotal.xAddLine(lDisplayLine);
                    lDayTotalMonth.xAddLine(lDisplayLine);
                }
                mDayTotals.Add(lDayTotalMonth);
                mMonthTotals.Add(lMonthTotalYear);
            }
        }
    }
}
