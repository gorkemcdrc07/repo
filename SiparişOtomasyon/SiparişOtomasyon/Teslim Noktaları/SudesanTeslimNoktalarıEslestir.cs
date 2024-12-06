using SiparişOtomasyon.entity;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Windows.Forms;

namespace SiparişOtomasyon.Teslim_Noktaları
{
    public partial class SudesanTeslimNoktalarıEslestir : Form
    {
        // dbContext nesnesini tanımlayın
        private DbSiparisOtomasyonEntities6 dbContext;

        public SudesanTeslimNoktalarıEslestir()
        {
            InitializeComponent();
        }

        private void SudesanTeslimNoktalarıEslestir_Load(object sender, EventArgs e)
        {
            // TODO: Bu kod satırı 'dbSiparisOtomasyonDataSet22.TblEslesmelerle' tablosuna veri yükler. Bunu gerektiği şekilde taşıyabilir, veya kaldırabilirsiniz.
            this.tblEslesmelerleTableAdapter5.Fill(this.dbSiparisOtomasyonDataSet22.TblEslesmelerle);
            // TODO: Bu kod satırı 'dbSiparisOtomasyonDataSet21.TblEslesmelerle' tablosuna veri yükler. Bunu gerektiği şekilde taşıyabilir, veya kaldırabilirsiniz.
            this.tblEslesmelerleTableAdapter4.Fill(this.dbSiparisOtomasyonDataSet21.TblEslesmelerle);
            // TODO: Bu kod satırı 'dbSiparisOtomasyonDataSet20.TblEslesmelerle' tablosuna veri yükler. Bunu gerektiği şekilde taşıyabilir, veya kaldırabilirsiniz.
            this.tblEslesmelerleTableAdapter3.Fill(this.dbSiparisOtomasyonDataSet20.TblEslesmelerle);
            // DbContext'i başlatın (sunucuya bağlı)
            dbContext = new DbSiparisOtomasyonEntities6();

            // Veritabanından verileri çekin
            var eslesmeler = dbContext.TblEslesmelerle.ToList();

            // Verileri DataGridView veya benzeri bir kontrola bağlayabilirsiniz
            gridControlSudesanEslesme.DataSource = eslesmeler;

            // Eğer sadece DbContext kullanıyorsanız, aşağıdaki yerel DataSet kodlarını kaldırabilirsiniz.
            // Yerel DataSet ve TableAdapter'ları kullanmak istemiyorsanız, yorumlayabilirsiniz:
            // this.tblEslesmelerleTableAdapter2.Fill(this.dbSiparisOtomasyonDataSet19.TblEslesmelerle);
            // this.tblEslesmelerleTableAdapter1.Fill(this.dbSiparisOtomasyonDataSet18.TblEslesmelerle);
            // this.tblEslesmelerleTableAdapter.Fill(this.dbSiparisOtomasyonDataSet16.TblEslesmelerle);

        }




        private void BtnOnayla_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            var gridControl1Data = gridControlSudesanEslesme.DataSource as List<TblEslesmelerle>;
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

