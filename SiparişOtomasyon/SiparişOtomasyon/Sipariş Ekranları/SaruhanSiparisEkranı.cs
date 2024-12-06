using SiparişOtomasyon.Teslim_Noktaları;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;
using System.Text.RegularExpressions;
using SiparişOtomasyon.entity;
using DevExpress.XtraGrid.Views.Grid;

namespace SiparişOtomasyon.Sipariş_Ekranları
{
    public partial class SaruhanSiparisEkranı : Form
    {
        public SaruhanSiparisEkranı()
        {
            InitializeComponent();

            // Sürükle bırak işlemi için gerekli ayarları yapıyoruz.
            this.AllowDrop = true;  // Form düzeyinde AllowDrop
            gridControlSaruhan.AllowDrop = true; // gridControlSaruhan üzerinde AllowDrop

            // DragEnter ve DragDrop olaylarını bağlıyoruz.
            this.DragEnter += new DragEventHandler(SaruhanSiparisEkranı_DragEnter);
            gridControlSaruhan.DragEnter += new DragEventHandler(GridControlSaruhan_DragEnter); // GridControl için DragEnter
            gridControlSaruhan.DragDrop += new DragEventHandler(GridControlSaruhan_DragDrop);   // GridControl için DragDrop
        }

        // Form'un DragEnter olayını tanımlıyoruz
        private void SaruhanSiparisEkranı_DragEnter(object sender, DragEventArgs e)
        {
            // Eğer sürüklenen veri metin (string) formatında ise bırakmaya izin ver
            if (e.Data.GetDataPresent(DataFormats.Text))
            {
                e.Effect = DragDropEffects.Copy; // Bırakmaya izin ver
            }
            else
            {
                e.Effect = DragDropEffects.None; // Geçerli veri değilse, bırakma işlemini engelle
            }
        }

