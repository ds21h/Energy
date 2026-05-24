using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for pgeProviders.xaml
    /// </summary>
    public partial class pgeProviders: Page {
        private enum eStatus {
            None,
            New,
            Edit,
            Delete
        }
        private ObservableCollection<Provider> mProviders;
        private Provider? mSelectedProvider;
        private eStatus mStatus;

        public pgeProviders() {
            InitializeComponent();
            mStatus = eStatus.None;
            mProviders = new ObservableCollection<Provider>();
            lstProviders.DataContext = mProviders;
            sLoadProviderList();
            if (mProviders.Count  > 0) {
                lstProviders.SelectedItem = mProviders[0];
            }
        }

        private void sLoadProviderList() {
            List<Provider> lProviders;

            lProviders = Data.getInstance.xProviders;
            mProviders.Clear();
            foreach (Provider bProvider in lProviders) {
                mProviders.Add(bProvider);
            }
        }

        private void btnNew_Click(object sender, RoutedEventArgs e) {
            mStatus = eStatus.New;
            sEnableId();
            sEnableDetails();
            txtProvider.Text = "";
            txtVariant.Text = "";
            txtFee.Text = "";
            txtConsumption.Text = "";
            txtPrice.Text = "";
            txtReturn.Text = "";
            txtReturnPrice.Text = "";
            btnDelete.Visibility = Visibility.Collapsed;
            txtProvider.Focus();
        }

        private void btnOK_Click(object sender, RoutedEventArgs e) {
            Provider lProvider;

            if (sCheckInput()) {
                if (mStatus == eStatus.Edit) {
                    mSelectedProvider!.xMonthlyTariff = double.Parse(txtFee.Text);
                    mSelectedProvider!.xConsumedFixedPrice = double.Parse(txtConsumption.Text);
                    mSelectedProvider!.xConsumedExtra = double.Parse(txtPrice.Text);
                    mSelectedProvider!.xProducedFixedPrice = double.Parse(txtReturn.Text);
                    mSelectedProvider!.xProducedExtra = double.Parse(txtReturnPrice.Text);
                    Data.getInstance.xProvidersChanged = true;
                } else {
                    if (mStatus == eStatus.New) {
                        lProvider = new Provider(txtProvider.Text, txtVariant.Text, double.Parse(txtFee.Text), double.Parse(txtConsumption.Text), double.Parse(txtPrice.Text), double.Parse(txtReturn.Text), double.Parse(txtReturnPrice.Text));
                        Data.getInstance.xAddProvider(lProvider);
                        sLoadProviderList();
                        foreach (Provider bProvider in mProviders) {
                            if (bProvider == lProvider) {
                                mSelectedProvider = bProvider;
                                lstProviders.SelectedItem = bProvider;
                                break;
                            }
                        }
                    }
                }
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e) {
            Data.getInstance.xDeleteProvider(mSelectedProvider!);
            sLoadProviderList();
            if (mProviders.Count > 0) {
                lstProviders.SelectedItem = mProviders[0];
            }
        }

        private bool sCheckInput() {
            double lValue;
            bool lResult;

            lResult = true;
            if (mStatus == eStatus.New) {
                if (string.IsNullOrEmpty(txtProvider.Text)) {
                    txtProvider.Background = Brushes.Red;
                    lResult = false;
                } else {
                    txtProvider.Background = Brushes.White;
                }
                txtVariant.Background = Brushes.White;
                if (lResult) {
                    if (Data.getInstance.xProviderPresent(txtProvider.Text, txtVariant.Text)) {
                        txtProvider.Background = Brushes.Red;
                        txtVariant.Background = Brushes.Red;
                        lResult = false;
                    }
                }
            }

            txtFee.Background = Brushes.White;
            if (double.TryParse(txtFee.Text, out lValue)) {
                if (lValue < 0d) {
                    txtFee.Background = Brushes.Red;
                    lResult = false;
                } else {
                    if (lValue > 0d && lValue < 1.5d) {
                        txtFee.Background = Brushes.Red;
                        lResult = false;
                    }
                }
            } else {
                txtFee.Background = Brushes.Red;
                lResult = false;
            }

            if (double.TryParse(txtConsumption.Text, out lValue)) {
                if (lValue < 0d || lValue > 1.5d) {
                    txtConsumption.Background = Brushes.Red;
                    lResult = false;
                } else {
                    txtConsumption.Background = Brushes.White;
                }
            } else {
                txtConsumption.Background = Brushes.Red;
                lResult = false;
            }
            if (double.TryParse(txtPrice.Text, out lValue)) {
                if (lValue < 0d || lValue > 1.5d) {
                    txtPrice.Background = Brushes.Red;
                    lResult = false;
                } else {
                    txtPrice.Background = Brushes.White;
                }
            } else {
                txtPrice.Background = Brushes.Red;
                lResult = false;
            }

            if (double.TryParse(txtReturn.Text, out lValue)) {
                if (lValue < 0d || lValue > 1.5d) {
                    txtReturn.Background = Brushes.Red;
                    lResult = false;
                } else {
                    txtReturn.Background = Brushes.White;
                }
            } else {
                txtReturn.Background = Brushes.Red;
                lResult = false;
            }

            if (double.TryParse(txtReturnPrice.Text, out lValue)) {
                if (lValue < 0d || lValue > 1.5d) {
                    txtReturnPrice.Background = Brushes.Red;
                    lResult = false;
                } else {
                    txtReturnPrice.Background = Brushes.White;
                }
            } else {
                txtReturnPrice.Background = Brushes.Red;
                lResult = false;
            }
            return lResult;
        }

        private void txtBox_LostFocus(object sender, RoutedEventArgs e) {
            sCheckInput();
        }

        private void lstProviders_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            mSelectedProvider = lstProviders.SelectedItem as Provider;
            sFillProviderDetails();
        }

        private void sFillProviderDetails() {
            sDisableAll();
            if (mSelectedProvider != null) {
                mStatus = eStatus.Edit;
                sEnableDetails();
                txtProvider.Text = mSelectedProvider.xProvider;
                txtVariant.Text = mSelectedProvider.xVariant;
                txtFee.Text = mSelectedProvider.xMonthlyTariff.ToString();
                txtConsumption.Text = mSelectedProvider.xConsumedFixedPrice.ToString();
                txtPrice.Text = mSelectedProvider.xConsumedExtra.ToString();
                txtReturn.Text = mSelectedProvider.xProducedFixedPrice.ToString();
                txtReturnPrice.Text = mSelectedProvider.xProducedExtra.ToString();
                btnDelete.Visibility = Visibility.Visible;
                txtFee.Focus();
            } else {
                mStatus = eStatus.None;
                txtProvider.Text = "";
                txtVariant.Text = "";
                txtFee.Text = "";
                txtConsumption.Text = "";
                txtPrice.Text = "";
                txtReturn.Text = "";
                txtReturnPrice.Text = "";
                btnDelete.Visibility = Visibility.Collapsed;
            }
        }

        private void sDisableAll() {
            txtProvider.IsEnabled = false;
            txtProvider.Background = Brushes.LightGray;
            txtVariant.IsEnabled = false;
            txtVariant.Background = Brushes.LightGray;
            txtFee.IsEnabled = false;
            txtFee.Background = Brushes.LightGray;
            txtConsumption.IsEnabled = false;
            txtConsumption.Background = Brushes.LightGray;
            txtPrice.IsEnabled = false;
            txtPrice.Background = Brushes.LightGray;
            txtReturn.IsEnabled = false;
            txtReturn.Background = Brushes.LightGray;
            txtReturnPrice.IsEnabled = false;
            txtReturnPrice.Background = Brushes.LightGray;
        }

        private void sEnableId() {
            txtProvider.IsEnabled = true;
            txtProvider.Background = Brushes.White;
            txtVariant.IsEnabled = true;
            txtVariant.Background = Brushes.White;
        }

        private void sEnableDetails() {
            txtFee.IsEnabled = true;
            txtFee.Background = Brushes.White;
            txtConsumption.IsEnabled = true;
            txtConsumption.Background = Brushes.White;
            txtPrice.IsEnabled = true;
            txtPrice.Background = Brushes.White;
            txtReturn.IsEnabled = true;
            txtReturn.Background = Brushes.White;
            txtReturnPrice.IsEnabled = true;
            txtReturnPrice.Background = Brushes.White;
        }
    }
}
