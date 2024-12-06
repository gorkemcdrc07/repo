using DevExpress.XtraGrid;
using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using System.IO;  // Excel dosyasını işlemek için gerekli namespace
using System.Runtime.InteropServices;
using SiparişOtomasyon.entity;
using SiparişOtomasyon.Yükleme_Noktaları;
using SiparişOtomasyon.Teslim_Noktaları;
using DevExpress.XtraBars;
using System.Collections.Generic;
using System.Drawing;
using System.Data;
using OfficeOpenXml;
using OfficeOpenXml.Table;
using DevExpress.XtraGrid.Views.Grid;


namespace SiparişOtomasyon.Sipariş_Ekranları
{
    public partial class GezerSiparisEkranı : Form
    {
        private BindingList<GezerVeri> gridData;

        public GezerSiparisEkranı()
        {
            InitializeComponent();
            DataTable dt = new DataTable();
            dt.Columns.Add("TeslimFirmaAdresAdı", typeof(string));  // TeslimFirmaAdresAdı sütunu
            dt.Columns.Add("AlıcıFirmaCariUnvanı", typeof(string)); // AlıcıFirmaCariUnvanı sütunu
            dt.Columns.Add("İstenilenAracTipi", typeof(string));   // İstenilenAracTipi sütunu
            gridControlGezer.DataSource = dt;

            gridData = new BindingList<GezerVeri>();
            gridControlGezer.DataSource = gridData;

            // DragEnter ve DragDrop olaylarını elle bağlama
            gridControlGezer.AllowDrop = true; // AllowDrop özelliğini true yapmamız gerekir
            gridControlGezer.DragEnter += gridControlGezer_DragEnter;
            gridControlGezer.DragDrop += gridControlGezer_DragDrop;
        }

        // GridControl'e eklenmek üzere kullanılacak veri modeli
        public class GezerVeri
        {
            public string YuklemeFirmasıAdresAdı { get; set; }
            public string TeslimFirmaAdresAdı { get; set; }
            public string MusteriSiparisNo { get; set; }
            public string İstenilenAracTipi { get; set; }
            public string Proje { get; set; }
            public string SiparisTarihi { get; set; }
            public string YuklemeTarihi { get; set; }
            public string TeslimTarihi { get; set; }
            public string AlıcıFirmaCariUnvanı {  get; set; }

            // Yeni özellikler
            public string MusteriVkn { get; set; } = "3950041441";
            public int Urun { get; set; } = 7;
            public int KapAdet { get; set; } = 25;
            public int AmbalajTipi { get; set; } = 1;
            public string BrutKG { get; set; } = "25.000";

        }

