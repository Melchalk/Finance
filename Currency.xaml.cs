using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.IO;
using Path = System.Windows.Shapes.Path;

namespace Финансы
{
    public partial class Currency : Page
    {
        public Currency()
        {
            InitializeComponent();
        }
        public const double usd = 0.013, eur = 0.012, rus = 1;
        public Dictionary<double, string> currIcon = new()
        {
            { usd, "$" }, { eur, "€" }, { rus, "₽"}
        };

        private void NewCurrenсy(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
            var currency = ((Button)sender).Content;
            double newCurr;
            if (currency == RUS.Content)
            {
                newCurr = rus;
                mainWindow.currentIconCurrency.Data = ((Path)RUS.Content).Data;
            }
            else if (currency == USD.Content)
            {
                newCurr = usd;
                mainWindow.currentIconCurrency.Data = ((Path)USD.Content).Data;
            }
            else //(currency == EUR.Content)
            {
                newCurr = eur;
                mainWindow.currentIconCurrency.Data = ((Path)EUR.Content).Data;
            }
            CurrencyCrossing(newCurr);
            mainWindow.currentCurrenty = newCurr;
        }

        public void CurrencyCrossing(double newCurr)
        {
            MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
            double kCurr = newCurr;
            kCurr /= mainWindow.currentCurrenty;
            mainWindow.currentCurrenty = newCurr;
            mainWindow.totalAmount *= kCurr;
            mainWindow.NewTotal();
            SortedDictionary<DateTime, List<Border>>[] mainsD = { mainWindow.mainDI, mainWindow.mainDE };
            foreach (var dictionary in mainsD)
            {
                foreach (var e in dictionary)
                {
                    foreach (var border in e.Value)
                    {
                        if (border.Child is not StackPanel) continue;
                        ((TextBlock)((StackPanel)border.Child).Children[^2]).Text =
                            $"{Math.Round(double.Parse(((TextBlock)((StackPanel)border.Child).Children[^2]).Text) * kCurr, 3)}";
                        ((TextBlock)((StackPanel)border.Child).Children[^1]).Text =
                            $"{currIcon[newCurr]}";
                    }
                }
            }

            foreach (var e in mainWindow.dictionaryExAmout.Keys)
                mainWindow.dictionaryExAmout[e] = Math.Round(mainWindow.dictionaryExAmout[e] * kCurr, 3);
            foreach (var e in mainWindow.dictionaryInAmout.Keys)
                mainWindow.dictionaryInAmout[e] = Math.Round(mainWindow.dictionaryInAmout[e] * kCurr, 3);

            mainWindow.GeneralExRecalculation();
            mainWindow.GeneralInRecalculation();
        }
    }
}