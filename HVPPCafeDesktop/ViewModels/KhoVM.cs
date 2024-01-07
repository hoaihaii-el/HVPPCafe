using HVPPCafeDesktop.CustomControl;
using HVPPCafeDesktop.Models;
using HVPPCafeDesktop.Resources;
using HVPPCafeDesktop.Resources.Utilities;
using HVPPCafeDesktop.Views.SubViews;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.Win32;
using Newtonsoft.Json;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Input;
using LicenseContext = OfficeOpenXml.LicenseContext;
using Paragraph = iTextSharp.text.Paragraph;

namespace HVPPCafeDesktop.ViewModels
{
    class KhoVM : BaseViewModel
    {
        private ObservableCollection<SanPham> _ProductsCol = new ObservableCollection<SanPham>();
        public ObservableCollection<SanPham> ProductsCol
        {
            get => _ProductsCol;
            set => SetProperty(ref _ProductsCol, value);
        }

        private ObservableCollection<Receipt> _ListCTN = new ObservableCollection<Receipt>();
        public ObservableCollection<Receipt> ListCTN
        {
            get => _ListCTN;
            set => SetProperty(ref _ListCTN, value);
        }

        private List<SanPham> ProductsRaw = new List<SanPham>();

        private SanPham _ProductSelected;
        public SanPham ProductSelected
        {
            get => _ProductSelected;
            set => SetProperty(ref _ProductSelected, value);
        }

        private Receipt _Receipt = new Receipt();
        public Receipt ReceiptItem
        {
            get => _Receipt;
            set => SetProperty(ref _Receipt, value);
        }

        private Receipt _ReceiptSelected = new Receipt();
        public Receipt ReceiptSelected
        {
            get => _ReceiptSelected;
            set => SetProperty(ref _ReceiptSelected, value);
        }

        private bool _IsNewReceipt = false;
        public bool IsNewReceipt
        {
            get => _IsNewReceipt;
            set => SetProperty(ref _IsNewReceipt, value);
        }

        private string _Search;
        public string Search
        {
            get => _Search;
            set
            {
                _Search = value;
                Filter(Search, GroupFilter);
                OnPropertyChanged();
            }
        }

        private string _GroupFilter = "Tất cả";
        public string GroupFilter
        {
            get => _GroupFilter;
            set
            {
                _GroupFilter = value;
                Filter(Search, GroupFilter);
                OnPropertyChanged();
            }
        }

        public ICommand CheckCM { get; set; }
        public ICommand NewReceiptCM { get; set; }
        public ICommand ExportCM { get; set; }
        public ICommand DetailCM { get; set; }
        public ICommand AddCM { get; set; }
        public ICommand DeleteCM { get; set; }

        public KhoVM()
        {
            GetKho();

            CheckCM = new RelayCommand<object>((p) => true, (p) =>
            {
                CheckCount();

                if (ProductsCol.Count <= 0)
                {
                    var msg1 = new CustomMessageBox("Chưa có sản phẩm nào cần nhập!");
                    msg1.ShowDialog();
                    Reset();
                    return;
                }

                var msg = new CustomMessageBox("Bạn có muốn in danh sách\nsản phẩm sắp hết!", true);
                msg.ShowDialog();

                if (msg.ACCEPT())
                {
                    PrintList();
                }

                Reset();
            });

            NewReceiptCM = new RelayCommand<object>((p) => true, (p) =>
            {
                ReceiptItem.NgayNhap = DateTime.Now;
                if (ProductSelected != null)
                {
                    var msg = new CustomMessageBox("Bạn có muốn nhập thêm sản phẩm được chọn?", true);
                    msg.ShowDialog();
                    if (msg.ACCEPT())
                    {
                        IsNewReceipt = false;
                        var window = new PhieuNhapMoi();
                        window.DataContext = this;
                        ReceiptItem.TenSanPham = ProductSelected.TenSanPham;
                        ReceiptItem.DonVi = ProductSelected.DonVi;
                        ReceiptItem.Nhom = ProductSelected.Nhom;
                        ReceiptItem.MucBaoNhap = ProductSelected.MucBaoNhap;
                        window.ShowDialog();
                        return;
                    }
                }

                IsNewReceipt = true;
                var newReceipt = new PhieuNhapMoi();
                newReceipt.DataContext = this;
                newReceipt.ShowDialog();
            });

            ExportCM = new RelayCommand<object>((p) => true, (p) =>
            {
                var chooseDate = new ChooseDate();
                chooseDate.ShowDialog();
                if (chooseDate.Cancel) return;
                ExportToFile(chooseDate.DateBegin, chooseDate.DateEnd);
            });

            DetailCM = new RelayCommand<SanPham>((p) => true, (p) =>
            {
                GetCTN(p.TenSanPham);
                var window = new ChiTietNhapKho();
                window.DataContext = this;
                window.ShowDialog();
            });

            AddCM = new RelayCommand<object>((p) => true, (p) =>
            {
                NewReceipt();
            });

            DeleteCM = new RelayCommand<SanPham>((p) => true, (p) =>
            {
                DeleteItem(p.TenSanPham);
            });
        }

