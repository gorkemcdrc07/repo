using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using SiparişOtomasyon.entity;
using System;
using System.Data;
using System.Windows.Forms;
using System.Linq;


namespace SiparişOtomasyon
{
    public partial class YüklemeAdresleri : Form
    {
        private PopupContainerControl popupContainer;
        private PopupContainerEdit popupEdit;
        private GridControl gridControlPopup;
        private GridView gridViewPopup;
        private SimpleButton btnKaydet;
        private DataTable popupDataTable;

        public YüklemeAdresleri()
        {
            InitializeComponent();
            InitializePopupAndGrid();
        }

        private void YüklemeAdresleri_Load(object sender, EventArgs e)
        {
            this.tblEslesmelerleTableAdapter.Fill(this.dbSiparisOtomasyonDataSet33.TblEslesmelerle);
        }

        private void InitializePopupAndGrid()
        {
            // PopupContainerControl oluştur
            popupContainer = new PopupContainerControl();
            popupContainer.Size = new System.Drawing.Size(600, 400);
            this.Controls.Add(popupContainer);

            // DataTable Oluştur
            popupDataTable = new DataTable();
            popupDataTable.Columns.Add("Vkn", typeof(string));
            popupDataTable.Columns.Add("MusteriID", typeof(string));
            popupDataTable.Columns.Add("Unvan", typeof(string));
            popupDataTable.Columns.Add("AdresID", typeof(string));
            popupDataTable.Columns.Add("AdresAdi", typeof(string));
            popupDataTable.Columns.Add("Il", typeof(string));
            popupDataTable.Columns.Add("Ilce", typeof(string));

            // GridControl ve GridView oluştur
            gridControlPopup = new GridControl { DataSource = popupDataTable };
            gridViewPopup = new GridView(gridControlPopup);
            gridControlPopup.MainView = gridViewPopup;
            gridControlPopup.Dock = DockStyle.Fill;
            popupContainer.Controls.Add(gridControlPopup);

            // Kolon Başlıkları Ekle
            gridViewPopup.OptionsView.NewItemRowPosition = NewItemRowPosition.Bottom; // Boş satır eklenecek
            gridViewPopup.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.True;

            // Kaydet Butonu Ekle
            btnKaydet = new SimpleButton { Text = "Kaydet", Dock = DockStyle.Bottom };
            btnKaydet.Click += BtnKaydet_Click;
            popupContainer.Controls.Add(btnKaydet);

            // PopupContainerEdit oluştur ve ayarla
            popupEdit = new PopupContainerEdit();
            popupEdit.Properties.PopupControl = popupContainer;
            popupEdit.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            popupEdit.Size = new System.Drawing.Size(150, 20);
            popupEdit.Location = new System.Drawing.Point(10, 10);
            this.Controls.Add(popupEdit);

            // Hücre Doğrulama
            gridViewPopup.ValidateRow += GridViewPopup_ValidateRow;
        }

