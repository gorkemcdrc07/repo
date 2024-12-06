using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Windows.Forms;
using ClosedXML.Excel; // Excel dosyası okumak için ClosedXML kütüphanesi
using DevExpress.Data.NetCompatibility.Extensions;
using SiparişOtomasyon.entity; // Entity sınıflarının bulunduğu namespace

namespace SiparişOtomasyon.Teslim_Noktaları
{
    public partial class MilhansTeslimNoktalarıEslestir : Form
    {
        // Declare the DbContext
        private readonly DbSiparisOtomasyonEntities6 dbContext; // Türetilmiş DbContext sınıfını kullanın

        public MilhansTeslimNoktalarıEslestir()
        {
            InitializeComponent();
            dbContext = new DbSiparisOtomasyonEntities6(); // Türetilmiş DbContext sınıfını başlatın

        }

        private void MilhansTeslimNoktalarıEslestir_Load(object sender, EventArgs e)
        {
            // Load data from the database
            LoadTblEslesmelerleData();
        }

        private void LoadTblEslesmelerleData()
        {
            // Veritabanındaki TblEslesmelerle verilerini al
            var data = dbContext.TblEslesmelerle.ToList();
            gridControlMilhansEslesme.DataSource = data; // gridControlMilhans adında bir grid control olduğunu varsayıyoruz
        }

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

        private List<string> ReadExcelFile(string filePath)
        {
            var teslimFirmaAdresList = new List<string>();

            using (var workbook = new XLWorkbook(filePath))
            {
                var worksheet = workbook.Worksheet(1); // İlk sayfayı al
                var rowCount = worksheet.LastRowUsed().RowNumber();

                for (int i = 1; i <= rowCount; i++)
                {
                    var cellValue = worksheet.Cell(i, 1).GetString(); // 1. sütundaki değeri al
                    teslimFirmaAdresList.Add(cellValue);
                }
            }

            return teslimFirmaAdresList;
        }

        public void SetMusteridenGelenData(List<string> teslimFirmaAdresList)
        {
            var gridControl1Data = gridControlMilhansEslesme.DataSource as List<TblEslesmelerle>;

            if (gridControl1Data == null)
            {
                MessageBox.Show("TeslimNoktaları GridControl verisi alınamadı.");
                return;
            }

            foreach (var teslimFirmaAdres in teslimFirmaAdresList)
            {
                TblEslesmelerle bestMatch = null;
                int bestScore = 0;
                string musteriID = string.Empty;

                // MusteriID belirleme
                if (teslimFirmaAdres.StartsWith("ŞOK", StringComparison.OrdinalIgnoreCase)) musteriID = "283";
                else if (teslimFirmaAdres.StartsWith("MİGROS", StringComparison.OrdinalIgnoreCase)) musteriID = "205";
                else if (teslimFirmaAdres.StartsWith("BİM", StringComparison.OrdinalIgnoreCase)) musteriID = "6";
                else if (teslimFirmaAdres.StartsWith("A101", StringComparison.OrdinalIgnoreCase)) musteriID = "15";

                // Belirlenen MusteriID'ye göre kayıtları filtrele
                var filteredData = gridControl1Data
                    .Where(x => x.MusteriID != null && x.MusteriID.Equals(musteriID, StringComparison.OrdinalIgnoreCase))
                    .ToList();

                foreach (var eslestirVeri in filteredData)
                {
                    int score = 0;

                    var adresKelimeListesi = teslimFirmaAdres.Split(new[] { '-', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var kelime in adresKelimeListesi)
                    {
                        if (eslestirVeri.AdresAdi != null && eslestirVeri.AdresAdi.IndexOf(kelime, StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            score++;
                        }
                    }

                    if (!string.IsNullOrEmpty(eslestirVeri.İl) && teslimFirmaAdres.Contains(eslestirVeri.İl, StringComparison.OrdinalIgnoreCase))
                        score++;

                    if (!string.IsNullOrEmpty(eslestirVeri.İlce) && teslimFirmaAdres.Contains(eslestirVeri.İlce, StringComparison.OrdinalIgnoreCase))
                        score++;

                    if (score > bestScore)
                    {
                        bestScore = score;
                        bestMatch = eslestirVeri;
                    }
                }

                if (bestMatch != null && bestScore > 0)
                {
                    bestMatch.Milhans = teslimFirmaAdres;
                    dbContext.Entry(bestMatch).State = EntityState.Modified;
                }
            }

            // İşlem yapılan kayıtları sıralayın
            var sortedRecords = gridControl1Data
                .Where(x => x.Milhans != null)
                .Concat(gridControl1Data.Where(x => x.Milhans == null))
                .ToList();

            // Verileri gridControl1'e yerleştir
            gridControlMilhansEslesme.DataSource = sortedRecords;

            // Veritabanını kaydet
            try
            {
                dbContext.SaveChanges();
                gridView1.RefreshData(); // gridView1'inizi güncelleyin
            }
            catch (DbUpdateException ex)
            {
                MessageBox.Show("Kayıt güncellenirken bir hata oluştu: " + ex.Message);
            }
        }


        private void BtnOnayla_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            var gridControl1Data = gridControlMilhansEslesme.DataSource as List<TblEslesmelerle>;
            if (gridControl1Data == null)
            {
                MessageBox.Show("GridControl verisi alınamadı.");
                return;
            }

            // Değişiklikleri veritabanına kaydet
            try
            {
                dbContext.SaveChanges();
                MessageBox.Show("Kayıt başarıyla güncellendi."); // Success message
            }
            catch (DbUpdateException ex)
            {
                MessageBox.Show("Kayıt güncellenirken bir hata oluştu: " + ex.Message);
            }
        }

    }
}
