using SiparişOtomasyon.entity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SiparişOtomasyon.Yükleme_Noktaları
{

    public partial class GezerYuklemeNoktası : Form
    {
        public GezerYuklemeNoktası()
        {
            InitializeComponent();
        }

        private void GezerYuklemeNoktası_Load(object sender, EventArgs e)
        {
            // TODO: Bu kod satırı 'dbSiparisOtomasyonDataSet26.TblGezer' tablosuna veri yükler. Bunu gerektiği şekilde taşıyabilir, veya kaldırabilirsiniz.
            this.tblGezerTableAdapter.Fill(this.dbSiparisOtomasyonDataSet26.TblGezer);

        }

        private void BtnOnayla_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            // Yeni bir veritabanı bağlamı başlat
            using (var context = new DbSiparisOtomasyonEntities6())
            {
                // gridControl1 üzerindeki tüm satırları döngü ile al
                for (int rowHandle = 0; rowHandle < gridView1.RowCount; rowHandle++)
                {
                    // Satırdaki her bir alanı, veritabanına kaydetmek için yeni bir TblGezer nesnesine atayalım
                    var yeniKayit = new TblGezer
                    {
                        MusteridenGelen = gridView1.GetRowCellValue(rowHandle, "MusteridenGelen")?.ToString(),

                        // 'AdresID' sütunu int türündeyse, Convert.ToInt32 kullanarak int'e çeviriyoruz
                        AdresID = gridView1.GetRowCellValue(rowHandle, "AdresID")?.ToString(),

                        // Diğer gerekli sütunları buraya ekleyin
                    };

                    // Yeni kaydı veritabanı bağlamına ekle
                    context.TblGezer.Add(yeniKayit);
                }

                // Değişiklikleri veritabanına kaydet
                context.SaveChanges();
            }

            MessageBox.Show("Veriler başarıyla kaydedildi.");
        }
    }
}

