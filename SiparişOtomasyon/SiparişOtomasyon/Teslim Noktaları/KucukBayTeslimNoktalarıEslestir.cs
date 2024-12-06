using SiparişOtomasyon.entity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace SiparişOtomasyon.Teslim_Noktaları
{
    public partial class KucukBayTeslimNoktalarıEslestir : Form
    {
        private List<int> updatedRows;
        private DbSiparisOtomasyonEntities6 dbContext; // dbContext değişkenini tanımlayın

        // Constructor that initializes the form and dbContext
        public KucukBayTeslimNoktalarıEslestir()
        {
            InitializeComponent();
            dbContext = new DbSiparisOtomasyonEntities6(); // Initialize dbContext
        }

        // Constructor that takes updated row indices as a parameter
        public KucukBayTeslimNoktalarıEslestir(List<int> updatedRows)
        {
            InitializeComponent();
            this.updatedRows = updatedRows;  // Store the updated row indices
            dbContext = new DbSiparisOtomasyonEntities6(); // Initialize dbContext
        }

        // Ensure that dbContext is initialized
        private void EnsureDbContext()
        {
            if (dbContext == null)
            {
                dbContext = new DbSiparisOtomasyonEntities6();
            }
        }

        // Form load event to bind data and apply sorting
        private void KucukBayTeslimNoktalarıEslestir_Load(object sender, EventArgs e)
        {
            EnsureDbContext(); // Make sure dbContext is initialized

            // Retrieve and sort the data so that non-empty 'KucukBay' comes first
            var sortedData = dbContext.TblEslesmelerle
                .OrderBy(x => string.IsNullOrEmpty(x.KucukBay) ? 1 : 0) // Non-empty KucukBay should come first
                .ThenBy(x => x.KucukBay) // Optional: sort by the KucukBay value
                .ToList();

            // Bind the sorted data to the grid
            gridControlKucukBayEslesme.DataSource = sortedData;

            // Subscribe to the custom draw event for row coloring
            gridViewKucukBayEslesme.CustomDrawCell += GridViewKucukBayEslesme_CustomDrawCell;
        }

        // Custom draw cell event to color updated rows in green
        // Custom draw cell event to color updated rows in light green
        private void GridViewKucukBayEslesme_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            // Ensure updatedRows is not null before using it
            if (updatedRows == null)
            {
                updatedRows = new List<int>();  // Initialize an empty list if it's null
            }

            // Check if the column is KucukBay and the row is in the updated list
            if (e.Column.FieldName == "KucukBay" && updatedRows.Contains(e.RowHandle))
            {
                // Set the background color to light green for updated rows
                e.Appearance.BackColor = Color.LightGreen;
                e.Appearance.ForeColor = Color.White; // Optional: Change text color to white
            }
        }


        // Event handler for "Onayla" button to save changes to the database
        private void BtnOnayla_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            var gridControl1Data = gridControlKucukBayEslesme.DataSource as List<TblEslesmelerle>;
            if (gridControl1Data == null)
            {
                MessageBox.Show("GridControl verisi alınamadı.");
                return;
            }

            // Save changes to the database
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


        // Form closed event to properly dispose dbContext
        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            dbContext.Dispose();
            base.OnFormClosed(e);
        }
    }
}
