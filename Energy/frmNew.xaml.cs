using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.IO;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Energy {
    /// <summary>
    /// Interaction logic for frmNew.xaml
    /// </summary>
    public partial class frmNew: Window {
        private readonly string[] ReservedNames = new[] { "CON", "PRN", "AUX", "NUL", "COM1", "COM2", "COM3", "COM4", "COM5", "COM6", "COM7", "COM8", "COM9", "LPT1", "LPT2", "LPT3", "LPT4", "LPT5", "LPT6", "LPT7", "LPT8", "LPT9" };
        private string mName;

        internal string xName {
            get {
                return mName;
            }
        }

        public frmNew() {
            InitializeComponent();
            mName = "";
            TxtName.Focus();
        }

        private void ButtonOK_Click(object sender, RoutedEventArgs e) {
            string lName;

            lName = TxtName.Text.Trim();
            if (string.IsNullOrEmpty(lName)) {
                MessageBox.Show("Naam is verplicht.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Error);
            } else {
                if (sCheckName(lName)) {
                    mName = lName;
                    DialogResult = true;
                    this.Hide();
                } else {
                    MessageBox.Show("Naam is ongeldig.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e) {
            DialogResult = false;
            this.Hide();
        }

        private bool sCheckName(string pName) {
            char lLast;
            char[] lInvalidChars;

            if (string.IsNullOrWhiteSpace(pName))
                return false;

            // "." and ".." are not valid names for a new directory segment
            if (pName == "." || pName == "..")
                return false;

            // Length check for a single component on NTFS (practical limit)
            if (pName.Length > 255) // adjust if you target other filesystems
                return false;

            // Cannot end with space or dot
            lLast = pName[pName.Length - 1];
            if (lLast == ' ' || lLast == '.')
                return false;

            // Invalid chars for file/directory names
            lInvalidChars = Path.GetInvalidFileNameChars();
            if (pName.IndexOfAny(lInvalidChars) >= 0)
                return false;

            // Check reserved device names. Windows disallows these even with extensions (e.g. "CON.txt")
            // Compare against the name up to first '.' (or entire name), after trimming trailing spaces/dots
            string trimmed = pName.TrimEnd(' ', '.');
            string upper = trimmed.ToUpperInvariant();

            // If the name is exactly a reserved name OR begins with reservedname + '.' (e.g. "CON.", "CON.txt")
            foreach (var r in ReservedNames) {
                if (upper == r)
                    return false;
                if (upper.StartsWith(r + ".", StringComparison.Ordinal))
                    return false;
            }

            return true;
        }
    }
}
