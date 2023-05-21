using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Финансы
{
    public partial class EditingExpenses : Page
    {
        public EditingExpenses()
        {
            InitializeComponent();

        }

        Button icon = new();
        DateTime maybeDate;

        private void newRecord_Loaded(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
            var children = ((StackPanel)((Border)mainWindow.Expenses.Li.SelectedItem).Child).Children;
            newRecord.Text = ((TextBlock)children[^2]).Text.ToString();

            iconCurrency.Data = mainWindow.currentIconCurrency.Data;
        }

        private void Date_Loaded(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
            Border children = (Border)mainWindow.Expenses.Li.SelectedItem;
            foreach (var child in mainWindow.mainDE)
            {
                foreach (var childB in child.Value)
                {
                    if (childB == children)
                    {
                        Date.SelectedDate = maybeDate = child.Key;
                        icon.Content = (Path)((Border)((StackPanel)childB.Child).Children[0]).Child;
                        break;
                    }
                }
            }
        }

        private void Edit(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = (MainWindow)Application.Current.MainWindow; 
            Border children = (Border)mainWindow.Expenses.Li.SelectedItem;
            NewDate();
            foreach (var child in mainWindow.mainDE)
            {
                for (int i = 0; i < child.Value.Count; i++)
                {
                    if (child.Value[i] == children)
                    {
                        TextBlock tet = (TextBlock)((StackPanel)mainWindow.mainDE[child.Key][i].Child).Children[^2];
                        if (tet.Text != newRecord.Text)
                        {
                            try { double.Parse(newRecord.Text); }
                            catch
                            {
                                newRecord.Text = "0";
                                return;
                            }
                            double cS = double.Parse(newRecord.Text) - double.Parse(tet.Text);

                            mainWindow.dictionaryExAmout[child.Key] += cS;
                            mainWindow.totalAmount -= (double)cS;
                            ((TextBlock)((StackPanel)mainWindow.mainDE[child.Key][i].Child).Children[^2]).Text = newRecord.Text;
                            mainWindow.NewTotal();
                            mainWindow.GeneralExRecalculation();
                        }
                        break;
                    }
                }
            }
        
            mainWindow.MainFrame.NavigationService.Navigate(mainWindow.Expenses);
        }

        private void NewIcon(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;

            icon = (Button)sender;

            Border b = new Border()
            {
                Child = new Path()
                {
                    Fill = Brushes.White,
                    Data = ((Path)icon.Content).Data,
                    Stretch = Stretch.Fill,
                    Height = ((Path)icon.Content).Height - 8,
                    Width = 17,
                },
                Background = icon.Background,
                Height = 35,
                Width = 35,
                CornerRadius = new CornerRadius(20),
                Margin = new Thickness(9, 0, 0, 0),
            };

            Border children = (Border)mainWindow.Expenses.Li.SelectedItem;

            foreach (var child in mainWindow.mainDE)
            {
                for (int i = 0; i < child.Value.Count; i++)
                {
                    if (child.Value[i] == children)
                    {
                        ((StackPanel)mainWindow.mainDE[child.Key][i].Child).Children.RemoveAt(0);
                        ((StackPanel)mainWindow.mainDE[child.Key][i].Child).Children.Insert(0, b);
                        ((TextBlock)((StackPanel)mainWindow.mainDE[child.Key][i].Child).Children[1]).Text = mainWindow.Categories.TextN(icon.Name);

                        mainWindow.GeneralExRecalculation();
                        break;
                    }
                }
            }
        }

        private void NewSum(object sender, RoutedEventArgs e)
        {
            newRecord.Text = String.Empty;
        }

        private void Del(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;

            Border children = (Border)mainWindow.Expenses.Li.SelectedItem;

            foreach (var child in mainWindow.mainDE)
            {
                if (child.Value.Contains(children))
                {
                    double cS = double.Parse(((TextBlock)((StackPanel)children.Child).Children[^2]).Text);
                    mainWindow.dictionaryExAmout[child.Key] -= cS;
                    mainWindow.totalAmount += (double)cS;
                    mainWindow.NewTotal();
                    mainWindow.mainDE[child.Key].Remove(children);
                    if (mainWindow.mainDE[child.Key].Count == 1) 
                        mainWindow.mainDE.Remove(child.Key);
                    break;
                }
            }

            mainWindow.GeneralExRecalculation();
            mainWindow.MainFrame.NavigationService.Navigate(mainWindow.Expenses);
        }

        void NewDate()
        {
            DateTime currentDate = (DateTime)Date.SelectedDate;
            if (currentDate == maybeDate) return;

            MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
            Border children = (Border)mainWindow.Expenses.Li.SelectedItem;
            bool find = false;
            foreach (var child in mainWindow.mainDE)
            {
                foreach (var childB in child.Value)
                {
                    if (childB == children)
                    {                        
                        double cS = double.Parse(((TextBlock)((StackPanel)children.Child).Children[^2]).Text);
                        mainWindow.dictionaryExAmout[child.Key] -= cS;
                        mainWindow.mainDE[child.Key].Remove(children);
                        if (mainWindow.mainDE[child.Key].Count == 1)
                            mainWindow.mainDE.Remove(child.Key);
                        find = true;
                        break;
                    }
                }
                if (find) break;
            }

            if (!mainWindow.mainDE.ContainsKey(currentDate))
            {
                mainWindow.mainDE[currentDate] = new List<Border>();
                Border f = new()
                {
                    Width = 270,
                    Child = new TextBlock()
                    {
                        Text = currentDate.Date.ToString("dd.MM"),
                        FontFamily = new FontFamily("Montserrat"),
                        Foreground = mainWindow.brush,
                        FontSize = 20,
                        FontWeight = FontWeights.SemiBold

                    }
                };
                mainWindow.mainDE[currentDate].Add(f);
                mainWindow.dictionaryExAmout[currentDate] = 0;
            }
            mainWindow.dictionaryExAmout[currentDate] += double.Parse(((TextBlock)((StackPanel)children.Child).Children[^2]).Text);
            mainWindow.mainDE[currentDate].Add(children);
            mainWindow.GeneralExRecalculation();
        }
    }
}
