﻿using MachineLearningSoftware.Common;
using MachineLearningSoftware.Controls.Entities;
using MachineLearningSoftware.Entities;
using MachineLearningSoftware.Views.DialogBoxes;
using System;
using System.ComponentModel.Composition;
using System.Windows;
using System.Windows.Input;

namespace MachineLearningSoftware.ViewModels
{
    [Export]
    public class MainWindowViewModel : BaseViewModel
    {
        #region Fields

        private readonly MainWindowFunctions _mainWindowFunctions;
        private HelpDialogBox _helpDialog;
        private Action<string> _searchControlAction;

        #endregion

        #region Properties

        public ICommand DisplayHelpDialogCommand
        {
            get { return new CommandDelegate(DisplayHelpDialog, CanExecute); }
        }

        public ICommand DisplayExitDialogCommand
        {
            get { return new CommandDelegate(DisplayExitDialog, CanExecute); }
        }

        public ICommand DisplayTensorFlowWebsiteCommand
        {
            get { return new CommandDelegate(NavigateToTensorFlow, CanExecute); }
        }

        public ICommand DisplayPythonWebsiteCommand
        {
            get { return new CommandDelegate(NavigateToPython, CanExecute); }
        }

        public ICommand YesButtonCommand
        {
            get { return new CommandDelegate(ExitSoftwareCommand, CanExecute); }
        }

        public Action<string> SearchControlAction
        {
            get { return _searchControlAction; }
            set
            {
                if (_searchControlAction != value)
                {
                    _searchControlAction = value;
                    OnPropertyChanged(nameof(SearchControlAction));
                }
            }
        }

        #endregion

        #region Constructor

        [ImportingConstructor]
        public MainWindowViewModel(MainWindowFunctions mainWindowFunctions)
        {
            ApplyTheme();
            _mainWindowFunctions = mainWindowFunctions;
            SearchControlAction = OnSearch;
        }

        #endregion

        #region Private Methods

        private void ExitSoftwareCommand(object parameter)
        {
            App.Current.Shutdown();
        }

        private void OnSearch(string parameter)
        {
            _mainWindowFunctions.OpenPage("Search", parameter);
        }

        private void DisplayHelpDialog(object context)
        {
            _helpDialog = new HelpDialogBox();
            _helpDialog.ShowDialog();            
        }

        private void DisplayExitDialog(object context)
        {
            ShowNewWindow<CloseSoftwareWindow>(this);
        }

        private void NavigateToTensorFlow(object context)
        {
            HyperlinkNavigation.NavigateTo("https://www.tensorflow.org/");
        }
        
        private void NavigateToPython(object context)
        {
            HyperlinkNavigation.NavigateTo("https://www.python.org/");
        }

        private void ApplyTheme()
        {
            var theme = new ThemeConstantsEntity();
            foreach (var prop in typeof(ThemeConstantsEntity).GetProperties())
            {
                if (prop.CanRead)
                {
                    var value = prop.GetValue(theme, null) as ThemeEntity;
                    var applicationTheme = Properties.Settings.Default["ApplicationTheme"].ToString();
                    if (string.Equals(value.ThemeName, applicationTheme, StringComparison.OrdinalIgnoreCase))
                    {
                        var dictionaryUri = new Uri(value.ThemeSource, UriKind.Relative);
                        var resourceDict = Application.LoadComponent(dictionaryUri) as ResourceDictionary;
                        Application.Current.Resources.MergedDictionaries.Clear();
                        Application.Current.Resources.MergedDictionaries.Add(resourceDict);
                    }
                }
            }
        }

        #endregion
    }
}
