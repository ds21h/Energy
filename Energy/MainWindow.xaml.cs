using Microsoft.Win32;
using System.IO;
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
        private DateTime mStart;
        private DateTime mEnd;

        public MainWindow() {
            InitializeComponent();
            mEnd = DateTime.SpecifyKind(DateTime.Now.AddDays(-1).ToUniversalTime(), DateTimeKind.Utc);
            mStart = DateTime.SpecifyKind(mEnd.AddDays(-7), DateTimeKind.Utc);
        }

        private void btnExport_Click(object sender, RoutedEventArgs e) {
            Data.getInstance.xWriteData();
        }

        private void btnImport_Click(object sender, RoutedEventArgs e) {
            string? lFileName;
            MeterFile lMeterConsumed;
            MeterFile lMeterProduced;
            PriceFile lPriceFile;

            lFileName = sOpendialog("Afname files (Afname*.csv)|Afname*.csv|All files (*.*)|*.*");
            if (lFileName != null) {
                lMeterConsumed = new MeterFile(lFileName, mStart, mEnd);
                lFileName = sOpendialog("Teruglevering files (Terug*.csv)|Terug*.csv|All files (*.*)|*.*");
                if (lFileName != null) {
                    lMeterProduced = new MeterFile(lFileName, mStart, mEnd);
                    lFileName = sOpendialog("Prijs files (Prijs*.xml)|Prijs*.xml|All files (*.*)|*.*");
                    if (lFileName != null) {
                        lPriceFile = new PriceFile(lFileName, mStart, mEnd);
                        Data.getInstance.xImportMeterFile(lMeterConsumed, lMeterProduced, lPriceFile);
                        lstIntervals.ItemsSource = Data.getInstance.xLines;
                        lstIntervals.InvalidateVisual();
                    }
                }
            }
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

        private void dpStart_SelectedDateChanged(object sender, SelectionChangedEventArgs e) {
            DateTime? lSelection;

            lSelection = dpStart.SelectedDate;
            if (lSelection != null) {
                mStart = DateTime.SpecifyKind((DateTime)lSelection.Value.ToUniversalTime(), DateTimeKind.Utc);
            }
        }

        private void dpEnd_SelectedDateChanged(object sender, SelectionChangedEventArgs e) {
            DateTime? lSelection;

            lSelection = dpEnd.SelectedDate;
            if (lSelection != null) {
                mEnd = DateTime.SpecifyKind((DateTime)lSelection.Value.AddDays(1).ToUniversalTime(), DateTimeKind.Utc);
            }
        }
    }
}
