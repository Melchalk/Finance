using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Shapes;
using System.IO;
using Path = System.Windows.Shapes.Path;
using System.Text.Json;
using System.Text.Json.Serialization;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace Финансы
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Expenses = new();
            Income = new();
            Categories = new();
            Currency = new();
            currentIconCurrency.Data = ((Path)Currency.RUS.Content).Data;
            Settings = new();
            file = Directory.GetCurrentDirectory() + "/MyTest.txt";
            if (!File.Exists(file))
                File.Create(file);
            jsonFile = File.ReadAllText(file);
        }

        public class SaveJson
        {
            public string[][] MainDEJ { get; set; }
            public string[][] MainDIJ { get; set; } 
            public double CurrentCurrenty { get; set; }
            public string CurrentPeriod { get; set; }
            public string Name { get; set; }    
        }

        public string file;
        public string jsonFile;

        public Expenses Expenses;
        public Income Income;
        public ListCategories Categories;
        public Currency Currency;
        public Settings Settings;

        public SortedDictionary<DateTime, List<Border>> mainDE = new(), mainDI = new();
        public Dictionary<DateTime, double> dictionaryExAmout = new(), dictionaryInAmout = new();
        public double[] iconAmoutEx = new double[10], 
            iconAmoutIn = new double[4];

        DateTime first = DateTime.Today.AddDays(7 - (int)DateTime.Today.DayOfWeek),
            second = DateTime.Today.AddDays(1 - (int)DateTime.Today.DayOfWeek),
            day = DateTime.Today;

        public double totalAmount = 0;

        public double currentCurrenty = 1;
        public string currentPeriod = "Неделя";
        public Path currentIconCurrency = new Path();

        public Brush? brush = new BrushConverter().ConvertFromString("#461E5C") as Brush;

        private void Window_Closed(object sender, EventArgs e)
        {
            var newJson = new SaveJson
            {
                MainDEJ = NewJsonD(mainDE),
                MainDIJ = NewJsonD(mainDI),
                CurrentCurrenty = currentCurrenty,
                CurrentPeriod = currentPeriod,
                Name = Settings.MyName.Text != "" ? Settings.MyName.Text : "Имя"
            };

            File.WriteAllText(file, JsonConvert.SerializeObject(newJson));
        }

        string[][] NewJsonD(SortedDictionary<DateTime, List<Border>> mainD)
        {
            string[][] jsonD = new string[mainD.Count][];
            int indexAll = -1;
            foreach (var listBorder in mainD)
            {
                indexAll++;
                for (int listIndex = 0; listIndex < listBorder.Value.Count; listIndex++)
                {
                    var child = listBorder.Value[listIndex].Child;
                    if (child is TextBlock)
                    {
                        if (jsonD[indexAll] == null) jsonD[indexAll] = new string[listBorder.Value.Count];
                        jsonD[indexAll][0] = ((TextBlock)child).Text;
                        continue;
                    }
                    jsonD[indexAll][listIndex] = ((TextBlock)((StackPanel)child).Children[1]).Text + " " +
                        ((TextBlock)((StackPanel)child).Children[^2]).Text;
                }
            }
            return jsonD;
        }

        private void ToExpenses(object sender, RoutedEventArgs e)
        {
            MainFrame.NavigationService.Navigate(Expenses);
            pig.Fill = Brushes.White;
            categories.Fill = brush;
            currency.Fill = brush;
            settings.Fill = brush;
        }

        private void ToCategories(object sender, RoutedEventArgs e)
        {
            MainFrame.NavigationService.Navigate(Categories);
            categories.Fill = Brushes.White;
            pig.Fill = brush;
            currency.Fill = brush;
            settings.Fill = brush;
        }

        private void ToCurrency(object sender, RoutedEventArgs e)
        {
            MainFrame.NavigationService.Navigate(Currency);
            currency.Fill = Brushes.White;
            categories.Fill = brush;
            pig.Fill = brush;
            settings.Fill = brush;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            MainFrame.NavigationService.Navigate(Expenses);
            Application.Current.MainWindow = this;
            pig.Fill = Brushes.White;
            categories.Fill = brush;
            currency.Fill = brush;

            var newJson = JsonConvert.DeserializeObject<SaveJson>(File.ReadAllText(file));
            if (newJson == null) return;

            CurrentMainD(newJson, Expenses, Categories.buttonEx);
            CurrentMainD(newJson, Income, Categories.buttonIn);

            foreach (ComboBoxItem combo in Settings.Period.Items)
                if ((string)combo.Content == newJson.CurrentPeriod) 
                    Settings.Period.SelectedIndex = Settings.Period.Items.IndexOf(combo);
            Settings.MyName.Text = newJson.Name;

            currentCurrenty = newJson.CurrentCurrenty;

            if (newJson.CurrentCurrenty == Currency.rus)
                Currency.RUS.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            else if (newJson.CurrentCurrenty == Currency.usd)
                Currency.USD.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            else
                Currency.EUR.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
        }

        void CurrentMainD(SaveJson json, Page page, Dictionary<string, Button> buttons)
        {
            var mainD = page is Expenses ? json.MainDEJ : json.MainDIJ;
            page = page is Expenses ? Expenses : Income;
            foreach (var list in mainD) 
            {
                for(int inList = 1; inList < list.Length; inList++)
                {
                    int indexProbel = list[inList].IndexOf(' ');
                    TextBox newR = new() { Text = list[inList].Substring(indexProbel+1)};
                    Add(buttons[list[inList].Substring(0, indexProbel)], newR, DateTime.Parse(list[0]), page);
                }
            }
        }

        private void ToSettings(object sender, RoutedEventArgs e)
        {
            MainFrame.NavigationService.Navigate(Settings);
            settings.Fill = Brushes.White;
            categories.Fill = brush;
            pig.Fill = brush;
            currency.Fill = brush;
        }

        public (Border, double) AddLocal(Button icon, TextBox newRecord)
        {
            var brPanel = new Border()
            {
                BorderBrush = Brushes.Black,
                CornerRadius = new CornerRadius(10),
                Width = 270,
                Background = Brushes.White,
                Margin = new Thickness(0, 0, 0, 10),
                Effect = new DropShadowEffect()
                {
                    Opacity = 0.5,
                    RenderingBias = RenderingBias.Quality,
                    BlurRadius = 5,
                    ShadowDepth = 2,
                    Direction = 270
                }
            };

            var newPanel = new StackPanel()
            {
                Height = 50,
                Width = 270,
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Left,
            };

            newPanel.Children.Add(new Border()
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
                Margin = new Thickness(5, 0, 0, 0),
            });

            newPanel.Children.Add(new TextBlock()
            {
                Text = Categories.TextN(icon.Name),
                Width = 104,
                FontSize = 15,
                TextWrapping = TextWrapping.Wrap,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(7, 0, 0, 0),
                FontFamily = new FontFamily("Montserrat"),
                FontWeight = FontWeights.Medium
            });

            newPanel.Children.Add(new TextBlock()
            {
                Text = "100%",
                TextWrapping = TextWrapping.Wrap,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(5, 0, 0, 0),
                FontFamily = new FontFamily("Montserrat"),
                FontSize = 12,
                FontWeight = FontWeights.Regular,
                Width = 37,
                TextAlignment = TextAlignment.Right,
            });

            newPanel.Children.Add(new TextBlock()
            {
                Text = double.Parse(newRecord.Text).ToString(),
                FontSize = 14,
                Width = 56.5,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(3, 0, 0, 0),
                FontFamily = new FontFamily("Montserrat"),
                TextAlignment = TextAlignment.Right,
                TextWrapping = TextWrapping.Wrap,
                FontWeight = FontWeights.Medium
            });

            newPanel.Children.Add(new TextBlock()
            {
                Text = $"{Currency.currIcon[currentCurrenty]}",
                FontSize = 14,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(0, 0, 0, 0),
                FontFamily = new FontFamily("Montserrat"),
                TextAlignment = TextAlignment.Right,
                TextWrapping = TextWrapping.Wrap,
                FontWeight = FontWeights.Medium
            });

            brPanel.Child = newPanel;
            return (brPanel, double.Parse(newRecord.Text));
        }

        public void Add(Button icon, TextBox newRecord, DateTime date, Page page)
        {
            var mainD = page is Expenses ? mainDE : mainDI;

            if (!mainD.ContainsKey(date))
            {
                mainD[date] = new List<Border>();
                Border f = new()
                {
                    Width = 270,
                    Child = new TextBlock()
                    {
                        Text = date.Date.ToString("dd.MM"),
                        FontFamily = new FontFamily("Montserrat"),
                        Foreground = brush,
                        FontSize = 20,
                        FontWeight = FontWeights.SemiBold
                    }
                };
                mainD[date].Add(f);
                if (page is Expenses) dictionaryExAmout[date] = 0;
                else dictionaryInAmout[date] = 0;
            }

            Border brPanel; double cS;
            (brPanel, cS) = AddLocal(icon, newRecord);

            mainD[date].Add(brPanel);

            if (page is Expenses)
            {
                dictionaryExAmout[date] += cS;
                totalAmount -= (double)cS;
            }
            else
            {
                dictionaryInAmout[date] += cS;
                totalAmount += (double)cS;
            }

            NewTotal();

            if (page is Expenses)
                GeneralExRecalculation();
            else
                GeneralInRecalculation();
        }

        public void AnotherPeriod(int days)
        {
            if (days == 1)
                Expenses.Period.Text = Income.Period.Text = day.ToString("dd.MM");
            else if (days == 7)
            {
                first = day.AddDays((7 - (int)day.DayOfWeek) % 7);
                second = first.AddDays(-6);
                Expenses.Period.Text = Income.Period.Text = second.ToString("dd.MM") + "-" + first.ToString("dd.MM");
            }
            else
                Expenses.Period.Text = Income.Period.Text = day.ToString("MMMM");

            Recalculation(Math.Abs(days), Expenses.Li.Items, Expenses.Rectangles, Expenses.LocalAmount, Expenses);
            Recalculation(Math.Abs(days), Income.Li.Items, Income.Rectangles, Income.LocalAmount, Income);
        }

        public void NewPeriod(int days)
        {
            var absDays = Math.Abs(days);
            if (absDays == 1)
            {
                day = day.AddDays(days);
                Expenses.Period.Text = day.ToString("dd.MM");
                Income.Period.Text = day.ToString("dd.MM");
            }
            else if (absDays == 7)
            {
                day = day.AddDays(days);
                first = first.AddDays(days);
                second = second.AddDays(days);

                Expenses.Period.Text = Income.Period.Text = second.ToString("dd.MM") + "-" + first.ToString("dd.MM");
            }
            else
            {
                first = first.AddMonths(days / absDays);
                second = second.AddMonths(days / absDays);
                day = day.AddMonths(days / absDays);
                Expenses.Period.Text = Income.Period.Text = day.ToString("MMMM");
            }
            Recalculation(Math.Abs(days), Expenses.Li.Items, Expenses.Rectangles, Expenses.LocalAmount, Expenses);
            Recalculation(Math.Abs(days), Income.Li.Items, Income.Rectangles, Income.LocalAmount, Income);
        }

        public void Recalculation(int days, ItemCollection items, StackPanel rec, Label tAmout, Page page)
        {
            double currentAmount;
            DateTime dayOne, dayTwo;
            (dayOne, dayTwo, currentAmount) = DaysAmout(days, page);

            items.Clear();
            rec.Children.Clear();
            iconAmoutEx = new double[10];
            iconAmoutIn = new double[4];

            var mainD = page is Expenses ? mainDE : mainDI;

            for (var day = dayTwo.Date; day <= dayOne; day = day.AddDays(1))
            {
                if (mainD.ContainsKey(day))
                    foreach (Border border in mainD[day])
                        items.Add(border);
            }

            BorderCrossing(items, currentAmount, page);

            if (currentAmount == 0)
                rec.Children.Add(StartRe(page));
            else
            {
                if (page is Expenses) RectangleCrossing(currentAmount, iconAmoutEx, rec, Expenses);
                else RectangleCrossing(currentAmount, iconAmoutIn, rec, Income);
            }

            tAmout.Content = $"{Math.Abs(currentAmount)}{Currency.currIcon[currentCurrenty]}";
        }

        (DateTime, DateTime, double) DaysAmout(int days, Page page)
        {
            double currentAmount = 0;
            DateTime dayOne, dayTwo;

            if (days == 7)
            {
                dayOne = first;
                dayTwo = second;
            }
            else if (days == 1)
                dayOne = dayTwo = day;
            else
            {
                dayTwo = day.AddDays(-day.Day + 1);
                dayOne = day.AddMonths(1).AddDays(-day.Day - 1);
            }

            for (var day = dayTwo.Date; day <= dayOne; day = day.AddDays(1))
            {
                if (page is Expenses && dictionaryExAmout.ContainsKey(day))
                    currentAmount += dictionaryExAmout[day];
                else if (page is Income && dictionaryInAmout.ContainsKey(day))
                    currentAmount += dictionaryInAmout[day];
            }

            return (dayOne, dayTwo, Math.Round(currentAmount, 3));
        }

        void BorderCrossing(ItemCollection items, double currentAmount, Page page)
        {
            foreach (Border border in items)
            {
                if (border.Child is not StackPanel) 
                    continue;

                var children = ((StackPanel)border.Child).Children;
                double sum = double.Parse(((TextBlock)children[^2]).Text.ToString());
                double percent = currentAmount != 0 ? Math.Round((sum / currentAmount) * 100, 1) : 0;
                ((TextBlock)children[2]).Text = $"{percent}%";

                if (page is Expenses)
                    iconAmoutEx[Categories.namesEx[((TextBlock)children[1]).Text]] += sum;
                else
                    iconAmoutIn[Categories.namesIn[((TextBlock)children[1]).Text]] += sum;
            }
        }

        Border StartRe(Page page)
        {
            var text = page is Expenses ? "Расходов" : "Доходов";

            return new Border()
            {
                Child = new TextBlock()
                {
                    Text = $"{text} не было",
                    TextAlignment = TextAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    Foreground = new BrushConverter().ConvertFromString("#FF4B4B4B") as Brush
                },
                Background = new BrushConverter().ConvertFromString("#FFDADADA") as Brush,
                CornerRadius = new CornerRadius(5),
                Width = 216
            };
        }

        Border NewRectangle(double amout, double amoutAll, Brush brush, int marginCount)
        {
            return new Border()
            {
                Width = (Expenses.Rectangles.Width - marginCount*2) * (amout / amoutAll),
                Background =  brush,
                Margin = new Thickness(0, 0, 2, 0),
            };
        }

        void RectangleCrossing(double currentAmount, double[] iconAmout, StackPanel rectangles, Page page)
        {
            var colors = page is Expenses ? Categories.colorsEx : Categories.colorsIn;
            
            int marginCount = iconAmout.Length - iconAmout.Count(x => x == 0);
            for (int rec = 0; rec < iconAmout.Length; rec++)
            {
                if (iconAmout[rec] == 0) continue;
                Brush brush = (Brush)new BrushConverter().ConvertFromString(colors[rec]);
                rectangles.Children.Add(NewRectangle(iconAmout[rec], currentAmount, brush, marginCount));
            }

            if (rectangles.Children.Count == 0) 
                return;

            Border last = (Border)rectangles.Children[^1], start = (Border)rectangles.Children[0];
            if (last == start) last.CornerRadius = new CornerRadius(5, 5, 5, 5);
            else
            {
                last.CornerRadius = new CornerRadius(0, 5, 5, 0);
                start.CornerRadius = new CornerRadius(5, 0, 0, 5);
            }

            last.Margin = new Thickness(0);
        }

        public void NewTotal()
        {
            totalAmount = Math.Round(totalAmount, 3);
            Expenses.TotalAmount.Content = Income.TotalAmount.Content = $"{totalAmount}{Currency.currIcon[currentCurrenty]}";
        }

        public void GeneralExRecalculation() => 
            Recalculation(Expenses.periodsDays[Expenses.currectPeriod], Expenses.Li.Items, Expenses.Rectangles, Expenses.LocalAmount, Expenses);
        
        public void GeneralInRecalculation() => 
            Recalculation(Income.periodsDays[Income.currectPeriod], Income.Li.Items, Income.Rectangles, Income.LocalAmount, Income);    
    }
}
