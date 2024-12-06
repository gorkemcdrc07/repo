using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraGrid;
using SiparişOtomasyon.entity;
using SiparişOtomasyon.Teslim_Noktaları;
using System.Drawing;
using DevExpress.XtraGrid.Views.Grid;

namespace SiparişOtomasyon.Sipariş_Ekranları
{
    public partial class TekirdagUnSiparisEkrani : Form
    {
        public TekirdagUnSiparisEkrani()
        {
            InitializeComponent();

            // Sürükle ve bırak işlemi için olayları tanımlıyoruz
            this.AllowDrop = true;
            this.DragEnter += new DragEventHandler(TekirdagUnSiparisEkrani_DragEnter);
            this.DragDrop += new DragEventHandler(TekirdagUnSiparisEkrani_DragDrop);
        }

        public class TekirdagUnVeri
        {
            public string YuklemeFirmasıAdresAdı { get; set; }         // Yükleme yapan firmanın adres adı
            public string TeslimFirmaAdresAdı { get; set; }            // Teslim alan firmanın adres adı
            public string MusteriSiparisNo { get; set; }               // Müşteri sipariş numarası
            public string İstenilenAracTipi { get; set; }              // Sipariş için istenilen araç tipi
            public string Proje { get; set; }                          // İlgili proje kodu veya adı
            public string SiparisTarihi { get; set; }                  // Siparişin yapıldığı tarih
            public string YuklemeTarihi { get; set; }                  // Yüklemenin yapılacağı tarih
            public string TeslimTarihi { get; set; }                   // Teslim edilmesi gereken tarih
            public string AlıcıFirmaCariUnvanı { get; set; }           // Alıcı firmanın ticari unvanı
        }

        // Sürüklenen veriyi kabul etmek için
        private void TekirdagUnSiparisEkrani_DragEnter(object sender, DragEventArgs e)
        {
            // Verinin formatını kontrol ediyoruz (örneğin, Text verisi)
            if (e.Data.GetDataPresent(DataFormats.Text))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        // Veriyi GridControl'e yerleştirmek için
        private void TekirdagUnSiparisEkrani_DragDrop(object sender, DragEventArgs e)
        {
            // Sürüklenen veriyi alıyoruz
            string data = (string)e.Data.GetData(DataFormats.Text);
            string[] rows = data.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

            // İlk satır başlık olduğu için, onu atlıyoruz
            rows = rows.Skip(1).ToArray();

            // DataTable'da GridControl için verileri işliyoruz
            DataTable dt = gridControlTekirdagUn.DataSource as DataTable;
            if (dt == null)
            {
                dt = new DataTable();
                dt.Columns.Add("SiparisTarihi");
                dt.Columns.Add("YuklemeTarihi");
                dt.Columns.Add("TeslimTarihi");
                dt.Columns.Add("TeslimFirmaAdresAdı");
                dt.Columns.Add("MusteriVkn");
                dt.Columns.Add("Proje");
                dt.Columns.Add("MusteriSiparisNo");
                dt.Columns.Add("İstenilenAracTipi");
                dt.Columns.Add("Urun");
                dt.Columns.Add("KapAdet");
                dt.Columns.Add("BrutKG");
                dt.Columns.Add("AlıcıFirmaCariUnvanı");
                dt.Columns.Add("YuklemeFirmasıAdresAdı");
                dt.Columns.Add("AmbalajTipi");
            }

            // Her satır için sütunları ayırıyoruz ve ilgili yerlere yerleştiriyoruz
            foreach (var row in rows)
            {
                if (!string.IsNullOrWhiteSpace(row))
                {
                    string[] columns = row.Split('\t'); // Örneğin, sekme ile ayrılmış veri
                    if (columns.Length >= 4)
                    {
                        // TeslimFirmaAdresAdı sütununda "+" işareti varsa, ayrı satırlara yaz
                        string teslimFirmaAdresAdı = columns[3];
                        string[] adresParts = teslimFirmaAdresAdı.Split(new[] { '+' }, StringSplitOptions.RemoveEmptyEntries);

                        foreach (var adres in adresParts)
                        {
                            DataRow dr = dt.NewRow();
                            dr["SiparisTarihi"] = columns[0];
                            dr["YuklemeTarihi"] = columns[0]; // 1. sütundaki veri
                            if (DateTime.TryParse(columns[0], out DateTime siparisTarihi))
                            {
                                // Tarihi yalnızca "yyyy-MM-dd" formatında string olarak ata
                                dr["TeslimTarihi"] = siparisTarihi.AddDays(1).ToString("dd.MM.yyyy");
                            }
                            else
                            {
                                dr["TeslimTarihi"] = DBNull.Value; // Eğer tarih formatı hatalıysa null atar
                            }

                            dr["TeslimFirmaAdresAdı"] = adres.Trim(); // "+" ile ayrılan adres parçası

                            // Belirttiğiniz sütunlara dolu olan satırlara göre atama yapıyoruz
                            dr["MusteriVkn"] = "8360015351";
                            dr["Proje"] = "13";
                            dr["YuklemeFirmasıAdresAdı"] = "22";
                            dr["MusteriSiparisNo"] = "TU-";
                            dr["İstenilenAracTipi"] = "1";
                            dr["Urun"] = "6";
                            dr["AmbalajTipi"] = "1";
                            dr["KapAdet"] = "25";
                            dr["BrutKG"] = "25.000";

                            dt.Rows.Add(dr);
                        }
                    }
                }
            }

            // GridControl'e yeni verileri ekliyoruz
            gridControlTekirdagUn.DataSource = dt;
        }


        private void BtnEslestir_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            // TekirdagUnTeslimNoktalarıEslestir formunu açıyoruz
            TekirdagUnTeslimNoktalarıEslestir eslestirFormu = new TekirdagUnTeslimNoktalarıEslestir();
            eslestirFormu.Show(); // veya eslestirFormu.ShowDialog(); modal olarak açmak için
        }



