using HVPPCafeDesktop.Models;
using HVPPCafeDesktop.Resources;
using HVPPCafeDesktop.Resources.Utilities;
using LiveCharts;
using LiveCharts.Wpf;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Windows;

namespace HVPPCafeDesktop.ViewModels
{
    class ThongKeVM : BaseViewModel
    {
        private ObservableCollection<string> _Types = new ObservableCollection<string>();
        public ObservableCollection<string> Types
        {
            get => _Types;
            set => SetProperty(ref _Types, value);
        }

        private ObservableCollection<string> _ListTime = new ObservableCollection<string>();
        public ObservableCollection<string> ListTime
        {
            get => _ListTime;
            set => SetProperty(ref _ListTime, value);
        }

        private string _SumOfPaid;
        public string SumOfPaid
        {
            get => _SumOfPaid;
            set => SetProperty(ref _SumOfPaid, value);
        }

        private string _SumOfProfit;
        public string SumOfProfit
        {
            get => _SumOfProfit;
            set => SetProperty(ref _SumOfProfit, value);
        }

        private Visibility _DateVisible;
        public Visibility DateVisible
        {
            get => _DateVisible;
            set => SetProperty(ref _DateVisible, value);
        }

        private string _DateBegin;
        public string DateBegin
        {
            get => _DateBegin;
            set
            {
                _DateBegin = value;
                if (DateBegin == null || DateEnd == null) return;
                GetRevenue("Ngày");
                OnPropertyChanged();
            }
        }
        private string _DateEnd;
        public string DateEnd
        {
            get => _DateEnd;
            set
            {
                _DateEnd = value;
                if (DateBegin == null || DateEnd == null) return;
                GetRevenue("Ngày");
                OnPropertyChanged();
            }
        }
        private SeriesCollection _SeriesCollectionRevenue;
        public SeriesCollection SeriesCollectionRevenue
        {
            get => _SeriesCollectionRevenue;
            set => SetProperty(ref _SeriesCollectionRevenue, value);
        }
        private SeriesCollection _SeriesCollectionCrowd;
        public SeriesCollection SeriesCollectionCrowd
        {
            get => _SeriesCollectionCrowd;
            set => SetProperty(ref _SeriesCollectionCrowd, value);
        }
        private SeriesCollection _SeriesCollectionTypeTable;
        public SeriesCollection SeriesCollectionTypeTable
        {
            get => _SeriesCollectionTypeTable;
            set => SetProperty(ref _SeriesCollectionTypeTable, value);
        }
        private SeriesCollection _SeriesCollectionStaff;
        public SeriesCollection SeriesCollectionStaff
        {
            get => _SeriesCollectionStaff;
            set => SetProperty(ref _SeriesCollectionStaff, value);
        }
        public Func<double, string> Formatter { get; set; }
        public Func<double, string> CrowdFormatter { get; set; }
        private string _CrowdMonth;
        public string CrowdMonth
        {
            get => _CrowdMonth;
            set
            {
                _CrowdMonth = value;
                GetCrowd();
                OnPropertyChanged();
            }
        }
        private string _StaffMonth;
        public string StaffMonth
        {
            get => _StaffMonth;
            set
            {
                _StaffMonth = value;
                GetStaffRevenue();
                OnPropertyChanged();
            }
        }
        private ObservableCollection<string> _ListMonths;
        public ObservableCollection<string> ListMonths
        {
            get => _ListMonths;
            set => SetProperty(ref _ListMonths, value);
        }
        private ObservableCollection<string> _ListCrowdMonths;
        public ObservableCollection<string> ListCrowdMonths
        {
            get => _ListCrowdMonths;
            set => SetProperty(ref _ListCrowdMonths, value);
        }
        private ObservableCollection<string> _LabelsCrowd;
        public ObservableCollection<string> LabelsCrowd
        {
            get => _LabelsCrowd;
            set => SetProperty(ref _LabelsCrowd, value);
        }
        private ObservableCollection<string> _LabelsRevenue;
        public ObservableCollection<string> LabelsRevenue
        {
            get => _LabelsRevenue;
            set => SetProperty(ref _LabelsRevenue, value);
        }
        private ObservableCollection<string> _LabelsStaff;
        public ObservableCollection<string> LabelsStaff
        {
            get => _LabelsStaff;
            set => SetProperty(ref _LabelsStaff, value);
        }

