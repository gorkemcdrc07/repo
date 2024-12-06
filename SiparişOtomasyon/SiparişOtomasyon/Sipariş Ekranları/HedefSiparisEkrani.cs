using System;
using System.Data;
using System.Windows.Forms;
using DevExpress.XtraGrid.Views.Grid;
using SiparişOtomasyon.entity;
using SiparişOtomasyon.Teslim_Noktaları;
using SiparişOtomasyon.Yükleme_Noktaları;
using ExcelApp = Microsoft.Office.Interop.Excel.Application; // Excel Application için takma ad
using System.Linq;
using System.Data.Entity;


namespace SiparişOtomasyon.Sipariş_Ekranları
{
    public partial class HedefSiparisEkrani : Form
    {
        public HedefSiparisEkrani()
        {
            InitializeComponent();
            gridControlHedef.AllowDrop = true; // AllowDrop özelliğini true yapalım
                                               // DragEnter ve DragDrop olaylarını bağlayalım
            this.gridControlHedef.DragEnter += new DragEventHandler(gridControlHedef_DragEnter);
            this.gridControlHedef.DragDrop += new DragEventHandler(gridControlHedef_DragDrop);

        }

        // DragEnter event: Dosya sürükleniyorsa doğru şekilde alınıp almadığını kontrol eder
        private void gridControlHedef_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))  // Dosya sürükleniyorsa kopyalama etkisini uygula
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None; // Dosya dışında başka veriler kabul edilmez
            }
        }

        // DragDrop event: Dosya sürüklendikten sonra bu işlem gerçekleşir
        private void gridControlHedef_DragDrop(object sender, DragEventArgs e)
        {
            var dataObject = e.Data.GetData(DataFormats.FileDrop) as string[];
            if (dataObject != null && dataObject.Length > 0)
            {
                string excelFilePath = dataObject[0];

                try
                {
                    // Excel dosyasını açıyoruz
                    var excelApp = new ExcelApp(); // ExcelApplication'ı başlatıyoruz
                    var workbook = excelApp.Workbooks.Open(excelFilePath);
                    var worksheet = workbook.Sheets[1]; // İlk sayfayı alıyoruz

                    var range = worksheet.UsedRange;
                    var dataTable = new DataTable();

                    // Excel'in başlık satırını alıyoruz
                    dataTable.Columns.Add("MusteriSiparisNo");
                    dataTable.Columns.Add("TeslimFirmaAdresAdı");
                    dataTable.Columns.Add("İstenilenAracTipi");
                    dataTable.Columns.Add("YuklemeFirmasıAdresAdı");
                    dataTable.Columns.Add("SiparisTarihi");
                    dataTable.Columns.Add("YuklemeTarihi");
                    dataTable.Columns.Add("TeslimTarihi");
                    dataTable.Columns.Add("MusteriVkn");
                    dataTable.Columns.Add("Urun");
                    dataTable.Columns.Add("KapAdet");
                    dataTable.Columns.Add("AmbalajTipi");
                    dataTable.Columns.Add("BrutKG");

                    // Excel verilerini DataTable'a ekliyoruz
                    for (int i = 2; i <= range.Rows.Count; i++) // 2. satırdan başlıyoruz çünkü 1. satır başlık
                    {
                        DataRow row = dataTable.NewRow();
                        row["MusteriSiparisNo"] = range.Cells[i, 2]?.Value?.ToString() ?? ""; // B: Sevk No -> MusteriSiparisNo
                        row["TeslimFirmaAdresAdı"] = range.Cells[i, 3]?.Value?.ToString() ?? ""; // C: Uğranan Bölge -> TeslimFirmaAdresAdı
                        row["İstenilenAracTipi"] = range.Cells[i, 4]?.Value?.ToString() ?? ""; // D: Araç Tipi -> İstenilenAracTipi
                        row["YuklemeFirmasıAdresAdı"] = range.Cells[i, 5]?.Value?.ToString() ?? ""; // E: Yükleme Yeri -> YuklemeFirmasıAdresAdı
                        row["YuklemeFirmasıAdresAdı"] = range.Cells[i, 6]?.Value?.ToString() ?? ""; // F: Kalkış Yeri -> KalkışYeri
                        dataTable.Rows.Add(row);
                    }

                    // gridControlHedef'e veriyi bağlıyoruz
                    gridControlHedef.DataSource = dataTable;

                    // gridControlHedef'teki sütunların görünür olduğundan emin olalım
                    GridView gridView = gridControlHedef.MainView as GridView;
                    if (gridView != null)
                    {
                        gridView.Columns["MusteriSiparisNo"].Visible = true;
                        gridView.Columns["TeslimFirmaAdresAdı"].Visible = true;
                        gridView.Columns["İstenilenAracTipi"].Visible = true;
                        gridView.Columns["YuklemeFirmasıAdresAdı"].Visible = true;
                        gridView.Columns["YuklemeFirmasıAdresAdı"].Visible = true;

                    }

                    // Excel dosyasını kapatıyoruz
                    workbook.Close(false);
                    excelApp.Quit();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Hata: " + ex.Message, "Dosya Okuma Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Lütfen bir Excel dosyası sürükleyin.", "Geçersiz Dosya", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnYuklemeNoktalarıEkranı_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            // HedefYuklemeNoktaları formunu oluşturup açıyoruz
            HedefYuklemeNoktaları hedefYuklemeNoktalarıForm = new HedefYuklemeNoktaları();
            hedefYuklemeNoktalarıForm.Show();
        }

        private void BtnEslestir_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            // HedefTeslimNoktalarıEslestir formunu oluşturup açıyoruz
            HedefTeslimNoktalarıEslestir hedefTeslimNoktalarıEslestirForm = new HedefTeslimNoktalarıEslestir();
            hedefTeslimNoktalarıEslestirForm.Show();
        }
        public class DbSiparisOtomasyonEntities6 : DbContext
        {
            public DbSet<Hedef> Hedef { get; set; }
        }


        private void BtnVerileriGetir_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            // GridControl'ün MainView'ini GridView olarak alıyoruz
            GridView gridView = gridControlHedef.MainView as GridView;

            if (gridView != null)
            {
                // Bugünün tarihini alıyoruz
                DateTime today = DateTime.Now;

                // GridView'deki tüm satırlar üzerinde işlem yapıyoruz
                for (int i = 0; i < gridView.RowCount; i++)
                {
                    // SiparisTarihi ve YuklemeTarihi sütunlarına günün tarihini atıyoruz
                    gridView.SetRowCellValue(i, "SiparisTarihi", today);
                    gridView.SetRowCellValue(i, "YuklemeTarihi", today);

                    // TeslimTarihi sütununa, SiparisTarihi'nin üstüne 1 gün ekliyoruz
                    DateTime teslimTarihi = today.AddDays(1);
                    gridView.SetRowCellValue(i, "TeslimTarihi", teslimTarihi);

                    // MusteriVkn sütununa otomatik olarak 4610395861 yazıyoruz
                    gridView.SetRowCellValue(i, "MusteriVkn", "4610395861");

                    // Urun sütununa 1 yazıyoruz
                    gridView.SetRowCellValue(i, "Urun", 1);

                    // KapAdet sütununa 25 yazıyoruz
                    gridView.SetRowCellValue(i, "KapAdet", 25);

                    // AmbalajTipi sütununa 1 yazıyoruz
                    gridView.SetRowCellValue(i, "AmbalajTipi", 1);

                    // BrutKG sütununa 25.000 yazıyoruz
                    gridView.SetRowCellValue(i, "BrutKG", 25000);

                    // İstenilenAracTipi sütununu kontrol ediyoruz
                    string aracTipi = gridView.GetRowCellValue(i, "İstenilenAracTipi")?.ToString();

                    if (aracTipi != null)
                    {
                        if (aracTipi.Equals("TIR", StringComparison.OrdinalIgnoreCase))
                        {
                            // TIR ise 1 yazıyoruz
                            gridView.SetRowCellValue(i, "İstenilenAracTipi", 1);
                        }
                        else if (aracTipi.Equals("KAMYON", StringComparison.OrdinalIgnoreCase))
                        {
                            // KAMYON ise 3 yazıyoruz
                            gridView.SetRowCellValue(i, "İstenilenAracTipi", 3);
                        }
                    }
                    using (var context = new DbSiparisOtomasyonEntities6())
                    {
                        // gridControlHedef'teki tüm satırlara bak
                        for (int rowHandle = 0; rowHandle < gridViewHedef.RowCount; rowHandle++)
                        {
                            // 'YuklemeFirmasıAdresAdı' sütunundaki veriyi al
                            var adres = gridViewHedef.GetRowCellValue(rowHandle, "YuklemeFirmasıAdresAdı");

                            // 'adres' değeri null veya boş değilse işlemi yap
                            if (adres != null && !string.IsNullOrEmpty(adres.ToString()))
                            {
                                // 'Hedef' tablosunda 'Tanımla' sütununda 'adres' kelimesini arıyoruz
                                var adresId = context.Hedef
                                    .Where(h => h.Tanımla.Contains(adres.ToString())) // 'Tanımla' sütununda 'adres' kelimesini ara
                                    .Select(h => h.AdresID)
                                    .FirstOrDefault(); // İlk eşleşeni al

                                // Eğer adresId null değilse
                                if (adresId != null)
                                {
                                    // 'AdresID' değerini 'YuklemeFirmasıAdresAdı' sütununa yaz
                                    gridViewHedef.SetRowCellValue(rowHandle, "YuklemeFirmasıAdresAdı", adresId);
                                }
                                else
                                {
                                    // Eşleşen adresId bulunamadığında konsola bilgi yazdır
                                    Console.WriteLine("Eşleşen AdresID bulunamadı: " + adres.ToString());
                                }
                            }
                            else
                            {
                                // Eğer 'adres' değeri null veya boşsa
                                Console.WriteLine("Adres değeri null veya boş: " + adres);
                            }
                        }
                    }

                }
            }
            }

        private void HedefSiparisEkrani_Load(object sender, EventArgs e)
        {
            // GridView objesini alın
            GridView gridView = gridControlHedef.MainView as GridView;

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
            GridView gridView = gridControlHedef.MainView as GridView;

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


