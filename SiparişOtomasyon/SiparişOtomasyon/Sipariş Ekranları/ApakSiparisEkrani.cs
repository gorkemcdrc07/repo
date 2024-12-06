using ClosedXML.Excel;
using DevExpress.XtraBars;
using DevExpress.XtraGrid.Views.Grid;
using SiparişOtomasyon.entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using System.Linq;
using DevExpress.Data.NetCompatibility.Extensions;
using System.ComponentModel;
using System.Drawing;
using SiparişOtomasyon.Teslim_Noktaları; // Point ve Size için
using OfficeOpenXml;
using OfficeOpenXml.Table;






namespace SiparişOtomasyon.Sipariş_Ekranları
{
    public partial class ApakSiparisEkrani : Form
    {
        public ApakSiparisEkrani()
        {
            InitializeComponent();

            // Butonları pasif yap
            BtnTeslimleriGetir.Enabled = false;


            // gridControlApak kontrolü için olayları ayarla
            gridControlApak.AllowDrop = true;
            gridControlApak.DragEnter += gridControlApak_DragEnter;
            gridControlApak.DragDrop += gridControlApak_DragDrop;
        }
    



    private void gridControlApak_DragEnter(object sender, DragEventArgs e)
        {
            // Sürüklenen verinin metin olduğundan emin ol
            if (e.Data.GetDataPresent(DataFormats.Text))
            {
                e.Effect = DragDropEffects.Copy; // Kopyalama etkisi göster
            }
            else
            {
                e.Effect = DragDropEffects.None; // Uygun değilse hiçbir şey yapma
            }
        }

        private void gridControlApak_DragDrop(object sender, DragEventArgs e)
        {
            // Sürüklenen veriyi al
            string data = (string)e.Data.GetData(DataFormats.Text);

            // Verileri satır satır ayır
            string[] lines = data.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

            // gridControlApak'ın veri kaynağını al
            DataTable dt = gridControlApak.DataSource as DataTable;

            if (dt == null)
            {
                // Eğer dt hala null ise, yeni bir DataTable oluştur
                dt = new DataTable();
                dt.Columns.Add("TeslimFirmaAdresAdı");
                dt.Columns.Add("MusteriVkn");
                dt.Columns.Add("Proje");
                dt.Columns.Add("SiparisTarihi");
                dt.Columns.Add("YuklemeTarihi");
                dt.Columns.Add("TeslimTarihi");
                dt.Columns.Add("MusteriSiparisNo");
                dt.Columns.Add("İstenilenAracTipi");
                dt.Columns.Add("YuklemeFirmasıAdresAdı");
                dt.Columns.Add("Urun");
                dt.Columns.Add("KapAdet");
                dt.Columns.Add("AmbalajTipi");
                dt.Columns.Add("BrutKG");
                dt.Columns.Add("AlıcıFirmaCariUnvanı"); // Yeni sütun eklendi

                gridControlApak.DataSource = dt; // gridControlApak'ın veri kaynağını ayarla
            }

            // Kullanıcıdan tarih seçmesini iste
            DateTime selectedDate;
            using (Form dateForm = new Form())
            {
                Label label = new Label { Text = "Lütfen sipariş tarihi seçin:", Dock = DockStyle.Top };
                DateTimePicker dateTimePicker = new DateTimePicker { Dock = DockStyle.Top, Format = DateTimePickerFormat.Short };
                Button confirmButton = new Button { Text = "Tamam", Dock = DockStyle.Bottom };

                confirmButton.Click += (s, args) => { dateForm.DialogResult = DialogResult.OK; dateForm.Close(); };

                dateForm.Controls.Add(label);
                dateForm.Controls.Add(dateTimePicker);
                dateForm.Controls.Add(confirmButton);

                if (dateForm.ShowDialog() == DialogResult.OK)
                {
                    selectedDate = dateTimePicker.Value;
                }
                else
                {
                    // Kullanıcı tarih seçmeyi iptal ettiyse, çıkış yap
                    return;
                }
            }

            // Seçilen tarihin bir gün sonrasını hesapla
            string deliveryDate = selectedDate.AddDays(1).ToString("dd.MM.yyyy");

            // Her bir satırı gridControlApak'a ekle
            foreach (var line in lines)
            {
                // "ODAK" kelimesini çıkar
                string trimmedLine = line.Replace("ODAK", "").Trim();

                if (!string.IsNullOrEmpty(trimmedLine)) // Boş değilse ekle
                {
                    DataRow row = dt.NewRow(); // Yeni bir satır oluştur
                    row["TeslimFirmaAdresAdı"] = trimmedLine; // Alanı doldur
                    row["MusteriVkn"] = "0700355885"; // VKN
                    row["Proje"] = "146"; // Proje
                    row["SiparisTarihi"] = selectedDate.ToString("dd.MM.yyyy"); // Seçilen sipariş tarihi
                    row["YuklemeTarihi"] = selectedDate.ToString("dd.MM.yyyy"); // Seçilen yükleme tarihi
                    row["TeslimTarihi"] = deliveryDate; // Seçilen tarihin bir gün sonrası
                    row["MusteriSiparisNo"] = "APAK"; // Müşteri sipariş numarası
                    row["İstenilenAracTipi"] = "1"; // İstenilen araç tipi
                    row["YuklemeFirmasıAdresAdı"] = "155"; // Yükleme firması adres adı
                    row["Urun"] = "12"; // Ürün
                    row["KapAdet"] = "25"; // Kap adet
                    row["AmbalajTipi"] = "1"; // Ambalaj tipi
                    row["BrutKG"] = "25.000"; // Brüt KG
                    row["AlıcıFirmaCariUnvanı"] = "6"; // Alıcı firma cari unvanı

                    dt.Rows.Add(row); // Yeni satırı ekle
                }
            }

            // gridControlApak'ı güncelleyin (gerekirse)
            gridControlApak.Refresh();
        }




