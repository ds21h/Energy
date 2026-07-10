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
        private pgeTable mPageTable;
        private pgeGraph mPageGraph;
        private pgeProviders mPageProviders;
        private pgeDayTotal mPageDayTotal;
        private pgeMonthTotal mPageMonthTotal;
        private bool mSetSituation;

        public MainWindow() {
            mPageTable = new pgeTable();
            mPageGraph = new pgeGraph();
            mPageProviders = new pgeProviders();
            mPageProviders.ProvidersChanged += hProvidersChanged;
            mPageDayTotal = new pgeDayTotal();
            mPageMonthTotal = new pgeMonthTotal();
            mSetSituation = false;
            InitializeComponent();
            sGetSituations();
            sGetResorts();
            sCalculate();
            sLoadProviders();
            frView.Navigate(mPageTable);
        }

        private void sGetSituations() {
            DirectoryInfo lDirInfo;

            cmbFile.Items.Clear();
            lDirInfo = new DirectoryInfo(Parameters.GetInstance.xDataDir);
            if (lDirInfo.Exists) {
                foreach (DirectoryInfo lSubDir in lDirInfo.GetDirectories()) {
                    cmbFile.Items.Add(lSubDir.Name);
                }
            }
        }

        private void sGetResorts() {
            List<ResortTariff> lResorts;

            cmbResort.Items.Clear();
            lResorts = Data.getInstance.xResort.xResortTariffs;
            foreach (ResortTariff lResort in lResorts) {
                cmbResort.Items.Add(lResort.xConnection);
            }
        }

        private void hProvidersChanged(object? sender, EventArgs e) {
            sCalculate();
            sLoadProviders();
        }

        private void sCalculate() {
            Data.getInstance.xCalculate();
            sRefresh();
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
            bool lFilesOK;

            lLine = Data.getInstance.xExtData.xLastEntry;
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
                    lFilesOK = false;
                    lFileName = sOpendialog("Afname files (Afname*.csv)|Afname*.csv|All files (*.*)|*.*");
                    if (lFileName != null) {
                        lMeterConsumed = new MeterFile(lFileName, lStart.Value, lEnd.Value);
                        if (lMeterConsumed.xFileOK) {
                            lFileName = sOpendialog("Teruglevering files (Terug*.csv)|Terug*.csv|All files (*.*)|*.*");
                            if (lFileName != null) {
                                lMeterProduced = new MeterFile(lFileName, lStart.Value, lEnd.Value);
                                if (lMeterProduced.xFileOK) {
                                    lFileName = sOpendialog("Prijs files (Prijs*.xml)|Prijs*.xml|All files (*.*)|*.*");
                                    if (lFileName != null) {
                                        lPriceFile = new PriceFile(lFileName, lStart.Value, lEnd.Value);
                                        Data.getInstance.xExtData.xImportMeterFile(lMeterConsumed, lMeterProduced, lPriceFile);
                                        Data.getInstance.xExtData.xSaveData();
                                        sRefresh();
                                        lFilesOK = true;
                                    }
                                }
                            }
                        }
                    }
                    if (!lFilesOK) {
                        MessageBox.Show("Onbruikbaar inputbestand!", "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
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
                Data.getInstance.xExtData.xWriteData(lFileName);
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
            Data.getInstance.xProviders.xSaveProviders();
            Data.getInstance.xSituation.xSaveSituation();
        }

        private void btnList_Click(object sender, RoutedEventArgs e) {
            frView.Navigate(mPageTable);
        }

        private void btnGraph_Click(object sender, RoutedEventArgs e) {
            frView.Navigate(mPageGraph);
        }

        private void btnContract_Click(object sender, RoutedEventArgs e) {
            frView.Navigate(mPageProviders);
        }

        private void btnDayTotal_Click(object sender, RoutedEventArgs e) {
            frView.Navigate(mPageDayTotal);
        }

        private void btnMonthTotal_Click(object sender, RoutedEventArgs e) {
            frView.Navigate(mPageMonthTotal);
        }

        private void udBattery_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e) {
            Data.getInstance.xSituation.xBattery = udBattery?.Value ?? 0;
            sCalculate();
        }

        private void sLoadProviders() {
            List<Provider> lProviders;
            string lSelection;
            int lIndex;

            lSelection = string.Empty;
            if (cmbProvider.SelectedIndex >= 0) {
                lSelection = cmbProvider.SelectedItem.ToString() ?? string.Empty;
            }
            lProviders = Data.getInstance.xProviders.xProviders;
            cmbProvider.Items.Clear();
            lIndex = 0;
            foreach (Provider bProvider in lProviders) {
                cmbProvider.Items.Add(bProvider.xLabel);
                if (bProvider.xLabel == lSelection) {
                    lIndex = lProviders.IndexOf(bProvider);
                }
            }
            cmbProvider.SelectedIndex = lIndex;
        }

        private void cmbProvider_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (cmbProvider.SelectedIndex >= 0) {
                Data.getInstance.xProviders.xSelectProvider(cmbProvider.SelectedItem.ToString() ?? string.Empty);
                if (!mSetSituation) {
                    Data.getInstance.xSituation.xProviderLabel = cmbProvider.SelectedItem.ToString() ?? string.Empty;
                    sRefresh();
                }
            }
        }

        private void cmbFile_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (cmbFile.SelectedIndex >= 0) {
                Data.getInstance.xSituation.xSaveSituation();
                Data.getInstance.xLoadSituation(cmbFile.SelectedItem.ToString() ?? string.Empty);
                mSetSituation = true;
                chkBusiness.IsChecked = Data.getInstance.xSituation.xBusiness;
                udBattery.Value = Data.getInstance.xSituation.xBattery;
                foreach (var bItem in cmbProvider.Items) {
                    if (bItem.ToString() == Data.getInstance.xSituation.xProviderLabel) {
                        cmbProvider.SelectedItem = bItem;
                        break;
                    }
                }
                foreach (var bItem in cmbResort.Items) {
                    if (bItem.ToString() == Data.getInstance.xSituation.xConnectionLabel) {
                        cmbResort.SelectedItem = bItem;
                        break;
                    }
                }
                sRefresh();
                mSetSituation = false;
            }
        }

        private void sRefresh() {
            Data.getInstance.xCalculate();
            mPageGraph.xRefresh();
            mPageTable.xRefresh();
            mPageProviders.xRefresh();
            mPageDayTotal.xRefresh();
            mPageMonthTotal.xRefresh();
        }

        private void cmbResort_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (cmbResort.SelectedIndex >= 0) {
                Data.getInstance.xResort.xSelectResortTariff(cmbResort.SelectedItem.ToString() ?? string.Empty);
                if (!mSetSituation) {
                    Data.getInstance.xSituation.xConnectionLabel = cmbResort.SelectedItem.ToString() ?? string.Empty;
                    sRefresh();
                }
            }
        }

        private void chkBusiness_CheckedChanged(object sender, RoutedEventArgs e) {
            Data.getInstance.xSituation.xBusiness = chkBusiness.IsChecked ?? false;
            sRefresh();
        }
    }
}