        private void BtnTeslimleriGetir_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                // GridControlTekirdagUn'deki tüm satırları alıyoruz
                for (int rowHandle = 0; rowHandle < gridView1.RowCount; rowHandle++) // Tüm satırlar üzerinde döngü başlatıyoruz
                {
                    // TeslimFirmaAdresAdı sütunundaki değeri alıyoruz
                    string teslimFirmaAdresAdi = gridView1.GetRowCellValue(rowHandle, "TeslimFirmaAdresAdı")?.ToString(); // Null kontrolü de ekledim

                    if (string.IsNullOrEmpty(teslimFirmaAdresAdi))
                        continue; // Eğer boş veya null ise, bu satırı atla

                    // DbSiparisOtomasyonEntities6 bağlamını kullanarak TblEslesmelerle tablosunda TekirdagUn sütununda ara
                    using (var context = new DbSiparisOtomasyonEntities6())
                    {
                        // TekirdagUn sütununda teslimFirmaAdresAdi ile eşleşen bir satır arıyoruz
                        var eslesme = context.TblEslesmelerle
                            .FirstOrDefault(e1 => e1.TekirdagUn == teslimFirmaAdresAdi); // TekirdagUn sütunuyla karşılaştırıyoruz

                        if (eslesme != null)
                        {
                            // AdresID'yi string olarak alıyoruz
                            string adresID = eslesme.AdresID.ToString();
                            // MusteriID'yi string olarak alıyoruz
                            string musteriID = eslesme.MusteriID.ToString();

                            // İlgili hücrelere AdresID ve MusteriID değerlerini yazıyoruz
                            gridView1.SetRowCellValue(rowHandle, "TeslimFirmaAdresAdı", adresID);
                            gridView1.SetRowCellValue(rowHandle, "AlıcıFirmaCariUnvanı", musteriID);
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
                        gridControlTekirdagUn.ExportToXlsx(saveFileDialog.FileName);

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
                int rowCount = gridView1.RowCount;

                // Satırları silme işlemi
                for (int i = rowCount - 1; i >= 0; i--)
                {
                    gridView1.DeleteRow(i); // Satırları tersten silmek, index sorunlarını engeller
                }
            }
            catch (Exception ex)
            {
                // Hata yönetimi
                MessageBox.Show($"Bir hata oluştu: {ex.Message}");
            }
        }

        private void BtnEslemeyi_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
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

                // Adresleri gridViewTekirdagUn'tan al
                List<string> adresListesi = new List<string>();
                for (int i = 0; i < gridView1.RowCount; i++)
                {
                    string adres = gridView1.GetRowCellValue(i, "TeslimFirmaAdresAdı")?.ToString()?.Trim();
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

                // TeslimNoktaları formunu oluştur
                TekirdagUnTeslimNoktalarıEslestir teslimNoktalarıForm = new TekirdagUnTeslimNoktalarıEslestir();
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

                    string ilce = null, il = null;
                    if (adresİçindekiVeri != null)
                    {
                        // Parantez içindeki veri - işareti ile ayrılıyor
                        string[] ilIlce = adresİçindekiVeri.Split('-');
                        if (ilIlce.Length == 2)
                        {
                            ilce = ilIlce[0].Trim();
                            il = ilIlce[1].Trim();
                        }
                    }

                    // TblEslesmelerle tablosunda arama yap
                    using (var context = new DbSiparisOtomasyonEntities6())
                    {
                        var eslesme = context.TblEslesmelerle
                            .FirstOrDefault(x => (x.AdresAdi.Contains(adresDışı) || x.Unvan.Contains(adresDışı)) &&
                                                 (string.IsNullOrEmpty(ilce) || x.İlce.Contains(ilce)) &&
                                                 (string.IsNullOrEmpty(il) || x.İl.Contains(il)));

                        if (eslesme != null)
                        {
                            // Eşleşme bulundu, TekirdagUn sütununa TeslimFirmaAdresAdı verisini getir
                            teslimNoktalarıForm.gridViewTekirdagUn.SetRowCellValue(
                                teslimNoktalarıForm.gridViewTekirdagUn.LocateByValue("AdresAdi", eslesme.AdresAdi),
                                "TekirdagUn", adres);

                            eslesenAdresler.Add(adres); // Eşleşen adresi ekle
                            eslesmeyenAdresler.Remove(adres); // Eşleşmeyenler listesinden çıkar
                        }
                    }
                }

                // gridControl1 veri kaynağını güncelle
                teslimNoktalarıForm.gridControlTekirdagUn.RefreshDataSource();

                // "TekirdagUn" sütununa göre sıralama yap
                teslimNoktalarıForm.gridViewTekirdagUn.Columns["TekirdagUn"].SortOrder = DevExpress.Data.ColumnSortOrder.Descending;

                // Yükleme formunu kapat
                loadingForm.Close();

                // RowCellStyle olayını tanımla
                teslimNoktalarıForm.gridViewTekirdagUn.RowCellStyle += (s, ev) =>
                {
                    string TekirdagUnDegeri = teslimNoktalarıForm.gridViewTekirdagUn.GetRowCellValue(ev.RowHandle, "TekirdagUn")?.ToString();
                    if (eslesenAdresler.Contains(TekirdagUnDegeri))
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

        private void TekirdagUnSiparisEkrani_Load(object sender, EventArgs e)
        {
            // GridView objesini alın
            GridView gridView = gridControlTekirdagUn.MainView as GridView;

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
            GridView gridView = gridControlTekirdagUn.MainView as GridView;

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


