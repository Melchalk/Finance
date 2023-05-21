using System;
using System.Windows;
using System.Windows.Controls;

namespace Финансы
{
    public partial class Settings : Page
    {
        public Settings()
        {
            InitializeComponent();
        }

        private void Name_GotFocus(object sender, RoutedEventArgs e)
        => MyName.Text = String.Empty;

        private void DeleteAll(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
            mainWindow.Expenses = new Expenses();
            mainWindow.Income = new Income();
            mainWindow.Categories = new ListCategories();
            mainWindow.mainDI = new(); mainWindow.mainDE = new();
            mainWindow.totalAmount = 0;
            mainWindow.dictionaryExAmout = new(); mainWindow.dictionaryInAmout = new();
            mainWindow.currentCurrenty = 1;
            NewChoicePeriod();
        }

        private void NewLanguage(object sender, SelectionChangedEventArgs e)
        {
            MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
            if (mainWindow == null || (ComboBoxItem)Period.SelectedItem == null) return;
        }

        private void Selection(object sender, SelectionChangedEventArgs e)
        {
            MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
            if (mainWindow == null || (ComboBoxItem)Period.SelectedItem == null) 
                return;
            NewChoicePeriod();
        }

        public void NewChoicePeriod() 
        {
            MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
            string period = (string)((ComboBoxItem)Period.SelectedItem).Content;
            if (period == "День")
                mainWindow.Expenses.Day.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            else if (period == "Неделя")
                mainWindow.Expenses.Week.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            else
                mainWindow.Expenses.Month.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            mainWindow.currentPeriod = period;
        }
    }
}