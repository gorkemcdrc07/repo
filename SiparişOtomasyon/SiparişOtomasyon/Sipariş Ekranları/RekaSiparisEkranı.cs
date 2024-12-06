using System;
using System.Collections.Generic;
using System.Data; // DataTable için gerekli
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraGrid.Views.Grid;
using SiparişOtomasyon.entity;
using SiparişOtomasyon.Teslim_Noktaları;
using OfficeOpenXml;
using System.Drawing; // Add this line at the top of your file



namespace SiparişOtomasyon.Sipariş_Ekranları
{
    public partial class RekaSiparisEkranı : Form
    {
        // DataTable tanımlıyoruz
        DataTable dataTable;

        public RekaSiparisEkranı()
        {
            InitializeComponent();

            // DataTable yapılandırma
            dataTable = new DataTable();
            dataTable.Columns.Add("SiparisTarihi", typeof(string));
            dataTable.Columns.Add("YuklemeTarihi", typeof(string)); // YuklemeTarihi sütunu
            dataTable.Columns.Add("TeslimFirmaAdresAdı", typeof(string));
            dataTable.Columns.Add("İstenilenAracTipi", typeof(string));
            dataTable.Columns.Add("TeslimTarihi", typeof(string));

            // Sabit değerlerin atanacağı sütunlar
            dataTable.Columns.Add("MusteriVkn", typeof(string));
            dataTable.Columns.Add("Proje", typeof(string));
            dataTable.Columns.Add("MusteriSiparisNo", typeof(string));
            dataTable.Columns.Add("YuklemeFirmasıAdresAdı", typeof(string));
            dataTable.Columns.Add("AlıcıFirmaCariUnvanı", typeof(string));
            dataTable.Columns.Add("Urun", typeof(string));
            dataTable.Columns.Add("KapAdet", typeof(string));
            dataTable.Columns.Add("AmbalajTipi", typeof(string));
            dataTable.Columns.Add("BrutKG", typeof(decimal));

            // GridControl'ü DataTable'a bağlıyoruz
            gridControlReka.DataSource = dataTable;

            // Drag and drop olaylarını tanımlıyoruz
            gridControlReka.AllowDrop = true;
            gridControlReka.DragEnter += gridControlReka_DragEnter;
            gridControlReka.DragDrop += gridControlReka_DragDrop;

            BtnTeslimleriGetir.Enabled = false;
        }

        // Drag işlemi başladığında yapılacaklar
        private void gridControlReka_DragEnter(object sender, DragEventArgs e)
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

        // Drag işlemi bırakıldığında yapılacaklar
        private void gridControlReka_DragDrop(object sender, DragEventArgs e)
        {
            // Sürüklenen veriyi alıyoruz
            string data = (string)e.Data.GetData(DataFormats.Text);

            // Veriyi parçala (örneğin satır bazında ayırarak)
            string[] rows = data.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string row in rows)
            {
                // Satırdaki sütunları ayrıştır
                string[] columns = row.Split('\t'); // Tab ile ayırılmış varsayıyoruz

                // DataTable'da yeni satır oluşturuyoruz
                DataRow newRow = dataTable.NewRow();

                // İlk sütunu SiparisTarihi ve YuklemeTarihi için kullanıyoruz
                newRow["SiparisTarihi"] = columns.Length > 0 ? columns[0] : "";
                newRow["YuklemeTarihi"] = columns.Length > 0 ? columns[0] : ""; // İlk sütundaki veriyi burada da kullanıyoruz

                // TeslimFirmaAdresAdı: İkinci ve üçüncü sütunları birleştir
                newRow["TeslimFirmaAdresAdı"] = (columns.Length > 1 ? columns[1] : "") + " " + (columns.Length > 2 ? columns[2] : "");

                // İstenilenAracTipi: Dördüncü sütun
                newRow["İstenilenAracTipi"] = columns.Length > 3 ? columns[3] : "";

                // TeslimTarihi: Yedinci sütun
                newRow["TeslimTarihi"] = columns.Length > 4 ? columns[4] : "";

                // Sabit değerlerin atanması
                newRow["MusteriVkn"] = "7340632800";
                newRow["Proje"] = "5";
                newRow["MusteriSiparisNo"] = "RKA";
                newRow["YuklemeFirmasıAdresAdı"] = "9";
                newRow["AlıcıFirmaCariUnvanı"] = "6";
                newRow["Urun"] = "3";
                newRow["KapAdet"] = "25";
                newRow["AmbalajTipi"] = "1";
                newRow["BrutKG"] = "25.000";

                // Yeni satırı DataTable'a ekliyoruz
                dataTable.Rows.Add(newRow);
            }

