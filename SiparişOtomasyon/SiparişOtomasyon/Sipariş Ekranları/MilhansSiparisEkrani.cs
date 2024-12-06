using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraBars;
using OfficeOpenXml;
using SiparişOtomasyon.entity;
using SiparişOtomasyon.Teslim_Noktaları;
using System.Drawing;
using DevExpress.XtraGrid.Views.Grid;

namespace SiparişOtomasyon.Sipariş_Ekranları
{
    public partial class MilhansSiparisEkrani : Form
    {
        public MilhansSiparisEkrani()
        {
            InitializeComponent();

            // EPPlus lisans bağlamını ayarlayın
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // Veya LicenseContext.Commercial, eğer ticari kullanım yapıyorsanız.

            // Drag and Drop olaylarını etkinleştirin
            gridControlMilhans.AllowDrop = true;
            gridControlMilhans.DragEnter += gridControlMilhans_DragEnter;
            gridControlMilhans.DragDrop += gridControlMilhans_DragDrop;

            BtnTeslimleriGetir.Enabled = false;
        }

        // DragEnter olayında sürüklenen dosyanın uygun olup olmadığını kontrol edin
        private void gridControlMilhans_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
        }

        // DragDrop olayında dosyayı alın ve işleyin
        private void gridControlMilhans_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (files.Length > 0)
            {
                string filePath = files[0];
                ImportExcelToGridControl(filePath);
            }
        }

        // Excel'den verileri gridControl'a aktar
        private void ImportExcelToGridControl(string filePath)
        {
            using (var package = new ExcelPackage(new FileInfo(filePath)))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets[0]; // İlk sayfayı alıyoruz

                // Veri tabloyu oluşturalım
                DataTable dt = new DataTable();
                dt.Columns.Add("SiparisTarihi", typeof(string));
                dt.Columns.Add("YuklemeTarihi", typeof(string));
                dt.Columns.Add("TeslimTarihi", typeof(string));
                dt.Columns.Add("TeslimFirmaAdresAdı", typeof(string));
                dt.Columns.Add("İstenilenAracTipi", typeof(string)); // D sütununu ekleyelim
                dt.Columns.Add("MusteriVkn", typeof(string));
                dt.Columns.Add("Proje", typeof(string));
                dt.Columns.Add("MusteriSiparisNo", typeof(string));
                dt.Columns.Add("YuklemeFirmasıAdresAdı", typeof(string));
                dt.Columns.Add("Urun", typeof(string));
                dt.Columns.Add("KapAdet", typeof(int));
                dt.Columns.Add("AmbalajTipi", typeof(int));
                dt.Columns.Add("BrutKG", typeof(decimal));

                // Excel'deki verileri okuyalım
                for (int row = 2; row <= worksheet.Dimension.End.Row; row++)
                {
                    // A sütunundaki yükleme tarihini alalım
                    string yuklemeTarihiStr = worksheet.Cells[row, 1].Text;
                    DateTime yuklemeTarihi = DateTime.Parse(yuklemeTarihiStr); // Tarihi DateTime türüne dönüştürelim

                    // C sütunundaki veriyi alalım
                    string teslimAdresler = worksheet.Cells[row, 3].Text;

                    // D sütunundaki İstenilen Arac Tipini alalım
                    string istenilenAracTipi = worksheet.Cells[row, 4].Text;

                    // Adresleri + işaretine göre ayıralım
                    string[] adresler = teslimAdresler.Split(new[] { '+' }, StringSplitOptions.RemoveEmptyEntries);
                    var tempAddresses = new List<string>();

                    // Adresleri ayıralım ve temizleyelim
                    foreach (var adres in adresler)
                    {
                        string temizlenmisAdres = adres.Trim();
                        if (!string.IsNullOrEmpty(temizlenmisAdres))
                        {
                            // Adresleri tempAddresses listesine ekle
                            tempAddresses.Add(temizlenmisAdres);
                        }
                    }

                    // Adresleri ters sırayla ekleyelim
                    for (int i = tempAddresses.Count - 1; i >= 0; i--)
                    {
                        // YuklemeTarihi'ni hem SiparisTarihi hem de YuklemeTarihi olarak kullanıyoruz
                        DataRow newRow = dt.NewRow();
                        newRow["SiparisTarihi"] = yuklemeTarihi.ToString("dd/MM/yyyy"); // Tarih formatı
                        newRow["YuklemeTarihi"] = yuklemeTarihi.ToString("dd/MM/yyyy");
                        newRow["TeslimTarihi"] = yuklemeTarihi.AddDays(1).ToString("dd/MM/yyyy"); // YuklemeTarihi + 1 gün
                        newRow["TeslimFirmaAdresAdı"] = tempAddresses[i];
                        newRow["İstenilenAracTipi"] = istenilenAracTipi;
                        newRow["MusteriVkn"] = "0680617957";
                        newRow["Proje"] = "202";
                        newRow["MusteriSiparisNo"] = "MILHANS";
                        newRow["YuklemeFirmasıAdresAdı"] = "21994";
                        newRow["Urun"] = "165";
                        newRow["KapAdet"] = 25;
                        newRow["AmbalajTipi"] = 1;
                        newRow["BrutKG"] = 25.000m;

                        dt.Rows.Add(newRow);
                    }

                    // Eğer C sütununda + olmayan bir veri varsa, onu da ekleyelim
                    if (!string.IsNullOrEmpty(teslimAdresler) && !tempAddresses.Any())
                    {
                        DataRow newRow = dt.NewRow();
                        newRow["SiparisTarihi"] = yuklemeTarihi.ToString("dd/MM/yyyy");
                        newRow["YuklemeTarihi"] = yuklemeTarihi.ToString("dd/MM/yyyy");
                        newRow["TeslimTarihi"] = yuklemeTarihi.AddDays(1).ToString("dd/MM/yyyy");
                        newRow["TeslimFirmaAdresAdı"] = teslimAdresler.Trim();
                        newRow["İstenilenAracTipi"] = istenilenAracTipi;
                        newRow["MusteriVkn"] = "0680617957";
                        newRow["Proje"] = "202";
                        newRow["MusteriSiparisNo"] = "MILHANS";
                        newRow["YuklemeFirmasıAdresAdı"] = "21994";
                        newRow["Urun"] = "165";
                        newRow["KapAdet"] = 25;
                        newRow["AmbalajTipi"] = 1;
                        newRow["BrutKG"] = 25.000m;

                        dt.Rows.Add(newRow);
                    }
                    else if (tempAddresses.Count == 0 && !string.IsNullOrEmpty(teslimAdresler))
                    {
                        DataRow newRow = dt.NewRow();
                        newRow["SiparisTarihi"] = yuklemeTarihi.ToString("dd/MM/yyyy");
                        newRow["YuklemeTarihi"] = yuklemeTarihi.ToString("dd/MM/yyyy");
                        newRow["TeslimTarihi"] = yuklemeTarihi.AddDays(1).ToString("dd/MM/yyyy");
                        newRow["TeslimFirmaAdresAdı"] = teslimAdresler.Trim();
                        newRow["İstenilenAracTipi"] = istenilenAracTipi;
                        newRow["MusteriVkn"] = "0680617957";
                        newRow["Proje"] = "202";
                        newRow["MusteriSiparisNo"] = "MILHANS";
                        newRow["YuklemeFirmasıAdresAdı"] = "21994";
                        newRow["Urun"] = "165";
                        newRow["KapAdet"] = 25;
                        newRow["AmbalajTipi"] = 1;
                        newRow["BrutKG"] = 25.000m;

                        dt.Rows.Add(newRow);
                    }
                }

                // Tekrar eden adresleri filtreleyelim
                var uniqueRows = dt.AsEnumerable()
                    .GroupBy(row => new
                    {
                        YuklemeTarihi = row.Field<string>("YuklemeTarihi"),
                        TeslimFirmaAdresAdı = row.Field<string>("TeslimFirmaAdresAdı")
                    })
                    .Select(g => g.First())
                    .CopyToDataTable();

                gridControlMilhans.DataSource = uniqueRows;

                // DataGridView'i güncelleyelim
                gridViewMilhans.BestFitColumns();
            }
        }

        private void BtnSatırEkle_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            // GridControl'deki mevcut DataTable'ı al
            DataTable dataTable = (DataTable)gridControlMilhans.DataSource;

            // Yeni bir satır ekle
            DataRow newRow = dataTable.NewRow();

            // Mevcut satırlardan birinin verilerini kopyala
            if (dataTable.Rows.Count > 0)
            {
                // İlk satırdan kopyalama yapıyoruz (gerekirse başka bir satırı da seçebilirsiniz)
                DataRow existingRow = dataTable.Rows[0];

                newRow["SiparisTarihi"] = existingRow["SiparisTarihi"];
                newRow["YuklemeTarihi"] = existingRow["YuklemeTarihi"];
                newRow["TeslimTarihi"] = existingRow["TeslimTarihi"];
                newRow["TeslimFirmaAdresAdı"] = string.Empty; // Boş bırak
                newRow["İstenilenAracTipi"] = existingRow["İstenilenAracTipi"];
                newRow["MusteriVkn"] = existingRow["MusteriVkn"];
                newRow["Proje"] = existingRow["Proje"];
                newRow["MusteriSiparisNo"] = existingRow["MusteriSiparisNo"];
                newRow["YuklemeFirmasıAdresAdı"] = existingRow["YuklemeFirmasıAdresAdı"];
                newRow["Urun"] = existingRow["Urun"];
                newRow["KapAdet"] = existingRow["KapAdet"];
                newRow["AmbalajTipi"] = existingRow["AmbalajTipi"];
                newRow["BrutKG"] = existingRow["BrutKG"];

                // Yeni satırı tabloya ekle
                dataTable.Rows.Add(newRow);

                // DataGridView'i güncelle
                gridViewMilhans.BestFitColumns();
            }
            else
            {
                // Eğer tablo boşsa, yeni bir satır oluşturmak için varsayılan değerleri ayarlayın
                newRow["SiparisTarihi"] = DateTime.Now.ToString("dd/MM/yyyy");
                newRow["YuklemeTarihi"] = DateTime.Now.ToString("dd/MM/yyyy");
                newRow["TeslimTarihi"] = DateTime.Now.AddDays(1).ToString("dd/MM/yyyy");
                newRow["TeslimFirmaAdresAdı"] = string.Empty; // Boş bırak
                newRow["İstenilenAracTipi"] = string.Empty;
                newRow["MusteriVkn"] = string.Empty;
                newRow["Proje"] = string.Empty;
                newRow["MusteriSiparisNo"] = string.Empty;
                newRow["YuklemeFirmasıAdresAdı"] = string.Empty;
                newRow["Urun"] = string.Empty;
                newRow["KapAdet"] = 0;
                newRow["AmbalajTipi"] = 0;
                newRow["BrutKG"] = 0m;

                // Yeni satırı tabloya ekle
                dataTable.Rows.Add(newRow);
            }
        }

        private void BtnTeslimleriGetir_ItemClick(object sender, ItemClickEventArgs e)
        {
            using (var context = new DbSiparisOtomasyonEntities6())
            {
                var teslimFirmaAdreslar = gridControlMilhans.DataSource as DataTable;

                if (teslimFirmaAdreslar != null)
                {
                    // Sütunları kontrol et
                    foreach (DataColumn column in teslimFirmaAdreslar.Columns)
                    {
                        Console.WriteLine(column.ColumnName);
                    }

                    int eslesenSatirSayisi = 0;
                    int eslesmeyenSatirSayisi = 0;

                    foreach (DataRow row in teslimFirmaAdreslar.Rows)
                    {
                        string teslimFirmaAdres = row["TeslimFirmaAdresAdı"].ToString();

                        var eslesme = context.TblEslesmelerle
                            .Where(es => es.Milhans == teslimFirmaAdres)
                            .Select(es => new
                            {
                                MilhansID = es.AdresID,
                                MusteriID = es.MusteriID
                            })
                            .FirstOrDefault();

                        if (eslesme != null)
                        {
                            row["TeslimFirmaAdresAdı"] = eslesme.MilhansID;

                            if (!teslimFirmaAdreslar.Columns.Contains("AlıcıFirmaCariUnvanı"))
                            {
                                teslimFirmaAdreslar.Columns.Add("AlıcıFirmaCariUnvanı", typeof(string));
                            }

                            row["AlıcıFirmaCariUnvanı"] = eslesme.MusteriID;

                            string aracTipi = row["İstenilenAracTipi"].ToString();
                            if (aracTipi == "KAMYON")
                            {
                                row["İstenilenAracTipi"] = 3;
                            }
                            else if (aracTipi == "TIR")
                            {
                                row["İstenilenAracTipi"] = 1;
                            }

                            eslesenSatirSayisi++;
                        }
                        else
                        {
                            eslesmeyenSatirSayisi++;
                        }
                    }

                    gridControlMilhans.Refresh();

                    string message = $"Eşleşen Satır Sayısı: {eslesenSatirSayisi}\nEşleşmeyen Satır Sayısı: {eslesmeyenSatirSayisi}";
                    MessageBox.Show(message, "Sonuç", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }


        private void BtnEslestirmeler_ItemClick(object sender, ItemClickEventArgs e)
        {

            BtnTeslimleriGetir.Enabled = true;
            BtnEslestirmeler.Enabled = false;
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

                // TeslimNoktalarıEslestir formunu oluştur
                var teslimNoktalarıForm = new MilhansTeslimNoktalarıEslestir(); // Ensure this is correct

                // Adresleri gridViewMilhans'tan al
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

                // gridControl1 veri kaynağını güncelle
                teslimNoktalarıForm.gridControlMilhansEslesme.RefreshDataSource();

                SortAndMoveMatchedAddresses(teslimNoktalarıForm);

                // Yükleme formunu kapat
                loadingForm.Close();

                // Sonuçları göster
                ShowResults(eslesenAdresler, eslesmeyenAdresler);

                // Satırları yeşile boyamak için RowStyle etkinliğini tanımla
                teslimNoktalarıForm.gridView1.RowStyle += (s, evt) =>
                {
                    var view = s as DevExpress.XtraGrid.Views.Grid.GridView;
                    if (view == null) return;

                    // "Milhans" sütunundaki veriyi kontrol et
                    string milhansValue = view.GetRowCellValue(evt.RowHandle, "Milhans")?.ToString();
                    if (eslesenAdresler.Contains(milhansValue))
                    {
                        evt.Appearance.BackColor = Color.LightGreen;
                        evt.HighPriority = true; // Highlight this style over others
                    }
                };
            }
        }


        private List<string> GetAdresListesi()
        {
            List<string> adresListesi = new List<string>();
            for (int i = 0; i < gridViewMilhans.RowCount; i++)
            {
                string adres = gridViewMilhans.GetRowCellValue(i, "TeslimFirmaAdresAdı")?.ToString()?.Trim();
                if (!string.IsNullOrEmpty(adres))
                {
                    adresListesi.Add(adres);
                }
            }
            return adresListesi;
        }

        private void MatchAddresses(List<string> adresListesi, MilhansTeslimNoktalarıEslestir teslimNoktalarıForm,
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
                        // Eşleşme durumu
                        bool isAlreadyMatched = (bool)(teslimNoktalarıForm.gridView1.GetRowCellValue(j, "IsNewMatch") ?? false);

                        // Eğer daha önce eşleşmemişse, yeni eşleşme olarak işaretle
                        if (!isAlreadyMatched)
                        {
                            teslimNoktalarıForm.gridView1.SetRowCellValue(j, "Milhans", adres); // Eşleşen satıra Milhans değerini yaz
                            teslimNoktalarıForm.gridView1.SetRowCellValue(j, "MusteriID", musteriID); // Müşteri ID'sini yaz

                            // gridControlMilhansEslesme'e eşleşen adresi ekle
                            var eslesmeRow = teslimNoktalarıForm.gridControlMilhansEslesme.DataSource as DataTable;
                            if (eslesmeRow != null)
                            {
                                DataRow newRow = eslesmeRow.NewRow();
                                newRow["Milhans"] = adres; // Eşleşen adresi Milhans sütununa ekle
                                newRow["MusteriID"] = musteriID; // Müşteri ID'sini ekle
                                eslesmeRow.Rows.Add(newRow); // Yeni satırı ekle
                            }

                            eslesenAdresler.Add(adres); // Eşleşen adresi ekle
                            eslesmeyenAdresler.Remove(adres); // Eşleşmeyenler listesinden çıkar

                            // Yeni eşleşmeyi işaretle
                            teslimNoktalarıForm.gridView1.SetRowCellValue(j, "IsNewMatch", true); // Yeni eşleşmeyi işaretle
                        }

                        break; // Eşleşme bulunduğunda döngüden çık
                    }
                }
            }
        }





        private void gridView1_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {
            if (e.Column.FieldName == "Milhans" && e.RowHandle >= 0)
            {
                var gridView = sender as DevExpress.XtraGrid.Views.Grid.GridView;
                bool isNewMatch = (bool?)gridView.GetRowCellValue(e.RowHandle, "IsNewMatch") ?? false;

                if (isNewMatch)
                {
                    e.Appearance.BackColor = Color.LightGreen; // Yeni eşleşmeler için yeşil arka plan
                    e.Appearance.BackColor2 = Color.LightGreen;
                }
            }
        }



        private bool IsMatch(string teslimFirmaAdres, string mevcutAdres)
        {
            // Adresleri kelime kelime kontrol et
            var teslimFirmaKelimeDizisi = teslimFirmaAdres.Split(new[] { ' ', '-', '*' }, StringSplitOptions.RemoveEmptyEntries);
            var mevcutKelimeDizisi = mevcutAdres.Split(new[] { ' ', '-', '*' }, StringSplitOptions.RemoveEmptyEntries);

            // İl kısımlarını kontrol et (ilk kelime)
            if (teslimFirmaKelimeDizisi.Length > 0 && mevcutKelimeDizisi.Length > 0)
            {
                // İl kısmı eşleşmesi
                string teslimFirmaIl = teslimFirmaKelimeDizisi[0]; // BİM
                string mevcutIl = mevcutKelimeDizisi.FirstOrDefault(); // mevcutAdres'in ilk kelimesi

                if (string.Equals(teslimFirmaIl, mevcutIl, StringComparison.OrdinalIgnoreCase))
                {
                    // Adres kısımlarını kontrol et
                    foreach (var kelime in teslimFirmaKelimeDizisi.Skip(1)) // İlk kelime dışında kalanları kontrol et
                    {
                        if (mevcutKelimeDizisi.Any(m => m.IndexOf(kelime, StringComparison.OrdinalIgnoreCase) >= 0))
                        {
                            return true; // En az bir kelime eşleşirse true döner
                        }
                    }
                }
            }
            return false; // Hiçbir eşleşme yoksa false döner
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

            return null; // Hiçbir eşleşme yoksa null döndür
        }

        private void SortAndMoveMatchedAddresses(MilhansTeslimNoktalarıEslestir teslimNoktalarıForm)
        {
            // Eşleşenleri en üste taşıma
            for (int j = 0; j < teslimNoktalarıForm.gridView1.RowCount; j++)
            {
                string mevcutAdres = teslimNoktalarıForm.gridView1.GetRowCellValue(j, "Milhans")?.ToString();
                if (!string.IsNullOrEmpty(mevcutAdres))
                {
                    teslimNoktalarıForm.gridView1.MoveFirst(); // Eşleşen satırı en üstte göster
                    break; // İlk eşleşen satırı bulduğumuzda döngüden çık
                }
            }

            // "Adko" sütununa göre sıralama yap
            teslimNoktalarıForm.gridView1.Columns["Milhans"].SortOrder = DevExpress.Data.ColumnSortOrder.Descending;
        }


        private void ShowResults(List<string> eslesenAdresler, List<string> eslesmeyenAdresler)
        {
            var resultsForm = new Form
            {
                StartPosition = FormStartPosition.CenterScreen,
                Size = new Size(300, 200)
            };

            var resultsLabel = new Label
            {
                AutoSize = true,
                Location = new Point(10, 10),
                Text = "Eşleşen Adresler:\n" + string.Join("\n", eslesenAdresler)
            };

            var unmatchedLabel = new Label
            {
                AutoSize = true,
                Location = new Point(10, 90),
                Text = "Eşleşmeyen Adresler:\n" + string.Join("\n", eslesmeyenAdresler)
            };

            resultsForm.Controls.Add(resultsLabel);
            resultsForm.Controls.Add(unmatchedLabel);
            resultsForm.ShowDialog();
        }





        private void BtnEslestir_ItemClick(object sender, ItemClickEventArgs e)
        {
            // MilhansTeslimNoktalarıEslestir formunu aç
            MilhansTeslimNoktalarıEslestir form = new MilhansTeslimNoktalarıEslestir();
            form.Show(); // Modal olarak açmak için ShowDialog() kullanabilirsiniz
        }

        private void BtnDısarıCıkart_ItemClick(object sender, ItemClickEventArgs e)
        {
            // gridControlApak'ın veri kaynağını DataTable olarak al
            DataTable dt = gridControlMilhans.DataSource as DataTable;

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
            DataTable dt = gridControlMilhans.DataSource as DataTable;

            if (dt != null)
            {
                // Tüm satırları temizle
                dt.Clear();

                // Değişiklikleri yansıt
                gridControlMilhans.Refresh();

                MessageBox.Show("Tüm satırlar silindi.", "Silme İşlemi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Veri kaynağı mevcut değil.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void MilhansSiparisEkrani_Load(object sender, EventArgs e)
        {
            // GridView objesini alın
            GridView gridView = gridControlMilhans.MainView as GridView;

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
            GridView gridView = gridControlMilhans.MainView as GridView;

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




