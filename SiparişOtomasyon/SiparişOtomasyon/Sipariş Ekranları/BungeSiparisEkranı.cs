using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Windows.Forms;
using OfficeOpenXml;
using SiparişOtomasyon.Teslim_Noktaları;
using System.Drawing;
using System.Text.RegularExpressions;
using DevExpress.XtraBars;
using SiparişOtomasyon.entity;
using System.Linq;
using DevExpress.XtraGrid.Views.Grid;

namespace SiparişOtomasyon.Sipariş_Ekranları
{
    public partial class BungeSiparisEkranı : Form
    {
        private string _selectedProject;

        public BungeSiparisEkranı(string selectedProject)
        {
            InitializeComponent();
            _selectedProject = selectedProject;

            // GridControl'e sürükleme desteği ekle
            gridControlBunge.AllowDrop = true;
            gridControlBunge.DragEnter += GridControl_DragEnter;
            gridControlBunge.DragDrop += GridControl_DragDrop;


        }

        // DragEnter olayı - Sürüklenen dosyanın kabul edilip edilmeyeceğini kontrol eder
        private void GridControl_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        // DragDrop olayı - Dosya bırakıldığında çalışır
        private void GridControl_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (files.Length > 0)
            {
                string filePath = files[0];
                if (Path.GetExtension(filePath).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
                {
                    DataTable dataTable = ReadExcelFile(filePath);
                    BindToGrid(dataTable);
                }
                else
                {
                    MessageBox.Show("Lütfen bir Excel dosyası (*.xlsx) sürükleyip bırakın.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        // Excel dosyasını okuyup DataTable döndüren metod
        private DataTable ReadExcelFile(string filePath)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // EPPlus lisansı ayarı

            using (ExcelPackage package = new ExcelPackage(new FileInfo(filePath)))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets[0]; // İlk sayfayı seç
                DataTable dataTable = new DataTable();

                // Sütun başlıklarını ekle
                for (int col = 1; col <= worksheet.Dimension.End.Column; col++)
                {
                    string columnHeader = worksheet.Cells[1, col].Text.Trim(); // İlk satırdan sütun başlıklarını al
                    if (string.IsNullOrEmpty(columnHeader)) columnHeader = $"Column{col}";
                    dataTable.Columns.Add(columnHeader);
                }

                // Verileri oku
                for (int row = 2; row <= worksheet.Dimension.End.Row; row++) // İlk satır başlık olduğu için 2. satırdan başla
                {
                    DataRow dataRow = dataTable.NewRow();
                    for (int col = 1; col <= worksheet.Dimension.End.Column; col++)
                    {
                        dataRow[col - 1] = worksheet.Cells[row, col].Text.Trim(); // Hücre verisini ekle
                    }
                    dataTable.Rows.Add(dataRow);
                }

                return dataTable;
            }
        }

        // Verileri gridControlBunge'ye bağlayan metod
        private void BindToGrid(DataTable dataTable)
        {
            // Yeni bir DataTable oluştur
            DataTable gridTable = new DataTable();

            // gridControl sütunlarını tanımla
            gridTable.Columns.Add("SiparisTarihi", typeof(string));
            gridTable.Columns.Add("YuklemeTarihi", typeof(string));
            gridTable.Columns.Add("TeslimFirmaAdresAdı", typeof(string));
            gridTable.Columns.Add("İstenilenAracTipi", typeof(string));
            gridTable.Columns.Add("MusteriSiparisNo", typeof(string));  // Nakliye numarası
            gridTable.Columns.Add("MusteriVkn", typeof(string));         // Yeni sütun
            gridTable.Columns.Add("Proje", typeof(string));              // Yeni sütun
            gridTable.Columns.Add("TeslimTarihi", typeof(string));       // Yeni sütun
            gridTable.Columns.Add("YuklemeFirmasıAdresAdı", typeof(string)); // Yeni sütun
            gridTable.Columns.Add("Urun", typeof(string));               // Yeni sütun
            gridTable.Columns.Add("KapAdet", typeof(string));            // Yeni sütun
            gridTable.Columns.Add("AmbalajTipi", typeof(string));        // Yeni sütun
            gridTable.Columns.Add("BrutKG", typeof(string));             // Yeni sütun

            // Benzersiz değerleri depolamak için HashSet kullan
            HashSet<string> uniqueTeslimFirmaAdresAdi = new HashSet<string>();

            // Excel verilerini gridControl'e uygun şekilde ekle
            foreach (DataRow row in dataTable.Rows)
            {
                // Excel sütunlarına göre verileri al
                string malCikisTarihi = row["Mal Çıkış Tarihi"].ToString(); // B sütunu
                string teslimFirma = row["Siparişi verenin adı"].ToString(); // C sütunu
                string teslimAdres = row["Malı teslim alanın adı"].ToString(); // D sütunu
                string aracTipi = row["Sevkiyat türü"].ToString(); // E sütunu
                string nakliyeNumarasi = row["Nakliye numarası"].ToString(); // F sütunu
                string teslimFirmaAdresAdi = string.Empty;

                // TeslimFirmaAdresAdı'nı doğru şekilde oluştur
                if (!string.IsNullOrEmpty(teslimFirma) && !string.IsNullOrEmpty(teslimAdres))
                {
                    teslimFirmaAdresAdi = $"{teslimFirma} ({teslimAdres})";
                }
                else if (!string.IsNullOrEmpty(teslimFirma))
                {
                    teslimFirmaAdresAdi = teslimFirma;
                }
                else if (!string.IsNullOrEmpty(teslimAdres))
                {
                    teslimFirmaAdresAdi = teslimAdres;
                }

                // Eğer değer geçerliyse ve daha önce eklenmemişse, gridTable'a ekle
                if (!string.IsNullOrEmpty(teslimFirmaAdresAdi) && !uniqueTeslimFirmaAdresAdi.Contains(teslimFirmaAdresAdi))
                {
                    // Benzersiz değeri ekle
                    uniqueTeslimFirmaAdresAdi.Add(teslimFirmaAdresAdi);

                    // Yeni satır oluştur ve gridControl'e ekle
                    DataRow gridRow = gridTable.NewRow();

                    // Eğer seçilen proje 'LÜLEBURGAZ FTL' ise
                    if (_selectedProject == "LÜLEBURGAZ FTL")
                    {
                        gridRow["SiparisTarihi"] = malCikisTarihi;
                        gridRow["YuklemeTarihi"] = malCikisTarihi; // Aynı veri
                        gridRow["TeslimFirmaAdresAdı"] = teslimFirmaAdresAdi;
                        gridRow["İstenilenAracTipi"] = aracTipi;
                        gridRow["MusteriSiparisNo"] = nakliyeNumarasi; // F sütunundaki Nakliye numarasını ekliyoruz
                        gridRow["Proje"] = "92"; // LÜLEBURGAZ FTL için Proje sütununa 92 yaz
                    }
                    // Eğer seçilen proje 'GEBZE' ise
                    else if (_selectedProject == "GEBZE")
                    {
                        gridRow["SiparisTarihi"] = malCikisTarihi;
                        gridRow["YuklemeTarihi"] = malCikisTarihi; // Aynı veri
                        gridRow["TeslimFirmaAdresAdı"] = teslimFirmaAdresAdi;
                        gridRow["İstenilenAracTipi"] = aracTipi;
                        gridRow["MusteriSiparisNo"] = nakliyeNumarasi; // C sütunu (Nakliye numarası) gridControlBunge'deki MusteriSiparisNo'ya
                        gridRow["Proje"] = "129"; // GEBZE için Proje sütununa 129 yaz
                    }
                    // Eğer seçilen proje 'EDİRNE' ise
                    else if (_selectedProject == "EDİRNE")
                    {
                        // Sürüklenip bırakılan Excel sütunlarına göre veri aktarımı
                        gridRow["MusteriSiparisNo"] = row["Nakliye numarası"].ToString(); // D sütunu -> MusteriSiparisNo
                        gridRow["İstenilenAracTipi"] = row["Sevkiyat türü"].ToString(); // C sütunu -> İstenilenAracTipi
                        gridRow["SiparisTarihi"] = row["Malı teslim alan"].ToString(); // J sütunu -> SiparisTarihi
                        gridRow["YuklemeTarihi"] = row["Malı teslim alan"].ToString(); // J sütunu -> YuklemeTarihi
                        gridRow["TeslimFirmaAdresAdı"] = $"{row["Siparişi verenin adı"]} ({row["Malı teslim alanın adı"]})"; // Q ve L sütunları -> TeslimFirmaAdresAdı
                        gridRow["Proje"] = "93"; // EDİRNE için Proje sütununa 93 yaz
                    }

                    // Yeni eklenen sütunlar
                    gridRow["MusteriVkn"] = "1900198568"; // MusteriVkn
                    gridRow["TeslimTarihi"] = AddOneDay(malCikisTarihi); // SiparisTarihi'ne 1 gün ekle
                    gridRow["YuklemeFirmasıAdresAdı"] = "5"; // YuklemeFirmasıAdresAdı
                    gridRow["Urun"] = "4"; // Urun
                    gridRow["KapAdet"] = "25"; // KapAdet
                    gridRow["AmbalajTipi"] = "1"; // AmbalajTipi
                    gridRow["BrutKG"] = "25.000"; // BrutKG

                    gridTable.Rows.Add(gridRow);
                }
            }

            // gridControlBunge'ye bağla
            gridControlBunge.DataSource = gridTable;
        }




        // Bir tarihe 1 gün ekleyen fonksiyon
        private string AddOneDay(string dateString)
        {
            DateTime date;
            if (DateTime.TryParse(dateString, out date))
            {
                date = date.AddDays(1);
                return date.ToString("dd.MM.yyyy");
            }
            return dateString; // Eğer geçerli bir tarih değilse, orijinal değeri döndür
        }

        private void BtnEslestir_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            // DogtasTeslimNoktalarıEslestir formunu oluştur
            BungeTeslimNoktalarıEslestir eslestirForm = new BungeTeslimNoktalarıEslestir();

            // Formu aç
            eslestirForm.Show();
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

                // Adresleri gridViewBunge'tan al
                List<string> adresListesi = new List<string>();
                for (int i = 0; i < gridViewBunge.RowCount; i++)
                {
                    string adres = gridViewBunge.GetRowCellValue(i, "TeslimFirmaAdresAdı")?.ToString()?.Trim();
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

                // BungeTeslimNoktaları formunu oluştur
                BungeTeslimNoktalarıEslestir teslimNoktalarıForm = new BungeTeslimNoktalarıEslestir();
                teslimNoktalarıForm.Show();

                // Eşleşen ve eşleşmeyen adresleri saklamak için listeler
                List<string> eslesenAdresler = new List<string>();
                List<string> eslesmeyenAdresler = new List<string>(adresListesi);

                // Adresleri eşleştirme işlemi
                foreach (var adres in adresListesi)
                {
                    // Parantez içindeki ve dışındaki verileri ayır
                    int startIndex = adres.IndexOf("(");
                    int endIndex = adres.IndexOf(")");
                    string adresDışı = startIndex >= 0 ? adres.Substring(0, startIndex).Trim() : adres;
                    string adresİçindekiVeri = (startIndex >= 0 && endIndex > startIndex) ? adres.Substring(startIndex + 1, endIndex - startIndex - 1) : null;

                    using (var context = new DbSiparisOtomasyonEntities6())
                    {
                        // DbSet'i bellek üzerinde işlenebilir hale getirmek için AsEnumerable() ekleyin
                        var eslesme = context.TblEslesmelerle
                            .AsEnumerable() // IQueryable'dan IEnumerable'a dönüştürür
                            .FirstOrDefault(x =>
                                (x.Unvan != null && x.Unvan.Contains(adresDışı)) && // Unvan null değilse, parantez dışında kalan kısım
                                (x.AdresAdi != null && x.AdresAdi.Contains(adresİçindekiVeri)) // AdresAdi null değilse, parantez içindeki kısım
                            );

                        if (eslesme != null)
                        {
                            // Eşleşme bulundu, Bunge sütununa TeslimFirmaAdresAdı verisini getir
                            teslimNoktalarıForm.gridViewBungeEslesme.SetRowCellValue(
                                teslimNoktalarıForm.gridViewBungeEslesme.LocateByValue("AdresAdi", eslesme.AdresAdi),
                                "Bunge", adres);

                            eslesenAdresler.Add(adres); // Eşleşen adresi ekle
                            eslesmeyenAdresler.Remove(adres); // Eşleşmeyenler listesinden çıkar
                        }
                    }


                }

                // gridControl1 veri kaynağını güncelle
                teslimNoktalarıForm.gridControlBungeEslesme.RefreshDataSource();

                // "Bunge" sütununa göre sıralama yap
                teslimNoktalarıForm.gridViewBungeEslesme.Columns["Bunge"].SortOrder = DevExpress.Data.ColumnSortOrder.Descending;

                // Yükleme formunu kapat
                loadingForm.Close();

                // RowCellStyle olayını tanımla
                teslimNoktalarıForm.gridViewBungeEslesme.RowCellStyle += (s, ev) =>
                {
                    string BungeDegeri = teslimNoktalarıForm.gridViewBungeEslesme.GetRowCellValue(ev.RowHandle, "Bunge")?.ToString();
                    if (eslesenAdresler.Contains(BungeDegeri))
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

        private void BungeSiparisEkranı_Load(object sender, EventArgs e)
        {
            // GridView objesini alın
            GridView gridView = gridControlBunge.MainView as GridView;

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
            GridView gridView = gridControlBunge.MainView as GridView;

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