            // GridControl'ü güncelle
            gridControlReka.RefreshDataSource();
        }

        private void BtnEslestir_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            // ApakTeslimNoktalarıEslestir formunu oluştur
            RekaTeslimNoktalarıEslestir eslestirForm = new RekaTeslimNoktalarıEslestir();

            // Formu aç
            eslestirForm.Show();
        }

        private void BtnTeslimleriGetir_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            using (var context = new DbSiparisOtomasyonEntities6())
            {
                // gridControlAdko'dan DataTable'ı al
                var teslimFirmaAdreslar = gridControlReka.DataSource as DataTable;

                if (teslimFirmaAdreslar != null)
                {
                    int eslesenSatirSayisi = 0; // Eşleşen satır sayısı
                    int eslesmeyenSatirSayisi = 0; // Eşleşmeyen satır sayısı

                    foreach (DataRow row in teslimFirmaAdreslar.Rows)
                    {
                        string teslimFirmaAdres = row["TeslimFirmaAdresAdı"].ToString();

                        // TblEslesmelerle'den Reka'ya göre eşleşen verileri al
                        var eslesme = context.TblEslesmelerle
                            .Where(es => es.Reka == teslimFirmaAdres) // Adko yerine Reka kullanıldı
                            .Select(es => new
                            {
                                RekaID = es.AdresID, // veya ihtiyaca göre uygun alanı seçin
                                MusteriID = es.MusteriID
                            })
                            .FirstOrDefault();

                        // eslesme değeri null ise devam et, aksi takdirde güncelle
                        if (eslesme != null)
                        {
                            // TeslimFirmaAdresAdı'nı güncelle
                            row["TeslimFirmaAdresAdı"] = eslesme.RekaID; // AdkoID'yi TeslimFirmaAdresAdı sütununa yaz

                            // AlıcıFirmaCariUnvanı'nı güncelle
                            row["AlıcıFirmaCariUnvanı"] = eslesme.MusteriID; // MusteriID'yi AlıcıFirmaCariUnvanı sütununa yaz

                            // İstenilenAracTipi'ni güncelle
                            string aracTipi = row["İstenilenAracTipi"].ToString();
                            if (aracTipi == "KAMYON")
                            {
                                row["İstenilenAracTipi"] = 3; // "KAMYON" ise 3 ile değiştir
                            }
                            else if (aracTipi == "TIR")
                            {
                                row["İstenilenAracTipi"] = 1; // "TIR" ise 1 ile değiştir
                            }

                            eslesenSatirSayisi++; // Eşleşen satır sayısını artır
                        }
                        else
                        {
                            eslesmeyenSatirSayisi++; // Eşleşmeyen satır sayısını artır
                        }
                    }

                    // Değişikliklerin GridControl'da yansımasını sağlamak için
                    gridControlReka.Refresh();

                    // Eşleşen ve eşleşmeyen satır sayılarını MessageBox ile göster
                    string message = $"Eşleşen Satır Sayısı: {eslesenSatirSayisi}\nEşleşmeyen Satır Sayısı: {eslesmeyenSatirSayisi}";
                    MessageBox.Show(message, "Sonuç", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

        }
        private List<string> yeniEslesenAdresler = new List<string>(); // Yeni eşleşen adresler listesi

        private void BtnEslesme_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            BtnTeslimleriGetir.Enabled = true;
            BtnEslesme.Enabled = false;
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

                RekaTeslimNoktalarıEslestir teslimNoktalarıForm = new RekaTeslimNoktalarıEslestir();

                // Adresleri gridView1'den al
                List<string> adresListesi = new List<string>();
                for (int i = 0; i < gridView1.RowCount; i++)
                {
                    string adres = gridView1.GetRowCellValue(i, "TeslimFirmaAdresAdı")?.ToString()?.Trim();
                    if (!string.IsNullOrEmpty(adres))
                    {
                        adresListesi.Add(adres);
                    }
                }

                if (adresListesi.Count == 0)
                {
                    MessageBox.Show("Adres listesi boş!");
                    loadingForm.Close();
                    return;
                }

                teslimNoktalarıForm.Show();

                List<string> eslesenAdresler = new List<string>();
                List<string> eslesmeyenAdresler = new List<string>(adresListesi);

                foreach (var adres in adresListesi)
                {
                    string arananKelime = adres;

                    for (int j = 0; j < teslimNoktalarıForm.gridViewRekaEslesme.RowCount; j++)
                    {
                        string mevcutAdres = teslimNoktalarıForm.gridViewRekaEslesme.GetRowCellValue(j, "AdresAdi")?.ToString()?.Trim();

                        if (mevcutAdres != null && mevcutAdres.StartsWith("BİM") && mevcutAdres.IndexOf(arananKelime, StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            teslimNoktalarıForm.gridViewRekaEslesme.SetRowCellValue(j, "Reka", adres);
                            eslesenAdresler.Add(adres);
                            eslesmeyenAdresler.Remove(adres);
                            break;
                        }
                    }
                }

                var dataList = (List<TblEslesmelerle>)teslimNoktalarıForm.gridControlRekaEslesme.DataSource;

                // "Reka" sütununa göre sıralama
                var sortedList = dataList
                    .OrderByDescending(item => !string.IsNullOrEmpty(item.Reka) ? 1 : 0)
                    .ThenBy(item => item.AdresAdi)
                    .ToList();

                teslimNoktalarıForm.gridControlRekaEslesme.DataSource = sortedList;
                teslimNoktalarıForm.gridViewRekaEslesme.RefreshData();

                // Yeni eşleşen satırları en üste taşı
                foreach (var adres in eslesenAdresler)
                {
                    var rowHandle = sortedList.FindIndex(x => x.Reka == adres);
                    if (rowHandle >= 0)
                    {
                        sortedList[rowHandle].Reka = adres; // Eşleşen adresin Reka sütununu güncelle
                        sortedList.Insert(0, sortedList[rowHandle]); // Yeni eşleşen adresi listenin en başına ekle
                        sortedList.RemoveAt(rowHandle + 1); // Orijinal yerinden sil
                    }
                }

                teslimNoktalarıForm.gridControlRekaEslesme.DataSource = sortedList;
                teslimNoktalarıForm.gridViewRekaEslesme.RefreshData();

                // RowCellStyle olayı ile renk değiştirme
                teslimNoktalarıForm.gridViewRekaEslesme.RowCellStyle += (s, ev) =>
                {
                    string rekaDegeri = teslimNoktalarıForm.gridViewRekaEslesme.GetRowCellValue(ev.RowHandle, "Reka")?.ToString();
                    if (eslesenAdresler.Contains(rekaDegeri))
                    {
                        ev.Appearance.BackColor = Color.LightGreen; // Yeşil renge boya
                    }
                };

                loadingForm.Close();

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







        private void BtnDısarıCıkart_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            // gridControlApak'ın veri kaynağını DataTable olarak al
            DataTable dt = gridControlReka.DataSource as DataTable;

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

        private void BtnSilme_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            // gridControlApak'ın veri kaynağını DataTable olarak al
            DataTable dt = gridControlReka.DataSource as DataTable;

            if (dt != null)
            {
                // Tüm satırları sil
                dt.Clear();

                // gridControlApak'ı güncelle
                gridControlReka.Refresh();
            }
        }

        private void RekaSiparisEkranı_Load(object sender, EventArgs e)
        {
            // GridView objesini alın
            GridView gridView = gridControlReka.MainView as GridView;

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
            GridView gridView = gridControlReka.MainView as GridView;

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



