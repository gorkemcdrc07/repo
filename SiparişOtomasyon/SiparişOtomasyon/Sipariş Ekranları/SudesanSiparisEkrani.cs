using DevExpress.Data.NetCompatibility.Extensions;
using DevExpress.XtraBars;
using SiparişOtomasyon.entity;
using SiparişOtomasyon.Teslim_Noktaları;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;
using DevExpress.XtraGrid.Views.Grid;


namespace SiparişOtomasyon.Sipariş_Ekranları
{
    public partial class SudesanSiparisEkrani : Form
    {
        public SudesanSiparisEkrani()
        {
            InitializeComponent();
            this.AllowDrop = true; // Drag-and-drop özelliğini etkinleştir
            this.DragEnter += new DragEventHandler(Form_DragEnter);
            this.DragDrop += new DragEventHandler(Form_DragDrop);

            BtnTeslimleriGetir.Enabled = false;
        }

        private void Form_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.Text))
            {
                e.Effect = DragDropEffects.Copy; // Veri metin ise kopyalamaya izin ver
            }
            else
            {
                e.Effect = DragDropEffects.None; // Diğer türler için hiçbir etki yok
            }
        }

        private void Form_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.Text))
            {
                string data = (string)e.Data.GetData(DataFormats.Text); // Get the dragged data
                string[] lines = data.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries); // Split into lines

                List<SiparisData> siparisList = new List<SiparisData>(); // List to hold order data

                // Variables to hold current shipment ID and type
                string currentSevkiyatID = null;
                string currentSevkiyatTipi = null;

                // Iterate through each line
                for (int i = 0; i < lines.Length; i++)
                {
                    var columns = lines[i].Split('\t'); // Split by tab

                    // Check for header lines containing "Sevkiyat ID" and "Sevkiyat Tipi"
                    if (lines[i].StartsWith("BASLIK :"))
                    {
                        // Read next lines for Sevkiyat ID and Sevkiyat Tipi
                        currentSevkiyatID = lines[i + 1].Split(':')[1].Trim(); // Get Sevkiyat ID
                        currentSevkiyatTipi = lines[i + 2].Split(':')[1].Trim(); // Get Sevkiyat Tipi
                        continue; // Move to next line
                    }

                    // Process data lines, skipping the header and any empty lines
                    if (columns.Length >= 9) // Minimum columns required
                    {
                        // Check for empty cells
                        if (!string.IsNullOrWhiteSpace(columns[3]) && // İstenilen Arac Tipi (from 3rd column)
                            !string.IsNullOrWhiteSpace(columns[4]) && // Sevk Emri Tarihi (from 4th column)
                            !string.IsNullOrWhiteSpace(columns[6]) && // Hesap Adı (from 6th column)
                            !string.IsNullOrWhiteSpace(columns[7]) && // Palet (from 7th column)
                            !string.IsNullOrWhiteSpace(columns[8]))   // Kilo (from 8th column)
                        {
                            DateTime sevkEmriTarihi;

                            // Validate and parse Sevk Emri Tarihi
                            if (DateTime.TryParse(columns[4], out sevkEmriTarihi))
                            {
                                // Determine the BrutKG value
                                string brutKGStr = columns[8].Trim();

                                // Add '0,' prefix if it's a whole number
                                if (decimal.TryParse(brutKGStr, out decimal brutKGValue))
                                {
                                    if (brutKGValue == Math.Truncate(brutKGValue))
                                    {
                                        brutKGStr = "0," + brutKGValue.ToString();
                                    }
                                    else
                                    {
                                        brutKGStr = brutKGValue.ToString(CultureInfo.InvariantCulture).Replace('.', ',');
                                    }

                                    // Create and add order data to the list
                                    siparisList.Add(new SiparisData
                                    {
                                        İstenilenAracTipi = columns[3], // Set from 3rd column
                                        SiparisTarihi = sevkEmriTarihi, // Set SiparisTarihi
                                        YuklemeTarihi = sevkEmriTarihi, // Set YuklemeTarihi
                                        TeslimFirmaAdresAdı = columns[6], // Hesap Adı
                                        KapAdet = int.Parse(columns[7]), // Palet
                                        BrutKG = decimal.Parse(brutKGStr.Replace(',', '.'), System.Globalization.CultureInfo.InvariantCulture), // Convert string to decimal
                                        MusteriSiparisNo = currentSevkiyatID // Get from current context
                                    });
                                }
                            }
                            else
                            {
                                continue; // Skip faulty row
                            }
                        }
                    }
                }

                // Group by MusteriSiparisNo and TeslimFirmaAdresAdı, and sum KapAdet and BrutKG
                var groupedData = siparisList
                    .GroupBy(x => new { x.MusteriSiparisNo, x.TeslimFirmaAdresAdı })
                    .Select(g => new SiparisData
                    {
                        MusteriSiparisNo = g.Key.MusteriSiparisNo,
                        TeslimFirmaAdresAdı = g.Key.TeslimFirmaAdresAdı,
                        KapAdet = 25, // Sabit KapAdet
                        BrutKG = 25.000m, // Sabit BrutKG
                        SiparisTarihi = g.First().SiparisTarihi, // Get the first SiparisTarihi in the group
                        YuklemeTarihi = g.First().YuklemeTarihi, // Get the first YuklemeTarihi in the group
                        TeslimTarihi = g.First().YuklemeTarihi.AddDays(1), // 1 gün ekle
                        İstenilenAracTipi = g.First().İstenilenAracTipi, // Get the first İstenilenAracTipi in the group
                        MusteriVkn = "8150001838", // Sabit VKN
                        Proje = 4, // Sabit Proje
                        YuklemeFirmasıAdresAdı = 10, // Sabit YuklemeFirmasıAdresAdı
                        Urun = 5, // Sabit Urun
                        AmbalajTipi = 1 // Sabit AmbalajTipi
                    }).ToList();

                // Format the BrutKG values as "x,xxx" for display
                foreach (var item in groupedData)
                {
                    item.BrutKG = Math.Round(item.BrutKG, 3); // Round to 3 decimal places
                }

                // Bind the grouped list to GridControl
                gridControlSudesan.DataSource = groupedData;
            }
            else
            {
                MessageBox.Show("Sürüklenen veri geçerli değil.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Sipariş verisi sınıfı
        public class SiparisData
        {
            public string İstenilenAracTipi { get; set; }
            public DateTime SiparisTarihi { get; set; }
            public DateTime YuklemeTarihi { get; set; }
            public DateTime TeslimTarihi { get; set; } // TeslimTarihi ekle
            public string TeslimFirmaAdresAdı { get; set; }
            public int KapAdet { get; set; }
            public decimal BrutKG { get; set; }
            public string MusteriSiparisNo { get; set; } // Yeni özellik
            public string MusteriVkn { get; set; } // Yeni özellik
            public int Proje { get; set; } // Yeni özellik
            public int YuklemeFirmasıAdresAdı { get; set; } // Yeni özellik
            public int Urun { get; set; } // Yeni özellik
            public int AmbalajTipi { get; set; } // Yeni özellik
            public string AlıcıFirmaCariUnvanı { get; set; }
        }

        private void BtnTeslimleriGetir_ItemClick(object sender, ItemClickEventArgs e)
        {
            using (var dbContext = new DbSiparisOtomasyonEntities6())
            {
                // gridControlSudesan'dan veri kaynağını al
                var dataSource = gridControlSudesan.DataSource as List<SiparisData>; // SiparisData kullanın

                if (dataSource != null)
                {
                    // Değerleri güncelle
                    foreach (var item in dataSource)
                    {
                        // TeslimFirmaAdresAdı değerini al
                        var teslimFirmaAdres = item.TeslimFirmaAdresAdı;

                        // Null veya boş değer kontrolü
                        if (string.IsNullOrWhiteSpace(teslimFirmaAdres))
                        {
                            continue; // Boş değer varsa bu öğeyi atla
                        }

                        // TblEslesmelerle tablosunda Sudesan ile eşleştir
                        var eslesme = dbContext.TblEslesmelerle
                            .FirstOrDefault(x => x.Sudesan.Equals(teslimFirmaAdres, StringComparison.OrdinalIgnoreCase));

                        if (eslesme != null)
                        {
                            // Eşleşen kaydın AdresID'sini al
                            var adresID = eslesme.AdresID;

                            // TeslimFirmaAdresAdı değerini güncelle
                            item.TeslimFirmaAdresAdı = adresID.ToString(); // Güncellenmiş değer (string'e çevirme)

                            // MusteriID'yi AlıcıFirmaCariUnvanı'na ata
                            item.AlıcıFirmaCariUnvanı = eslesme.MusteriID; // MusteriID'yi buraya atayın
                        }

                        // İstenilenAracTipi sütununa bak
                        switch (item.İstenilenAracTipi)
                        {
                            case "Kamyon":
                                item.İstenilenAracTipi = "3"; // Kamyon için 3 yaz
                                break;
                            case "Tır":
                                item.İstenilenAracTipi = "1"; // Tır için 1 yaz
                                break;
                        }
                    }

                    // gridControlSudesan'ın veri kaynağını güncelle ve hemen göster
                    gridControlSudesan.RefreshDataSource(); // Kaynağı güncelle
                }
            }
        }


        private void BtnEslesme_ItemClick(object sender, ItemClickEventArgs e)
        {
            BtnTeslimleriGetir.Enabled = true;
            BtnEslesme.Enabled = false;
            // Yükleme göstergesi
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

                // SudesanTeslimNoktalarıEslestir formunu oluştur
                var teslimNoktalarıForm = new SudesanTeslimNoktalarıEslestir();

                // Adresleri gridViewSudesan'dan al
                List<string> adresListesi = GetAdresListesi();

                // Eğer adres listesi boşsa kullanıcıya bilgi ver
                if (!adresListesi.Any())
                {
                    MessageBox.Show("Adres listesi boş!");
                    loadingForm.Close();
                    return;
                }

                // TeslimNoktaları formunu göster
                teslimNoktalarıForm.Show();

                // Eşleşen ve eşleşmeyen adresleri saklamak için listeler
                var eslesenAdresler = new List<string>();
                var eslesmeyenAdresler = new List<string>(adresListesi);

                // Adresleri eşleştirme işlemi
                MatchAddresses(adresListesi, teslimNoktalarıForm, eslesenAdresler, eslesmeyenAdresler);

                // gridControlSudesanEslesme veri kaynağını güncelle
                teslimNoktalarıForm.gridControlSudesanEslesme.RefreshDataSource();

                // Eşleşenleri en üste taşıma ve sıralama yap
                SortAndMoveMatchedAddresses(teslimNoktalarıForm);

                // Yükleme formunu kapat
                loadingForm.Close();

                // Sonuçları göster
                ShowResults(eslesenAdresler, eslesmeyenAdresler);
            }
        }

        private List<string> GetAdresListesi()
        {
            List<string> adresListesi = new List<string>();
            for (int i = 0; i < gridView1.RowCount; i++)
            {
                string adres = gridView1.GetRowCellValue(i, "TeslimFirmaAdresAdı")?.ToString()?.Trim();
                if (!string.IsNullOrEmpty(adres))
                {
                    adresListesi.Add(adres);
                }
            }
            return adresListesi;
        }

        private void MatchAddresses(List<string> adresListesi, SudesanTeslimNoktalarıEslestir teslimNoktalarıForm,
    List<string> eslesenAdresler, List<string> eslesmeyenAdresler)
        {
            foreach (var adres in adresListesi)
            {
                string musteriID = DetermineMusteriID(adres); // Müşteri ID'sini belirle
                if (string.IsNullOrEmpty(musteriID))
                {
                    continue; // Eğer müşteri ID'si boşsa, bir sonraki adresi kontrol et
                }

                // Müşteri ID'sine göre filtreleme yap
                for (int j = 0; j < teslimNoktalarıForm.gridView1.RowCount; j++)
                {
                    string mevcutMusteriID = teslimNoktalarıForm.gridView1.GetRowCellValue(j, "MusteriID")?.ToString();
                    string mevcutAdres = teslimNoktalarıForm.gridView1.GetRowCellValue(j, "AdresAdi")?.ToString()?.Trim();

                    // Müşteri ID'si kontrolü
                    if (musteriID == mevcutMusteriID && !string.IsNullOrEmpty(mevcutAdres) &&
                        IsMatch(adres, mevcutAdres))
                    {
                        teslimNoktalarıForm.gridView1.SetRowCellValue(j, "Sudesan", adres); // Eşleşen satıra Sudesan değerini yaz
                        teslimNoktalarıForm.gridView1.SetRowCellValue(j, "MusteriID", musteriID); // Müşteri ID'sini yaz

                        // gridControlSudesanEslesme'e eşleşen adresi ekle
                        var eslesmeRow = teslimNoktalarıForm.gridControlSudesanEslesme.DataSource as DataTable;
                        if (eslesmeRow != null)
                        {
                            DataRow newRow = eslesmeRow.NewRow();
                            newRow["Sudesan"] = adres; // Eşleşen adresi Sudesan sütununa ekle
                            newRow["MusteriID"] = musteriID; // Müşteri ID'sini ekle

                            // Yeni satırı en üstte ekle
                            eslesmeRow.Rows.InsertAt(newRow, 0); // Yeni satırı en üste ekle

                            // Satırı yeşil renkte boya (yeni eklenen satır)
                            int newRowHandle = teslimNoktalarıForm.gridView1.GetRowHandle(0); // Yeni eklenen satırın handle'ı
                            teslimNoktalarıForm.gridView1.SetRowCellValue(newRowHandle, "Sudesan", adres);

                            // Set appearance for the new row to highlight it
                            // Only set the appearance for the new row
                            teslimNoktalarıForm.gridView1.Appearance.Row.BackColor = Color.LightGreen; // Satır arka plan rengini yeşil yap
                            teslimNoktalarıForm.gridView1.Appearance.Row.Options.UseBackColor = true; // Arka plan rengini kullan
                        }

                        eslesenAdresler.Add(adres); // Eşleşen adresi ekle
                        eslesmeyenAdresler.Remove(adres); // Eşleşmeyenler listesinden çıkar
                        break; // Eşleşme bulunduğunda döngüden çık
                    }
                }
            }
        }




        private bool IsMatch(string teslimFirmaAdres, string mevcutAdres)
        {
            var teslimFirmaKelimeDizisi = teslimFirmaAdres.Split(new[] { ' ', '-', '*' }, StringSplitOptions.RemoveEmptyEntries);
            var mevcutKelimeDizisi = mevcutAdres.Split(new[] { ' ', '-', '*' }, StringSplitOptions.RemoveEmptyEntries);

            if (teslimFirmaKelimeDizisi.Length > 0 && mevcutKelimeDizisi.Length > 0)
            {
                string teslimFirmaIl = teslimFirmaKelimeDizisi[0];
                string mevcutIl = mevcutKelimeDizisi.FirstOrDefault();

                if (string.Equals(teslimFirmaIl, mevcutIl, StringComparison.OrdinalIgnoreCase))
                {
                    foreach (var kelime in teslimFirmaKelimeDizisi.Skip(1))
                    {
                        if (mevcutKelimeDizisi.Any(m => m.IndexOf(kelime, StringComparison.OrdinalIgnoreCase) >= 0))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        private string DetermineMusteriID(string teslimFirmaAdres)
        {
            if (teslimFirmaAdres.StartsWith("ŞOK", StringComparison.OrdinalIgnoreCase))
            {
                return "283";
            }
            else if (teslimFirmaAdres.StartsWith("MİGROS", StringComparison.OrdinalIgnoreCase))
            {
                return "205";
            }
            else if (teslimFirmaAdres.StartsWith("BİM", StringComparison.OrdinalIgnoreCase))
            {
                return "6";
            }
            else if (teslimFirmaAdres.StartsWith("A101", StringComparison.OrdinalIgnoreCase))
            {
                return "15";
            }

            return null;
        }

        private void SortAndMoveMatchedAddresses(SudesanTeslimNoktalarıEslestir teslimNoktalarıForm)
        {
            for (int j = 0; j < teslimNoktalarıForm.gridView1.RowCount; j++)
            {
                string mevcutAdres = teslimNoktalarıForm.gridView1.GetRowCellValue(j, "Sudesan")?.ToString();
                if (!string.IsNullOrEmpty(mevcutAdres))
                {
                    teslimNoktalarıForm.gridView1.MoveFirst();
                    break;
                }
            }

            teslimNoktalarıForm.gridView1.Columns["Sudesan"].SortOrder = DevExpress.Data.ColumnSortOrder.Descending;
        }

        private void ShowResults(List<string> eslesenAdresler, List<string> eslesmeyenAdresler)
        {
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



        private void BtnEslestir_ItemClick(object sender, ItemClickEventArgs e)
        {
            // Eğer form zaten açıksa, tekrar açmamak için kontrol edelim.
            SudesanTeslimNoktalarıEslestir form = Application.OpenForms.OfType<SudesanTeslimNoktalarıEslestir>().FirstOrDefault();

            if (form == null)
            {
                // Form zaten açık değilse, yeni bir form oluştur ve göster
                form = new SudesanTeslimNoktalarıEslestir();
                form.Show();
            }
            else
            {
                // Form açıksa, onu öne getir
                form.Show();
            }
        }

        private void BtnDısarıCıkart_ItemClick(object sender, ItemClickEventArgs e)
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
                        gridControlSudesan.ExportToXlsx(saveFileDialog.FileName);

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

        private void BtnSilme_ItemClick(object sender, ItemClickEventArgs e)
        {

            // GridControl'ün veri kaynağını temizle
            var gridView = gridControlSudesan.MainView as DevExpress.XtraGrid.Views.Grid.GridView;

            if (gridView != null)
            {
                // Seçili tüm satırları kaldır
                gridView.ClearSelection(); // Öncelikle seçimi temizleyin (isteğe bağlı)

                // GridControl'deki verileri temizle
                gridView.DeleteRow(0); // İlk satırı silerek başla
                while (gridView.RowCount > 0)
                {
                    gridView.DeleteRow(0); // Her seferinde ilk satırı sil
                }

                // Veritabanından da silme işlemi yapmanız gerekiyorsa buraya yazın
                // db.SaveChanges();
            }
        }

        private void SudesanSiparisEkrani_Load(object sender, EventArgs e)
        {
            // GridView objesini alın
            GridView gridView = gridControlSudesan.MainView as GridView;

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
            GridView gridView = gridControlSudesan.MainView as GridView;

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