        public List<string> GetTeslimFirmaAdresList()
        {
            var teslimFirmaAdresList = new List<string>();

            if (gridControlApak.DataSource == null)
            {
                return teslimFirmaAdresList; // DataSource null ise boş liste döndür
            }

            // gridControlApak'tan veri kaynağını al
            var dataSource = gridControlApak.DataSource as DataTable;

            if (dataSource != null)
            {
                // Her bir satırı kontrol et
                foreach (DataRow row in dataSource.Rows)
                {
                    var teslimFirmaAdresi = row["TeslimFirmaAdresAdı"]; // TeslimFirmaAdresAdı sütununu al
                    if (teslimFirmaAdresi != null)
                    {
                        teslimFirmaAdresList.Add(teslimFirmaAdresi.ToString()); // Listeye ekle
                    }
                }
            }

            return teslimFirmaAdresList; // Listeyi döndür
        }

        private void BtnEslestir_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            // ApakTeslimNoktalarıEslestir formunu oluştur
            ApakTeslimNoktalarıEslestir eslestirForm = new ApakTeslimNoktalarıEslestir();

            // Formu aç
            eslestirForm.Show();
        }







        private void BtnTeslimleriGetir_ItemClick(object sender, ItemClickEventArgs e)
        {
            using (var context = new DbSiparisOtomasyonEntities6())
            {
                var teslimFirmaAdreslar = gridControlApak.DataSource as DataTable;

                if (teslimFirmaAdreslar != null)
                {
                    int eslesenSatirSayisi = 0;
                    int eslesmeyenSatirSayisi = 0;

                    foreach (DataRow row in teslimFirmaAdreslar.Rows)
                    {
                        string teslimFirmaAdres = row["TeslimFirmaAdresAdı"].ToString();

                        var eslesme = context.TblEslesmelerle
                            .Where(es => es.Apak == teslimFirmaAdres) // Apak ile sorgulama
                            .Select(es => new
                            {
                                ApakID = es.AdresID,
                                MusteriID = es.MusteriID
                            })
                            .FirstOrDefault();

                        if (eslesme != null)
                        {
                            row["TeslimFirmaAdresAdı"] = eslesme.ApakID; // ApakID'yi TeslimFirmaAdresAdı sütununa yaz
                            row["AlıcıFirmaCariUnvanı"] = eslesme.MusteriID; // MusteriID'yi AlıcıFirmaCariUnvanı sütununa yaz

                            eslesenSatirSayisi++; // Eşleşen satır sayısını artır
                        }
                        else
                        {
                            eslesmeyenSatirSayisi++; // Eşleşmeyen satır sayısını artır
                        }
                    }

                    // Değişikliklerin GridControl'da yansımasını sağlamak için
                    gridControlApak.Refresh();

                    // Eşleşen ve eşleşmeyen satır sayılarını göster
                    MessageBox.Show($"Eşleşen Satır Sayısı: {eslesenSatirSayisi}, Eşleşmeyen Satır Sayısı: {eslesmeyenSatirSayisi}",
                                    "Sonuç", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
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

                // ApakTeslimNoktalarıEslestir formunu oluştur
                ApakTeslimNoktalarıEslestir teslimNoktalarıForm = new ApakTeslimNoktalarıEslestir();

                // Adresleri gridViewApak'tan al
                List<string> adresListesi = new List<string>();
                for (int i = 0; i < gridViewApak.RowCount; i++)
                {
                    string adres = gridViewApak.GetRowCellValue(i, "TeslimFirmaAdresAdı")?.ToString()?.Trim();
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

                // TeslimNoktaları formunu göster
                teslimNoktalarıForm.Show();

                // Eşleşen ve eşleşmeyen adresleri saklamak için listeler
                List<string> eslesenAdresler = new List<string>();
                List<string> eslesmeyenAdresler = new List<string>(adresListesi);

                // Adresleri eşleştirme işlemi
                foreach (var adres in adresListesi)
                {
                    string arananKelime = adres; // Eşleşme için adresi al

                    for (int j = 0; j < teslimNoktalarıForm.gridView1.RowCount; j++)
                    {
                        string mevcutAdres = teslimNoktalarıForm.gridView1.GetRowCellValue(j, "AdresAdi")?.ToString()?.Trim();

                        // Eşleşme kontrolü
                        if (mevcutAdres != null && mevcutAdres.StartsWith("BİM") && mevcutAdres.Contains(arananKelime, StringComparison.OrdinalIgnoreCase))
                        {
                            teslimNoktalarıForm.gridView1.SetRowCellValue(j, "Apak", adres); // Eşleşen satıra TeslimFirmaAdresAdı değerini yaz
                            eslesenAdresler.Add(adres); // Eşleşen adresi ekle
                            eslesmeyenAdresler.Remove(adres); // Eşleşmeyenler listesinden çıkar
                            break; // Eşleşme bulunduğunda döngüden çık
                        }
                    }
                }

                // gridControl1 veri kaynağını güncelle
                teslimNoktalarıForm.gridControlApak.RefreshDataSource();

                // Eşleşenleri en üste taşıma
                for (int j = 0; j < teslimNoktalarıForm.gridView1.RowCount; j++)
                {
                    string mevcutAdres = teslimNoktalarıForm.gridView1.GetRowCellValue(j, "Apak")?.ToString();
                    if (!string.IsNullOrEmpty(mevcutAdres))
                    {
                        teslimNoktalarıForm.gridView1.MoveFirst(); // Eşleşen satırı en üstte göster
                        break; // İlk eşleşen satırı bulduğumuzda döngüden çık
                    }
                }

                // "Apak" sütununa göre sıralama yap
                teslimNoktalarıForm.gridView1.Columns["Apak"].SortOrder = DevExpress.Data.ColumnSortOrder.Descending;

                // Yükleme formunu kapat
                loadingForm.Close();

                // RowCellStyle olayını tanımla
                teslimNoktalarıForm.gridView1.RowCellStyle += (s, ev) =>
                {
                    string apakDegeri = teslimNoktalarıForm.gridView1.GetRowCellValue(ev.RowHandle, "Apak")?.ToString();
                    if (eslesenAdresler.Contains(apakDegeri))
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


        private void BtnSilme_ItemClick(object sender, ItemClickEventArgs e)
        {
            DataTable dt = gridControlApak.DataSource as DataTable;

            if (dt != null)
            {
                // Tüm satırları temizle
                dt.Clear();

                // Değişiklikleri yansıt
                gridControlApak.Refresh();

                MessageBox.Show("Tüm satırlar silindi.", "Silme İşlemi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Veri kaynağı mevcut değil.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }




           

        private void barButtonItem2_ItemClick(object sender, ItemClickEventArgs e)
        {
                // gridControlApak'ın veri kaynağını DataTable olarak al
                DataTable dt = gridControlApak.DataSource as DataTable;

                if (dt != null && dt.Rows.Count > 0) // Eğer DataTable varsa ve satır varsa
                {
                    using (SaveFileDialog saveFileDialog = new SaveFileDialog())
                    {
                        saveFileDialog.Filter = "Excel Dosyası|*.xlsx";
                        saveFileDialog.Title = "Excel'e Aktar";
                        saveFileDialog.FileName = "GridControlData.xlsx";

                        if (saveFileDialog.ShowDialog() == DialogResult.OK)
                        {
                            string filePath = saveFileDialog.FileName;

                            try
                            {
                                // EPPlus lisansını ayarla (ücretsiz kullanım için)
                                ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

                                // Excel dosyasını oluşturmak için EPPlus kullan
                                using (var package = new ExcelPackage())
                                {
                                    var worksheet = package.Workbook.Worksheets.Add("GridControlApak");

                                    // Sütun başlıklarını belirle
                                    string[] columnNames = {
                            "MusteriVkn",
                            "Proje",
                            "SiparisTarihi",
                            "YuklemeTarihi",
                            "TeslimTarihi",
                            "MusteriSiparisNo",
                            "MusteriReferansNo",
                            "İstenilenAracTipi",
                            "Aciklama",
                            "YuklemeFirmasıAdresAdı",
                            "AlıcıFirmaCariUnvanı",
                            "TeslimFirmaAdresAdı",
                            "İrsaliyeNo",
                            "İrsaliyeMiktar",
                            "Urun",
                            "KapAdet",
                            "AmbalajTipi",
                            "BrutKG",
                            "M3",
                            "Desi"
                        };

                                    // Sütun başlıklarını ekle
                                    for (int i = 0; i < columnNames.Length; i++)
                                    {
                                        worksheet.Cells[1, i + 1].Value = columnNames[i];
                                    }

                                    // Verileri ekle
                                    for (int i = 0; i < dt.Rows.Count; i++)
                                    {
                                        for (int j = 0; j < columnNames.Length; j++)
                                        {
                                            // Sadece mevcut sütunları al
                                            if (dt.Columns.Contains(columnNames[j]))
                                            {
                                                worksheet.Cells[i + 2, j + 1].Value = dt.Rows[i][columnNames[j]];
                                            }
                                            else
                                            {
                                                worksheet.Cells[i + 2, j + 1].Value = ""; // Sütun yoksa boş bırak
                                            }
                                        }
                                    }

                                    // Excel dosyasını kaydet
                                    var file = new System.IO.FileInfo(filePath);
                                    package.SaveAs(file);

                                    MessageBox.Show("Veriler başarıyla Excel'e aktarıldı.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("Excel'e aktarım sırasında bir hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Veri yok!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }

        private void ApakSiparisEkrani_Load(object sender, EventArgs e)
        {
            // GridView objesini alın
            GridView gridView = gridControlApak.MainView as GridView;

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
            GridView gridView = gridControlApak.MainView as GridView;

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


