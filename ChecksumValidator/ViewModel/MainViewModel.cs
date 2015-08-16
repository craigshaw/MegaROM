using ChecksumValidator.Framework;
using ChecksumValidator.Model;
using Microsoft.Win32;
using System;
using System.Windows;
using System.Windows.Input;

namespace ChecksumValidator
{
    internal class MainViewModel : Observable
    {
        private ICommand openCommand;
        private string validationResult;

        public string ValidationResult
        {
            get { return validationResult; }

            private set
            {
                validationResult = value;
                RaisePropertyChangedEvent("ValidationResult");
            }
        }

        public ICommand OpenCommand
        {
            get
            {
                return openCommand ?? (openCommand = new RelayCommand<object>(
                    async o => 
                    {
                        string romPath = GetRomFilePath();
                        if (!string.IsNullOrEmpty(romPath))
                            ValidationResult = await new ROMValidator(romPath).ValidateROM();
                    }
                    ));
            }
        }

        private string GetRomFilePath()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Megadrive ROM Files (*.md)|*.md|All Files (*.*)|*.*";
            ofd.Title = "Open ROM File";
            ofd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            if (ofd.ShowDialog() == true)
                return ofd.FileName;

            return string.Empty;
        }
    }
}
