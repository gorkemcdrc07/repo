using SiparişOtomasyon.entity;
using SiparişOtomasyon.Teslim_Noktaları;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;
using System.Text.RegularExpressions;
using DevExpress.XtraGrid.Views.Grid;

namespace SiparişOtomasyon.Sipariş_Ekranları
{
    public partial class EsGlobalSiparisEkranı : Form
    {
        public EsGlobalSiparisEkranı()
        {
            InitializeComponent();
            gridControlEsGlobal.AllowDrop = true;
            gridControlEsGlobal.DragEnter += GridControlEsGlobal_DragEnter;
            gridControlEsGlobal.DragDrop += GridControlEsGlobal_DragDrop;
        }

        private void GridControlEsGlobal_DragEnter(object sender, DragEventArgs e)
        {
            // Sürüklenen içerik metin veya HTML ise bırakma işlemini etkinleştir
            if (e.Data.GetDataPresent(DataFormats.Text) || e.Data.GetDataPresent(DataFormats.Html))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void GridControlEsGlobal_DragDrop(object sender, DragEventArgs e)
        {
            DataTable dt = new DataTable();

            // Sütunları tanımla
            dt.Columns.Add("SiparisTarihi", typeof(DateTime));
            dt.Columns.Add("YuklemeTarihi", typeof(DateTime));
            dt.Columns.Add("TeslimTarihi", typeof(DateTime));
            dt.Columns.Add("TeslimFirmaAdresAdı", typeof(string));

            // Eklenen statik sütunlar
            dt.Columns.Add("MusteriVkn", typeof(string));
            dt.Columns.Add("Proje", typeof(string));
            dt.Columns.Add("İstenilenAracTipi", typeof(string));
            dt.Columns.Add("YuklemeFirmasıAdresAdı", typeof(string));
            dt.Columns.Add("AlıcıFirmaCariUnvanı", typeof(string));
            dt.Columns.Add("Urun", typeof(string));
            dt.Columns.Add("KapAdet", typeof(int));
            dt.Columns.Add("AmbalajTipi", typeof(int));
            dt.Columns.Add("BrutKG", typeof(int));
            dt.Columns.Add("MusteriSiparisNo", typeof(int));

            // Tarih formatları
            string[] formats = { "dd.MM.yyyy", "dd/MM/yyyy" };

            // Metin verisini al ve satırları işleyerek DataTable'a ekle
            if (e.Data.GetDataPresent(DataFormats.Text))
            {
                string textData = (string)e.Data.GetData(DataFormats.Text);
                string[] rows = textData.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

                foreach (var row in rows)
                {
                    string[] columns = row.Split('\t'); // Tablo sütunları arasındaki ayraç karakteri
                    if (columns.Length >= 3) // Her satırda en az 3 sütun varsa
                    {
                        // Yükleme ve Sipariş Tarihi
                        string yuklemeTarihiStr = columns[0];
                        if (yuklemeTarihiStr.Length >= 10 && yuklemeTarihiStr.Substring(6, 1) == "0")
                        {
                            yuklemeTarihiStr = yuklemeTarihiStr.Substring(0, 6) + yuklemeTarihiStr.Substring(7);
                        }

                        if (!DateTime.TryParseExact(yuklemeTarihiStr, formats, null, System.Globalization.DateTimeStyles.None, out DateTime yuklemeTarihi))
                        {
                            MessageBox.Show($"Geçersiz tarih formatı: {columns[0]}", "Tarih Hatası", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            continue;
                        }

                        // Teslim Tarihi
                        string teslimTarihiStr = columns[1];
                        if (teslimTarihiStr.Length >= 10 && teslimTarihiStr.Substring(6, 1) == "0")
                        {
                            teslimTarihiStr = teslimTarihiStr.Substring(0, 6) + teslimTarihiStr.Substring(7);
                        }

                        if (!DateTime.TryParseExact(teslimTarihiStr, formats, null, System.Globalization.DateTimeStyles.None, out DateTime teslimTarihi))
                        {
                            MessageBox.Show($"Geçersiz tarih formatı: {columns[1]}", "Tarih Hatası", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            continue;
                        }

                        // Teslim Firma Adresi - '+' işaretine göre böl ve her adresi yeni satıra ekle
                        string teslimFirmaAdresAdı = columns[2];
                        string[] adresParts = teslimFirmaAdresAdı.Split(new[] { '+' }, StringSplitOptions.RemoveEmptyEntries);

                        foreach (var adres in adresParts)
                        {
                            DataRow dataRow = dt.NewRow();
                            dataRow["SiparisTarihi"] = yuklemeTarihi;
                            dataRow["YuklemeTarihi"] = yuklemeTarihi;
                            dataRow["TeslimTarihi"] = teslimTarihi;
                            dataRow["TeslimFirmaAdresAdı"] = "BİM " + adres.Trim();  // "BİM" ekleniyor

                            // Statik sütunlara verileri ekle
                            dataRow["MusteriVkn"] = "3770908845";
                            dataRow["Proje"] = "193";
                            dataRow["İstenilenAracTipi"] = "1";
                            dataRow["YuklemeFirmasıAdresAdı"] = "20284";
                            dataRow["AlıcıFirmaCariUnvanı"] = "6";
                            dataRow["Urun"] = "157";
                            dataRow["KapAdet"] = 25;
                            dataRow["AmbalajTipi"] = 1;
                            dataRow["BrutKG"] = 25000;
                            dataRow["MusteriSiparisNo"] = "ES GLOBAL";

                            dt.Rows.Add(dataRow);
                        }
                    }
                }
            }

            // DataTable'ı GridControl'e bağla
            gridControlEsGlobal.DataSource = dt;
        }

        private void BtnEslesme_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs itemClickEventArgs)
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
                EsGlobalTeslimNoktalarıEslestir teslimNoktalarıForm = new EsGlobalTeslimNoktalarıEslestir();

                // Adresleri gridViewEsGlobal'tan al
                List<string> adresListesi = new List<string>();
                for (int i = 0; i < gridViewEsGlobal.RowCount; i++)
                {
                    string adres = gridViewEsGlobal.GetRowCellValue(i, "TeslimFirmaAdresAdı")?.ToString()?.Trim();
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
                    // Adresin sonundaki sayıyı ayıklama (örneğin: "HASANOĞLAN 2")
                    var adresSayisiMatch = Regex.Match(adres, @"\s(\d+)$");
                    string adresSayisi = adresSayisiMatch.Success ? adresSayisiMatch.Groups[1].Value : "";

                    // "BİM" ve yanında kelimelerle eşleşme için düzenli ifade kullan
                    if (adres.StartsWith("BİM", StringComparison.OrdinalIgnoreCase))
                    {
                        // Parantez içindeki sayıyı kaldır
                        string temizAdres = Regex.Replace(adres, @"\s*\(\d+\)$", "").Trim();

                        // Eşleşme yapacak adresleri kontrol et
                        for (int j = 0; j < teslimNoktalarıForm.gridViewEsGlobalEslesme.RowCount; j++)
                        {
                            string mevcutAdres = teslimNoktalarıForm.gridViewEsGlobalEslesme.GetRowCellValue(j, "AdresAdi")?.ToString()?.Trim();

                            // "BİM" ve kelimelerle eşleşme kontrolü
                            if (mevcutAdres != null && mevcutAdres.StartsWith("BİM", StringComparison.OrdinalIgnoreCase))
                            {
                                // Parantez içindeki sayıyı kaldır
                                string mevcutTemizAdres = Regex.Replace(mevcutAdres, @"\s*\(\d+\)$", "").Trim();

                                // Mevcut adresin sonundaki sayıyı ayıkla
                                var mevcutAdresSayisiMatch = Regex.Match(mevcutAdres, @"-(\d+)$");
                                string mevcutAdresSayisi = mevcutAdresSayisiMatch.Success ? mevcutAdresSayisiMatch.Groups[1].Value : "";

                                // Eğer hem adres hem de sayılar eşleşiyorsa
                                if (mevcutTemizAdres.Equals(temizAdres, StringComparison.OrdinalIgnoreCase) && adresSayisi == mevcutAdresSayisi)
                                {
                                    // Eşleşen satıra TeslimFirmaAdresAdı değerini yaz
                                    teslimNoktalarıForm.gridViewEsGlobalEslesme.SetRowCellValue(j, "EsGlobal", adres);
                                    eslesenAdresler.Add(adres); // Eşleşen adresi ekle
                                    eslesmeyenAdresler.Remove(adres); // Eşleşmeyenler listesinden çıkar
                                }
                            }
                        }
                    }
                }

                // gridControl1 veri kaynağını güncelle
                teslimNoktalarıForm.gridControlEsGlobalEslesme.RefreshDataSource();

                // Eşleşenleri en üste taşıma
                for (int j = 0; j < teslimNoktalarıForm.gridViewEsGlobalEslesme.RowCount; j++)
                {
                    string mevcutAdres = teslimNoktalarıForm.gridViewEsGlobalEslesme.GetRowCellValue(j, "EsGlobal")?.ToString();
                    if (!string.IsNullOrEmpty(mevcutAdres))
                    {
                        teslimNoktalarıForm.gridViewEsGlobalEslesme.MoveFirst(); // Eşleşen satırı en üstte göster
                        break; // İlk eşleşen satırı bulduğumuzda döngüden çık
                    }
                }

                // "EsGlobal" sütununa göre sıralama yap
                teslimNoktalarıForm.gridViewEsGlobalEslesme.Columns["EsGlobal"].SortOrder = DevExpress.Data.ColumnSortOrder.Descending;

                // Yükleme formunu kapat
                loadingForm.Close();

                // RowCellStyle olayını tanımla
                teslimNoktalarıForm.gridViewEsGlobalEslesme.RowCellStyle += (s, ev) =>
                {
                    string EsGlobalDegeri = teslimNoktalarıForm.gridViewEsGlobalEslesme.GetRowCellValue(ev.RowHandle, "EsGlobal")?.ToString();
                    if (eslesenAdresler.Contains(EsGlobalDegeri))
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

        private void BtnEslestir_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            // EsGlobalTeslimNoktalarıEslestir formunu aç
            EsGlobalTeslimNoktalarıEslestir teslimNoktalarıForm = new EsGlobalTeslimNoktalarıEslestir();
            teslimNoktalarıForm.Show();
        }

        private void BtnTeslimleriGetir_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                // GridControlEsGlobal'deki tüm satırları alıyoruz
                for (int rowHandle = 0; rowHandle < gridViewEsGlobal.RowCount; rowHandle++) // Tüm satırlar üzerinde döngü başlatıyoruz
                {
                    // TeslimFirmaAdresAdı sütunundaki değeri alıyoruz
                    string teslimFirmaAdresAdi = gridViewEsGlobal.GetRowCellValue(rowHandle, "TeslimFirmaAdresAdı")?.ToString(); // Null kontrolü de ekledim

                    if (string.IsNullOrEmpty(teslimFirmaAdresAdi))
                        continue; // Eğer boş veya null ise, bu satırı atla

                    // DbSiparisOtomasyonEntities6 bağlamını kullanarak TblEslesmelerle tablosunda EsGlobal sütununda ara
                    using (var context = new DbSiparisOtomasyonEntities6())
                    {
                        // EsGlobal sütununda teslimFirmaAdresAdi ile eşleşen bir satır arıyoruz
                        var eslesme = context.TblEslesmelerle
                            .FirstOrDefault(e1 => e1.EsGlobal == teslimFirmaAdresAdi); // EsGlobal sütunuyla karşılaştırıyoruz

                        if (eslesme != null)
                        {
                            // AdresID'yi string olarak alıyoruz
                            string adresID = eslesme.AdresID.ToString();

                            // İlgili hücreye AdresID değerini yazıyoruz
                            gridViewEsGlobal.SetRowCellValue(rowHandle, "TeslimFirmaAdresAdı", adresID);
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

        private void BtnDısarıCıkart_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            // SaveFileDialog ile kullanıcıdan dosya yolu alın
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "Excel Dosyası (*.xlsx)|*.xlsx|Tüm Dosyalar (*.*)|*.*";
                saveFileDialog.Title = "Excel Olarak Kaydet";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    // gridControlAdko'daki verileri Excel dosyası olarak kaydet
                    try
                    {
                        // DevExpress'in Excel export methodunu kullan
                        gridControlEsGlobal.ExportToXlsx(saveFileDialog.FileName);

                        // İşlem başarılı ise kullanıcıya bilgi ver
                        MessageBox.Show("Veriler başarıyla dışa aktarıldı.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        // Hata durumunda kullanıcıya bilgi ver
                        MessageBox.Show("Veriler dışa aktarılırken bir hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void BtnSilme_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                // Satırları silmek için gridViewGezer'in RowCount özelliğini kontrol edip, her satırı tek tek siliyoruz
                int rowCount = gridViewEsGlobal.RowCount;

                // Satırları silme işlemi
                for (int i = rowCount - 1; i >= 0; i--)
                {
                    gridViewEsGlobal.DeleteRow(i); // Satırları tersten silmek, index sorunlarını engeller
                }
            }
            catch (Exception ex)
            {
                // Hata yönetimi
                MessageBox.Show($"Bir hata oluştu: {ex.Message}");
            }
        }

        private void EsGlobalSiparisEkranı_Load(object sender, EventArgs e)
        {
            // GridView objesini alın
            GridView gridView = gridControlEsGlobal.MainView as GridView;

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
            GridView gridView = gridControlEsGlobal.MainView as GridView;

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

 










