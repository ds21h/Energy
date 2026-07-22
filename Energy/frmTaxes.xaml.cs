using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Energy {
    /// <summary>
    /// Interaction logic for frmTaxes.xaml
    /// </summary>
    public partial class frmTaxes: Window {
        public frmTaxes() {
            InitializeComponent();
            sGetValues();
        }

        private void sGetValues() {
            TxtEnergyTax.Text = Data.getInstance.xTaxes.xTax.ToString("0.00000");
            TxtReduction.Text = (-Data.getInstance.xTaxes.xReturn).ToString("0.00000");
        }

        private void txtBox_LostFocus(object sender, RoutedEventArgs e) {
            sCheckInput();
        }

        private bool sCheckInput() {
            double lValue;
            bool lResult;

            lResult = true;
            TxtEnergyTax.Background = Brushes.White;
            if (double.TryParse(TxtEnergyTax.Text, out lValue)) {
                if (lValue < 0 || lValue > 0.1) {
                    TxtEnergyTax.Background = Brushes.Red;
                    lResult = false;
                }
            } else {
                TxtEnergyTax.Background = Brushes.Red;
                lResult = false;
            }

            TxtReduction.Background = Brushes.White;
            if (double.TryParse(TxtReduction.Text, out lValue)) {
                if (lValue < 0 || lValue > 10) {
                    TxtReduction.Background = Brushes.Red;
                    lResult = false;
                }
            } else {
                TxtReduction.Background = Brushes.Red;
                lResult = false;
            }
            return lResult;
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e) {
            DialogResult = false;
            this.Hide();
        }

        private void ButtonOK_Click(object sender, RoutedEventArgs e) {
            if (sCheckInput()) {
                Data.getInstance.xTaxes.xTax = double.Parse(TxtEnergyTax.Text);
                Data.getInstance.xTaxes.xReturn = -double.Parse(TxtReduction.Text);
                Data.getInstance.xTaxes.xSaveTaxes();
                DialogResult = true;
                this.Hide();
            }
        }
    }
}
