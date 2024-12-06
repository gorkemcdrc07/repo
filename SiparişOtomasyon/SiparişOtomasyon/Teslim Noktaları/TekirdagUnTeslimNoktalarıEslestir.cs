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

namespace SiparişOtomasyon.Teslim_Noktaları
{
    public partial class TekirdagUnTeslimNoktalarıEslestir : Form
    {
        private DbSiparisOtomasyonEntities6 dbContext; // dbContext değişkenini tanımlayın

        public TekirdagUnTeslimNoktalarıEslestir()
        {
            InitializeComponent();
            dbContext = new DbSiparisOtomasyonEntities6(); // dbContext nesnesini oluşturun
        }

        private void TekirdagUnTeslimNoktalarıEslestir_Load(object sender, EventArgs e)
        {
            // Verileri dbContext üzerinden yükleyin
            this.tblEslesmelerleTableAdapter.Fill(this.dbSiparisOtomasyonDataSet30.TblEslesmelerle);
            gridControlTekirdagUn.DataSource = dbContext.TblEslesmelerle.ToList(); // Verileri gridControl'e bağlayın
        }

        private void BtnOnayla_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            var gridControl1Data = gridControlTekirdagUn.DataSource as List<TblEslesmelerle>;
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