        private async void DeleteItem(string TenSP)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(HVPPStringRes.BaseAPIAddress);

                var response = await client.DeleteAsync($"api/SanPham/{TenSP}");
            }
        }

        private async void NewReceipt()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(HVPPStringRes.BaseAPIAddress);
                var msgBox = new CustomMessageBox(true);
                msgBox.Show();

                var json = JsonConvert.SerializeObject(ReceiptItem);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PostAsync("api/SanPham", content);

                if (response.IsSuccessStatusCode)
                {
                    msgBox.TaskDone("Nhập thành công!");
                    GetKho();
                }
            }
        }

        private async void ExportToFile(DateTime dateBegin, DateTime dateEnd)
        {
            string filePath;
            var dialog = new SaveFileDialog();
            dialog.Filter = "Excel (*.xlsx)|*.xlsx";
            dialog.FileName = $"Thông tin nhập kho từ {dateBegin.Day} - {dateBegin.Month} - {dateBegin.Year} đến {dateEnd.Day} - {dateEnd.Month} - {dateEnd.Year}";

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
                    x.Workbook.Properties.Title = $"Thông tin nhập kho từ {dateBegin.Day} - {dateBegin.Month} - {dateBegin.Year} đến {dateEnd.Day} - {dateEnd.Month} - {dateEnd.Year}";

                    x.Workbook.Worksheets.Add("Sheet");

                    var ws = x.Workbook.Worksheets[0];

                    ws.Cells.Style.Font.Name = "Times New Roman";


                    string[] columnHeader = { "Mã nhập", "Tên sản phẩm", "Đơn vị", "Đơn giá(VNĐ)", "Nhóm sản phẩm", "Số lượng", "Ngày nhập", "Nguồn nhập", "Liên lạc" };
                    ws.Column(1).Width = 10;
                    ws.Column(2).Width = 16;
                    ws.Column(3).Width = 10;
                    ws.Column(4).Width = 14;
                    ws.Column(5).Width = 15;
                    ws.Column(6).Width = 12;
                    ws.Column(7).Width = 14;
                    ws.Column(8).Width = 14;
                    ws.Column(9).Width = 12;

                    int countColumn = columnHeader.Count();
                    ws.Cells[1, 1].Value = $"Thông tin nhập kho từ {dateBegin.ToShortDateString()} đến {dateEnd.ToShortDateString()}";
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

                    var list = await GetChiTietNhap(dateBegin, dateEnd);
                    foreach (var temp in list)
                    {
                        col = 1;
                        if (temp.TenSanPham != ws.Cells[row, 2].Value.ToString()) row++;
                        row++;
                        ws.Cells[row, col++].Value = temp.MaNhap;
                        ws.Cells[row, col++].Value = temp.TenSanPham;
                        ws.Cells[row, col++].Value = temp.DonVi;
                        ws.Cells[row, col++].Value = temp.GiaNhap;
                        ws.Cells[row, col++].Value = temp.Nhom;
                        ws.Cells[row, col++].Value = temp.SoLuong;
                        ws.Cells[row, col++].Value = temp.NgayNhap;
                        ws.Cells[row, col++].Value = temp.NguonNhap;
                        ws.Cells[row, col++].Value = temp.LienLac;
                    }

                    row += 2;
                    ws.Cells[row, 4].Value = "Tổng số tiền";
                    ws.Cells[row, 4].Style.Font.Bold = true;
                    ws.Cells[row + 1, 4].Value = list.Select(p => p.GiaNhap * (decimal)p.SoLuong).Sum();


                    Byte[] bin = x.GetAsByteArray();
                    File.WriteAllBytes(filePath, bin);
                };
                var msb = new CustomMessageBox("Xuất file thành công!");
                msb.ShowDialog();
            }
        }

        public async Task<List<Receipt>> GetChiTietNhap(DateTime begin, DateTime end)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(HVPPStringRes.BaseAPIAddress);
                var request = new HttpRequestMessage(HttpMethod.Get, $"api/SanPham/chi-tiet-nhap?begin={begin}&end={end}");
                var response = await client.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    var receipts = JsonConvert.DeserializeObject<List<Receipt>>(result);
                    return receipts;
                }

                return new List<Receipt>();
            }
        }

        public async void GetCTN(string TenSP)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(HVPPStringRes.BaseAPIAddress);
                var response = await client.GetAsync($"api/SanPham/ctn/{TenSP}"); 

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    var ctn = JsonConvert.DeserializeObject<ObservableCollection<Receipt>>(result);

                    if (ctn == null) return;
                    ListCTN = ctn;
                }
            }
        }

        private void PrintList()
        {
            var sfd = new SaveFileDialog();
            sfd.Filter = "PDF (*.pdf)|*.pdf";
            sfd.FileName = "Danh sách cần nhập " + DateTime.Now.Day + "-" + DateTime.Now.Month + "-" + DateTime.Now.Year;
            if (sfd.ShowDialog() == true)
            {
                if (File.Exists(sfd.FileName))
                {
                    try
                    {
                        File.Delete(sfd.FileName);
                    }
                    catch
                    {
                        var msb = new CustomMessageBox("File đã tồn tại!");
                        msb.ShowDialog();
                    }
                }
                try
                {
                    var pdfTable = new PdfPTable(3);
                    pdfTable.DefaultCell.Padding = 3;
                    pdfTable.WidthPercentage = 100;
                    pdfTable.HorizontalAlignment = Element.ALIGN_LEFT;

                    var bf = BaseFont.CreateFont(
                        Environment.GetEnvironmentVariable("windir") + @"\fonts\TIMES.TTF",
                        BaseFont.IDENTITY_H,
                        true
                    );
                    var f = new Font(bf, 16, Font.NORMAL);

                    var cell = new PdfPCell(new Phrase("Tên sản phẩm", f));
                    pdfTable.AddCell(cell);
                    cell = new PdfPCell(new Phrase("Tồn dư", f));
                    pdfTable.AddCell(cell);
                    cell = new PdfPCell(new Phrase("Đơn vị", f));
                    pdfTable.AddCell(cell);
                    foreach (var item in ProductsCol)
                    {
                        pdfTable.AddCell(new Phrase(item.TenSanPham, f));
                        pdfTable.AddCell(new Phrase(item.TonDu.ToString(), f));
                        pdfTable.AddCell(new Phrase(item.DonVi, f));
                    }

                    using (var stream = new FileStream(sfd.FileName, FileMode.Create))
                    {
                        var pdfDoc = new Document(PageSize.A4, 50f, 50f, 40f, 40f);
                        PdfWriter.GetInstance(pdfDoc, stream);
                        pdfDoc.Open();
                        pdfDoc.Add(new Paragraph("              DANH SÁCH SẢN PHẨM CẦN NHẬP THÊM " + DateTime.Now.ToShortDateString(), f));
                        pdfDoc.Add(new Paragraph("    "));
                        pdfDoc.Add(pdfTable);
                        pdfDoc.Close();
                        stream.Close();
                    }

                    var mess = new CustomMessageBox("In thành công!");
                    mess.ShowDialog();
                }
                catch
                {
                    var msb = new CustomMessageBox("Đã có lỗi xảy ra!");
                    msb.ShowDialog();
                }
            }
        }

        public void Reset()
        {
            ProductsCol.Clear();
            foreach (var item in ProductsRaw)
            {
                ProductsCol.Add(item);
            }

            Search = "";
            GroupFilter = "Tất cả";
        }

        public async void GetKho()
        {
            ProductsRaw.Clear();
            ProductsCol.Clear();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(HVPPStringRes.BaseAPIAddress);
                var response = await client.GetAsync($"api/SanPham/all");

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    var menu = JsonConvert.DeserializeObject<ObservableCollection<SanPham>>(result);

                    if (menu == null)
                    {
                        return;
                    }

                    foreach (var item in menu)
                    {
                        ProductsRaw.Add(item);
                    }

                    ProductsCol = menu;
                }
            }
        }

        public void Filter(string search, string filter)
        {
            ProductsCol.Clear();

            if (!string.IsNullOrEmpty(filter))
            {
                var menu = filter == "Tất cả" ? ProductsRaw
                : ProductsRaw
                .Where(m => m.Nhom.ToLower() == filter.ToLower())
                .ToList();

                foreach (var item in menu)
                {
                    ProductsCol.Add(item);
                }
            }

            if (string.IsNullOrEmpty(search)) return;
            var list = ProductsCol
                .Where(m => m.TenSanPham.ToLower().Contains(search.ToLower()))
                .ToList();
            ProductsCol.Clear();

            foreach (var item in list)
            {
                ProductsCol.Add(item);
            }
        }

        public void CheckCount()
        {
            var products = ProductsRaw.Where(p => p.TonDu <= p.MucBaoNhap).ToList();

            ProductsCol.Clear();
            foreach (var item in products)
            {
                ProductsCol.Add(item);
            }
        }
    }
}
