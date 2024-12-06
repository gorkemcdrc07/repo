using System;
using System.Data;
using System.Windows.Forms;
using DevExpress.XtraBars;
using DevExpress.XtraGrid.Views.Grid;
using SiparişOtomasyon.entity;
using System.Linq;
using DevExpress.Data.NetCompatibility.Extensions;
using SiparişOtomasyon.Teslim_Noktaları;
using System.Collections.Generic;
using System.Drawing;
using OfficeOpenXml; // EPPlus kütüphanesi için




namespace SiparişOtomasyon.Sipariş_Ekranları
{
    public partial class DogtasSiparisEkranı : Form
    {
        public DogtasSiparisEkranı()
        {
            InitializeComponent();
            gridControlDogtas.AllowDrop = true;

            // Drag and drop event handlers
            gridControlDogtas.DragEnter += GridControlDogtas_DragEnter;
            gridControlDogtas.DragDrop += GridControlDogtas_DragDrop;


        }

        private void GridControlDogtas_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.Text))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void GridControlDogtas_DragDrop(object sender, DragEventArgs e)
        {
            string droppedData = (string)e.Data.GetData(DataFormats.Text);

            var lines = droppedData.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            DataTable dt = new DataTable();
            dt.Columns.Add("CARİ UNVAN");
            dt.Columns.Add("YÜKLEME YERİ");
            dt.Columns.Add("TESLİM NOKTASI");
            dt.Columns.Add("TESLİM İLİ");
            dt.Columns.Add("ARAÇ TİPİ");
            dt.Columns.Add("İstenilenAracTipi");
            dt.Columns.Add("TeslimFirmaAdresAdı");

            // New columns to be populated automatically
            dt.Columns.Add("MusteriVkn");
            dt.Columns.Add("Proje");
            dt.Columns.Add("MusteriSiparisNo");
            dt.Columns.Add("YuklemeFirmasıAdresAdı");
            dt.Columns.Add("Urun");
            dt.Columns.Add("KapAdet");
            dt.Columns.Add("AmbalajTipi");
            dt.Columns.Add("BrutKG");
            dt.Columns.Add("SiparisTarihi"); // Add new columns
            dt.Columns.Add("YuklemeTarihi");
            dt.Columns.Add("TeslimTarihi");

            string currentCariUnvan = string.Empty;
            string currentYuklemeYeri = string.Empty;
            string currentAracTipi = string.Empty;

            foreach (var line in lines)
            {
                var columns = line.Split('\t');

                if (columns.Length > 0)
                {
                    if (!string.IsNullOrWhiteSpace(columns[0]))
                    {
                        currentCariUnvan = columns[0];
                    }
                    if (!string.IsNullOrWhiteSpace(columns[1]))
                    {
                        currentYuklemeYeri = columns[1];
                    }
                    if (columns.Length > 4 && !string.IsNullOrWhiteSpace(columns[4]))
                    {
                        currentAracTipi = columns[4];
                    }

                    if (columns.Length >= 3 && !string.IsNullOrWhiteSpace(columns[2]))
                    {
                        // Add the row only if the required columns are filled
                        dt.Rows.Add(currentCariUnvan, currentYuklemeYeri, columns[2], columns.Length > 3 ? columns[3] : "", currentAracTipi, currentAracTipi, "", "", "", "", "", "", "", "", DateTime.Now, DateTime.Now, DateTime.Now); // Add empty new columns
                    }
                }
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

            // Set the selected date and delivery date in the DataTable
            foreach (DataRow row in dt.Rows)
            {
                row["SiparisTarihi"] = selectedDate.ToString("dd.MM.yyyy");
                row["YuklemeTarihi"] = selectedDate.ToString("dd.MM.yyyy");
                row["TeslimTarihi"] = deliveryDate; // Use the calculated delivery date
            }

            // Automatically fill specific fields based on the number of filled rows
            FillAutomaticValues(dt);

            gridControlDogtas.DataSource = dt;

            PopulateDeliveryAddresses();
            CompactDeliveryAddresses();
        }


        private void FillAutomaticValues(DataTable dt)
        {
            foreach (DataRow row in dt.Rows)
            {
                // Ensure that the current row has valid data before filling the automatic values
                if (!string.IsNullOrEmpty(row["CARİ UNVAN"].ToString()) &&
                    !string.IsNullOrEmpty(row["YÜKLEME YERİ"].ToString()) &&
                    !string.IsNullOrEmpty(row["TESLİM NOKTASI"].ToString()))
                {
                    row["MusteriVkn"] = "5420055837";
                    row["Proje"] = "272";
                    row["MusteriSiparisNo"] = "DOĞTAŞ";
                    row["Urun"] = "218";
                    row["KapAdet"] = "25";
                    row["AmbalajTipi"] = "1";
                    row["BrutKG"] = "25.000";
                }
            }
        }



        private void PopulateDeliveryAddresses()
        {
            GridView gridView = gridControlDogtas.MainView as GridView;
            int rowCount = gridView.RowCount;

            for (int i = 0; i < rowCount; i++)
            {
                string teslimNoktasi = gridView.GetRowCellValue(i, "TESLİM NOKTASI")?.ToString();
                string aracTipi = gridView.GetRowCellValue(i, "ARAÇ TİPİ")?.ToString();

                if (!string.IsNullOrEmpty(teslimNoktasi))
                {
                    gridView.SetRowCellValue(i, "TeslimFirmaAdresAdı", teslimNoktasi);
                }

                if (!string.IsNullOrEmpty(aracTipi))
                {
                    gridView.SetRowCellValue(i, "İstenilenAracTipi", aracTipi);
                }
            }

            // Bu satırı kaldırın veya düzenlenebilir yapmak için true yapın
            gridView.OptionsBehavior.Editable = true;
            gridView.RefreshData();
        }






        private void CompactDeliveryAddresses()
        {
            GridView gridView = gridControlDogtas.MainView as GridView;
            int rowCount = gridView.RowCount;

            var nonEmptyAddresses = new System.Collections.Generic.List<string>();
            var nonEmptyAracTipleri = new System.Collections.Generic.List<string>();

            // Collect non-empty values from the TeslimFirmaAdresAdı and İstenilenAracTipi columns
            for (int i = 0; i < rowCount; i++)
            {
                string address = gridView.GetRowCellValue(i, "TeslimFirmaAdresAdı")?.ToString();
                string aracTipi = gridView.GetRowCellValue(i, "İstenilenAracTipi")?.ToString();

                if (!string.IsNullOrEmpty(address))
                {
                    nonEmptyAddresses.Add(address);
                }

                if (!string.IsNullOrEmpty(aracTipi))
                {
                    nonEmptyAracTipleri.Add(aracTipi);
                }
            }

            // Clear the columns and set the non-empty values back to the grid
            for (int i = 0; i < rowCount; i++)
            {
                gridView.SetRowCellValue(i, "TeslimFirmaAdresAdı", null);
                gridView.SetRowCellValue(i, "İstenilenAracTipi", null);
            }

            for (int i = 0; i < nonEmptyAddresses.Count; i++)
            {
                gridView.SetRowCellValue(i, "TeslimFirmaAdresAdı", nonEmptyAddresses[i]);
            }

            for (int i = 0; i < nonEmptyAracTipleri.Count; i++)
            {
                gridView.SetRowCellValue(i, "İstenilenAracTipi", nonEmptyAracTipleri[i]);
            }

            gridView.RefreshData();
        }

        private void BtnTeslimleriGetir_ItemClick(object sender, ItemClickEventArgs e)
        {
            using (var context = new DbSiparisOtomasyonEntities6())
            {
                var teslimFirmaAdreslar = gridControlDogtas.DataSource as DataTable;

                // DataTable'ın boş olup olmadığını kontrol et
                if (teslimFirmaAdreslar == null)
                {
                    MessageBox.Show("GridControl verisi alınamadı.");
                    return;
                }

                // AlıcıFirmaCariUnvanı sütununun mevcut olup olmadığını kontrol et
                if (!teslimFirmaAdreslar.Columns.Contains("AlıcıFirmaCariUnvanı"))
                {
                    // Gerekirse sütunu ekleyin
                    teslimFirmaAdreslar.Columns.Add("AlıcıFirmaCariUnvanı", typeof(string));
                }

                // İstenilenAracTipi sütununun mevcut olup olmadığını kontrol et
                if (!teslimFirmaAdreslar.Columns.Contains("İstenilenAracTipi"))
                {
                    // Gerekirse sütunu ekleyin
                    teslimFirmaAdreslar.Columns.Add("İstenilenAracTipi", typeof(string));
                }

                int eslesenSatirSayisi = 0;
                int eslesmeyenSatirSayisi = 0;

                foreach (DataRow row in teslimFirmaAdreslar.Rows)
                {
                    string teslimFirmaAdres = row["TeslimFirmaAdresAdı"].ToString();

                    var eslesme = context.TblEslesmelerle
                        .Where(es => es.Dogtas == teslimFirmaAdres)
                        .Select(es => new
                        {
                            DogtasID = es.AdresID,
                            MusteriID = es.MusteriID
                        })
                        .FirstOrDefault();

                    if (eslesme != null)
                    {
                        row["TeslimFirmaAdresAdı"] = eslesme.DogtasID; // DogtasID'yi TeslimFirmaAdresAdı sütununa yaz
                        row["AlıcıFirmaCariUnvanı"] = eslesme.MusteriID; // MusteriID'yi AlıcıFirmaCariUnvanı sütununa yaz

                        eslesenSatirSayisi++; // Eşleşen satır sayısını artır
                    }
                    else
                    {
                        eslesmeyenSatirSayisi++; // Eşleşmeyen satır sayısını artır
                    }

                    // İstenilenAracTipi sütununu kontrol et ve güncelle
                    string aracTipi = row["İstenilenAracTipi"].ToString();
                    if (aracTipi.Equals("KAMYON", StringComparison.OrdinalIgnoreCase))
                    {
                        row["İstenilenAracTipi"] = 3; // KAMYON için 3 yaz
                    }
                    else if (aracTipi.Equals("TIR", StringComparison.OrdinalIgnoreCase))
                    {
                        row["İstenilenAracTipi"] = 1; // TIR için 1 yaz
                    }
                }

                // Değişikliklerin GridControl'da yansımasını sağlamak için
                gridControlDogtas.Refresh();

                // Eşleşen ve eşleşmeyen satır sayılarını göster
                MessageBox.Show($"Eşleşen Satır Sayısı: {eslesenSatirSayisi}, Eşleşmeyen Satır Sayısı: {eslesmeyenSatirSayisi}",
                                "Sonuç", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                DogtasTeslimNoktalarıEslestir teslimNoktalarıForm = new DogtasTeslimNoktalarıEslestir();

                // Adresleri gridViewDogtas'tan al
                List<string> adresListesi = new List<string>();
                for (int i = 0; i < gridViewDogtas.RowCount; i++)
                {
                    string adres = gridViewDogtas.GetRowCellValue(i, "TeslimFirmaAdresAdı")?.ToString()?.Trim();
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
                    // Adresleri eşleştirme
                    for (int j = 0; j < teslimNoktalarıForm.gridView1.RowCount; j++)
                    {
                        string mevcutAdres = teslimNoktalarıForm.gridView1.GetRowCellValue(j, "AdresAdi")?.ToString()?.Trim();

                        // Tam eşleşme kontrolü
                        if (mevcutAdres != null && mevcutAdres.Equals(adres, StringComparison.OrdinalIgnoreCase))
                        {
                            // Eşleşen satıra TeslimFirmaAdresAdı değerini yaz
                            teslimNoktalarıForm.gridView1.SetRowCellValue(j, "Dogtas", adres);
                            eslesenAdresler.Add(adres); // Eşleşen adresi ekle
                            eslesmeyenAdresler.Remove(adres); // Eşleşmeyenler listesinden çıkar
                            break; // Eşleşme bulunduğunda döngüden çık
                        }
                    }
                }

                // gridControl1 veri kaynağını güncelle
                teslimNoktalarıForm.gridControlDogtas.RefreshDataSource();

                // Eşleşenleri en üste taşıma
                for (int j = 0; j < teslimNoktalarıForm.gridView1.RowCount; j++)
                {
                    string mevcutAdres = teslimNoktalarıForm.gridView1.GetRowCellValue(j, "Dogtas")?.ToString();
                    if (!string.IsNullOrEmpty(mevcutAdres))
                    {
                        teslimNoktalarıForm.gridView1.MoveFirst(); // Eşleşen satırı en üstte göster
                        break; // İlk eşleşen satırı bulduğumuzda döngüden çık
                    }
                }

                // "Dogtas" sütununa göre sıralama yap
                teslimNoktalarıForm.gridView1.Columns["Dogtas"].SortOrder = DevExpress.Data.ColumnSortOrder.Descending;

                // Yükleme formunu kapat
                loadingForm.Close();

                // RowCellStyle olayını tanımla
                teslimNoktalarıForm.gridView1.RowCellStyle += (s, ev) =>
                {
                    string dogtasDegeri = teslimNoktalarıForm.gridView1.GetRowCellValue(ev.RowHandle, "Dogtas")?.ToString();
                    if (eslesenAdresler.Contains(dogtasDegeri))
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


        private void BtnEslestir_ItemClick(object sender, ItemClickEventArgs e)
        {

                // DogtasTeslimNoktalarıEslestir formunu oluştur
                DogtasTeslimNoktalarıEslestir eslestirForm = new DogtasTeslimNoktalarıEslestir();

                // Formu aç
                eslestirForm.Show();
            }

        private void BtnDısarıCıkart_ItemClick(object sender, ItemClickEventArgs e)
        {
            // gridControlApak'ın veri kaynağını DataTable olarak al
            DataTable dt = gridControlDogtas.DataSource as DataTable;

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

        private void BtnSilme_ItemClick(object sender, ItemClickEventArgs e)
        {
            DataTable dt = gridControlDogtas.DataSource as DataTable;

            if (dt != null)
            {
                // Tüm satırları temizle
                dt.Clear();

                // Değişiklikleri yansıt
                gridControlDogtas.Refresh();

                MessageBox.Show("Tüm satırlar silindi.", "Silme İşlemi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Veri kaynağı mevcut değil.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnYuklemeFirma_ItemClick(object sender, ItemClickEventArgs e)
        {
            // Küçük ekranı (seçim ekranı) oluşturuyoruz
            var secimFormu = new Form();
            secimFormu.Text = "Yükleme Firması Seçimi";
            secimFormu.Size = new Size(300, 150);

            // Seçenekler için ComboBox ekliyoruz
            ComboBox comboBox = new ComboBox();
            comboBox.Items.Add("DOĞTAŞ KAYSERİ");
            comboBox.Items.Add("DOĞTAŞ İNEGÖL");
            comboBox.Dock = DockStyle.Fill;
            secimFormu.Controls.Add(comboBox);

            // Onay butonu ekliyoruz
            Button onayButton = new Button();
            onayButton.Text = "Onayla";
            onayButton.Dock = DockStyle.Bottom;
            onayButton.Click += (s, args) =>
            {
                // Seçilen firmaya göre işlem yapıyoruz
                string secilenFirma = comboBox.SelectedItem.ToString();

                // DogtasSiparisEkranı formuna erişim
                var dogtasSiparisEkrani = Application.OpenForms["DogtasSiparisEkranı"] as Form;
                if (dogtasSiparisEkrani != null)
                {
                    var gridControl = dogtasSiparisEkrani.Controls["gridControlDogtas"] as DevExpress.XtraGrid.GridControl;
                    if (gridControl != null)
                    {
                        var gridView = gridControl.MainView as DevExpress.XtraGrid.Views.Grid.GridView;
                        if (gridView != null)
                        {
                            int rowCount = gridView.RowCount;

                            // Seçilen firmaya göre yazılacak değer
                            int yazilacakDeger = 0;
                            if (secilenFirma == "DOĞTAŞ KAYSERİ")
                            {
                                yazilacakDeger = 25250;
                            }
                            else if (secilenFirma == "DOĞTAŞ İNEGÖL")
                            {
                                yazilacakDeger = 29667;
                            }

                            // Her satırda YuklemeFirmasıAdresAdı sütununa yazı yazıyoruz
                            for (int i = 0; i < rowCount; i++)
                            {
                                // 'YuklemeFirmasıAdresAdı' sütun indeksini burada varsayalım
                                gridView.SetRowCellValue(i, "YuklemeFirmasıAdresAdı", yazilacakDeger);
                            }
                        }
                    }
                }

                // Formu kapatıyoruz
                secimFormu.Close();
            };
            secimFormu.Controls.Add(onayButton);

            // Formu gösteriyoruz
            secimFormu.ShowDialog();
        }

        private void DogtasSiparisEkranı_Load(object sender, EventArgs e)
        {
            // GridView objesini alın
            GridView gridView = gridControlDogtas.MainView as GridView;

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
                labelControl2.Text = "GridView bulunamadı!";
            }
        }

        // Satır sayısını güncellemek için bir yöntem
        private void UpdateRowCountLabel()
        {
            GridView gridView = gridControlDogtas.MainView as GridView;

            if (gridView != null)
            {
                // Satır sayısını al ve label'ı güncelle
                int rowCount = gridView.RowCount;
                labelControl2.Text = $"Toplam Satır Sayısı: {rowCount}";
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
 
    


