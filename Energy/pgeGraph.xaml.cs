using LiveChartsCore.SkiaSharpView;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Energy {

    public partial class pgeGraph: Page {

        private ObservableCollection<DataLine> mChartData;
        private List<Axis> mXAxes;
        private int mIndexStart;
        private int mPeriod;

        public pgeGraph() {
            mChartData = new ObservableCollection<DataLine>();
            InitializeComponent();
            mXAxes = new List<Axis> {
                new Axis {
                    IsVisible = false
                }
            };
            crtKWh.XAxes = mXAxes;
            crtEuros.XAxes = mXAxes;
            clmAfnKWh.XToolTipLabelFormatter = sXFormatter;
            clmAfnKWh.Values = mChartData;
            clmAfnKWh.Mapping = (pObject, pIndex) => pObject is DataLine pDataLine
                ? new LiveChartsCore.Kernel.Coordinate(pIndex, pDataLine.xNetConsumed)
                : LiveChartsCore.Kernel.Coordinate.Empty;
            clmLevKWh.Values = mChartData;
            clmLevKWh.Mapping = (pObject, pIndex) => pObject is DataLine pDataLine
                ? new LiveChartsCore.Kernel.Coordinate(pIndex, -pDataLine.xNetProduced)
                : LiveChartsCore.Kernel.Coordinate.Empty;
            clmAfnEur.XToolTipLabelFormatter = sXFormatter;
            clmAfnEur.Values = mChartData;
            clmAfnEur.Mapping = (pObject, pIndex) => pObject is DataLine pDataLine
                ? new LiveChartsCore.Kernel.Coordinate(pIndex, pDataLine.xConsumedPrice)
                : LiveChartsCore.Kernel.Coordinate.Empty;
            clmLevEur.Values = mChartData;
            clmLevEur.Mapping = (pObject, pIndex) => pObject is DataLine pDataLine
                ? new LiveChartsCore.Kernel.Coordinate(pIndex, -pDataLine.xProducedPrice)
                : LiveChartsCore.Kernel.Coordinate.Empty;
            clmBatt.Values = mChartData;
            clmBatt.Mapping = (pObject, pIndex) => pObject is DataLine pDataLine
                ? new LiveChartsCore.Kernel.Coordinate(pIndex, pDataLine.xBattery)
                : LiveChartsCore.Kernel.Coordinate.Empty;
            mIndexStart = 0;
        }

        private string sXFormatter(LiveChartsCore.Kernel.ChartPoint pValue) {
            int lIndex;
            string lResult;

            lIndex = (int)pValue.Index;
            if (lIndex < 0 || lIndex >= mChartData.Count) {
                lResult = string.Empty;
            } else {
                lResult = mChartData[lIndex].xTimeStampLocal.ToString("dd-MM-yyyy HH:mm");
            }
            return lResult;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e) {
            DataLine? lLastLine;

            lLastLine = Data.getInstance.xLastEntry;
            if (lLastLine != null) {
                dpStart.SelectedDate = lLastLine.xTimeStampLocal.AddDays(-6);
                sSetPeriod();
                sFillChart();
            } else {
                mIndexStart = 0;
                mPeriod = 0;
            }
        }

        internal void xRefresh() {
            sFillChart();
        }

        private void sSetPeriod() {
            DateTime? lStart;
            DateTime lWork;
            int lPeriod;

            lStart = dpStart.SelectedDate;
            if (lStart != null) {
                lWork = lStart.Value.ToUniversalTime();

                switch (cmbPeriod.SelectedIndex) {
                    case 0:
                        lPeriod = 1;
                        break;
                    case 1:
                        lPeriod = 3;
                        break;
                    case 2:
                        lPeriod = 7;
                        break;
                    case 3:
                        lPeriod = 14;
                        break;
                    case 4:
                        lPeriod = (lWork.AddMonths(1) - lWork).Days;
                        break;
                    default:
                        lPeriod = 0;
                        break;
                }

                mIndexStart = (int)(lWork - Data.getInstance.xLines[0].xTimeStampUTC).TotalMinutes / 15;
                mPeriod = lPeriod * 24 * 4;
            }
        }

        private void sFillChart() {
            int lIndex;
            int lIndexData;

            mChartData.Clear();

            for (lIndex = 0; lIndex <= mPeriod; lIndex++) {
                lIndexData = mIndexStart + lIndex;
                if (lIndexData >= 0 && lIndexData < Data.getInstance.xLines.Count) {
                    mChartData.Add(Data.getInstance.xLines[lIndexData]);
                }
            }
        }

        private void dpStart_SelectedDateChanged(object sender, SelectionChangedEventArgs e) {
            sSetPeriod();
            sFillChart();
        }

        private void cmbPeriod_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            sSetPeriod();
            sFillChart();
        }
    }
}