        // DragEnter olayını doğru şekilde işleme
        private void gridControlGezer_DragEnter(object sender, DragEventArgs e)
        {
            // Dosya tipinin Excel olduğundan emin olun
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] filePaths = e.Data.GetData(DataFormats.FileDrop) as string[];
                if (filePaths != null && filePaths.Length > 0 &&
                    (filePaths[0].EndsWith(".xls") || filePaths[0].EndsWith(".xlsx")))
                {
                    e.Effect = DragDropEffects.Copy; // Dosya bırakılabilir
                }
                else
                {
                    e.Effect = DragDropEffects.None; // Excel dosyası dışında bir şey bırakılmaya çalışılıyorsa engelle
                }
            }
            else
            {
                e.Effect = DragDropEffects.None; // Dosya tipi geçerli değilse engelle
            }
        }

        // DragDrop olayını işleme
        private void gridControlGezer_DragDrop(object sender, DragEventArgs e)
        {
            // Excel dosyasını sürükleyip bırakınca veriyi alıyoruz
            string[] filePath = e.Data.GetData(DataFormats.FileDrop) as string[];
            if (filePath != null && filePath.Length > 0)
            {
                string excelFilePath = filePath[0];
                if (File.Exists(excelFilePath))
                {
                    LoadExcelData(excelFilePath); // Excel dosyasını yükleme işlemi
                }
            }
        }

        // Excel dosyasını okuma ve verileri işleme metodu
        private void LoadExcelData(string excelFilePath)
        {
            var excelApp = new Microsoft.Office.Interop.Excel.Application();
            var workbook = excelApp.Workbooks.Open(excelFilePath);
            var worksheet = (Microsoft.Office.Interop.Excel.Worksheet)workbook.Sheets[1];
            var range = worksheet.UsedRange;
            var rowCount = range.Rows.Count;

            try
            {
                // Excel'in her satırındaki verileri alıp grid'e ekleme
                for (int i = 2; i <= rowCount; i++) // Başlık satırından sonrasına başlıyoruz
                {
                    string musteriSiparisNo = range.Cells[i, 3].Value2?.ToString();  // C sütunundaki veri
                    string yuklemeYeri1 = range.Cells[i, 4].Value2?.ToString();
                    string teslimNoktasi = range.Cells[i, 11].Value2?.ToString();
                    string varis1 = range.Cells[i, 7].Value2?.ToString();
                    string varis2 = range.Cells[i, 8].Value2?.ToString();
                    string varis3 = range.Cells[i, 9].Value2?.ToString();
                    string varis4 = range.Cells[i, 10].Value2?.ToString();
                    string yuklemeYeri2 = range.Cells[i, 5].Value2?.ToString();
                    string yuklemeYeri3 = range.Cells[i, 6].Value2?.ToString();
                    string aracTipi = range.Cells[i, 15].Value2?.ToString();

                    // 1.Yük Yeri (D) dolu, 2.Yük Yeri (E) boş, 3.Yük Yeri (F) boş
                    if (!string.IsNullOrEmpty(yuklemeYeri1) &&
                        string.IsNullOrEmpty(range.Cells[i, 5].Value2?.ToString()) &&
                        string.IsNullOrEmpty(range.Cells[i, 6].Value2?.ToString()))
                    {
                        string[] teslimler = teslimNoktasi?.Split('-');
                        if (teslimler != null)
                        {
                            if (teslimler.Length == 1)
                            {
                                gridData.Add(new GezerVeri
                                {
                                    MusteriSiparisNo = musteriSiparisNo,
                                    YuklemeFirmasıAdresAdı = yuklemeYeri1,
                                    TeslimFirmaAdresAdı = $"{teslimler[0]} ({varis1})",
                                    İstenilenAracTipi = aracTipi,
                                });
                            }
                            else if (teslimler.Length == 2)
                            {
                                gridData.Add(new GezerVeri
                                {
                                    MusteriSiparisNo = musteriSiparisNo,
                                    YuklemeFirmasıAdresAdı = yuklemeYeri1,
                                    TeslimFirmaAdresAdı = $"{teslimler[0]} ({varis1})",
                                    İstenilenAracTipi = aracTipi,
                                });
                                gridData.Add(new GezerVeri
                                {
                                    MusteriSiparisNo = musteriSiparisNo,
                                    YuklemeFirmasıAdresAdı = yuklemeYeri1,
                                    TeslimFirmaAdresAdı = $"{teslimler[1]} ({varis2})",
                                    İstenilenAracTipi = aracTipi,
                                });
                            }
                            else if (teslimler.Length == 3)
                            {
                                gridData.Add(new GezerVeri
                                {
                                    MusteriSiparisNo = musteriSiparisNo,
                                    YuklemeFirmasıAdresAdı = yuklemeYeri1,
                                    TeslimFirmaAdresAdı = $"{teslimler[0]} ({varis1})",
                                    İstenilenAracTipi = aracTipi,
                                });
                                gridData.Add(new GezerVeri
                                {
                                    MusteriSiparisNo = musteriSiparisNo,
                                    YuklemeFirmasıAdresAdı = yuklemeYeri1,
                                    TeslimFirmaAdresAdı = $"{teslimler[1]} ({varis2})",
                                    İstenilenAracTipi = aracTipi,
                                });
                                gridData.Add(new GezerVeri
                                {
                                    MusteriSiparisNo = musteriSiparisNo,
                                    YuklemeFirmasıAdresAdı = yuklemeYeri1,
                                    TeslimFirmaAdresAdı = $"{teslimler[2]} ({varis3})",
                                    İstenilenAracTipi = aracTipi,
                                });
                            }
                            else if (teslimler.Length == 4)
                            {
                                gridData.Add(new GezerVeri
                                {
                                    MusteriSiparisNo = musteriSiparisNo,
                                    YuklemeFirmasıAdresAdı = yuklemeYeri1,
                                    TeslimFirmaAdresAdı = $"{teslimler[0]} ({varis1})",
                                    İstenilenAracTipi = aracTipi,
                                });
                                gridData.Add(new GezerVeri
                                {
                                    MusteriSiparisNo = musteriSiparisNo,
                                    YuklemeFirmasıAdresAdı = yuklemeYeri1,
                                    TeslimFirmaAdresAdı = $"{teslimler[1]} ({varis2})",
                                    İstenilenAracTipi = aracTipi,
                                });
                                gridData.Add(new GezerVeri
                                {
                                    MusteriSiparisNo = musteriSiparisNo,
                                    YuklemeFirmasıAdresAdı = yuklemeYeri1,
                                    TeslimFirmaAdresAdı = $"{teslimler[2]} ({varis3})",
                                    İstenilenAracTipi = aracTipi,
                                });
                                gridData.Add(new GezerVeri
                                {
                                    MusteriSiparisNo = musteriSiparisNo,
                                    YuklemeFirmasıAdresAdı = yuklemeYeri1,
                                    TeslimFirmaAdresAdı = $"{teslimler[3]} ({varis4})",
                                    İstenilenAracTipi = aracTipi,
                                });
                            }
                        }
                    }

                    // 2. Kural: D ve E sütunu dolu, F sütunu boş
                    if (!string.IsNullOrEmpty(yuklemeYeri1) && !string.IsNullOrEmpty(yuklemeYeri2) &&
                        string.IsNullOrEmpty(range.Cells[i, 6].Value2?.ToString()))
                    {
                        string[] teslimler = teslimNoktasi?.Split('-');
                        string kValue = range.Cells[i, 11].Value2?.ToString(); // K sütunu
                        string musteriSiparisNo2 = range.Cells[i, 3].Value2?.ToString(); // C sütunu
                        string aracTipi2 = range.Cells[i, 15].Value2?.ToString();

                        if (teslimler != null)
                        {
                            // K sütununda - işareti yoksa
                            if (!kValue.Contains("-"))
                            {
                                gridData.Add(new GezerVeri
                                {
                                    YuklemeFirmasıAdresAdı = yuklemeYeri1,
                                    TeslimFirmaAdresAdı = $"{kValue} ({varis1})",
                                    MusteriSiparisNo = musteriSiparisNo2,
                                    İstenilenAracTipi = aracTipi2
                                });
                                gridData.Add(new GezerVeri
                                {
                                    YuklemeFirmasıAdresAdı = yuklemeYeri2,
                                    TeslimFirmaAdresAdı = $"{kValue} ({varis1})",
                                    MusteriSiparisNo = musteriSiparisNo2,
                                    İstenilenAracTipi = aracTipi2
                                });
                            }
                            // K sütununda 1 tane - işareti varsa
                            else if (kValue.Split('-').Length == 2)
                            {
                                string[] kValues = kValue.Split('-');
                                gridData.Add(new GezerVeri
                                {
                                    YuklemeFirmasıAdresAdı = yuklemeYeri1,
                                    TeslimFirmaAdresAdı = $"{kValues[0].Trim()} ({varis1})",
                                    MusteriSiparisNo = musteriSiparisNo2,
                                    İstenilenAracTipi = aracTipi2
                                });
                                gridData.Add(new GezerVeri
                                {
                                    YuklemeFirmasıAdresAdı = yuklemeYeri2,
                                    TeslimFirmaAdresAdı = $"{kValues[1].Trim()} ({varis2})",
                                    MusteriSiparisNo = musteriSiparisNo2,
                                    İstenilenAracTipi = aracTipi2
                                });
                            }
                            // K sütununda 2 tane - işareti varsa
                            else if (kValue.Split('-').Length == 3)
                            {
                                string[] kValues = kValue.Split('-');
                                gridData.Add(new GezerVeri
                                {
                                    YuklemeFirmasıAdresAdı = yuklemeYeri1,
                                    TeslimFirmaAdresAdı = $"{kValues[0].Trim()} ({varis1})",
                                    MusteriSiparisNo = musteriSiparisNo2,
                                    İstenilenAracTipi = aracTipi2
                                });
                                gridData.Add(new GezerVeri
                                {
                                    YuklemeFirmasıAdresAdı = yuklemeYeri2,
                                    TeslimFirmaAdresAdı = $"{kValues[1].Trim()} ({varis2})",
                                    MusteriSiparisNo = musteriSiparisNo2,
                                    İstenilenAracTipi = aracTipi2
                                });
                                gridData.Add(new GezerVeri
                                {
                                    YuklemeFirmasıAdresAdı = yuklemeYeri2,
                                    TeslimFirmaAdresAdı = $"{kValues[2].Trim()} ({varis3})",
                                    MusteriSiparisNo = musteriSiparisNo2,
                                    İstenilenAracTipi = aracTipi2
                                });
                            }
                            // K sütununda 3 tane - işareti varsa
                            else if (kValue.Split('-').Length == 4)
                            {
                                string[] kValues = kValue.Split('-');
                                gridData.Add(new GezerVeri
                                {
                                    YuklemeFirmasıAdresAdı = yuklemeYeri1,
                                    TeslimFirmaAdresAdı = $"{kValues[0].Trim()} ({varis1})",
                                    MusteriSiparisNo = musteriSiparisNo2,
                                    İstenilenAracTipi = aracTipi2
                                });
                                gridData.Add(new GezerVeri
                                {
                                    YuklemeFirmasıAdresAdı = yuklemeYeri1,
                                    TeslimFirmaAdresAdı = $"{kValues[1].Trim()} ({varis2})",
                                    MusteriSiparisNo = musteriSiparisNo2,
                                    İstenilenAracTipi = aracTipi2
                                });
                                gridData.Add(new GezerVeri
                                {
                                    YuklemeFirmasıAdresAdı = yuklemeYeri2,
                                    TeslimFirmaAdresAdı = $"{kValues[2].Trim()} ({varis3})",
                                    MusteriSiparisNo = musteriSiparisNo2,
                                    İstenilenAracTipi = aracTipi2
                                });
                                gridData.Add(new GezerVeri
                                {
                                    YuklemeFirmasıAdresAdı = yuklemeYeri2,
                                    TeslimFirmaAdresAdı = $"{kValues[3].Trim()} ({varis4})",
                                    MusteriSiparisNo = musteriSiparisNo2,
                                    İstenilenAracTipi = aracTipi2
                                });
                            }
                        }
                    }

                    // 3. Kural: D, E ve F sütunları dolu
                    if (!string.IsNullOrEmpty(yuklemeYeri1) && !string.IsNullOrEmpty(yuklemeYeri2) && !string.IsNullOrEmpty(yuklemeYeri3))
                    {
                        string fValue = range.Cells[i, 6].Value2?.ToString();   // F sütunu
                        string kValue = range.Cells[i, 11].Value2?.ToString();  // K sütunu
                        string gValue = range.Cells[i, 7].Value2?.ToString();   // G sütunu
                        string hValue = range.Cells[i, 8].Value2?.ToString();   // H sütunu
                        string iValue = range.Cells[i, 9].Value2?.ToString();   // I sütunu
                        string jValue = range.Cells[i, 10].Value2?.ToString();  // J sütunu
                        string musteriSiparisNo3 = range.Cells[i, 3].Value2?.ToString(); // C sütunu
                        string aracTipi3 = range.Cells[i, 15].Value2?.ToString();

                        if (!string.IsNullOrEmpty(kValue))
                        {
                            string[] kParts = kValue.Split('-');

                            // K sütununda hiç - işareti yoksa
                            if (kParts.Length == 1)
                            {
                                gridData.Add(new GezerVeri
                                {
                                    YuklemeFirmasıAdresAdı = yuklemeYeri1,
                                    TeslimFirmaAdresAdı = $"{kParts[0]} ({gValue})",
                                    MusteriSiparisNo = musteriSiparisNo3,
                                    İstenilenAracTipi = aracTipi3,
                                });
                                gridData.Add(new GezerVeri
                                {
                                    YuklemeFirmasıAdresAdı = yuklemeYeri2,
                                    TeslimFirmaAdresAdı = $"{kParts[0]} ({gValue})",
                                    MusteriSiparisNo = musteriSiparisNo3,
                                    İstenilenAracTipi = aracTipi3,
                                });
                                gridData.Add(new GezerVeri
                                {
                                    YuklemeFirmasıAdresAdı = yuklemeYeri3,
                                    TeslimFirmaAdresAdı = $"{kParts[0]} ({gValue})",
                                    MusteriSiparisNo = musteriSiparisNo3,
                                    İstenilenAracTipi = aracTipi3,
                                });
                            }
                            // K sütununda 1 tane - işareti varsa
                            else if (kParts.Length == 2)
                            {
                                gridData.Add(new GezerVeri
                                {
                                    YuklemeFirmasıAdresAdı = yuklemeYeri1,
                                    TeslimFirmaAdresAdı = $"{kParts[0].Trim()} ({gValue})",
                                    MusteriSiparisNo = musteriSiparisNo3,
                                    İstenilenAracTipi = aracTipi3,
                                });
                                gridData.Add(new GezerVeri
                                {
                                    YuklemeFirmasıAdresAdı = yuklemeYeri2,
                                    TeslimFirmaAdresAdı = $"{kParts[1].Trim()} ({hValue})",
                                    MusteriSiparisNo = musteriSiparisNo3,
                                    İstenilenAracTipi = aracTipi3,
                                });
                                gridData.Add(new GezerVeri
                                {
                                    YuklemeFirmasıAdresAdı = yuklemeYeri3,
                                    TeslimFirmaAdresAdı = $"{kParts[1].Trim()} ({hValue})",
                                    MusteriSiparisNo = musteriSiparisNo3,
                                    İstenilenAracTipi = aracTipi3,
                                });
                            }
                            // K sütununda 2 tane - işareti varsa
                            else if (kParts.Length == 3)
                            {
                                gridData.Add(new GezerVeri
                                {
                                    YuklemeFirmasıAdresAdı = yuklemeYeri1,
                                    TeslimFirmaAdresAdı = $"{kParts[0].Trim()} ({gValue})",
                                    MusteriSiparisNo = musteriSiparisNo3,
                                    İstenilenAracTipi = aracTipi3,
                                });
                                gridData.Add(new GezerVeri
                                {
                                    YuklemeFirmasıAdresAdı = yuklemeYeri2,
                                    TeslimFirmaAdresAdı = $"{kParts[1].Trim()} ({hValue})",
                                    MusteriSiparisNo = musteriSiparisNo3,
                                    İstenilenAracTipi = aracTipi3,
                                });
                                gridData.Add(new GezerVeri
                                {
                                    YuklemeFirmasıAdresAdı = yuklemeYeri3,
                                    TeslimFirmaAdresAdı = $"{kParts[2].Trim()} ({iValue})",
                                    MusteriSiparisNo = musteriSiparisNo3,
                                    İstenilenAracTipi = aracTipi3,
                                });
                            }
                            // K sütununda 3 tane - işareti varsa
                            else if (kParts.Length == 4)
                            {
                                gridData.Add(new GezerVeri
                                {
                                    YuklemeFirmasıAdresAdı = yuklemeYeri1,
                                    TeslimFirmaAdresAdı = $"{kParts[0].Trim()} ({gValue})",
                                    MusteriSiparisNo = musteriSiparisNo3,
                                    İstenilenAracTipi = aracTipi3,
                                });
                                gridData.Add(new GezerVeri
                                {
                                    YuklemeFirmasıAdresAdı = yuklemeYeri2,
                                    TeslimFirmaAdresAdı = $"{kParts[1].Trim()} ({hValue})",
                                    MusteriSiparisNo = musteriSiparisNo3,
                                    İstenilenAracTipi = aracTipi3,
                                });
                                gridData.Add(new GezerVeri
                                {
                                    YuklemeFirmasıAdresAdı = yuklemeYeri3,
                                    TeslimFirmaAdresAdı = $"{kParts[2].Trim()} ({iValue})",
                                    MusteriSiparisNo = musteriSiparisNo3,
                                    İstenilenAracTipi = aracTipi3,
                                });
                                gridData.Add(new GezerVeri
                                {
                                    YuklemeFirmasıAdresAdı = yuklemeYeri3,
                                    TeslimFirmaAdresAdı = $"{kParts[3].Trim()} ({jValue})",
                                    MusteriSiparisNo = musteriSiparisNo3,
                                    İstenilenAracTipi = aracTipi3,
                                });
                            }
                        }
                    }



                }

                gridControlGezer.DataSource = gridData; // Veriyi grid'e aktar
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Bir hata oluştu: {ex.Message}");
            }
            finally
            {
                workbook.Close(false); // Excel dosyasını kapatıyoruz
                excelApp.Quit(); // Excel uygulamasını kapatıyoruz
                Marshal.ReleaseComObject(excelApp); // Excel COM nesnesini serbest bırakıyoruz
            }
        }

        private void BtnProje_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            DateTime selectedDate = DateTime.Now.Date; // Varsayılan olarak bugünün tarihini alalım, saati sıfırlayalım

            // DateTimePicker'ı program içinde oluşturuyoruz
            using (Form dateForm = new Form())
            {
                Label label = new Label { Text = "Lütfen sipariş tarihi seçin:", Dock = DockStyle.Top };
                DateTimePicker dateTimePicker = new DateTimePicker { Dock = DockStyle.Top, Format = DateTimePickerFormat.Short };
                Button confirmButton = new Button { Text = "Tamam", Dock = DockStyle.Bottom };

                confirmButton.Click += (s, args) =>
                {
                    selectedDate = dateTimePicker.Value.Date;  // Kullanıcının seçtiği tarihi al ve saati sıfırla
                    dateForm.DialogResult = DialogResult.OK;
                    dateForm.Close();
                };

                // Form elemanlarını ekle
                dateForm.Controls.Add(label);
                dateForm.Controls.Add(dateTimePicker);
                dateForm.Controls.Add(confirmButton);

                // Tarih seçildikten sonra işlemi tamamla
                if (dateForm.ShowDialog() == DialogResult.OK)
                {
                    // Seçilen tarihi aldık
                }
                else
                {
                    // Kullanıcı tarih seçmeyi iptal ettiyse, çıkış yap
                    return;
                }
            }

            // Seçilen tarihin bir gün sonrasını hesapla
            DateTime deliveryDate = selectedDate.AddDays(1);

            // gridControlGezer'deki tüm satırlara bak
            for (int rowHandle = 0; rowHandle < gridViewGezer.RowCount; rowHandle++)
            {
                gridViewGezer.SetRowCellValue(rowHandle, "SiparisTarihi", selectedDate.ToString("dd.MM.yyyy"));
                gridViewGezer.SetRowCellValue(rowHandle, "YuklemeTarihi", selectedDate.ToString("dd.MM.yyyy"));
                gridViewGezer.SetRowCellValue(rowHandle, "TeslimTarihi", deliveryDate.ToString("dd.MM.yyyy"));

                // İstenilenAracTipi sütununda kontrol yap
                var aracTipi = gridViewGezer.GetRowCellValue(rowHandle, "İstenilenAracTipi")?.ToString();

                if (aracTipi != null)
                {
                    if (aracTipi.ToUpper() == "KAMYON")
                    {
                        gridViewGezer.SetRowCellValue(rowHandle, "İstenilenAracTipi", 3);
                    }
                    else if (aracTipi.ToUpper() == "TIR")
                    {
                        gridViewGezer.SetRowCellValue(rowHandle, "İstenilenAracTipi", 1);
                    }
                }
            }

            using (var context = new DbSiparisOtomasyonEntities6())
            {
                // gridControlGezer'deki tüm satırlara bak
                for (int rowHandle = 0; rowHandle < gridViewGezer.RowCount; rowHandle++)
                {
                    // 'YuklemeFirmasıAdresAdı' sütunundaki veriyi al
                    var adres = gridViewGezer.GetRowCellValue(rowHandle, "YuklemeFirmasıAdresAdı");

                    if (adres != null)
                    {
                        // 'TblGezerProjeler' tablosunda 'Proje' sütununda arama yap
                        var projeId = context.TblGezerProjeler
                            .Where(p => p.Proje.Contains(adres.ToString())) // 'Proje' sütununda 'adres' kelimesini ara
                            .Select(p => p.projelerID)
                            .FirstOrDefault(); // İlk eşleşeni al

                        // Eğer projeId boş değilse
                        if (!string.IsNullOrEmpty(projeId))
                        {
                            // Eşleşen 'ProjeID' değerini 'gridControlGezer' Proje sütununa yaz
                            gridViewGezer.SetRowCellValue(rowHandle, "Proje", projeId);
                        }

                        // 'TblGezer' tablosunda 'MusteridenGelen' sütununda arama yap
                        var adresId = context.TblGezer
                            .Where(g => g.MusteridenGelen == adres.ToString()) // 'MusteridenGelen' sütununda 'adres' kelimesini ara
                            .Select(g => g.AdresID)
                            .FirstOrDefault(); // İlk eşleşeni al

                        // Eğer adresId null değilse
                        if (adresId != null)
                        {
                            // 'AdresID' değerini 'YuklemeFirmasıAdresAdı' sütununa yaz
                            gridViewGezer.SetRowCellValue(rowHandle, "YuklemeFirmasıAdresAdı", adresId);
                        }
                    }
                }
            }
        }




        private void BtnYuklemeNoktası_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            // GezerYuklemeNoktası formunun mevcut olup olmadığını kontrol et
            GezerYuklemeNoktası form = Application.OpenForms["GezerYuklemeNoktası"] as GezerYuklemeNoktası;

            if (form == null) // Form açılmamışsa
            {
                form = new GezerYuklemeNoktası();
                form.Show();  // Yeni formu göster
            }
            else
            {
                form.BringToFront();  // Form zaten açıksa, onu ön plana getir
            }
        }

        private void BtnEslestir_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GezerTeslimNoktalarıEslestir form = new GezerTeslimNoktalarıEslestir();
            form.Show(); // Modal olarak açılır, form kapatılmadan diğer işlemler yapılamaz
        }

        private void BtnEslesme_ItemClick(object sender, ItemClickEventArgs e)
        {
            BtnTeslimleriGetir.Enabled = true;
            BtnEslesme.Enabled = false;

            // Yükleme göstergesi oluştur
            using (var loadingForm = new Form())
            {
                loadingForm.StartPosition = FormStartPosition.CenterScreen;
                loadingForm.Size = new Size(200, 100);
                var label = new Label
                {
                    Text = "Eşleşmeler tamamlanıyor...",
                    AutoSize = true,
                    Location = new Point(20, 20)
                };
                loadingForm.Controls.Add(label);
                loadingForm.Show();

                // Adresleri gridViewGezer'tan al
                List<string> adresListesi = new List<string>();
                for (int i = 0; i < gridViewGezer.RowCount; i++)
                {
                    string adres = gridViewGezer.GetRowCellValue(i, "TeslimFirmaAdresAdı")?.ToString()?.Trim();
                    if (!string.IsNullOrEmpty(adres))
                    {
                        adresListesi.Add(adres);
                    }
                }

                // Eğer adres listesi boşsa kullanıcıya bilgi ver
                if (adresListesi.Count == 0)
                {
                    MessageBox.Show("Adres listesi boş!");
                    loadingForm.Close();
                    return;
                }

                // TeslimNoktaları formunu oluştur
                GezerTeslimNoktalarıEslestir teslimNoktalarıForm = new GezerTeslimNoktalarıEslestir();
                teslimNoktalarıForm.Show();

                // Eşleşen ve eşleşmeyen adresleri saklamak için listeler
                List<string> eslesenAdresler = new List<string>();
                List<string> eslesmeyenAdresler = new List<string>(adresListesi);

                // Adresleri eşleştirme işlemi
                foreach (var adres in adresListesi)
                {
                    // Parantez içindeki ve dışındaki verileri ayır
                    int startIndex = adres.IndexOf("(");
                    int endIndex = adres.IndexOf(")");
                    string adresDışı = startIndex >= 0 ? adres.Substring(0, startIndex).Trim() : adres;
                    string adresİçindekiVeri = (startIndex >= 0 && endIndex > startIndex) ? adres.Substring(startIndex + 1, endIndex - startIndex - 1) : null;

                    string ilce = null, il = null;
                    if (adresİçindekiVeri != null)
                    {
                        // Parantez içindeki veri - işareti ile ayrılıyor
                        string[] ilIlce = adresİçindekiVeri.Split('-');
                        if (ilIlce.Length == 2)
                        {
                            ilce = ilIlce[0].Trim();
                            il = ilIlce[1].Trim();
                        }
                    }

                    // TblEslesmelerle tablosunda arama yap
                    using (var context = new DbSiparisOtomasyonEntities6())
                    {
                        var eslesme = context.TblEslesmelerle
                            .FirstOrDefault(x => (x.AdresAdi.Contains(adresDışı) || x.Unvan.Contains(adresDışı)) &&
                                                 (string.IsNullOrEmpty(ilce) || x.İlce.Contains(ilce)) &&
                                                 (string.IsNullOrEmpty(il) || x.İl.Contains(il)));

                        if (eslesme != null)
                        {
                            // Eşleşme bulundu, Gezer sütununa TeslimFirmaAdresAdı verisini getir
                            teslimNoktalarıForm.gridViewGezerEslesme.SetRowCellValue(
                                teslimNoktalarıForm.gridViewGezerEslesme.LocateByValue("AdresAdi", eslesme.AdresAdi),
                                "Gezer", adres);

                            eslesenAdresler.Add(adres); // Eşleşen adresi ekle
                            eslesmeyenAdresler.Remove(adres); // Eşleşmeyenler listesinden çıkar
                        }
                    }
                }

                // gridControl1 veri kaynağını güncelle
                teslimNoktalarıForm.gridControlGezerEslesme.RefreshDataSource();

                // "Gezer" sütununa göre sıralama yap
                teslimNoktalarıForm.gridViewGezerEslesme.Columns["Gezer"].SortOrder = DevExpress.Data.ColumnSortOrder.Descending;

                // Yükleme formunu kapat
                loadingForm.Close();

                // RowCellStyle olayını tanımla
                teslimNoktalarıForm.gridViewGezerEslesme.RowCellStyle += (s, ev) =>
                {
                    string GezerDegeri = teslimNoktalarıForm.gridViewGezerEslesme.GetRowCellValue(ev.RowHandle, "Gezer")?.ToString();
                    if (eslesenAdresler.Contains(GezerDegeri))
                    {
                        ev.Appearance.BackColor = Color.LightGreen; // Yeşil renge boya
                    }
                };

                // Sonuçları göster
                string message = $"{eslesenAdresler.Count} eşleşme bulundu.\n";
                if (eslesmeyenAdresler.Count > 0)
                {
                    message += "Eşleşmeyen adresler:\n" + string.Join("\n", eslesmeyenAdresler);
                }
                else
                {
                    message += "Tüm adresler eşleşti.";
                }

                MessageBox.Show(message, "Sonuç");
            }
        }





        private void BtnTeslimleriGetir_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                // GridControlGezer'deki tüm satırları alıyoruz
                for (int rowHandle = 0; rowHandle < gridViewGezer.RowCount; rowHandle++) // Tüm satırlar üzerinde döngü başlatıyoruz
                {
                    // TeslimFirmaAdresAdı sütunundaki değeri alıyoruz
                    string teslimFirmaAdresAdi = gridViewGezer.GetRowCellValue(rowHandle, "TeslimFirmaAdresAdı")?.ToString(); // Null kontrolü de ekledim

                    if (string.IsNullOrEmpty(teslimFirmaAdresAdi))
                        continue; // Eğer boş veya null ise, bu satırı atla

                    // DbSiparisOtomasyonEntities6 bağlamını kullanarak TblEslesmelerle tablosunda Gezer sütununda ara
                    using (var context = new DbSiparisOtomasyonEntities6())
                    {
                        // Gezer sütununda teslimFirmaAdresAdi ile eşleşen bir satır arıyoruz
                        var eslesme = context.TblEslesmelerle
                            .FirstOrDefault(e1 => e1.Gezer == teslimFirmaAdresAdi); // Gezer sütunuyla karşılaştırıyoruz

                        if (eslesme != null)
                        {
                            // AdresID'yi string olarak alıyoruz
                            string adresID = eslesme.AdresID?.ToString() ?? string.Empty;
                            // MusteriID'yi string olarak alıyoruz
                            string musteriID = eslesme.MusteriID?.ToString() ?? string.Empty;

                            // İlgili hücrelere AdresID ve MusteriID değerlerini yazıyoruz
                            gridViewGezer.SetRowCellValue(rowHandle, "TeslimFirmaAdresAdı", adresID);
                            gridViewGezer.SetRowCellValue(rowHandle, "AlıcıFirmaCariUnvanı", musteriID);
                        }
                        else
                        {
                            // Eşleşme bulunmazsa, o satır atlanır, herhangi bir işlem yapılmaz
                            continue; // Bir sonraki satıra geçiyor
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Hata yönetimi
                MessageBox.Show($"Bir hata oluştu: {ex.Message}");
            }
        }



        private void barButtonItem2_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                // SaveFileDialog ile dosya kaydetme penceresi açıyoruz
                using (SaveFileDialog saveFileDialog = new SaveFileDialog())
                {
                    saveFileDialog.Filter = "Excel Files (*.xlsx)|*.xlsx";
                    saveFileDialog.DefaultExt = "xlsx";

                    // Kullanıcı bir dosya seçerse, verileri bu dosyaya kaydediyoruz
                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        // gridViewGezer'deki veriyi Excel dosyasına aktarıyoruz
                        gridViewGezer.ExportToXlsx(saveFileDialog.FileName);

                        // Kullanıcıya başarılı işlem mesajı
                        MessageBox.Show("Veriler Excel dosyasına başarıyla aktarıldı.");
                    }
                }
            }
            catch (Exception ex)
            {
                // Hata yönetimi
                MessageBox.Show($"Bir hata oluştu: {ex.Message}");
            }
        }


        private void BtnSilme_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                // Satırları silmek için gridViewGezer'in RowCount özelliğini kontrol edip, her satırı tek tek siliyoruz
                int rowCount = gridViewGezer.RowCount;

                // Satırları silme işlemi
                for (int i = rowCount - 1; i >= 0; i--)
                {
                    gridViewGezer.DeleteRow(i); // Satırları tersten silmek, index sorunlarını engeller
                }
            }
            catch (Exception ex)
            {
                // Hata yönetimi
                MessageBox.Show($"Bir hata oluştu: {ex.Message}");
            }
        }

        private void GezerSiparisEkranı_Load(object sender, EventArgs e)
        {
            // GridView objesini alın
            GridView gridView = gridControlGezer.MainView as GridView;

            if (gridView != null)
            {
                // İlk satır sayısını göster
                UpdateRowCountLabel();

                // RowCount değiştiğinde tetiklenecek olayları dinle
                gridView.RowCountChanged += GridView_RowCountChanged;
                gridView.RowDeleted += GridView_RowDeleted;
                gridView.RowUpdated += GridView_RowUpdated;
            }
            else
            {
                // Eğer GridView bulunamazsa hata mesajı yazdır
                labelControl1.Text = "GridView bulunamadı!";
            }
        }

        // Satır sayısını güncellemek için bir yöntem
        private void UpdateRowCountLabel()
        {
            GridView gridView = gridControlGezer.MainView as GridView;

            if (gridView != null)
            {
                // Satır sayısını al ve label'ı güncelle
                int rowCount = gridView.RowCount;
                labelControl1.Text = $"Toplam Satır Sayısı: {rowCount}";
            }
        }

        // RowCount değişikliklerini dinleyen olaylar
        private void GridView_RowCountChanged(object sender, EventArgs e)
        {
            UpdateRowCountLabel();
        }

        private void GridView_RowDeleted(object sender, DevExpress.Data.RowDeletedEventArgs e)
        {
            UpdateRowCountLabel();
        }

        private void GridView_RowUpdated(object sender, DevExpress.XtraGrid.Views.Base.RowObjectEventArgs e)
        {
            UpdateRowCountLabel();
        }
    }
}


















