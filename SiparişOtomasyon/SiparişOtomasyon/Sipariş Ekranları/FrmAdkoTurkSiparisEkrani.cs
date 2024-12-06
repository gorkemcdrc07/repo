using DevExpress.XtraBars;
using DevExpress.XtraGrid.Views.Grid;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using ClosedXML.Excel;
using System.Collections;
using SiparişOtomasyon.entity;
using DevExpress.Data.NetCompatibility.Extensions;
using System.Drawing;



namespace SiparişOtomasyon.Sipariş_Ekranları
{
    public partial class FrmAdkoTurkSiparisEkrani : Form
    {
        public FrmAdkoTurkSiparisEkrani()
        {
            InitializeComponent();
            BtnTeslimleriGetir.Enabled = false;

        }
        private int? selectedRowHandle = null; // İlk seçilen satırın indeksi
        private void FrmAdkoTurkSiparisEkrani_Load(object sender, EventArgs e)
        {
            // Formun Drag and Drop olaylarını ayarla
            this.AllowDrop = true;
            this.DragEnter += new DragEventHandler(FrmAdkoTurkSiparisEkrani_DragEnter);
            this.DragDrop += new DragEventHandler(FrmAdkoTurkSiparisEkrani_DragDrop);

            this.gridViewAdko.MouseDown += GridViewAdko_MouseDown; // MouseDown olayını yakala

            // GridView objesini alın
            GridView gridView = gridControlAdko.MainView as GridView;

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
            GridView gridView = gridControlAdko.MainView as GridView;

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
    


        
        private void GridViewAdko_MouseDown(object sender, MouseEventArgs e)
        {
            // Tıklanan hücreyi al
            var hitInfo = gridViewAdko.CalcHitInfo(e.Location);

            if (hitInfo.InRow) // Sadece bir satıra tıklandıysa işlem yap
            {
                if (e.Button == MouseButtons.Left) // Sol tık
                {
                    // İlk satırı seç ve sakla
                    selectedRowHandle = hitInfo.RowHandle;
                }
                else if (e.Button == MouseButtons.Right && selectedRowHandle.HasValue) // Sağ tık
                {
                    // İkinci satırın indeksini al
                    int targetRowHandle = hitInfo.RowHandle;

                    // DataTable'dan veri al
                    var dataTable = gridControlAdko.DataSource as DataTable;

                    if (dataTable != null && selectedRowHandle.Value != targetRowHandle) // Satırlar farklı mı?
                    {
                        // İlk ve ikinci satırları değiştir
                        DataRow firstRow = dataTable.Rows[selectedRowHandle.Value];
                        DataRow secondRow = dataTable.Rows[targetRowHandle];

                        // Geçici veri tutma
                        object[] tempData = firstRow.ItemArray;

                        // İlk satırı ikinci satırla değiştir
                        firstRow.ItemArray = secondRow.ItemArray;

                        // İkinci satırı geçici veriyle değiştir
                        secondRow.ItemArray = tempData;

                        // GridView'i yenile
                        gridControlAdko.RefreshDataSource();

                        // İlk seçim sıfırla
                        selectedRowHandle = null;

                        
                    }
                }
            }
        }

        private void FrmAdkoTurkSiparisEkrani_DragEnter(object sender, DragEventArgs e)
        {
            // Eğer veri metin olarak sürükleniyorsa, kabul et
            if (e.Data.GetDataPresent(DataFormats.Text))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void FrmAdkoTurkSiparisEkrani_DragDrop(object sender, DragEventArgs e)
        {
            // Sürüklenen metin verisini al
            string droppedData = (string)e.Data.GetData(DataFormats.Text);

            // DataTable oluştur
            DataTable dt = new DataTable();
            dt.Columns.Add("SiparisTarihi", typeof(DateTime));
            dt.Columns.Add("YuklemeTarihi", typeof(DateTime));
            dt.Columns.Add("TeslimTarihi", typeof(DateTime));
            dt.Columns.Add("TeslimFirmaAdresAdı", typeof(string));
            dt.Columns.Add("İstenilenAracTipi", typeof(string)); // İstenilenAracTipi sütununu ekle
            dt.Columns.Add("MusteriVkn", typeof(string)); // MusteriVkn sütununu ekle
            dt.Columns.Add("Proje", typeof(string)); // Proje sütununu ekle
            dt.Columns.Add("YuklemeFirmasıAdresAdı", typeof(string)); // YuklemeFirmasıAdresAdı sütununu ekle
            dt.Columns.Add("MusteriSiparisNo", typeof(string)); // MusteriSiparisNo sütununu ekle
            dt.Columns.Add("AracTipiKod", typeof(int)); // AracTipiKod sütununu ekle
            dt.Columns.Add("Urun", typeof(string)); // Urun sütununu ekle
            dt.Columns.Add("KapAdet", typeof(int)); // KapAdet sütununu ekle
            dt.Columns.Add("AmbalajTipi", typeof(int)); // AmbalajTipi sütununu ekle
            dt.Columns.Add("BrutKG", typeof(decimal)); // BrutKG sütununu ekle
            dt.Columns.Add("AlıcıFirmaCariUnvanı", typeof(int)); // AlıcıFirmaCariUnvanı sütununu ekle

            // Sürüklenen veriyi satırlara ayır
            var rows = droppedData.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

            // Her bir satırı işle
            foreach (string row in rows)
            {
                var columns = row.Split('\t'); // Tab ile ayrıldığını varsayıyoruz
                if (columns.Length >= 12) // En az 12 sütun var mı kontrol et
                {
                    DateTime siparisTarihi;

                    // Tarihleri parse et
                    if (DateTime.TryParse(columns[1], out siparisTarihi))
                    {
                        // Hem SiparisTarihi hem de YuklemeTarihi aynı olacak
                        DateTime teslimTarihi = siparisTarihi.AddDays(1); // Sipariş tarihine 1 gün ekle

                        // TeslimFirmaAdresAdı'ndan veriyi al ve '+' işaretine göre ayır
                        string firmaAdres = columns[3].Trim();
                        string[] adresParcalari = firmaAdres.Split(new[] { '+' }, StringSplitOptions.RemoveEmptyEntries);

                        // DataTable'a ekle
                        foreach (var adresParca in adresParcalari)
                        {
                            // Yeni bir satır oluştur
                            var newRow = dt.NewRow();
                            newRow["SiparisTarihi"] = siparisTarihi;
                            newRow["YuklemeTarihi"] = siparisTarihi;
                            newRow["TeslimTarihi"] = teslimTarihi;
                            newRow["TeslimFirmaAdresAdı"] = adresParca.Trim();

                            // İstenilenAracTipi verisini al ve ekle
                            string istenilenAracTipi = columns[11].Trim(); // 12. sütundaki veriyi al
                            newRow["İstenilenAracTipi"] = istenilenAracTipi; // Yeni satıra atama yap

                            // AracTipiKod değerini her durumda 14 yap
                            newRow["AracTipiKod"] = 14; // Her durumda 14 ata

                            // MusteriVkn ve Proje alanlarını doldur
                            newRow["MusteriVkn"] = "0080758228"; // Müşteri VKN değeri
                            newRow["Proje"] = "24"; // Proje adı

                            // YuklemeFirmasıAdresAdı ve MusteriSiparisNo alanlarını doldur
                            newRow["YuklemeFirmasıAdresAdı"] = "156"; // YuklemeFirmasıAdresAdı değeri
                            newRow["MusteriSiparisNo"] = "ADKO"; // MusteriSiparisNo değeri

                            // Urun alanına kontrol ekle
                            if (!string.IsNullOrEmpty(columns[4].Trim())) // Urun sütunu dolu mu?
                            {
                                newRow["Urun"] = 14; // Dolu ise 14 yaz
                            }
                            else
                            {
                                newRow["Urun"] = null; // Dolu değilse null ata
                            }

                            // KapAdet alanına kontrol ekle
                            if (!string.IsNullOrEmpty(columns[5].Trim())) // KapAdet sütunu dolu mu?
                            {
                                newRow["KapAdet"] = 25; // Dolu ise 25 yaz
                            }

                            // AmbalajTipi sütununu 1 olarak ata
                            newRow["AmbalajTipi"] = 1; // AmbalajTipi sütunu için 1 yaz

                            // BrutKG alanına kontrol ekle
                            if (!string.IsNullOrEmpty(columns[7].Trim())) // BrutKG sütunu dolu mu?
                            {
                                newRow["BrutKG"] = 25.000m; // Dolu ise 25.000 yaz
                            }
                            else
                            {
                                newRow["BrutKG"] = null; // Dolu değilse null ata (veya uygun bir varsayılan değer)
                            }

                            // Yeni satırı DataTable'a ekle
                            dt.Rows.Add(newRow);
                        }
                    }
                }
            }

            // DataTable'ı gridControl'a ata
            gridControlAdko.DataSource = dt;

            // gridControl'da yeni sütunu güncelle
            gridControlAdko.Refresh(); // Eğer hala güncellenmiyorsa, refresh ekleyebiliriz
        }










        public List<string> GetTeslimFirmaAdresList()
        {
            var teslimFirmaAdresList = new List<string>();

            if (gridControlAdko.DataSource == null)
            {
                return teslimFirmaAdresList; // DataSource null ise boş liste döndür
            }

            foreach (var row in gridControlAdko.DataSource as IEnumerable)
            {
                var teslimFirmaAdresi = row.GetType().GetProperty("TeslimFirmaAdresAdı")?.GetValue(row, null);
                if (teslimFirmaAdresi != null)
                {
                    teslimFirmaAdresList.Add(teslimFirmaAdresi.ToString());
                }
            }

            return teslimFirmaAdresList;
        }



        private void BtnEslestir_ItemClick(object sender, ItemClickEventArgs e)
        {
            // TeslimNoktalarıEslestir formunu oluştur ve göster
            AdkoTeslimNoktalarıEslestir teslimNoktalarıEslestirForm = new AdkoTeslimNoktalarıEslestir();
            teslimNoktalarıEslestirForm.Show(); // Formu normal bir pencerede açar
                                                // Eğer formu modal (diğer formların üstünde) açmak isterseniz, ShowDialog() kullanabilirsiniz:
                                                // teslimNoktalarıEslestirForm.ShowDialog();
        }
        public void UpdateReelIDs(List<int> reelIDs)
        {
            // reelIDs listesini kullanarak gerekli güncellemeleri yapın
            foreach (var id in reelIDs)
            {
                // İlgili işlemleri gerçekleştirin
                // Örneğin, ilgili kayıtları bulup güncelleme yapabilirsiniz.
            }

            // gridControlAdko'yu yenileyin
            gridControlAdko.Refresh();
        }






        private void BtnExcel_ItemClick(object sender, ItemClickEventArgs e)
        {
            // GridView'den "TeslimFirmaAdresAdı" alanını çekmek
            GridView view = gridControlAdko.MainView as GridView;

            if (view != null)
            {
                // Excel dosyası için bir çalışma kitabı oluşturma
                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("TeslimFirmaAdresları");

                    // Başlık ekleme
                    worksheet.Cell(1, 1).Value = "TeslimFirmaAdresAdı";

                    // Grid'deki verileri döngü ile dolaşmak ve Excel'e yazmak
                    int rowIndex = 2; // Excel'deki yazma satırını başlat
                    for (int i = 0; i < view.RowCount; i++)
                    {
                        object cellValue = view.GetRowCellValue(i, "TeslimFirmaAdresAdı");
                        if (cellValue != null && !IsNumeric(cellValue.ToString())) // Sadece metin olanları al
                        {
                            worksheet.Cell(rowIndex++, 1).Value = cellValue.ToString(); // Metin olanları yaz
                        }
                    }

                    // Dosya kaydetme işlemi için SaveFileDialog açmak
                    SaveFileDialog saveDialog = new SaveFileDialog
                    {
                        Filter = "Excel Files|*.xlsx",
                        Title = "Excel Dosyasını Kaydet",
                        FileName = "TeslimFirmaAdresAdı.xlsx"
                    };

                    if (saveDialog.ShowDialog() == DialogResult.OK)
                    {
                        // Excel dosyasını kaydet
                        workbook.SaveAs(saveDialog.FileName);
                        MessageBox.Show("Veriler başarıyla Excel'e aktarıldı!", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
        }

        // Sayı kontrolü yapan metod
        private bool IsNumeric(string value)
        {
            return double.TryParse(value, out _); // Eğer parse edilebiliyorsa sayı demektir
        }




        private void BtnTeslimleriGetir_ItemClick(object sender, ItemClickEventArgs e)
        {
            using (var context = new DbSiparisOtomasyonEntities6())
            {
                // gridControlAdko'dan DataTable'ı al
                var teslimFirmaAdreslar = gridControlAdko.DataSource as DataTable;

                if (teslimFirmaAdreslar != null)
                {
                    int eslesenSatirSayisi = 0; // Eşleşen satır sayısı
                    int eslesmeyenSatirSayisi = 0; // Eşleşmeyen satır sayısı

                    foreach (DataRow row in teslimFirmaAdreslar.Rows)
                    {
                        string teslimFirmaAdres = row["TeslimFirmaAdresAdı"].ToString();

                        // TblEslesmelerle'den Adko'ya göre eşleşen verileri al
                        var eslesme = context.TblEslesmelerle
                            .Where(es => es.Adko == teslimFirmaAdres)
                            .Select(es => new
                            {
                                AdkoID = es.AdresID, // veya ihtiyaca göre uygun alanı seçin
                                MusteriID = es.MusteriID
                            })
                            .FirstOrDefault();

                        // eslesme değeri null ise devam et, aksi takdirde güncelle
                        if (eslesme != null)
                        {
                            // TeslimFirmaAdresAdı'nı güncelle
                            row["TeslimFirmaAdresAdı"] = eslesme.AdkoID; // AdkoID'yi TeslimFirmaAdresAdı sütununa yaz

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
                    gridControlAdko.Refresh();

                    // Eşleşen ve eşleşmeyen satır sayılarını MessageBox ile göster
                    string message = $"Eşleşen Satır Sayısı: {eslesenSatirSayisi}\nEşleşmeyen Satır Sayısı: {eslesmeyenSatirSayisi}";
                    MessageBox.Show(message, "Sonuç", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
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
                        gridControlAdko.ExportToXlsx(saveFileDialog.FileName);

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
            var gridView = gridControlAdko.MainView as DevExpress.XtraGrid.Views.Grid.GridView;

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

                // AdkoTeslimNoktalarıEslestir formunu oluştur
                var teslimNoktalarıForm = new AdkoTeslimNoktalarıEslestir();

                // Adresleri gridViewAdko'tan al
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
                teslimNoktalarıForm.gridControlAdkoEslesme.RefreshDataSource();

                // Eşleşenleri en üste taşıma ve sıralama yap
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

                    // "Adko" sütunundaki veriyi kontrol et
                    string adkoValue = view.GetRowCellValue(evt.RowHandle, "Adko")?.ToString();
                    if (eslesenAdresler.Contains(adkoValue))
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
            for (int i = 0; i < gridViewAdko.RowCount; i++)
            {
                string adres = gridViewAdko.GetRowCellValue(i, "TeslimFirmaAdresAdı")?.ToString()?.Trim();
                if (!string.IsNullOrEmpty(adres))
                {
                    adresListesi.Add(adres);
                }
            }
            return adresListesi;
        }

        private void MatchAddresses(List<string> adresListesi, AdkoTeslimNoktalarıEslestir teslimNoktalarıForm,
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
                        teslimNoktalarıForm.gridView1.SetRowCellValue(j, "Adko", adres); // Eşleşen satıra Adko değerini yaz
                        teslimNoktalarıForm.gridView1.SetRowCellValue(j, "MusteriID", musteriID); // Müşteri ID'sini yaz

                        // gridControlAdkoEslesme'e eşleşen adresi ekle
                        var eslesmeRow = teslimNoktalarıForm.gridControlAdkoEslesme.DataSource as DataTable;
                        if (eslesmeRow != null)
                        {
                            DataRow newRow = eslesmeRow.NewRow();
                            newRow["Adko"] = adres; // Eşleşen adresi Adko sütununa ekle
                            newRow["MusteriID"] = musteriID; // Müşteri ID'sini ekle
                            eslesmeRow.Rows.Add(newRow); // Yeni satırı ekle
                        }

                        eslesenAdresler.Add(adres); // Eşleşen adresi ekle
                        eslesmeyenAdresler.Remove(adres); // Eşleşmeyenler listesinden çıkar

                        // Eşleşen satırı işaretle
                        teslimNoktalarıForm.gridView1.SetRowCellValue(j, "Adko", adres); // Eşleşen satıra Adko değerini yaz
                        teslimNoktalarıForm.gridView1.Tag = "Highlight"; // Satırı işaretle

                        break; // Eşleşme bulunduğunda döngüden çık
                    }
                }
            }
        }

        // GridView'deki RowCellStyle olayını kullanarak yeni eşleşen satırları renklendir
        private void gridView1_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {
            if (e.Column.FieldName == "Adko" && e.RowHandle >= 0)
            {
                var gridView = sender as DevExpress.XtraGrid.Views.Grid.GridView;
                if (gridView.Tag != null && gridView.Tag.ToString() == "Highlight")
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

        private void SortAndMoveMatchedAddresses(AdkoTeslimNoktalarıEslestir teslimNoktalarıForm)
        {
            // Eşleşenleri en üste taşıma
            for (int j = 0; j < teslimNoktalarıForm.gridView1.RowCount; j++)
            {
                string mevcutAdres = teslimNoktalarıForm.gridView1.GetRowCellValue(j, "Adko")?.ToString();
                if (!string.IsNullOrEmpty(mevcutAdres))
                {
                    teslimNoktalarıForm.gridView1.MoveFirst(); // Eşleşen satırı en üstte göster
                    break; // İlk eşleşen satırı bulduğumuzda döngüden çık
                }
            }

            // "Adko" sütununa göre sıralama yap
            teslimNoktalarıForm.gridView1.Columns["Adko"].SortOrder = DevExpress.Data.ColumnSortOrder.Descending;
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


    }
}






