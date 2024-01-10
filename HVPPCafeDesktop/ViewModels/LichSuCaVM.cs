using HVPPCafeDesktop.Models;
using HVPPCafeDesktop.Resources;
using HVPPCafeDesktop.Resources.Utilities;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Collections.Generic;
using System;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Threading.Tasks;
using System.Linq;
using System.Windows.Input;
using HVPPCafeDesktop.Views.SubViews;
using HVPPCafeDesktop.CustomControl;
using Microsoft.Win32;
using OfficeOpenXml.Style;
using OfficeOpenXml;
using System.IO;

namespace HVPPCafeDesktop.ViewModels
{
    class LichSuCaVM : BaseViewModel
    {
        private ObservableCollection<HoaDon> _ListBill = new ObservableCollection<HoaDon>();
        public ObservableCollection<HoaDon> ListBill
        {
            get => _ListBill;
            set => SetProperty(ref _ListBill, value);
        }

        private List<HoaDon> Raw = new List<HoaDon>();

        private ObservableCollection<CTHD> _Details = new ObservableCollection<CTHD>();
        public ObservableCollection<CTHD> Details
        {
            get => _Details;
            set => SetProperty(ref _Details, value);
        }

        private DateTime _StartTime;
        public DateTime StartTime
        {
            get => _StartTime;
            set => SetProperty(ref _StartTime, value);
        }

        private string _PhuongThucTT;
        public string PhuongThucTT
        {
            get => _PhuongThucTT;
            set
            {
                _PhuongThucTT = value;
                OnPropertyChanged();
                Filter(PhuongThucTT);
            }
        }

        private string _Total;
        public string Total
        {
            get => _Total;
            set => SetProperty(ref _Total, value);
        }

        private string _DetailOrderID;
        public string DetailOrderID
        {
            get => _DetailOrderID;
            set => SetProperty(ref _DetailOrderID, value);
        }

        public ICommand DetailCM { get; set; }
        public ICommand SummaryCM { get; set; }

        public LichSuCaVM()
        {
            if (DateTime.Now.Hour < 12 && DateTime.Now.Hour > 6)
            {
                StartTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 7, 0, 0);
            }
            else if (DateTime.Now.Hour < 18 && DateTime.Now.Hour > 12)
            {
                StartTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 13, 0, 0);
            }
            else if (DateTime.Now.Hour < 23 && DateTime.Now.Hour >= 18)
            {
                StartTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 18, 0, 0);
            }
            else
            {
                StartTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 0, 0);
            }

            PhuongThucTT = "Tất cả";
            GetBills();

            DetailCM = new RelayCommand<HoaDon>((p) => { return true; }, async (p) =>
            {
                DetailOrderID = "#" + p.SoHoaDon.ToString();
                await GetDetail(p.SoHoaDon);
                var window = new ChiTietHoaDon();
                window.DataContext = this;
                window.ShowDialog();
            });

            SummaryCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                Summary();
            });
        }

        private async void Summary()
        {
            string filePath = "";
            string title = $"Tổng kết ca {DateTime.Now}";

            var dialog = new SaveFileDialog();
            dialog.Filter = "Excel (*.xlsx)|*.xlsx";
            dialog.FileName = title;

            if (dialog.ShowDialog() == true)
            {
                filePath = dialog.FileName;

                if (string.IsNullOrEmpty(filePath))
                {
                    var mess = new CustomMessageBox("Đường dẫn không hợp lệ!");
                    mess.ShowDialog();
                    return;
                }

                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                using (var x = new ExcelPackage())
                {
                    x.Workbook.Properties.Title = title;
                    x.Workbook.Worksheets.Add("Sheet");
                    var ws = x.Workbook.Worksheets[0];
                    ws.Cells.Style.Font.Name = "Times New Roman";

                    string[] columnHeader = { "Số hóa đơn", "Ngày hóa đơn", "Tên món",
                        "Số lượng", "Thành tiền(VNĐ)", "Tổng tiền(VNĐ)" };
                    ws.Column(1).Width = 12;
                    ws.Column(2).Width = 15;
                    ws.Column(3).Width = 17;
                    ws.Column(4).Width = 14;
                    ws.Column(6).Width = 15;
                    ws.Column(7).Width = 16;

                    int countColumn = columnHeader.Count();
                    ws.Cells[1, 1].Value = title;
                    ws.Cells[1, 1, 1, countColumn].Merge = true;
                    ws.Cells[1, 1, 1, countColumn].Style.Font.Bold = true;
                    ws.Cells[1, 1, 1, countColumn].Style.Font.Size = 16;
                    ws.Cells[1, 1, 1, countColumn].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    int row = 2;
                    int col = 1;

                    foreach (string column in columnHeader)
                    {
                        var cell = ws.Cells[row, col];
                        cell.Value = column;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        col++;
                    }
                    var bills = Raw.OrderBy(b => b.HinhThucThanhToan);
                    foreach (var temp in bills)
                    {
                        col = 1;
                        row++;
                        ws.Cells[row, col++].Value = temp.SoHoaDon;
                        ws.Cells[row, col++].Value = temp.NgayHoaDon;
                        ws.Cells[row, countColumn].Value = temp.TriGia;

                        await GetDetail(temp.SoHoaDon);
                        foreach (var cthd in Details)
                        {
                            row++;
                            ws.Cells[row, col].Value = cthd.TenMon;
                            ws.Cells[row, col + 1].Value = cthd.SoLuong;
                            ws.Cells[row, col + 2].Value = cthd.ThanhTien;
                        }
                    }

                    Byte[] bin = x.GetAsByteArray();
                    File.WriteAllBytes(filePath, bin);
                };
                var msb = new CustomMessageBox("Xuất file thành công!");
                msb.ShowDialog();
            }
        }

        public async void GetBills()
        {
            ListBill.Clear();
            Raw.Clear();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(HVPPStringRes.BaseAPIAddress);
                var response = await client.GetAsync($"api/HoaDon/all");

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    var menu = JsonConvert.DeserializeObject<List<HoaDon>>(result);

                    if (menu == null)
                    {
                        return;
                    }

                    foreach (var item in menu)
                    {
                        Raw.Add(item);
                    }
                }
            }
            Raw = Raw.Where(hd => hd.NgayHoaDon >= StartTime).ToList();
            foreach (var item in Raw)
            {
                ListBill.Add(item);
            }
            var total = ListBill.Select(b => b.TriGia).Sum();
            Total = String.Format("{0:0,0 VND}", total);
        }

        public void Filter(string pttt)
        {
            var bills = Raw;
            if (pttt != "Tất cả")
            {
                bills = bills.Where(b => b.HinhThucThanhToan.ToLower() == pttt.ToLower()).ToList();
            }

            ListBill.Clear();
            foreach (var item in bills)
            {
                ListBill.Add(item);
            }
        }

        public async Task GetDetail(int SoHD)
        {
            Details.Clear();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(HVPPStringRes.BaseAPIAddress);
                var response = await client.GetAsync($"api/HoaDon/cthd-all/{SoHD}");

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    var menu = JsonConvert.DeserializeObject<List<CTHD>>(result);

                    if (menu == null)
                    {
                        return;
                    }

                    foreach (var item in menu)
                    {
                        Details.Add(item);
                    }
                }
            }
        }
    }
}
