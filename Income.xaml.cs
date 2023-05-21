using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace Финансы
{
    public partial class Income : Page
    {
        public Income()
        {
            InitializeComponent();
            Period.Text = DateTime.Today.AddDays(1 - (int)DateTime.Today.DayOfWeek).ToString("dd.MM") + "-" +
                DateTime.Today.AddDays(7 - (int)DateTime.Today.DayOfWeek).ToString("dd.MM");
            periods = new[] { DayT, WeekT, MonthT };
            periodsDays = new() { { DayT, 1 }, { WeekT, 7 }, { MonthT, 30 } };
            currectPeriod = periods[1];
        }

        public TextBlock[] periods;
        public Dictionary<TextBlock, int> periodsDays;
        public TextBlock currectPeriod;

        private void ToExpenses(object sender, RoutedEventArgs e)
        {
            MainWindow? mainWindow = Window.GetWindow(this) as MainWindow;
            mainWindow.MainFrame.NavigationService.Navigate(mainWindow.Expenses);
        }
        private void NewIncome(object sender, RoutedEventArgs e)
        {
            MainWindow? mainWindow = Window.GetWindow(this) as MainWindow;
            mainWindow.MainFrame.NavigationService.Navigate(new NewIncome());
        }

        private void PreviousPeriod(object sender, RoutedEventArgs e)
        {
            MainWindow? mainWindow = Window.GetWindow(this) as MainWindow;
            mainWindow.NewPeriod(-periodsDays[currectPeriod]);
        }

        private void NextPeriod(object sender, RoutedEventArgs e)
        {
            MainWindow? mainWindow = Window.GetWindow(this) as MainWindow;
            mainWindow.NewPeriod(periodsDays[currectPeriod]);
        }

        private void NewPeriod(object sender, RoutedEventArgs e)
        {
            MainWindow? mainWindow = Window.GetWindow(this) as MainWindow;

            foreach (var period in periods)
            {
                period.TextDecorations = null;
                period.Background = null;
            }

            foreach (var period in mainWindow.Expenses.periods)
            {
                period.TextDecorations = null;
                period.Background = null;
            }

            currectPeriod = ((Button)sender).Content as TextBlock;
            mainWindow.Expenses.currectPeriod = mainWindow.Expenses.periods[Array.IndexOf(periods, currectPeriod)];

            currectPeriod.TextDecorations = TextDecorations.Underline;
            currectPeriod.Background = Brushes.White;

            mainWindow.Expenses.currectPeriod.TextDecorations = TextDecorations.Underline;
            mainWindow.Expenses.currectPeriod.Background = Brushes.White;

            mainWindow.AnotherPeriod(periodsDays[currectPeriod]);
        }

        private void Edit(object sender, SelectionChangedEventArgs e)
        {
            if (Li.SelectedItem != null && !(((Border)Li.SelectedItem).Child is TextBlock))
            {
                MainWindow? mainWindow = Window.GetWindow(this) as MainWindow;
                mainWindow.MainFrame.NavigationService.Navigate(new EditingIncome());
            }
        }
    }
}
