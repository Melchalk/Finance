using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Финансы
{
    public partial class ListCategories : Page
    {
        public ListCategories()
        {
            InitializeComponent();
            names = new Dictionary<string, Label>()
            {
                {"Prezent", PrezentT}, {"Health", HealthT}, {"Home", HomeT }, {"Beauty", BeautyT}, {"Cafe", CafeT }, {"Other", OtherT},
                {"Investment", InvestmentT}, {"Transport", TransportT}, {"Play", PlayT}, {"Shop", ShopT}, {"Salary", SalaryT}
            };

            NewExpenses = new(); NewIncome = new();
            buttonEx = new Dictionary<string, Button>()
            {
                { "Инвестиции", NewExpenses.Investment }, { "Другое", NewExpenses.Other }, { "Красота", NewExpenses.Beauty }, 
                { "Здоровье", NewExpenses.Health }, { "Развлечения", NewExpenses.Play }, {"Подарки", NewExpenses.Prezent }, 
                { "Рестораны", NewExpenses.Cafe }, { "Магазины", NewExpenses.Shop }, { "Дом", NewExpenses.Home }, { "Транспорт", NewExpenses.Transport },
            };
            buttonIn = new Dictionary<string, Button>()
            {
                { "Инвестиции", NewIncome.Investment }, { "Другое", NewIncome.Other }, {"Зарплата", NewIncome.Salary }, {"Подарки", NewIncome.Prezent},
            };
        }
        NewExpenses NewExpenses;
        NewIncome NewIncome;

        public Dictionary<string, Label> names;

        public Dictionary<string, int> namesEx = new Dictionary<string, int>()
        {
            { "Инвестиции", 0}, { "Другое", 1 }, { "Красота", 2 }, { "Здоровье", 3 }, { "Развлечения", 4 }, 
            {"Подарки", 5 }, { "Рестораны", 6 }, { "Магазины", 7 }, { "Дом", 8 }, { "Транспорт", 9 },
        },
        namesIn = new Dictionary<string, int>()
        {
            { "Инвестиции", 0 }, { "Другое", 1 }, {"Зарплата", 2}, {"Подарки", 3}, 
        };

        public Dictionary<string, Button> buttonEx, buttonIn;

        public string[] colorsEx = new[]{ "#F0F922", "#FCCD25", "#FCA635", "#F1844B", "#E16462", "#B12A90", "#8F0DA3", "#6A00A8", "#41049F", "#0D0888" },
            colorsIn = new[] { "#F0F922", "#FCCD25", "#CC4678", "#B12A90" };

        public string TextN(string name)
        {
            return (string)names[name].Content;
        }
    }
}
