using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Automation.Provider;
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
    /// Interaction logic for pgeConnection.xaml
    /// </summary>
    public partial class pgeConnection: Page {
        private enum eStatus {
            None,
            New,
            Edit,
            Delete
        }
        private ObservableCollection<ResortTariff> mResortTariffs;
        private ResortTariff? mSelectedConnection;
        private eStatus mStatus;

        internal event EventHandler<EventArgs>? ResortsChanged;

        public pgeConnection() {
            InitializeComponent();
            mStatus = eStatus.None;
            mResortTariffs = new ObservableCollection<ResortTariff>();
            lstConnections.DataContext = mResortTariffs;
            sLoadTariffList();
            if (mResortTariffs.Count > 0) {
                lstConnections.SelectedItem = mResortTariffs[0];
            }
        }

        internal void xRefresh() {
            sLoadTariffList();
            if (mResortTariffs.Count > 0) {
                if (mSelectedConnection == null) {
                    lstConnections.SelectedItem = mResortTariffs[0];
                } else {
                    foreach (ResortTariff bTariff in mResortTariffs) {
                        if (bTariff == mSelectedConnection) {
                            lstConnections.SelectedItem = bTariff;
                            break;
                        }
                    }
                }
            }
        }

        private void sPostResortsChanged() {
            EventHandler<EventArgs>? lHandler;

            lHandler = ResortsChanged;
            if (lHandler != null) {
                lHandler.Invoke(this, EventArgs.Empty);
            }
        }

        private void sLoadTariffList() {
            List<ResortTariff> lTariffs;

            lTariffs = Data.getInstance.xResort.xResortTariffs;
            mResortTariffs.Clear();
            foreach (ResortTariff bProvider in lTariffs) {
                mResortTariffs.Add(bProvider);
            }
        }

        private void btnNew_Click(object sender, RoutedEventArgs e) {
            mStatus = eStatus.New;
            sEnableId();
            sEnableDetails();
            rdoFasen3.IsChecked = true;
            txtMax.Text = "";
            txtYear.Text = "";
            txtDay.Text = "";
            btnDelete.Visibility = Visibility.Collapsed;
            txtMax.Focus();
        }

        private void btnOK_Click(object sender, RoutedEventArgs e) {
            ResortTariff lTariff;
            int lFases;

            if (sCheckInput()) {
                if (mStatus == eStatus.Edit) {
                    mSelectedConnection!.xPriceYear = double.Parse(txtYear.Text);
                    mSelectedConnection!.xPriceDay = double.Parse(txtDay.Text);
                    Data.getInstance.xResort.xResortChanged = true;
                    sPostResortsChanged();
                } else {
                    if (mStatus == eStatus.New) {
                        if (rdoFasen3.IsChecked == true) {
                            lFases = 3;
                        } else {
                            lFases = 1;
                        }
                        lTariff = new ResortTariff(lFases, int.Parse(txtMax.Text), double.Parse(txtDay.Text), double.Parse(txtYear.Text));
                        Data.getInstance.xResort.xAddResortTariff(lTariff);
                        sPostResortsChanged();
                        sLoadTariffList();
                        foreach (ResortTariff bTariff in mResortTariffs) {
                            if (bTariff == lTariff) {
                                mSelectedConnection = bTariff;
                                lstConnections.SelectedItem = bTariff;
                                break;
                            }
                        }
                    }
                }
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e) {
            Data.getInstance.xResort.xRemoveResortTariff(mSelectedConnection!);
            sLoadTariffList();
            sPostResortsChanged();
            if (mResortTariffs.Count > 0) {
                lstConnections.SelectedItem = mResortTariffs[0];
            }
        }

        private bool sCheckInput() {
            double lValue;
            bool lResult;
            int lResultInt;

            lResult = true;
            if (mStatus == eStatus.New) {
                txtMax.Background = Brushes.White;
                if (int.TryParse(txtMax.Text, out lResultInt)) {
                    if (lResultInt < 6 || lResultInt > 90) {
                        txtMax.Background = Brushes.Red;
                        lResult = false;
                    }
                } else {
                    txtMax.Background = Brushes.Red;
                    lResult = false;
                }
            }

            txtYear.Background = Brushes.White;
            if (double.TryParse(txtYear.Text, out lValue)) {
                if (lValue < 30d) {
                    txtYear.Background = Brushes.Red;
                    lResult = false;
                }
            } else {
                txtYear.Background = Brushes.Red;
                lResult = false;
            }

            if (double.TryParse(txtDay.Text, out lValue)) {
                if (lValue < 0.1d) {
                    txtDay.Background = Brushes.Red;
                    lResult = false;
                } else {
                    txtDay.Background = Brushes.White;
                }
            } else {
                txtDay.Background = Brushes.Red;
                lResult = false;
            }
            return lResult;
        }

        private void txtBox_LostFocus(object sender, RoutedEventArgs e) {
            sCheckInput();
        }

        private void lstProviders_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            mSelectedConnection = lstConnections.SelectedItem as ResortTariff;
            sFillResortDetails();
        }

        private void sFillResortDetails() {
            sDisableAll();
            if (mSelectedConnection != null) {
                mStatus = eStatus.Edit;
                sEnableDetails();
                if (mSelectedConnection.xFases == 3) {
                    rdoFasen3.IsChecked = true;
                } else {
                    rdoFasen1.IsChecked = true;
                }
                txtMax.Text = mSelectedConnection.xMax.ToString();
                txtYear.Text = mSelectedConnection.xPriceYear.ToString();
                txtDay.Text = mSelectedConnection.xPriceDay.ToString();
                btnDelete.Visibility = Visibility.Visible;
                txtYear.Focus();
            } else {
                mStatus = eStatus.None;
                txtMax.Text = "";
                txtYear.Text = "";
                txtDay.Text = "";
                btnDelete.Visibility = Visibility.Collapsed;
            }
        }

        private void sDisableAll() {
            rdoFasen1.IsEnabled = false;
            rdoFasen3.IsEnabled = false;
            txtMax.IsEnabled = false;
            txtMax.Background = Brushes.LightGray;
            txtYear.IsEnabled = false;
            txtYear.Background = Brushes.LightGray;
            txtDay.IsEnabled = false;
            txtDay.Background = Brushes.LightGray;
        }

        private void sEnableId() {
            rdoFasen1.IsEnabled = true;
            rdoFasen3.IsEnabled = true;
            txtMax.IsEnabled = true;
            txtMax.Background = Brushes.White;
        }

        private void sEnableDetails() {
            txtYear.IsEnabled = true;
            txtYear.Background = Brushes.White;
            txtDay.IsEnabled = true;
            txtDay.Background = Brushes.White;
        }
    }
}
