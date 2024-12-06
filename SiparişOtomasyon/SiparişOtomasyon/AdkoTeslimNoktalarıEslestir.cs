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
using System.Data.SqlClient;

namespace SiparişOtomasyon
{
    public partial class TeslimNoktalarıEslestir : Form
    {
        private readonly DbSiparisOtomasyonEntities1 dbContext;

        public TeslimNoktalarıEslestir()
        {
            InitializeComponent();
            dbContext = new DbSiparisOtomasyonEntities1();

            // Drag and Drop için ayarları yap
            gridControl1.AllowDrop = true;
            gridControl1.DragEnter += GridControl1_DragEnter;
            gridControl1.DragDrop += GridControl1_DragDrop;
        }

        private void TeslimNoktalarıEslestir_Load(object sender, EventArgs e)
        {
            // TblEslestirme tablosundaki verileri al
            var eslestirmeVerileri = dbContext.TblEslesmelerle.ToList();

            // gridControl1'in DataSource'unu ayarlayın
            gridControl1.DataSource = eslestirmeVerileri;
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

        // Excel verilerini set etme ve eşleştirme
        public void SetMusteridenGelenData(List<string> teslimFirmaAdresList)
        {
            // gridControl1'deki mevcut verileri al
            var gridControl1Data = gridControl1.DataSource as List<TblEslesmelerle>;

            if (gridControl1Data == null)
            {
                MessageBox.Show("TeslimNoktaları GridControl verisi alınamadı.");
                return;
            }

            // Her bir teslim firma adresini işleme al
            foreach (var teslimFirmaAdres in teslimFirmaAdresList)
            {
                TblEslesmelerle closestMatch = null;
                int closestDistance = int.MaxValue;
                string firstWordFromExcel = teslimFirmaAdres.Split(' ')[0]; // Excel'den gelen ilk kelimeyi al

                // Reel sütunundaki verilerle karşılaştır
                foreach (var eslestirVeri in gridControl1Data)
                {
                    string reelVeri = eslestirVeri.AdresAdi;

                    // Levenshtein mesafesi ile benzerlik kontrolü
                    int distance = LevenshteinDistance(reelVeri, teslimFirmaAdres);

                    // BİM gibi anahtar kelimenin varlığını kontrol et
                    if (reelVeri.Contains(firstWordFromExcel, StringComparison.OrdinalIgnoreCase) ||
                        teslimFirmaAdres.Contains(reelVeri, StringComparison.OrdinalIgnoreCase))
                    {
                        // Eşleşme bulunduysa mesafeyi dikkate al
                        if (distance < closestDistance)
                        {
                            closestDistance = distance;
                            closestMatch = eslestirVeri; // Burada `closestMatch` bir nesne olmalı
                        }
                    }
                }

                // En yakın eşleşme bulunduysa MusteridenGelen kısmına at
                if (closestMatch != null)
                {
                    // closestMatch, TblEslesmeler tipinde olmalı
                    closestMatch.Adko = teslimFirmaAdres; // Adko'yu güncelle
                }
            }

            // İşlem yapılan kayıtları ve diğer kayıtları ayırarak sıralama
            var sortedRecords = gridControl1Data
                .Where(x => x.Adko != null) // İşlem yapılmış olanlar
                .Concat(gridControl1Data.Where(x => x.Adko == null)) // İşlem yapılmamış olanlar
                .ToList();

            // Verileri gridControl1'e yerleştir
            gridControl1.DataSource = sortedRecords;

            // Veritabanını kaydet ve grid'i yenile
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

        private void BtnOnayla_Click(object sender, EventArgs e)
        {
            var gridControl1Data = gridControl1.DataSource as List<TblEslesmelerle>;

            if (gridControl1Data == null)
            {
                MessageBox.Show("TeslimNoktaları GridControl verisi alınamadı.");
                return;
            }

            var adkoForm = new FrmAdkoTurkSiparisEkrani();
            var teslimFirmaAdresList = adkoForm.GetTeslimFirmaAdresList();

            foreach (var eslestirme in gridControl1Data)
            {
                var matchingAddress = teslimFirmaAdresList.FirstOrDefault(addr => addr.Equals(eslestirme.AdresAdi, StringComparison.OrdinalIgnoreCase));

                if (matchingAddress != null)
                {
                    eslestirme.Adko = matchingAddress; // Adko değerini güncelle
                    dbContext.Entry(eslestirme).State = EntityState.Modified; // Entity state'i güncelle
                }
            }

            try
            {
                dbContext.SaveChanges(); // Kayıtları veritabanına kaydet
                MessageBox.Show("Kayıtlar başarıyla kaydedildi.");
            }
            catch (DbUpdateException ex)
            {
                MessageBox.Show("Kayıt güncellenirken bir hata oluştu: " + ex.Message);
            }
        }







    }
}