        // gridControlSaruhan için DragEnter olayını tanımlıyoruz
        private void GridControlSaruhan_DragEnter(object sender, DragEventArgs e)
        {
            // Eğer sürüklenen veri metin (string) formatında ise bırakmaya izin ver
            if (e.Data.GetDataPresent(DataFormats.Text))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        // gridControlSaruhan için DragDrop olayını tanımlıyoruz
        private void GridControlSaruhan_DragDrop(object sender, DragEventArgs e)
        {
            // Gelen veriyi alıyoruz
            string data = (string)e.Data.GetData(DataFormats.Text);

            // Veriyi satırlara ayırarak işliyoruz
            List<string[]> rawData = ParseData(data);

            // İşlenen veriyi gridControlSaruhan'a yüklüyoruz
            OnDataDropped(rawData);
        }

        // Veriyi satırlara ayırarak işleyen fonksiyon
        private List<string[]> ParseData(string data)
        {
            var rows = data.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            var parsedData = new List<string[]>();

            foreach (var row in rows)
            {
                // Satırları tab ile ayırıyoruz
                var columns = row.Split(new[] { '\t' }, StringSplitOptions.None);
                parsedData.Add(columns);
            }

            return parsedData;
        }

        // Veriyi işleyip istediğimiz formata çeviren fonksiyon
        private List<Siparis> ProcessData(List<string[]> rawData)
        {
            var processedData = new List<Siparis>();
            string currentDate = "", yuklemeN = "";

            foreach (var row in rawData)
            {
                // Eğer "TALEP T." sütunu doluysa o değeri kaydet
                if (!string.IsNullOrWhiteSpace(row[0]))
                    currentDate = row[0];

                // Eğer "YÜKLEME N." sütunu doluysa o değeri kaydet
                if (!string.IsNullOrWhiteSpace(row[2]))
                    yuklemeN = row[2];

                // DateTime.Parse yerine TryParseExact kullanıyoruz
                DateTime parsedDate;

                string[] validFormats = { "yyyy-MM-dd", "dd/MM/yyyy", "MM/dd/yyyy" };

                if (DateTime.TryParseExact(currentDate, validFormats, null, System.Globalization.DateTimeStyles.None, out parsedDate))
                {
                    var siparis = new Siparis
                    {
                        SiparisTarihi = parsedDate,
                        YuklemeTarihi = parsedDate,
                        YuklemeNoktasi = yuklemeN,
                        TeslimFirmaAdresAdı = GetFormattedAddress(row[3]), // Teslim adresini formatlıyoruz
                        TeslimTarihi = parsedDate.AddDays(1), // Teslim tarihini bir gün sonrası olarak belirleme

                        // Sabit değerler
                        MusteriVkn = "7510055607",
                        Proje = "32",
                        MusteriSiparisNo = "SRH",
                        YuklemeFirmasıAdresAdı = "291",
                        Urun = "19",
                        KapAdet = 25,
                        AmbalajTipi = 1,
                        BrutKG = 25.000M
                    };

                    processedData.Add(siparis);
                }
                else
                {
                    MessageBox.Show($"Geçersiz tarih formatı: {currentDate}");
                }
            }

            return processedData;
        }

        // Teslim adresini formatlayan fonksiyon
        private string GetFormattedAddress(string address)
        {
            if (address.Contains("BİM") && address.EndsWith("BİM"))
            {
                // "BİM" kelimesi cümlenin sonunda ise, "BİM" ile adresi ters çeviriyoruz
                var parts = address.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                var addressWithoutBim = string.Join(" ", parts.Take(parts.Length - 1)); // Sondaki "BİM" kısmını ayırıyoruz
                return "BİM " + addressWithoutBim; // "BİM" ve adresi ters çevirerek döndürüyoruz
            }

            return address; // Eğer "BİM" son kısımda değilse, adresi olduğu gibi döndürüyoruz
        }



        // Veriyi işleyip gridControlSaruhan'a yüklüyoruz
        private void OnDataDropped(List<string[]> rawData)
        {
            var processedData = ProcessData(rawData);

            // gridControlSaruhan'a veriyi doğrudan atıyoruz
            gridControlSaruhan.DataSource = processedData;
        }

        private void BtnEslesme_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
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

                // SaruhanTeslimNoktalarıEslestir formunu oluştur
                SaruhanTeslimNoktalarıEslestir teslimNoktalarıForm = new SaruhanTeslimNoktalarıEslestir();

                // Adresleri gridViewSaruhan'dan al
                List<string> adresListesi = new List<string>();
                for (int i = 0; i < gridViewSaruhan.RowCount; i++)
                {
                    string adres = gridViewSaruhan.GetRowCellValue(i, "TeslimFirmaAdresAdı")?.ToString()?.Trim();
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
                    // Adresi temizle (parantez içeriğini çıkar, gereksiz boşlukları temizle)
                    string temizAdres = TemizleAdres(adres);

                    // Debug: Temizlenmiş adresi yazdır
                    Console.WriteLine("Orijinal Adres: " + adres);
                    Console.WriteLine("Temizlenmiş Adres: " + temizAdres);

                    // Adresleri eşleştirme
                    for (int j = 0; j < teslimNoktalarıForm.gridViewSaruhanEslesme.RowCount; j++)
                    {
                        string mevcutAdres = teslimNoktalarıForm.gridViewSaruhanEslesme.GetRowCellValue(j, "AdresAdi")?.ToString()?.Trim();

                        // Adresi temizle (parantez içeriğini çıkar, gereksiz boşlukları temizle)
                        string temizMevcutAdres = TemizleAdres(mevcutAdres);

                        // Debug: Temizlenmiş mevcut adresi yazdır
                        Console.WriteLine("Mevcut Adres: " + mevcutAdres);
                        Console.WriteLine("Temizlenmiş Mevcut Adres: " + temizMevcutAdres);

                        // Temizlenmiş adresleri karşılaştır
                        if (temizMevcutAdres.Equals(temizAdres, StringComparison.OrdinalIgnoreCase))
                        {
                            // Eşleşen satıra TeslimFirmaAdresAdı değerini yaz
                            teslimNoktalarıForm.gridViewSaruhanEslesme.SetRowCellValue(j, "Saruhan", adres);
                            eslesenAdresler.Add(adres); // Eşleşen adresi ekle
                            eslesmeyenAdresler.Remove(adres); // Eşleşmeyenler listesinden çıkar
                            break; // Eşleşme bulunduğunda döngüden çık
                        }
                    }
                }

                // gridControl1 veri kaynağını güncelle
                teslimNoktalarıForm.gridControlSaruhanEslesme.RefreshDataSource();

                // Eşleşenleri en üste taşıma
                for (int j = 0; j < teslimNoktalarıForm.gridViewSaruhanEslesme.RowCount; j++)
                {
                    string mevcutAdres = teslimNoktalarıForm.gridViewSaruhanEslesme.GetRowCellValue(j, "Saruhan")?.ToString();
                    if (!string.IsNullOrEmpty(mevcutAdres))
                    {
                        teslimNoktalarıForm.gridViewSaruhanEslesme.MoveFirst(); // Eşleşen satırı en üstte göster
                        break; // İlk eşleşen satırı bulduğumuzda döngüden çık
                    }
                }

                // "Saruhan" sütununa göre sıralama yap
                teslimNoktalarıForm.gridViewSaruhanEslesme.Columns["Saruhan"].SortOrder = DevExpress.Data.ColumnSortOrder.Descending;

                // Yükleme formunu kapat
                loadingForm.Close();

                // RowCellStyle olayını tanımla
                teslimNoktalarıForm.gridViewSaruhanEslesme.RowCellStyle += (s, ev) =>
                {
                    string SaruhanDegeri = teslimNoktalarıForm.gridViewSaruhanEslesme.GetRowCellValue(ev.RowHandle, "Saruhan")?.ToString();
                    if (eslesenAdresler.Contains(SaruhanDegeri))
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

        private string TemizleAdres(string adres)
        {
            // Parantez içeriğini çıkar (eğer varsa)
            string temizAdres = Regex.Replace(adres, @"\s*\(.*\)", "").Trim();
            return temizAdres;
        }



        // Sipariş bilgilerini temsil eden sınıf
        public class Siparis
        {
            // Mevcut özellikler
            public DateTime SiparisTarihi { get; set; }
            public DateTime? YuklemeTarihi { get; set; }
            public string YuklemeNoktasi { get; set; }
            public string TeslimFirmaAdresAdı { get; set; }
            public DateTime TeslimTarihi { get; set; }

            // Yeni sabit değerler
            public string MusteriVkn { get; set; }
            public string Proje { get; set; }
            public string MusteriSiparisNo { get; set; }
            public string YuklemeFirmasıAdresAdı { get; set; }
            public string Urun { get; set; }
            public int KapAdet { get; set; }
            public int AmbalajTipi { get; set; }
            public decimal BrutKG { get; set; }
            public string AlıcıFirmaCariUnvanı {  get; set; }

        }

        private void BtnTeslimleriGetir_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                // GridControlSaruhan'deki tüm satırları alıyoruz
                for (int rowHandle = 0; rowHandle < gridViewSaruhan.RowCount; rowHandle++) // Tüm satırlar üzerinde döngü başlatıyoruz
                {
                    // TeslimFirmaAdresAdı sütunundaki değeri alıyoruz
                    string teslimFirmaAdresAdi = gridViewSaruhan.GetRowCellValue(rowHandle, "TeslimFirmaAdresAdı")?.ToString(); // Null kontrolü de ekledim

                    if (string.IsNullOrEmpty(teslimFirmaAdresAdi))
                        continue; // Eğer boş veya null ise, bu satırı atla

                    // DbSiparisOtomasyonEntities6 bağlamını kullanarak TblEslesmelerle tablosunda Saruhan sütununda ara
                    using (var context = new DbSiparisOtomasyonEntities6())
                    {
                        // Saruhan sütununda teslimFirmaAdresAdi ile eşleşen bir satır arıyoruz
                        var eslesme = context.TblEslesmelerle
                            .FirstOrDefault(e1 => e1.Saruhan == teslimFirmaAdresAdi); // Saruhan sütunuyla karşılaştırıyoruz

                        if (eslesme != null)
                        {
                            // AdresID'yi string olarak alıyoruz
                            string adresID = eslesme.AdresID.ToString();
                            // MusteriID'yi string olarak alıyoruz
                            string musteriID = eslesme.MusteriID.ToString();

                            // İlgili hücrelere AdresID ve MusteriID değerlerini yazıyoruz
                            gridViewSaruhan.SetRowCellValue(rowHandle, "TeslimFirmaAdresAdı", adresID);
                            gridViewSaruhan.SetRowCellValue(rowHandle, "AlıcıFirmaCariUnvanı", musteriID);
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
                        gridControlSaruhan.ExportToXlsx(saveFileDialog.FileName);

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
                int rowCount = gridViewSaruhan.RowCount;

                // Satırları silme işlemi
                for (int i = rowCount - 1; i >= 0; i--)
                {
                    gridViewSaruhan.DeleteRow(i); // Satırları tersten silmek, index sorunlarını engeller
                }
            }
            catch (Exception ex)
            {
                // Hata yönetimi
                MessageBox.Show($"Bir hata oluştu: {ex.Message}");
            }
        }

        private void BtnEslestir_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            // Eğer form zaten açıksa, tekrar açmamak için kontrol edelim.
            SaruhanTeslimNoktalarıEslestir form = Application.OpenForms.OfType<SaruhanTeslimNoktalarıEslestir>().FirstOrDefault();

            if (form == null)
            {
                // Form zaten açık değilse, yeni bir form oluştur ve göster
                form = new SaruhanTeslimNoktalarıEslestir();
                form.Show();
            }
            else
            {
                // Form açıksa, onu öne getir
                form.Show();
            }
        }

        private void SaruhanSiparisEkranı_Load(object sender, EventArgs e)
        {
            // GridView objesini alın
            GridView gridView = gridControlSaruhan.MainView as GridView;

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
            GridView gridView = gridControlSaruhan.MainView as GridView;

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



