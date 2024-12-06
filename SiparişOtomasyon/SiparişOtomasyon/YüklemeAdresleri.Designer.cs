namespace SiparişOtomasyon
{
    partial class YüklemeAdresleri
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(YüklemeAdresleri));
            this.ribbonControl1 = new DevExpress.XtraBars.Ribbon.RibbonControl();
            this.barButtonItem1 = new DevExpress.XtraBars.BarButtonItem();
            this.BtnEkle = new DevExpress.XtraBars.BarButtonItem();
            this.BtnSil = new DevExpress.XtraBars.BarButtonItem();
            this.ribbonPage1 = new DevExpress.XtraBars.Ribbon.RibbonPage();
            this.ribbonPageGroup2 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.gridControlKayıtlar = new DevExpress.XtraGrid.GridControl();
            this.tblEslesmelerleBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.dbSiparisOtomasyonDataSet33 = new SiparişOtomasyon.DbSiparisOtomasyonDataSet33();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colVkn = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colMusteriID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colUnvan = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colAdresID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colAdresAdi = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colİl = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colİlce = new DevExpress.XtraGrid.Columns.GridColumn();
            this.tblEslesmelerleTableAdapter = new SiparişOtomasyon.DbSiparisOtomasyonDataSet33TableAdapters.TblEslesmelerleTableAdapter();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControlKayıtlar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tblEslesmelerleBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dbSiparisOtomasyonDataSet33)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // ribbonControl1
            // 
            this.ribbonControl1.ExpandCollapseItem.Id = 0;
            this.ribbonControl1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.ribbonControl1.ExpandCollapseItem,
            this.barButtonItem1,
            this.BtnEkle,
            this.BtnSil});
            this.ribbonControl1.Location = new System.Drawing.Point(0, 0);
            this.ribbonControl1.MaxItemId = 4;
            this.ribbonControl1.Name = "ribbonControl1";
            this.ribbonControl1.Pages.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPage[] {
            this.ribbonPage1});
            this.ribbonControl1.Size = new System.Drawing.Size(1248, 150);
            // 
            // barButtonItem1
            // 
            this.barButtonItem1.Caption = "Kaydet";
            this.barButtonItem1.Id = 1;
            this.barButtonItem1.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("barButtonItem1.ImageOptions.SvgImage")));
            this.barButtonItem1.Name = "barButtonItem1";
            // 
            // BtnEkle
            // 
            this.BtnEkle.Caption = "Satır Ekle";
            this.BtnEkle.Id = 2;
            this.BtnEkle.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("BtnEkle.ImageOptions.SvgImage")));
            this.BtnEkle.Name = "BtnEkle";
            this.BtnEkle.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.BtnEkle_ItemClick);
            // 
            // BtnSil
            // 
            this.BtnSil.Caption = "Satır Sil";
            this.BtnSil.Id = 3;
            this.BtnSil.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("BtnSil.ImageOptions.SvgImage")));
            this.BtnSil.Name = "BtnSil";
            this.BtnSil.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.BtnSil_ItemClick);
            // 
            // ribbonPage1
            // 
            this.ribbonPage1.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] {
            this.ribbonPageGroup2});
            this.ribbonPage1.Name = "ribbonPage1";
            this.ribbonPage1.Text = "YÜKLEME ADRESLERİ";
            // 
            // ribbonPageGroup2
            // 
            this.ribbonPageGroup2.ItemLinks.Add(this.BtnEkle);
            this.ribbonPageGroup2.Name = "ribbonPageGroup2";
            this.ribbonPageGroup2.Text = "İŞLEMLER";
            // 
            // gridControlKayıtlar
            // 
            this.gridControlKayıtlar.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridControlKayıtlar.DataSource = this.tblEslesmelerleBindingSource;
            this.gridControlKayıtlar.Location = new System.Drawing.Point(0, 156);
            this.gridControlKayıtlar.MainView = this.gridView1;
            this.gridControlKayıtlar.MenuManager = this.ribbonControl1;
            this.gridControlKayıtlar.Name = "gridControlKayıtlar";
            this.gridControlKayıtlar.Size = new System.Drawing.Size(1248, 446);
            this.gridControlKayıtlar.TabIndex = 1;
            this.gridControlKayıtlar.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // tblEslesmelerleBindingSource
            // 
            this.tblEslesmelerleBindingSource.DataMember = "TblEslesmelerle";
            this.tblEslesmelerleBindingSource.DataSource = this.dbSiparisOtomasyonDataSet33;
            // 
            // dbSiparisOtomasyonDataSet33
            // 
            this.dbSiparisOtomasyonDataSet33.DataSetName = "DbSiparisOtomasyonDataSet33";
            this.dbSiparisOtomasyonDataSet33.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // gridView1
            // 
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colVkn,
            this.colMusteriID,
            this.colUnvan,
            this.colAdresID,
            this.colAdresAdi,
            this.colİl,
            this.colİlce});
            this.gridView1.GridControl = this.gridControlKayıtlar;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsView.ShowGroupPanel = false;
            // 
            // colVkn
            // 
            this.colVkn.FieldName = "Vkn";
            this.colVkn.Name = "colVkn";
            this.colVkn.Visible = true;
            this.colVkn.VisibleIndex = 0;
            // 
            // colMusteriID
            // 
            this.colMusteriID.FieldName = "MusteriID";
            this.colMusteriID.Name = "colMusteriID";
            this.colMusteriID.Visible = true;
            this.colMusteriID.VisibleIndex = 1;
            // 
            // colUnvan
            // 
            this.colUnvan.FieldName = "Unvan";
            this.colUnvan.Name = "colUnvan";
            this.colUnvan.Visible = true;
            this.colUnvan.VisibleIndex = 2;
            // 
            // colAdresID
            // 
            this.colAdresID.FieldName = "AdresID";
            this.colAdresID.Name = "colAdresID";
            this.colAdresID.Visible = true;
            this.colAdresID.VisibleIndex = 3;
            // 
            // colAdresAdi
            // 
            this.colAdresAdi.FieldName = "AdresAdi";
            this.colAdresAdi.Name = "colAdresAdi";
            this.colAdresAdi.Visible = true;
            this.colAdresAdi.VisibleIndex = 4;
            // 
            // colİl
            // 
            this.colİl.FieldName = "İl";
            this.colİl.Name = "colİl";
            this.colİl.Visible = true;
            this.colİl.VisibleIndex = 5;
            // 
            // colİlce
            // 
            this.colİlce.FieldName = "İlce";
            this.colİlce.Name = "colİlce";
            this.colİlce.Visible = true;
            this.colİlce.VisibleIndex = 6;
            // 
            // tblEslesmelerleTableAdapter
            // 
            this.tblEslesmelerleTableAdapter.ClearBeforeFill = true;
            // 
            // YüklemeAdresleri
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1248, 602);
            this.Controls.Add(this.gridControlKayıtlar);
            this.Controls.Add(this.ribbonControl1);
            this.Name = "YüklemeAdresleri";
            this.Text = "YüklemeAdresleri";
            this.Load += new System.EventHandler(this.YüklemeAdresleri_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControlKayıtlar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tblEslesmelerleBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dbSiparisOtomasyonDataSet33)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraBars.Ribbon.RibbonControl ribbonControl1;
        private DevExpress.XtraBars.Ribbon.RibbonPage ribbonPage1;
        private DevExpress.XtraBars.BarButtonItem barButtonItem1;
        private DevExpress.XtraGrid.GridControl gridControlKayıtlar;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DbSiparisOtomasyonDataSet33 dbSiparisOtomasyonDataSet33;
        private System.Windows.Forms.BindingSource tblEslesmelerleBindingSource;
        private DbSiparisOtomasyonDataSet33TableAdapters.TblEslesmelerleTableAdapter tblEslesmelerleTableAdapter;
        private DevExpress.XtraGrid.Columns.GridColumn colVkn;
        private DevExpress.XtraGrid.Columns.GridColumn colMusteriID;
        private DevExpress.XtraGrid.Columns.GridColumn colUnvan;
        private DevExpress.XtraGrid.Columns.GridColumn colAdresID;
        private DevExpress.XtraGrid.Columns.GridColumn colAdresAdi;
        private DevExpress.XtraGrid.Columns.GridColumn colİl;
        private DevExpress.XtraGrid.Columns.GridColumn colİlce;
        private DevExpress.XtraBars.BarButtonItem BtnEkle;
        private DevExpress.XtraBars.BarButtonItem BtnSil;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup2;
    }
}