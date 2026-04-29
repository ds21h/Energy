using Microsoft.Win32;
using System.IO;
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
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow: Window {
        private List<double> mChartData;

        public MainWindow() {
            int lIndex;

            InitializeComponent();
            lstIntervals.ItemsSource = Data.getInstance.xLines;
            lstIntervals.InvalidateVisual();
            mChartData = new List<double>();
            for (lIndex = 0; lIndex < 600; lIndex++) {
                mChartData.Add(Data.getInstance.xLines[lIndex].xConsumed);
            }
            clmValues.Values = mChartData;
        }

        private string? sOpendialog(string pFilter) {
            OpenFileDialog lDialog;
            string? lFileName = null;

            lDialog = new OpenFileDialog();
            lDialog.Filter = pFilter;
            lDialog.Multiselect = false;
            if (lDialog.ShowDialog() == true) {
                lFileName = lDialog.FileName;
            }
            return lFileName;
        }

        private void mnuLoad_Click(object sender, RoutedEventArgs e) {
            string? lFileName;
            MeterFile lMeterConsumed;
            MeterFile lMeterProduced;
            PriceFile lPriceFile;
            frmLoad lFrmLoad;
            DateTime? lStart;
            DateTime? lEnd;
            DataLine? lLine;
            bool? lFrmLoadResult;

            lLine = Data.getInstance.xLastEntry;
            if (lLine == null) {
                lStart = null;
                lEnd = null;
            } else {
                lStart = lLine.xTimeStampLocal.AddDays(1);
                lEnd = DateTime.Now;
            }
            lFrmLoad = new frmLoad(lStart, lEnd);
            lFrmLoadResult = lFrmLoad.ShowDialog();
            if (lFrmLoadResult.GetValueOrDefault()) {
                lStart = lFrmLoad.xStart;
                lEnd = lFrmLoad.xEnd;
                if (lStart != null && lEnd != null) {
                    lFileName = sOpendialog("Afname files (Afname*.csv)|Afname*.csv|All files (*.*)|*.*");
                    if (lFileName != null) {
                        lMeterConsumed = new MeterFile(lFileName, lStart.Value, lEnd.Value);
                        lFileName = sOpendialog("Teruglevering files (Terug*.csv)|Terug*.csv|All files (*.*)|*.*");
                        if (lFileName != null) {
                            lMeterProduced = new MeterFile(lFileName, lStart.Value, lEnd.Value);
                            lFileName = sOpendialog("Prijs files (Prijs*.xml)|Prijs*.xml|All files (*.*)|*.*");
                            if (lFileName != null) {
                                lPriceFile = new PriceFile(lFileName, lStart.Value, lEnd.Value);
                                Data.getInstance.xImportMeterFile(lMeterConsumed, lMeterProduced, lPriceFile);
                                lstIntervals.ItemsSource = Data.getInstance.xLines;
                                lstIntervals.InvalidateVisual();
                            }
                        }
                    }
                }
            }
            lFrmLoad.Close();

        }

        private void mnuExport_Click(object sender, RoutedEventArgs e) {
            SaveFileDialog lDialog;
            string? lFileName = null;

            lDialog = new SaveFileDialog();
            lDialog.DefaultExt = ".csv";
            lDialog.Filter = "csv files (*.csv)|*.csv|All files (*.*)|*.*";
            lDialog.AddExtension = true;
            if (lDialog.ShowDialog() == true) {
                lFileName = lDialog.FileName;
                Data.getInstance.xWriteData(lFileName);
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
            Data.getInstance.xSaveData();
        }
    }
}
