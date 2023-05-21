using System;
using System.Windows;
using System.Windows.Controls;

namespace Финансы
{
    public partial class NewIncome : Page
    {
        public NewIncome()
        {
            InitializeComponent();
            Date.SelectedDate = DateTime.Today;
            iconCurrency.Data = mainWindow.currentIconCurrency.Data;
        }

        Button icon = new();
        MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;

        private void AddIncome(object sender, RoutedEventArgs e)
        {
            try { double.Parse(newRecord.Text); }
            catch
            {
                newRecord.Text = "0";
                return;
            }

            mainWindow.Add(icon, newRecord, Date.SelectedDate.Value, mainWindow.Income);

            mainWindow.MainFrame.NavigationService.Navigate(mainWindow.Income);
        }

        private void Enab(object sender, RoutedEventArgs e)
        {
            icon = (Button)sender;
            AddB.IsEnabled = true;
        }

        private void NewExpenses(object sender, RoutedEventArgs e)
        => mainWindow.MainFrame.NavigationService.Navigate(new NewExpenses());

        private void NewSum(object sender, RoutedEventArgs e)
        => newRecord.Text = String.Empty;
    }
}
