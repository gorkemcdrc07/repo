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
    public partial class DogtasTeslimNoktalarıEslestir : Form
    {
        private DbSiparisOtomasyonEntities6 dbContext; // DbContext'i burada tanımlıyoruz

        public DogtasTeslimNoktalarıEslestir()
        {
            InitializeComponent();
            dbContext = new DbSiparisOtomasyonEntities6(); // DbContext'i başlatıyoruz
        }

        private void DogtasTeslimNoktalarıEslestir_Load(object sender, EventArgs e)
        {
            // DbContext üzerinden verileri al
            var data = dbContext.TblEslesmelerle.ToList();

            // GridControl'ün veri kaynağını ayarla
            gridControlDogtas.DataSource = data;

            // Veri kontrolü
            if (data == null || !data.Any())
            {
                MessageBox.Show("TblEslesmelerle tablosunda hiç veri yok.");
            }
        }


        private void BtnOnayla_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            // gridControlDogtas'ın DataSource'unun doğru bir şekilde ayarlandığını kontrol et
            var gridControl1Data = gridControlDogtas.DataSource as List<TblEslesmelerle>;

            if (gridControl1Data == null || !gridControl1Data.Any())
            {
                MessageBox.Show("GridControl verisi alınamadı veya veri listesi boş.");
                return;
            }

            // Değişiklikleri veritabanına kaydet
            try
            {
                dbContext.SaveChanges(); // Veritabanındaki değişiklikleri kaydet
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
            dbContext.Dispose(); // dbContext'i kapatıyoruz
            base.OnFormClosed(e);
        }
    }
}
