using SiparişOtomasyon.entity;
using ClosedXML.Excel; // ClosedXML kütüphanesini kullanmak için ekleyin
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using DevExpress.Data.NetCompatibility.Extensions;
using SiparişOtomasyon.Sipariş_Ekranları;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace SiparişOtomasyon
{
    public partial class AdkoTeslimNoktalarıEslestir : Form
    {
        private readonly DbSiparisOtomasyonEntities6 dbContext;

        public AdkoTeslimNoktalarıEslestir()
        {
            InitializeComponent();
            dbContext = new DbSiparisOtomasyonEntities6();

            // Drag and Drop için ayarları yap
            gridControlAdkoEslesme.AllowDrop = true;
            gridControlAdkoEslesme.DragEnter += GridControl1_DragEnter;
            gridControlAdkoEslesme.DragDrop += GridControl1_DragDrop;
        }

        private void AdkoTeslimNoktalarıEslestir_Load(object sender, EventArgs e)
        {
            // TODO: Bu kod satırı 'dbSiparisOtomasyonDataSet23.TblEslesmelerle' tablosuna veri yükler. Bunu gerektiği şekilde taşıyabilir, veya kaldırabilirsiniz.
            this.tblEslesmelerleTableAdapter3.Fill(this.dbSiparisOtomasyonDataSet23.TblEslesmelerle);
            // TODO: Bu kod satırı 'dbSiparisOtomasyonDataSet23.TblEslesmelerle' tablosuna veri yükler. Bunu gerektiği şekilde taşıyabilir, veya kaldırabilirsiniz.
            this.tblEslesmelerleTableAdapter3.Fill(this.dbSiparisOtomasyonDataSet23.TblEslesmelerle);
            // TblEslestirme tablosundaki verileri al
            var eslestirmeVerileri = dbContext.TblEslesmelerle.ToList();

            // gridControl1'in DataSource'unu ayarlayın
            gridControlAdkoEslesme.DataSource = eslestirmeVerileri;
            gridView1.BestFitColumns();
        }

        // DragEnter olayı
        private void GridControl1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy; // Kopyalama etkisini belirt
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        // DragDrop olayı
        private void GridControl1_DragDrop(object sender, DragEventArgs e)
        {
            // Excel dosyasını alın
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

            if (files.Length > 0)
            {
                string filePath = files[0];
                // Excel dosyasından verileri oku
                List<string> teslimFirmaAdresList = ReadExcelFile(filePath);

                // Okunan verileri gridControl1'e yerleştir
                SetMusteridenGelenData(teslimFirmaAdresList);
            }
        }

        // Excel dosyasından verileri okuma
        private List<string> ReadExcelFile(string filePath)
        {
            var teslimFirmaAdresList = new List<string>();

            using (var workbook = new XLWorkbook(filePath))
            {
                var worksheet = workbook.Worksheet(1); // İlk sayfayı al
                var rowCount = worksheet.LastRowUsed().RowNumber();

                for (int i = 1; i <= rowCount; i++) // İlk satır başlık ise 2'den başlatabilirsiniz
                {
                    var cellValue = worksheet.Cell(i, 1).GetString(); // 1. sütundaki değeri al
                    teslimFirmaAdresList.Add(cellValue);
                }
            }

            return teslimFirmaAdresList;
        }

        public void SetMusteridenGelenData(List<string> teslimFirmaAdresList)
        {
            // gridControl1'deki mevcut verileri al
            var gridControl1Data = gridControlAdkoEslesme.DataSource as List<TblEslesmelerle>;

            if (gridControl1Data == null)
            {
                MessageBox.Show("TeslimNoktaları GridControl verisi alınamadı.");
                return;
            }

            // Her bir teslim firma adresini işleme al
            foreach (var teslimFirmaAdres in teslimFirmaAdresList)
            {
                TblEslesmelerle bestMatch = null;
                int bestScore = 0; // En yüksek puanı tutacak
                string musteriID = string.Empty; // Kullanılacak MusteriID

                // 1. TeslimFirmaAdres'e göre MusteriID belirle
                if (teslimFirmaAdres.StartsWith("ŞOK", StringComparison.OrdinalIgnoreCase))
                {
                    musteriID = "283";
                }
                else if (teslimFirmaAdres.StartsWith("MİGROS", StringComparison.OrdinalIgnoreCase))
                {
                    musteriID = "205";
                }
                else if (teslimFirmaAdres.StartsWith("BİM", StringComparison.OrdinalIgnoreCase))
                {
                    musteriID = "6";
                }
                else if (teslimFirmaAdres.StartsWith("A101", StringComparison.OrdinalIgnoreCase))
                {
                    musteriID = "15";
                }

                // 2. Belirlenen MusteriID'ye göre kayıtları filtrele
                var filteredData = gridControl1Data
                    .Where(x => x.MusteriID != null && x.MusteriID.Equals(musteriID, StringComparison.OrdinalIgnoreCase))
                    .ToList();

                // 3. Tüm kayıtları dolaş
                foreach (var eslestirVeri in filteredData)
                {
                    int score = 0;

                    // 4. Adres karşılaştırması
                    var adresKelimeListesi = teslimFirmaAdres.Split(new[] { '-', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var kelime in adresKelimeListesi)
                    {
                        if (eslestirVeri.AdresAdi != null && eslestirVeri.AdresAdi.Contains(kelime, StringComparison.OrdinalIgnoreCase))
                        {
                            score++; // Eşleşen kelime başına puan ekle
                        }
                    }

                    // 5. İl karşılaştırması
                    if (!string.IsNullOrEmpty(eslestirVeri.İl) && teslimFirmaAdres.Contains(eslestirVeri.İl, StringComparison.OrdinalIgnoreCase))
                    {
                        score++; // İl eşleşmesi için puan ekle
                    }

                    // 6. İlçe karşılaştırması
                    if (!string.IsNullOrEmpty(eslestirVeri.İlce) && teslimFirmaAdres.Contains(eslestirVeri.İlce, StringComparison.OrdinalIgnoreCase))
                    {
                        score++; // İlçe eşleşmesi için puan ekle
                    }

                    // 7. En iyi eşleşmeyi kontrol et
                    if (score > bestScore)
                    {
                        bestScore = score;
                        bestMatch = eslestirVeri; // En iyi eşleşme güncelleniyor
                    }
                }

                // En iyi eşleşme bulunduysa MusteridenGelen kısmına at
                if (bestMatch != null && bestScore > 0) // Eğer en az 1 puan aldıysa
                {
                    bestMatch.Adko = teslimFirmaAdres; // Adko'yu güncelle
                    dbContext.Entry(bestMatch).State = System.Data.Entity.EntityState.Modified; // Veritabanındaki değişiklikleri işaretleyin
                }
            }

            // İşlem yapılan kayıtları ve diğer kayıtları ayırarak sıralama
            var sortedRecords = gridControl1Data
                .Where(x => x.Adko != null) // İşlem yapılmış olanlar
                .Concat(gridControl1Data.Where(x => x.Adko == null)) // İşlem yapılmamış olanlar
                .ToList();

            // Verileri gridControl1'e yerleştir
            gridControlAdkoEslesme.DataSource = sortedRecords;

            // Veritabanını kaydet ve grid'i yenile
            try
            {
                dbContext.SaveChanges(); // Kayıtları veritabanına kaydet
                MessageBox.Show("Eşleştirme işlemi tamamlandı.");
            }
            catch (DbUpdateException ex)
            {
                MessageBox.Show("Kayıt güncellenirken bir hata oluştu: " + ex.Message);
            }

            gridView1.RefreshData();
        }

        // Levenshtein Mesafesi Hesaplama - String Benzerlik Kontrolü
        private int LevenshteinDistance(string s, string t)
        {
            int n = s.Length;
            int m = t.Length;
            int[,] d = new int[n + 1, m + 1];

            // İlk satır ve sütunu başlat
            for (int i = 0; i <= n; d[i, 0] = i++) ;
            for (int j = 0; j <= m; d[0, j] = j++) ;

            // Mesafeyi hesapla
            for (int i = 1; i <= n; i++)
            {
                for (int j = 1; j <= m; j++)
                {
                    int cost = (t[j - 1] == s[i - 1]) ? 0 : 1;
                    d[i, j] = Math.Min(
                        Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1),
                        d[i - 1, j - 1] + cost);
                }
            }
            return d[n, m];
        }



        private void BtnEslenEnUstte_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            // AdkoTeslimNoktalarıEslestir formunu al
            var adkoForm = Application.OpenForms.OfType<AdkoTeslimNoktalarıEslestir>().FirstOrDefault();
            if (adkoForm != null)
            {
                // gridControlAdkoEslesme'yi al
                var gridControl = adkoForm.Controls.Find("gridControlAdkoEslesme", true).FirstOrDefault() as DevExpress.XtraGrid.GridControl;
                if (gridControl != null)
                {
                    // gridView'i al
                    var gridView = gridControl.MainView as DevExpress.XtraGrid.Views.Grid.GridView;
                    if (gridView != null)
                    {
                        // Adko sütununu bul
                        var adkoColumn = gridView.Columns["Adko"];

                        if (adkoColumn == null)
                        {
                            MessageBox.Show("Adko sütunu bulunamadı!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        // Tüm satırları temizle
                        gridView.ActiveFilter.Clear();

                        // Adko sütunu dolu olanları göstermek için bir filtre uygula
                        gridView.ActiveFilter.Add(adkoColumn,
                            new DevExpress.XtraGrid.Columns.ColumnFilterInfo($"{adkoColumn.FieldName} Is Not Null")); // Null olanları filtreleme

                        // Filtre uygulandıktan sonra görünümü güncelle
                        gridView.RefreshData();

                        // Filtrenin doğru çalıştığını kontrol etmek için bir mesaj kutusu ekleyelim
                        int visibleRowCount = gridView.DataRowCount;
                        MessageBox.Show($"{visibleRowCount} satır filtrelendi.", "Filtre Durumu", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("GridView bulunamadı!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("GridControl bulunamadı!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Form bulunamadı!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void BtnOnayla_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            var gridControl1Data = gridControlAdkoEslesme.DataSource as List<TblEslesmelerle>;
            if (gridControl1Data == null)
            {
                MessageBox.Show("GridControl verisi alınamadı.");
                return;
            }

            // Değişiklikleri veritabanına kaydet
            try
            {
                dbContext.SaveChanges();
                MessageBox.Show("Kayıtlar başarıyla kaydedildi.");
            }
            catch (DbUpdateException ex)
            {
                MessageBox.Show("Kayıt güncellenirken bir hata oluştu: " + ex.Message);
            }
        }

        // Form kapandığında dbContext'i uygun şekilde kapatın
        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            dbContext.Dispose();
            base.OnFormClosed(e);
        }
    }
}
