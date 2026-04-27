using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Security;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Energy {
    internal class Parameters {
        private static readonly Parameters Instance;

        private const string cIniFileName = @"Energy.ini";
        private string mBaseDir = "";
        private string mLoadDir = "";
        private string mDataDir = "";

        static Parameters() {
            Instance = new Parameters();
        }

        internal static Parameters GetInstance {
            get {
                return Instance;
            }
        }

        internal string xLoadDir {
            get {
                return mLoadDir;
            }
        }

        internal string xDataDir {
            get {
                return mDataDir;
            }
        }


        private Parameters() {
            sInitParameters();
            sProcessIni();
        }

        private void sInitParameters() {
            mBaseDir = System.AppDomain.CurrentDomain.BaseDirectory;
            mLoadDir = Path.Combine(mBaseDir, "Load");
            mDataDir = Path.Combine(mBaseDir, "Data");
        }

        private void sProcessIni() {
            StreamReader lStream = null;
            string? lLine;

            try {
                lStream = new StreamReader(mBaseDir + @"\" + cIniFileName);
                do {
                    lLine = lStream.ReadLine();
                    if (lLine != null) {
                        sProcessIniLine(lLine);
                    } else {
                        lStream.Close();
                        break;
                    }
                } while (true);
                lStream.Close();
            } catch (FileNotFoundException) {
                sCreateIni();
            }
        }

        private void sProcessIniLine(string pLine) {
            string lKeyWord;
            string lValue;
            int lSplit;

            lSplit = pLine.IndexOf('=');
            if (lSplit > 0) {
                lKeyWord = pLine.Substring(0, lSplit).ToLower();
                if (pLine.Length > lSplit + 1) {
                    lValue = pLine.Substring(lSplit + 1);
                    switch (lKeyWord) {
                        case "loaddir": {
                                mLoadDir = lValue;
                                break;
                            }
                        case "datadir": {
                                mDataDir = lValue;
                                break;
                            }
                    }

                }
            }
        }

        private void sCreateIni() {
            StreamWriter lStream;

            lStream = new StreamWriter(mBaseDir + @"\" + cIniFileName, false);
            lStream.WriteLine("LoadDir=" + mLoadDir);
            lStream.WriteLine("DataDir=" + mDataDir);
            lStream.Close();

        }
    }
}
