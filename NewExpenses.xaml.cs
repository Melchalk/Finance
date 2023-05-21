using System;
using System.Windows;
using System.Windows.Controls;

namespace Финансы
{
    public partial class NewExpenses : Page
    {
        public NewExpenses()
        {
            InitializeComponent();
            Date.SelectedDate = DateTime.Today;
            newRecord.Text = 0.ToString();
            iconCurrency.Data = mainWindow.currentIconCurrency.Data;
        }

        Button icon = new();
        MainWindow mainWindow = (MainWindow)Application.Current.MainWindow; 

        private void AddExpenses(object sender, RoutedEventArgs e)
        {
            try { double.Parse(newRecord.Text); }
            catch
            {
                newRecord.Text = "0";
                return;
            }   
            mainWindow.Add(icon, newRecord, Date.SelectedDate.Value, mainWindow.Expenses);

            mainWindow.MainFrame.NavigationService.Navigate(mainWindow.Expenses);
        }
         
        private void Enab(object sender, RoutedEventArgs e)
        {
            icon = (Button)sender;
            AddB.IsEnabled = true;
        }

        private void NewIncome(object sender, RoutedEventArgs e) => 
            mainWindow.MainFrame.NavigationService.Navigate(new NewIncome());

        private void NewSum(object sender, RoutedEventArgs e) => 
            newRecord.Text = String.Empty;   
    }
}
