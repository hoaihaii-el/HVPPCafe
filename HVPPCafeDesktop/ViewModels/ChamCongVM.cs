using HVPPCafeDesktop.CustomControl;
using HVPPCafeDesktop.Models;
using HVPPCafeDesktop.Resources;
using HVPPCafeDesktop.Resources.Utilities;
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
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HVPPCafeDesktop.ViewModels
{
    class ChamCongVM : BaseViewModel
    {
        private ObservableCollection<ChamCong> _ListStaff = new ObservableCollection<ChamCong>();
        public ObservableCollection<ChamCong> ListStaff
        {
            get => _ListStaff;
            set => SetProperty(ref _ListStaff, value);
        }

        private ObservableCollection<ChiTietCC> _ListCheck = new ObservableCollection<ChiTietCC>();
        public ObservableCollection<ChiTietCC> ListCheck
        {
            get => _ListCheck;
            set => SetProperty(ref _ListCheck, value);
        }

        private ObservableCollection<string> _ListMonth = new ObservableCollection<string>();
        public ObservableCollection<string> ListMonth
        {
            get => _ListMonth;
            set => SetProperty(ref _ListMonth, value);
        }

        private ObservableCollection<string> _ListDay = new ObservableCollection<string>();
        public ObservableCollection<string> ListDay
        {
            get => _ListDay;
            set => SetProperty(ref _ListDay, value);
        }

        private string _MonthSelected;
        public string MonthSelected
        {
            get => _MonthSelected;
            set 
            {
                _MonthSelected = value;
                OnPropertyChanged();
                GetListDay();
                GetListStaff();
            }
        }

        private string _DaySelected;
        public string DaySelected
        {
            get => _DaySelected;
            set 
            {
                _DaySelected = value;
                OnPropertyChanged();
                GetListCheck();
            }
        }

        public ICommand ImportCM { get; set; }
        public ICommand ExportCM { get; set; }
        public ICommand SaveCM { get; set; }

        public ChamCongVM()
        {
            GetListMonth();
            MonthSelected = ListMonth[ListMonth.Count - 1];

            ImportCM = new RelayCommand<object>((p) => true, (p) =>
            {
                ImportFile();
            });

            ExportCM = new RelayCommand<object>((p) => true, (p) =>
            {
                ExportFile();
            });

            SaveCM = new RelayCommand<object>((p) => true, (p) =>
            {
                SaveDetail();
            });
        }

        private async void SaveDetail()
        {
            if (!IsValidHour())
            {
                var msg1 = new CustomMessageBox("Số giờ không hợp lệ!");
                msg1.ShowDialog();
                return;
            }

            try
            {
                foreach (var cc in ListCheck)
                {
                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri(HVPPStringRes.BaseAPIAddress);

                        var tar = new
                        {
                            MaNV = cc.MaNV,
                            Ngay = DateTime.Parse(DaySelected),
                            SoGio = cc.SoGio,
                            GhiChu = cc.GhiChu
                        };

                        var json = JsonConvert.SerializeObject(tar);
                        var content = new StringContent(json, Encoding.UTF8, "application/json");
                        var response = await client.PutAsync("api/NhanVien/chi-tiet", content);

                        if (!response.IsSuccessStatusCode)
                        {
                            throw new Exception();
                        }
                    }
                }
            }
            catch
            {
                var msg2 = new CustomMessageBox("Đã có lỗi xảy ra!");
                msg2.ShowDialog();
                return;
            }

            var msg = new CustomMessageBox("Sửa thành công!");
            msg.ShowDialog();
        }

        private bool IsValidHour()
        {
            foreach (var cc in ListCheck)
            {
                if (!double.TryParse(cc.SoGio.ToString(), out _)) return false;
            }
            return true;
        }

        private void ExportFile()
        {
            var filePath = "";
            var dialog = new SaveFileDialog();
            dialog.Filter = "Excel (*.xlsx)|*.xlsx";
            dialog.FileName = "Bảng chấm công tháng " + DateTime.Now.Month + "-" + DateTime.Now.Year;

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
                    x.Workbook.Properties.Title = "Chấm công tháng " + GetMonth(MonthSelected) + "/" + DateTime.Now.Year;
                    x.Workbook.Worksheets.Add("Sheet");
                    var ws = x.Workbook.Worksheets[0];
                    ws.Cells.Style.Font.Name = "Times New Roman";

                    for (int i = 0; i < ListStaff.Count + 1; i++)
                    {
                        ws.Column(i + 1).Width = 20;
                    }

                    int countColumn = ListStaff.Count();
                    ws.Cells[1, 1].Value = "Bảng chấm công tháng " + GetMonth(MonthSelected) + "/" + DateTime.Now.Year;
                    ws.Cells[1, 1, 1, countColumn].Merge = true;
                    ws.Cells[1, 1, 1, countColumn].Style.Font.Bold = true;
                    ws.Cells[1, 1, 1, countColumn].Style.Font.Size = 16;
                    ws.Cells[1, 1, 1, countColumn].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    int row = 3;
                    int col = 2;
                    foreach (var nv in ListStaff)
                    {
                        var cell = ws.Cells[row, col];
                        cell.Value = nv.HoTen;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        col += 2;
                    }

                    row++;
                    foreach (var date in ListDay)
                    {
                        ws.Cells[row, 1].Value = date;
                        col = 2;
                        DaySelected = date;
                        foreach (var cc in ListCheck)
                        {
                            ws.Cells[row, col].Value = Math.Round(cc.SoGio, 2);
                            ws.Cells[row, col + 1].Value = cc.GhiChu;
                            col += 2;
                        }
                        row++;
                    }

                    ws.Cells[row, 1].Value = "Tổng số giờ";
                    ws.Cells[row, 1].Style.Font.Bold = true;
                    for (int i = 0; i < ListStaff.Count; i++)
                    {
                        ws.Cells[row, i + 2].Value = Math.Round(ListStaff[i].TongSogio, 2);
                    }

                    Byte[] bin = x.GetAsByteArray();
                    File.WriteAllBytes(filePath, bin);
                };
                var msb = new CustomMessageBox("Xuất file thành công!");
                msb.ShowDialog();
            }
        }

        private async void ImportFile()
        {
            var filePath = "";
            var dialog = new OpenFileDialog();
            dialog.Filter = "Excel (*.xlsx)|*.xlsx";

            if (dialog.ShowDialog() == true)
            {
                filePath = dialog.FileName;
                if (File.Exists(filePath))
                {
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    var excelFile = new FileInfo(filePath);
                    using (var package = new ExcelPackage(excelFile))
                    {
                        var ws = package.Workbook.Worksheets[0];
                        int rowCount = ws.Dimension.Rows + 10;
                        int colCount = ws.Dimension.Columns;
                        var startDate = DateTime.Now;
                        while (startDate.DayOfWeek != DayOfWeek.Monday)
                        {
                            startDate = startDate.AddDays(1);
                        }

                        var endDate = startDate.AddDays(6);

                        for (int row = 1; row <= rowCount; row++)
                        {
                            var value = ws.Cells[row, 1].Value?.ToString()?.ToLower();
                            if (value == "ca 1" || value == "ca1" || value == "ca 2" || value == "ca2" || value == "ca 3" || value == "ca3")
                            {
                                var col = 1;
                                for (DateTime dt = startDate; dt <= endDate; dt = dt.AddDays(1))
                                {
                                    col++;
                                    if (ws.Cells[row, col].Value == null) continue;
                                    await AutoChamCong(dt, ws.Cells[row, col].Value.ToString() ?? "", 5);
                                }
                            }
                            if (value == "ca 4" || value == "ca4")
                            {
                                var col = 1;
                                for (DateTime dt = startDate; dt <= endDate; dt = dt.AddDays(1))
                                {
                                    col++;
                                    if (ws.Cells[row, col].Value == null) continue;
                                    await AutoChamCong(dt, ws.Cells[row, col].Value.ToString() ?? "", 7.5);
                                }
                            }
                        }
                    }
                }
            }
        }

        public async Task AutoChamCong(DateTime date, string maMV, double soGio)
        {
            if (maMV == "") return;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(HVPPStringRes.BaseAPIAddress);

                var tar = new
                {
                    MaNV = maMV.ToUpper(),
                    Ngay = date,
                    SoGio = soGio,
                    GhiChu = ""
                };

                var json = JsonConvert.SerializeObject(tar);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PutAsync("api/NhanVien/chi-tiet", content);

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception();
                }
            }
        }

        private async void GetListStaff()
        {
            if (string.IsNullOrEmpty(MonthSelected)) return;

            ListStaff.Clear();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(HVPPStringRes.BaseAPIAddress);
                var response = await client.GetAsync($"api/NhanVien/cham-cong/{GetMonth(MonthSelected)}/{MonthSelected.Substring(MonthSelected.IndexOf("/") + 1)}");

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    var staffs = JsonConvert.DeserializeObject<List<ChamCong>>(result);

                    foreach (var item in staffs)
                    {
                        ListStaff.Add(item);
                    }
                }
            }
            DaySelected = ListDay[ListDay.Count - 1];
        }


        private async void GetListCheck()
        {
            if (string.IsNullOrEmpty(DaySelected)) return;
            ListCheck.Clear();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(HVPPStringRes.BaseAPIAddress);
                var response = await client.GetAsync($"api/NhanVien/chi-tiet-cc?date={DaySelected}");

                var result = await response.Content.ReadAsStringAsync();
                var ct = JsonConvert.DeserializeObject<List<ChiTietCC>>(result);

                if (ct == null || ct.Count <= 0)
                {
                    for (int i = 0; i < ListStaff.Count; i++)
                    {
                        ListCheck.Add(new ChiTietCC
                        {
                            MaNV = ListStaff[i].MaNV,
                            SoGio = 0,
                            GhiChu = ""
                        });
                    }
                }
                else
                {
                    foreach (var item in ct)
                    {
                        ListCheck.Add(item);
                    }
                    while (ListCheck.Count < ListStaff.Count)
                    {
                        ListCheck.Add(new ChiTietCC
                        {
                            MaNV = ListStaff[ListCheck.Count].MaNV,
                            SoGio = 0,
                            GhiChu = ""
                        });
                    }
                }
            }
        }


        private void GetListMonth()
        {
            ListMonth.Clear();
            for (int i = 6; i <= 12; i++)
            {
                ListMonth.Add(i.ToString() + "/" + (DateTime.Now.Year - 1));
            }
            int month = 1;
            while (month <= DateTime.Now.Month)
            {
                ListMonth.Add(month.ToString() + "/" + DateTime.Now.Year.ToString());
                month++;
            }
        }

        private void GetListDay()
        {
            ListDay.Clear();
            var month = GetMonth(MonthSelected);
            var year = MonthSelected.Substring(MonthSelected.IndexOf("/") + 1);

            var daysInMonth = DateTime.DaysInMonth(int.Parse(year), int.Parse(month));
            for (int i = 1; i <= daysInMonth; i++)
            {
                ListDay.Add($"{month}/{i}/{year}");
            }
        }

        private string GetMonth(string dt)
        {
            string temp = "";
            int i = 0;
            while (dt[i] != '/')
            {
                temp += dt[i];
                i++;
            }
            return temp;
        }
    }
}
