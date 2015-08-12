using MahApps.Metro.Controls;
using MegadriveUtilities;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ChecksumValidator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public async void OpenROM(object sender, ExecutedRoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Megadrive ROM Files (*.md)|*.md|All Files (*.*)|*.*";
            ofd.Title = "Open ROM File";
            ofd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            if (ofd.ShowDialog() == true)
                await ValidateChecksum(ofd.FileName);
        }

        private async Task ValidateChecksum(string path)
        {
            try
            {
                ROM rom = new ROM(new BinROMLoader(path, new BinROMValidator()));
                await rom.LoadAsync();

                UInt16 checksum = rom.Checksum;
                UInt16 calculatedChecksum = rom.CalculateChecksum();

                AddOutputText(string.Format("Checksum: 0x{0:X}", checksum));
                AddOutputText(string.Format("Calculated checksum: 0x{0:X}", calculatedChecksum));
            }
            catch (Exception ex)
            {
                AddOutputText(string.Format("Failed with: {0}", ex.Message));
            }
        }

        private void AddOutputText(string output)
        {
            TextBlock tb = new TextBlock();
            tb.Text = output;
            OutputPanel.Children.Add(tb);
        }

        public void OpenROM_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
    }
}
