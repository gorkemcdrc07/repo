using DevExpress.Data.NetCompatibility.Extensions;
using DevExpress.XtraBars;
using DevExpress.XtraGrid.Views.Grid;
using SiparişOtomasyon.entity;
using SiparişOtomasyon.Teslim_Noktaları;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SiparişOtomasyon.Sipariş_Ekranları
{
    public partial class KucukBaySiparisEkrani : Form
    {
       
        public KucukBaySiparisEkrani()
        {
            InitializeComponent();
            gridControlKucukBay.AllowDrop = true;
            gridControlKucukBay.DragEnter += gridControlKucukBay_DragEnter;
            gridControlKucukBay.DragDrop += gridControlKucukBay_DragDrop;
            BtnTeslimleriGetir.Enabled = false;




        }
        private void gridControlKucukBay_DragEnter(object sender, DragEventArgs e)
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

        private void gridControlKucukBay_DragDrop(object sender, DragEventArgs e)
        {
            // Sürüklenen veriyi al
            string data = (string)e.Data.GetData(DataFormats.Text);

            // Verileri satır satır ayır
            string[] lines = data.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

            // gridControlKucukBay'ın veri kaynağını al
            DataTable dt = gridControlKucukBay.DataSource as DataTable;

            if (dt == null)
            {
                // Eğer dt null ise, yeni bir DataTable oluştur
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
                dt.Columns.Add("AlıcıFirmaCariUnvanı");

                gridControlKucukBay.DataSource = dt; // gridControlKucukBay'ın veri kaynağını ayarla
            }

            // Her bir satırı işleyelim
            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i].Trim();

                // Satırı sütunlara ayır
                string[] columns = line.Split(new[] { '\t' }, StringSplitOptions.None);

                // Eğer 5 sütun içeriyorsa
                if (columns.Length >= 5)
                {
                    string teslimFirmaAdresAdı = columns[3].Trim();  // 4. sütun
                    string teslimAdresDetay = columns[4].Trim();     // 5. sütun

                    // "+" işaretine göre ayırma
                    string[] teslimFirmaAdresAdıParts = teslimFirmaAdresAdı.Split(new[] { '+' }, StringSplitOptions.None);
                    string[] teslimAdresDetayParts = teslimAdresDetay.Split(new[] { '+' }, StringSplitOptions.None);

                    // Her bir teslimFirmaAdresAdı parçasını işleyelim
                    for (int j = 0; j < teslimFirmaAdresAdıParts.Length; j++)
                    {
                        string firmaAdı = teslimFirmaAdresAdıParts[j].Trim();
                        string adresDetay = j < teslimAdresDetayParts.Length ? teslimAdresDetayParts[j].Trim() : "";

                        if (!string.IsNullOrEmpty(firmaAdı))
                        {
                            DataRow newRow = dt.NewRow();
                            newRow["TeslimFirmaAdresAdı"] = string.IsNullOrEmpty(adresDetay)
                                ? firmaAdı
                                : $"{firmaAdı} ({adresDetay})";

                            newRow["TeslimTarihi"] = DateTime.Now.ToString("dd.MM.yyyy");
                            newRow["YuklemeTarihi"] = DateTime.Now.ToString("dd.MM.yyyy");
                            newRow["SiparisTarihi"] = DateTime.Now.ToString("dd.MM.yyyy");

                            // Diğer varsayılan değerleri ekle
                            newRow["YuklemeFirmasıAdresAdı"] = "19611";
                            newRow["MusteriVkn"] = "6030047754";
                            newRow["Proje"] = "41";
                            newRow["Urun"] = "29";
                            newRow["KapAdet"] = "25";
                            newRow["AmbalajTipi"] = "1";
                            newRow["BrutKG"] = "25.000";

                            dt.Rows.Add(newRow); // Yeni satırı ekle
                        }
                    }
                }
            }
        }




        // Adresin "( A101 )" gibi bölümlerini temizleyen yardımcı metod
        private string temizleAdres(string adres)
        {
            // "( A101 )" gibi kısmı temizle
            return Regex.Replace(adres, @"\(\s*([^\)]+)\s*\)", "").Trim();
        }




        private void BtnEslesme_ItemClick(object sender, ItemClickEventArgs e)
        {
            BtnTeslimleriGetir.Enabled = true;
            // 1. gridControlKucukBay'deki TeslimFirmaAdresAdı verilerini al
            var teslimFirmaAdresVerileri = gridControlKucukBay.DataSource as DataTable;
            if (teslimFirmaAdresVerileri == null) return;

            int eslesmeSayisi = 0;  // Eşleşme sayısını tutacak değişken
            List<string> eslesenAdresler = new List<string>();  // Eşleşen adresleri tutacak liste
            List<string> yeniEslesenAdresler = new List<string>();  // Yeni eşleşen adresleri tutacak liste

            foreach (DataRow row in teslimFirmaAdresVerileri.Rows)
            {
                string teslimFirmaAdres = row["TeslimFirmaAdresAdı"].ToString();
                if (string.IsNullOrWhiteSpace(teslimFirmaAdres)) continue;

                // 2. TeslimFirmaAdresAdı'ndan parantez dışı ve içindeki verileri ayır
                string parantezDisi = teslimFirmaAdres.Split('(')[0].Trim();
                string parantezIcinde = teslimFirmaAdres.Contains("(") ? teslimFirmaAdres.Split('(')[1].Replace(")", "").Trim() : string.Empty;

                // 3. Parantez içindeki veriyi '/' ile ayır
                var parantezIcindeKelimeler = parantezIcinde.Split('/').Select(p => p.Trim()).ToList();

                // 4. TblEslesmelerle tablosunda Unvan veya AdresAdi sütunlarında arama yap
                using (var context = new DbSiparisOtomasyonEntities6())
                {
                    var eslesmeler = context.TblEslesmelerle
                        .Where(eslesme => eslesme.Unvan.Contains(parantezDisi) || eslesme.AdresAdi.Contains(parantezDisi))
                        .ToList();

                    foreach (var eslesme in eslesmeler)
                    {
                        bool eslesmeBulundu = false;

                        foreach (var kelime in parantezIcindeKelimeler)
                        {
                            if (eslesme.İl.Contains(kelime) || eslesme.İlce.Contains(kelime))
                            {
                                eslesmeBulundu = true;
                                break;
                            }
                        }

                        if (eslesmeBulundu)
                        {
                            eslesmeSayisi++;
                            // Eğer adres daha önce eşleşmediyse, yeni eşleşenler listesine ekle
                            if (!eslesenAdresler.Contains(teslimFirmaAdres))
                            {
                                eslesenAdresler.Add(teslimFirmaAdres);
                                yeniEslesenAdresler.Add(teslimFirmaAdres); // Yeni eşleşen adresi ekle
                            }
                            eslesme.KucukBay = teslimFirmaAdres;
                            context.SaveChanges();
                            break;
                        }
                    }
                }
            }

            // 5. Eşleşenleri göstermek için formu aç (veya yeniden kullan)
            KucukBayTeslimNoktalarıEslestir teslimNoktalarıForm = Application.OpenForms
                .OfType<KucukBayTeslimNoktalarıEslestir>()
                .FirstOrDefault();

            if (teslimNoktalarıForm == null)
            {
                teslimNoktalarıForm = new KucukBayTeslimNoktalarıEslestir();
                teslimNoktalarıForm.Show();
            }

            teslimNoktalarıForm.gridViewKucukBayEslesme.RowCellStyle += (s, ev) =>
            {
                string teslimFirmaAdres = teslimNoktalarıForm.gridViewKucukBayEslesme.GetRowCellValue(ev.RowHandle, "TeslimFirmaAdresAdı")?.ToString();

                // Eşleşen adreslerin içinde olup olmadığını kontrol et
                if (eslesenAdresler.Contains(teslimFirmaAdres))
                {
                    ev.Appearance.BackColor = Color.LightGreen; // Yeşil renge boya
                    Console.WriteLine($"Eşleşen adres: {teslimFirmaAdres}");

                    // Yeni eşleşenleri koyu renkte yap
                    if (yeniEslesenAdresler.Contains(teslimFirmaAdres))
                    {
                        ev.Appearance.ForeColor = Color.Black; // Yazı rengini koyu yap
                        ev.Appearance.Font = new Font(ev.Appearance.Font, FontStyle.Bold); // Yazıyı koyu (bold) yap
                    }
                }
                else
                {
                    Console.WriteLine($"Eşleşmeyen adres: {teslimFirmaAdres}");
                }
            };

            // Grid'i yenile ve görünümü güncelle
            teslimNoktalarıForm.gridViewKucukBayEslesme.RefreshData();

            // 7. Eşleşme bilgisi ver
            if (eslesmeSayisi > 0)
            {
                // Yeni eşleşenleri kullanıcıya göster
                string yeniEslesenAdreslerText = string.Join(", ", yeniEslesenAdresler);
                MessageBox.Show($"{eslesmeSayisi} eşleşme bulundu. Yeni eşleşen adresler: {yeniEslesenAdreslerText}");
            }
            else
            {
                MessageBox.Show("Hiç eşleşme bulunamadı.");
            }
        }





















        private void BtnEslestir_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            // Create an instance of KucukBayTeslimNoktalarıEslestir form
            KucukBayTeslimNoktalarıEslestir form = new KucukBayTeslimNoktalarıEslestir();

            // Show the form
            form.Show();
        }

        private void BtnTeslimleriGetir_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                // GridControlTekirdagUn'deki tüm satırları alıyoruz
                for (int rowHandle = 0; rowHandle < gridViewKucukBay.RowCount; rowHandle++) // Tüm satırlar üzerinde döngü başlatıyoruz
                {
                    // TeslimFirmaAdresAdı sütunundaki değeri alıyoruz
                    string teslimFirmaAdresAdi = gridViewKucukBay.GetRowCellValue(rowHandle, "TeslimFirmaAdresAdı")?.ToString(); // Null kontrolü de ekledim

                    if (string.IsNullOrEmpty(teslimFirmaAdresAdi))
                        continue; // Eğer boş veya null ise, bu satırı atla

                    // DbSiparisOtomasyonEntities6 bağlamını kullanarak TblEslesmelerle tablosunda ara
                    using (var context = new DbSiparisOtomasyonEntities6())
                    {
                        // TekirdagUn sütununda teslimFirmaAdresAdi ile eşleşen bir satır arıyoruz
                        var eslesme = context.TblEslesmelerle
                            .FirstOrDefault(e1 => e1.KucukBay == teslimFirmaAdresAdi); // TekirdagUn sütunuyla karşılaştırıyoruz

                        if (eslesme != null)
                        {
                            // AdresID'yi string olarak alıyoruz
                            string adresID = eslesme.AdresID.ToString();
                            // MusteriID'yi string olarak alıyoruz
                            string musteriID = eslesme.MusteriID.ToString();

                            // İlgili hücrelere AdresID ve MusteriID değerlerini yazıyoruz
                            gridViewKucukBay.SetRowCellValue(rowHandle, "TeslimFirmaAdresAdı", adresID);
                            gridViewKucukBay.SetRowCellValue(rowHandle, "AlıcıFirmaCariUnvanı", musteriID);
                        }
                    }

                    // İstenilenAracTipi sütunundaki değeri kontrol et
                    string istenilenAracTipi = gridViewKucukBay.GetRowCellValue(rowHandle, "İstenilenAracTipi")?.ToString();

                    if (!string.IsNullOrEmpty(istenilenAracTipi))
                    {
                        if (istenilenAracTipi.Equals("TIR", StringComparison.OrdinalIgnoreCase))
                        {
                            gridViewKucukBay.SetRowCellValue(rowHandle, "İstenilenAracTipi", 1);
                        }
                        else if (istenilenAracTipi.Equals("KAMYON", StringComparison.OrdinalIgnoreCase))
                        {
                            gridViewKucukBay.SetRowCellValue(rowHandle, "İstenilenAracTipi", 3);
                        }
                        else if (istenilenAracTipi.Equals("KIRKAYAK", StringComparison.OrdinalIgnoreCase))
                        {
                            gridViewKucukBay.SetRowCellValue(rowHandle, "İstenilenAracTipi", 2);
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

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
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
                        gridControlKucukBay.ExportToXlsx(saveFileDialog.FileName);

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
                // Satırları silmek için gridViewKucukBay'in RowCount özelliğini kontrol edip, her satırı tek tek siliyoruz
                int rowCount = gridViewKucukBay.RowCount;

                // Satırları silme işlemi
                for (int i = rowCount - 1; i >= 0; i--)
                {
                    gridViewKucukBay.DeleteRow(i); // Satırları tersten silmek, index sorunlarını engeller
                }
            }
            catch (Exception ex)
            {
                // Hata yönetimi
                MessageBox.Show($"Bir hata oluştu: {ex.Message}");
            }
        }

        private void KucukBaySiparisEkrani_Load(object sender, EventArgs e)
        {
            // GridView objesini alın
            GridView gridView = gridControlKucukBay.MainView as GridView;

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
            GridView gridView = gridControlKucukBay.MainView as GridView;

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







           