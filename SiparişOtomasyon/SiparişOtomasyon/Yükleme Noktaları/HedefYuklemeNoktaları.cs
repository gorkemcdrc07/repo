using DevExpress.XtraEditors;
using SiparişOtomasyon.entity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.Entity;  // Bu satırın ekli olduğuna emin olun

namespace SiparişOtomasyon.Yükleme_Noktaları
{
    public partial class HedefYuklemeNoktaları : Form
    {
        public HedefYuklemeNoktaları()
        {
            InitializeComponent();
        }

        private void HedefYuklemeNoktaları_Load(object sender, EventArgs e)
        {
            // TODO: Bu kod satırı 'dbSiparisOtomasyonDataSet42.Hedef' tablosuna veri yükler. Bunu gerektiği şekilde taşıyabilir, veya kaldırabilirsiniz.
            this.hedefTableAdapter1.Fill(this.dbSiparisOtomasyonDataSet42.Hedef);
        }

        private void BtnKaydet_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                // Create a new instance of your database context
                using (var context = new DbSiparisOtomasyonEntities6())
                {
                    // Loop through the rows in gridControl1
                    for (int rowIndex = 0; rowIndex < gridView1.RowCount; rowIndex++)
                    {
                        // Retrieve data from each row using gridView1.GetRow()
                        var rowData = gridView1.GetRow(rowIndex) as DataRowView;

                        if (rowData != null)
                        {
                            // Retrieve each value directly from the row (avoiding get_Item())
                            string adresID = rowData["AdresID"]?.ToString();
                            string unvan = rowData["Unvan"]?.ToString();
                            string adresAdı = rowData["AdresAdı"]?.ToString();
                            string il = rowData["İl"]?.ToString();
                            string ilce = rowData["İlce"]?.ToString();
                            string tanimla = rowData["Tanımla"]?.ToString();

                            // Find the existing record in the database based on a unique key (e.g., AdresID)
                            var hedefEntity = context.Hedef.FirstOrDefault(h => h.AdresID == adresID);

                            if (hedefEntity != null)
                            {
                                // Update the existing entity's properties
                                hedefEntity.Unvan = unvan;
                                hedefEntity.AdresAdı = adresAdı;
                                hedefEntity.İl = il;
                                hedefEntity.İlce = ilce;
                                hedefEntity.Tanımla = tanimla;

                                // Optionally, mark the entity as modified
                                context.Entry(hedefEntity).State = System.Data.Entity.EntityState.Modified;
                            }
                        }
                    }

                    // Save changes to the database
                    context.SaveChanges();
                }

                // Optionally, show a message to the user
                XtraMessageBox.Show("Veriler başarıyla güncellendi.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                // Handle errors, maybe log the exception or show an error message
                XtraMessageBox.Show("Bir hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