        // Satır Kaydetme İşlemi
        private void BtnKaydet_Click(object sender, EventArgs e)
        {
            // Boş hücreleri kontrol et ve uyarı ver
            for (int i = 0; i < gridViewPopup.RowCount - 1; i++)
            {
                foreach (DataColumn column in popupDataTable.Columns)
                {
                    if (gridViewPopup.GetRowCellValue(i, column.ColumnName) == null ||
                        string.IsNullOrWhiteSpace(gridViewPopup.GetRowCellValue(i, column.ColumnName).ToString()))
                    {
                        MessageBox.Show("Tüm hücreler doldurulmalıdır!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }
            }

            using (var context = new DbSiparisOtomasyonEntities6())
            {
                foreach (DataRow currentRow in popupDataTable.Rows)
                {
                    // Vkn, MusteriID ve AdresID'yi kontrol et
                    string vkn = currentRow["Vkn"].ToString();
                    string musteriID = currentRow["MusteriID"].ToString();
                    string adresID = currentRow["AdresID"].ToString();

                    // Veritabanında aynı Vkn, MusteriID, ve AdresID'ye sahip bir satır var mı kontrol et
                    var existingRow = context.TblEslesmelerle
                                            .FirstOrDefault(r => r.Vkn == vkn && r.MusteriID == musteriID && r.AdresID == adresID);

                    if (existingRow != null)
                    {
                        // Eğer böyle bir satır varsa, hata mesajı göster
                        MessageBox.Show("Aynı Vkn, MusteriID ve AdresID'ye sahip bir kayıt zaten mevcut!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return; // Kaydetme işlemini iptal et
                    }

                    // Yeni TblEslesmelerle nesnesi oluşturulup dolduruluyor
                    var newRow = new TblEslesmelerle
                    {
                        Vkn = vkn,
                        MusteriID = musteriID,
                        Unvan = currentRow["Unvan"].ToString(),
                        AdresID = adresID,
                        AdresAdi = currentRow["AdresAdi"].ToString(),
                        İl = currentRow["Il"].ToString(),
                        İlce = currentRow["Ilce"].ToString(),
                    };

                    // Yeni satırı veritabanı bağlamına ekle
                    context.TblEslesmelerle.Add(newRow);
                }

                // Veritabanına değişiklikleri kaydet
                context.SaveChanges();
            }

            // Ana formdaki gridControl'u güncelle
            RefreshGridControlKayıtlar();
            popupEdit.ClosePopup();

            // Başarılı kayıt mesajı göster
            MessageBox.Show("Müşteri kayıtları başarılı bir şekilde eklendi.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        // gridControlKayıtlar'ı güncelleyen metot
        private void RefreshGridControlKayıtlar()
        {
            using (var context = new DbSiparisOtomasyonEntities6())
            {
                // Yeni veriyi Entity Framework üzerinden çekerek BindingSource'u günceller
                gridControlKayıtlar.DataSource = context.TblEslesmelerle.ToList();
            }
        }






        // Hücre Doğrulama
        private void GridViewPopup_ValidateRow(object sender, DevExpress.XtraGrid.Views.Base.ValidateRowEventArgs e)
        {
            foreach (DataColumn column in popupDataTable.Columns)
            {
                var cellValue = gridViewPopup.GetRowCellValue(e.RowHandle, column.ColumnName);
                if (cellValue == null || string.IsNullOrWhiteSpace(cellValue.ToString()))
                {
                    e.Valid = false;
                    e.ErrorText = "Tüm hücreler doldurulmalıdır!";
                    break;
                }
            }
        }

        private void BtnEkle_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            popupEdit.ShowPopup();
        }

        private void BtnSil_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            // Seçili satırları al
            int[] selectedRows = gridView1.GetSelectedRows();

            if (selectedRows.Length > 0)
            {
                // Seçilen satırdaki değerleri al
                int rowHandle = selectedRows[0];

                // Vkn, MusteriID, AdresID gibi bilgileri al
                string vkn = gridView1.GetRowCellValue(rowHandle, "Vkn").ToString();
                string musteriID = gridView1.GetRowCellValue(rowHandle, "MusteriID").ToString();
                string adresID = gridView1.GetRowCellValue(rowHandle, "AdresID").ToString();

                // Veritabanında eşleşen kaydı bul
                using (var context = new DbSiparisOtomasyonEntities6())
                {
                    var existingRow = context.TblEslesmelerle
                                            .FirstOrDefault(r => r.Vkn == vkn && r.MusteriID == musteriID && r.AdresID == adresID);

                    if (existingRow != null)
                    {
                        // Bulunan satırı sil
                        context.TblEslesmelerle.Remove(existingRow);

                        // Değişiklikleri kaydet
                        context.SaveChanges();

                        // GridControl'u güncelle
                        RefreshGridControlKayıtlar();

                        // Başarı mesajı göster
                        MessageBox.Show("Seçilen kayıt başarıyla silindi.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        // Kayıt bulunamazsa hata mesajı göster
                        MessageBox.Show("Seçilen kayıt veritabanında bulunamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                // Hiçbir satır seçilmediyse uyarı göster
                MessageBox.Show("Lütfen silmek için bir satır seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

    }
}