        private string _TypeSelected;
        public string TypeSelected
        {
            get => _TypeSelected;
            set
            {
                _TypeSelected = value;
                if (TypeSelected == "Theo ngày")
                {
                    DateVisible = Visibility.Visible;
                    ListTimeVisible = Visibility.Hidden;
                    DateBegin = DateTime.Now.Month + "/1/" + DateTime.Now.Year;
                    DateEnd = DateTime.Now.ToShortDateString();
                }
                if (TypeSelected == "Theo tháng")
                {
                    DateVisible = Visibility.Hidden;
                    ListTimeVisible = Visibility.Visible;
                    GetListTime("Tháng");
                    TimeSelected = ListTime[ListTime.Count - 1];
                }
                if (TypeSelected == "Theo năm")
                {
                    DateVisible = Visibility.Hidden;
                    ListTimeVisible = Visibility.Visible;
                    GetListTime("Năm");
                    TimeSelected = ListTime[ListTime.Count - 1];
                }
                OnPropertyChanged();
            }
        }
        private string _PercentProOnRevenue;
        public string PercentProOnRevenue
        {
            get => _PercentProOnRevenue;
            set => SetProperty(ref _PercentProOnRevenue, value);
        }
        private Visibility _ListTimeVisible;
        public Visibility ListTimeVisible
        {
            get => _ListTimeVisible;
            set => SetProperty(ref _ListTimeVisible, value);
        }
        private string _TimeSelected;
        public string TimeSelected
        {
            get => _TimeSelected;
            set
            {
                _TimeSelected = value;
                if (TypeSelected == "Theo tháng") GetRevenue("Tháng");
                if (TypeSelected == "Theo năm") GetRevenue("Năm");
                OnPropertyChanged();
            }
        }

        public ThongKeVM()
        {
            ListMonths = new ObservableCollection<string>();
            ListCrowdMonths = new ObservableCollection<string>();
            LabelsCrowd = new ObservableCollection<string>();
            LabelsRevenue = new ObservableCollection<string>();
            LabelsStaff = new ObservableCollection<string>();
            Types = new ObservableCollection<string>();
            ListTime = new ObservableCollection<string>();
            SeriesCollectionRevenue = new SeriesCollection();
            SeriesCollectionCrowd = new SeriesCollection();
            SeriesCollectionStaff = new SeriesCollection();
            SeriesCollectionTypeTable = new SeriesCollection();

            Formatter = value => String.Format("{0:0,0 VND}", Math.Round(value));
            CrowdFormatter = value => Math.Round(value).ToString("G");

            Types.Add("Theo ngày");
            Types.Add("Theo tháng");
            Types.Add("Theo năm");

            TypeSelected = "Theo ngày";
            DateVisible = Visibility.Visible;
            ListTimeVisible = Visibility.Hidden;
            CrowdMonth = StaffMonth = DateTime.Now.Month + "/" + DateTime.Now.Year;

            GetListMonths();
        }
        public async void GetCrowd()
        {
            SeriesCollectionCrowd.Clear();
            LabelsCrowd.Clear();

            if (CrowdMonth == null) return;

            var month = GetMonth(CrowdMonth);
            var year = GetYear(CrowdMonth);
            int[] crowd = new int[DateTime.DaysInMonth(DateTime.Now.Year, int.Parse(month))];
            for (int i = 0; i < crowd.Length; i++)
            {
                crowd[i] = 0;
                LabelsCrowd.Add("Ngày " + (i + 1).ToString());
            }

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(HVPPStringRes.BaseAPIAddress);
                var response = await client.GetAsync($"api/Stats/so-luong/{month}/{year}");

                var result = await response.Content.ReadAsStringAsync();
                var col = JsonConvert.DeserializeObject<List<StatCrowd>>(result);

                foreach (var item in col)
                {
                    crowd[item.Day - 1] = item.Count;
                }
            }

