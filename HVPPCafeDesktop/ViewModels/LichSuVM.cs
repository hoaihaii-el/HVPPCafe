using HVPPCafeDesktop.CustomControl;
using HVPPCafeDesktop.Models;
using HVPPCafeDesktop.Resources;
using HVPPCafeDesktop.Resources.Utilities;
using HVPPCafeDesktop.Views.SubViews;
using Microsoft.Win32;
using Newtonsoft.Json;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.TextFormatting;

namespace HVPPCafeDesktop.ViewModels
{
    class LichSuVM : BaseViewModel
    {
        private ObservableCollection<HoaDon> _ListBill = new ObservableCollection<HoaDon>();
        public ObservableCollection<HoaDon> ListBill
        {
            get => _ListBill;
            set => SetProperty(ref _ListBill, value);
        }

        private List<HoaDon> Raw = new List<HoaDon>();

        private HoaDon _BillSelected;
        public HoaDon BillSelected
        {
            get => _BillSelected;
            set => SetProperty(ref _BillSelected, value);
        }

        private ObservableCollection<CTHD> _Details = new ObservableCollection<CTHD>();
        public ObservableCollection<CTHD> Details
        {
            get => _Details;
            set => SetProperty(ref _Details, value);
        }

        private ObservableCollection<string> _Staffs = new ObservableCollection<string>();
        public ObservableCollection<string> Staffs
        {
            get => _Staffs;
            set => SetProperty(ref _Staffs, value);
        }

        private string _DetailOrderID;
        public string DetailOrderID
        {
            get => _DetailOrderID;
            set => SetProperty(ref _DetailOrderID, value);
        }

        private string _Search;
        public string Search
        {
            get => _Search;
            set
            {
                _Search = value;
                OnPropertyChanged();
                Filter(Search, DateBegin, DateEnd, FilterMaNV);
            }
        }

        private string _FilterMaNV;
        public string FilterMaNV
        {
            get => _FilterMaNV;
            set
            {
                _FilterMaNV = value;
                OnPropertyChanged();
                Filter(Search, DateBegin, DateEnd, FilterMaNV);
            }
        }

        private string _DateBegin;
        public string DateBegin
        {
            get => _DateBegin;
            set
            {
                _DateBegin = value;
                OnPropertyChanged();
                Filter(Search, DateBegin, DateEnd, FilterMaNV);
            }
        }

        private string _DateEnd;
        public string DateEnd
        {
            get => _DateEnd;
            set
            {
                _DateEnd = value;
                OnPropertyChanged();
                Filter(Search, DateBegin, DateEnd, FilterMaNV);
            }
        }

        public ICommand DetailCM { get; set; }
        public ICommand ExportCM { get; set; }

        public LichSuVM()
        {
            GetBills();
            GetStaffs();
            DateBegin = DateTime.Now.ToShortDateString();
            DateEnd = DateTime.Now.ToShortDateString();

            DetailCM = new RelayCommand<HoaDon>((p) => { return true; }, async (p) =>
            {
                DetailOrderID = "#" + p.SoHoaDon.ToString();
                await GetDetail(p.SoHoaDon);
                var window = new ChiTietHoaDon();
                window.DataContext = this;
                window.ShowDialog();
            });

            ExportCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                Export();
            });
        }

        private async void Export()
        {
            var chooseDate = new ChooseDate();
            chooseDate.ShowDialog();
            if (chooseDate.Cancel)
            {
                return;
            }

            var dt1 = chooseDate.DateBegin;
            var dt2 = chooseDate.DateEnd;

            string filePath = "";
            string title = "Chi tiết hóa đơn từ " + dt1.Month + "-" + dt1.Day + "-" + dt1.Year + " đến " + dt2.Month + "-" + dt2.Day + "-" + dt2.Year;

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
                    Filter("", dt1.ToShortDateString(), dt2.ToShortDateString(), "Tất cả");
                    foreach (var temp in ListBill)
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

        public async void GetStaffs()
        {
            FilterMaNV = "Tất cả";
            Staffs.Clear();
            Staffs.Add("Tất cả");
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(HVPPStringRes.BaseAPIAddress);
                var response = await client.GetAsync($"api/NhanVien/all");

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    var menu = JsonConvert.DeserializeObject<List<NhanVien>>(result);

                    if (menu == null)
                    {
                        return;
                    }

                    foreach (var item in menu)
                    {
                        Staffs.Add(item.MaNV);
                    }
                }
            }
        }

        private void Filter(string search, string dateBein, string dateEnd, string MaNV)
        {
            var bill = string.IsNullOrEmpty(search) ? Raw : Raw.Where(h => h.SoHoaDon.ToString().Contains(search));

            if (!string.IsNullOrEmpty(dateBein) && !string.IsNullOrEmpty(dateEnd))
            {
                bill = bill.Where(h => h.NgayHoaDon.Date >= DateTime.Parse(dateBein)
                                    && h.NgayHoaDon.Date <= DateTime.Parse(dateEnd));
            }
            if (MaNV != "Tất cả")
            {
                bill = bill.Where(b => b.MaNV == MaNV);
            }

            ListBill.Clear();
            foreach (var h in bill)
            {
                ListBill.Add(h);
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
                        ListBill.Add(item);
                        Raw.Add(item);
                    }
                }
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
