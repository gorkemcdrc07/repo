using SiparişOtomasyon.entity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Windows.Forms;

namespace SiparişOtomasyon.Teslim_Noktaları
{
    public partial class GezerTeslimNoktalarıEslestir : Form
    {
        private DbSiparisOtomasyonEntities6 dbContext; // DbContext nesnesi

        public GezerTeslimNoktalarıEslestir()
        {
            InitializeComponent();
            dbContext = new DbSiparisOtomasyonEntities6(); // DbContext'i başlatıyoruz
        }

        private void GezerTeslimNoktalarıEslestir_Load(object sender, EventArgs e)
        {
            // TODO: Bu kod satırı 'dbSiparisOtomasyonDataSet27.TblEslesmelerle' tablosuna veri yükler. Bunu gerektiği şekilde taşıyabilir, veya kaldırabilirsiniz.
            this.tblEslesmelerleTableAdapter.Fill(this.dbSiparisOtomasyonDataSet27.TblEslesmelerle);
            // Veritabanından verileri alıyoruz
            var eslesmeler = dbContext.TblEslesmelerle.ToList(); // TblEslesmelerle tablosundaki tüm veriler

            // gridControl'e veri kaynağını atıyoruz
            gridControlGezerEslesme.DataSource = eslesmeler;
        }

        private void BtnOnayla_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

            var gridControl1Data = gridControlGezerEslesme.DataSource as List<TblEslesmelerle>;
            if (gridControl1Data == null)
            {
                MessageBox.Show("GridControl verisi alınamadı.");
                return;
            }

            // Değişiklikleri veritabanına kaydet
            try
            {
                dbContext.SaveChanges();
                MessageBox.Show("Kayıtlar başarıyla kaydedildi.");
            }
            catch (DbUpdateException ex)
            {
                MessageBox.Show("Kayıt güncellenirken bir hata oluştu: " + ex.Message);
            }
        }

        // Form kapandığında dbContext'i uygun şekilde kapatın
        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            dbContext.Dispose();
            base.OnFormClosed(e);
        }
    }
}