            SeriesCollectionCrowd.Add(new LineSeries
            {
                Title = "Số lượng đơn",
                Values = new ChartValues<int>(crowd)
            });
        }
        public async void GetStaffRevenue()
        {
            SeriesCollectionStaff.Clear();
            LabelsStaff.Clear();

            var month = GetMonth(StaffMonth);
            var year = GetYear(StaffMonth);
            var title = "";

            var profit = new List<int>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(HVPPStringRes.BaseAPIAddress);
                var response = await client.GetAsync($"api/Stats/ban-chay/{month}/{year}");

                var result = await response.Content.ReadAsStringAsync();
                var col = JsonConvert.DeserializeObject<List<StatSold>>(result);

                foreach (var item in col.OrderByDescending(o => o.SoLuong).Take(3))
                {
                    LabelsStaff.Add(item.TenMon);
                    profit.Add(item.SoLuong);
                }
            }

            SeriesCollectionStaff.Add(new ColumnSeries
            {
                Title = title,
                Values = new ChartValues<int>(profit)
            });
        }
        public async void GetRevenue(string type)
        {
            SeriesCollectionRevenue.Clear();
            LabelsRevenue.Clear();

            var paid = new List<decimal>();
            var profit = new List<decimal>();

            decimal sumPaid = 0, sumProfit = 0;
            if (type == "Ngày")
            {
                var index = new List<int>();

                var dt1 = DateTime.Parse(DateBegin);
                var dt2 = DateTime.Parse(DateEnd);
                for (var dt = dt1; dt <= dt2; dt = dt.AddDays(1))
                {
                    index.Add(dt.Day);
                    LabelsRevenue.Add(dt.Day.ToString());
                    paid.Add(0);
                    profit.Add(0);
                }

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(HVPPStringRes.BaseAPIAddress);
                    var response = await client.GetAsync($"api/Stats/thu/{type}/{1}/{2024}?begin={DateBegin}&end={DateEnd}");

                    var result = await response.Content.ReadAsStringAsync();
                    var col = JsonConvert.DeserializeObject<List<Profit>>(result);

                    foreach (var item in col)
                    {
                        var day = item.Time;
                        profit[index.IndexOf(day)] = item.Value;
                        sumProfit += profit[index.IndexOf(day)];
                    }
                }

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(HVPPStringRes.BaseAPIAddress);
                    var response = await client.GetAsync($"api/Stats/chi/{type}/{1}/{2024}?begin={DateBegin}&end={DateEnd}");

                    var result = await response.Content.ReadAsStringAsync();
                    var col = JsonConvert.DeserializeObject<List<Profit>>(result);

                    foreach (var item in col)
                    {
                        var day = item.Time;
                        paid[index.IndexOf(day)] = item.Value;
                        sumPaid += paid[index.IndexOf(day)];
                    }
                }
            }

            if (type == "Tháng")
            {
                if (TimeSelected == null) return;
                string month = GetMonth(TimeSelected);
                string year = GetYear(TimeSelected);

                for (int i = 1; i <= DateTime.DaysInMonth(int.Parse(year), int.Parse(month)); i++)
                {
                    LabelsRevenue.Add(i.ToString());
                    paid.Add(0);
                    profit.Add(0);
                }
                paid.Add(0);
                profit.Add(0);

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(HVPPStringRes.BaseAPIAddress);
                    var response = await client.GetAsync($"api/Stats/thu/{type}/{month}/{year}?begin={DateBegin}&end={DateEnd}");

                    var result = await response.Content.ReadAsStringAsync();
                    var col = JsonConvert.DeserializeObject<List<Profit>>(result);

                    foreach (var item in col)
                    {
                        profit[item.Time - 1] = item.Value;
                        sumProfit += profit[item.Time - 1];
                    }
                }

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(HVPPStringRes.BaseAPIAddress);
                    var response = await client.GetAsync($"api/Stats/chi/{type}/{month}/{year}?begin={DateBegin}&end={DateEnd}");

                    var result = await response.Content.ReadAsStringAsync();
                    var col = JsonConvert.DeserializeObject<List<Profit>>(result);

                    foreach (var item in col)
                    {
                        paid[item.Time - 1] = item.Value;
                        sumPaid += paid[item.Time - 1];
                    }
                }
            }

            if (type == "Năm")
            {
                if (TimeSelected == null) return;
                string year = TimeSelected;
                for (int i = 1; i <= 12; i++)
                {
                    LabelsRevenue.Add(i.ToString() + "/" + year);
                    paid.Add(0);
                    profit.Add(0);
                }

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(HVPPStringRes.BaseAPIAddress);
                    var response = await client.GetAsync($"api/Stats/thu/{type}/{1}/{year}?begin={DateBegin}&end={DateEnd}");

                    var result = await response.Content.ReadAsStringAsync();
                    var col = JsonConvert.DeserializeObject<List<Profit>>(result);

                    foreach (var item in col)
                    {
                        profit[item.Time - 1] = item.Value;
                        sumProfit += profit[item.Time - 1];
                    }
                }

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(HVPPStringRes.BaseAPIAddress);
                    var response = await client.GetAsync($"api/Stats/chi/{type}/{1}/{year}?begin={DateBegin}&end={DateEnd}");

                    var result = await response.Content.ReadAsStringAsync();
                    var col = JsonConvert.DeserializeObject<List<Profit>>(result);

                    foreach (var item in col)
                    {
                        paid[item.Time - 1] = item.Value;
                        sumPaid += paid[item.Time - 1];
                    }
                }
            }

            SeriesCollectionRevenue.Add(new LineSeries
            {
                Title = "Thu",
                Values = new ChartValues<decimal>(profit)
            });
            SeriesCollectionRevenue.Add(new LineSeries
            {
                Title = "Chi",
                Values = new ChartValues<decimal>(paid)
            });
            SumOfPaid = String.Format("{0:0,0 VND}", Math.Round(sumPaid));
            SumOfProfit = String.Format("{0:0,0 VND}", Math.Round(sumProfit));
        }
        public void GetListTime(string month)
        {
            ListTime.Clear();

            if (month == "Tháng")
            {
                for (int i = 6; i <= 12; i++)
                {
                    ListTime.Add(i.ToString() + "/" + (DateTime.Now.Year - 1));
                }
                for (int i = 1; i <= DateTime.Now.Month; i++)
                {
                    ListTime.Add(i.ToString() + "/" + DateTime.Now.Year);
                }
            }
            else
            {
                for (int i = DateTime.Now.Year - 3; i <= DateTime.Now.Year; i++)
                {
                    ListTime.Add(i.ToString());
                }
            }
        }
        public void GetListMonths()
        {
            ListCrowdMonths.Clear();
            ListMonths.Clear();
            for (int i = 6; i <= 12; i++)
            {
                ListMonths.Add(i.ToString() + "/" + (DateTime.Now.Year - 1));
                ListCrowdMonths.Add(i.ToString() + "/" + (DateTime.Now.Year - 1));
            }
            int currentMonth = DateTime.Now.Month;
            for (int i = 1; i <= currentMonth; i++)
            {
                ListMonths.Add(i.ToString() + "/" + DateTime.Now.Year);
                ListCrowdMonths.Add(i.ToString() + "/" + DateTime.Now.Year);
            }
        }
        private string GetMonth(string dt)
        {
            int i = 0;
            string month = "";
            while (i < dt.Length && dt[i] != '/')
            {
                month += dt[i];
                i++;
            }
            return month;
        }

        private string GetYear(string dt)
        {
            return dt.Substring(dt.IndexOf("/") + 1);
        }
    }
}
