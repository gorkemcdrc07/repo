namespace SiparişOtomasyon.Teslim_Noktaları
{
    partial class ApakTeslimNoktalarıEslestir
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ApakTeslimNoktalarıEslestir));
            this.gridControlApak = new DevExpress.XtraGrid.GridControl();
            this.tblEslesmelerleBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.dbSiparisOtomasyonDataSet13 = new SiparişOtomasyon.DbSiparisOtomasyonDataSet13();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn4 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn5 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn6 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn7 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn8 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.tblEslesmelerleTableAdapter = new SiparişOtomasyon.DbSiparisOtomasyonDataSet13TableAdapters.TblEslesmelerleTableAdapter();
            this.ribbonControl1 = new DevExpress.XtraBars.Ribbon.RibbonControl();
            this.BtnOnayla = new DevExpress.XtraBars.BarButtonItem();
            this.BtnEslenEnUstte = new DevExpress.XtraBars.BarButtonItem();
            this.ribbonPage1 = new DevExpress.XtraBars.Ribbon.RibbonPage();
            this.ribbonPageGroup1 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            ((System.ComponentModel.ISupportInitialize)(this.gridControlApak)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tblEslesmelerleBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dbSiparisOtomasyonDataSet13)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).BeginInit();
            this.SuspendLayout();
            // 
            // gridControlApak
            // 
            this.gridControlApak.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridControlApak.DataSource = this.tblEslesmelerleBindingSource;
            this.gridControlApak.Location = new System.Drawing.Point(0, 156);
            this.gridControlApak.MainView = this.gridView1;
            this.gridControlApak.Name = "gridControlApak";
            this.gridControlApak.Size = new System.Drawing.Size(1248, 443);
            this.gridControlApak.TabIndex = 0;
            this.gridControlApak.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // tblEslesmelerleBindingSource
            // 
            this.tblEslesmelerleBindingSource.DataMember = "TblEslesmelerle";
            this.tblEslesmelerleBindingSource.DataSource = this.dbSiparisOtomasyonDataSet13;
            // 
            // dbSiparisOtomasyonDataSet13
            // 
            this.dbSiparisOtomasyonDataSet13.DataSetName = "DbSiparisOtomasyonDataSet13";
            this.dbSiparisOtomasyonDataSet13.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // gridView1
            // 
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn1,
            this.gridColumn2,
            this.gridColumn3,
            this.gridColumn4,
            this.gridColumn5,
            this.gridColumn6,
            this.gridColumn7,
            this.gridColumn8});
            this.gridView1.GridControl = this.gridControlApak;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsView.ShowGroupPanel = false;
            // 
            // gridColumn1
            // 
            this.gridColumn1.Caption = "Vkn";
            this.gridColumn1.FieldName = "Vkn";
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.Visible = true;
            this.gridColumn1.VisibleIndex = 0;
            // 
            // gridColumn2
            // 
            this.gridColumn2.Caption = "Müşteri ID";
            this.gridColumn2.FieldName = "MusteriID";
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.Visible = true;
            this.gridColumn2.VisibleIndex = 1;
            // 
            // gridColumn3
            // 
            this.gridColumn3.Caption = "Ünvan";
            this.gridColumn3.FieldName = "Unvan";
            this.gridColumn3.Name = "gridColumn3";
            this.gridColumn3.Visible = true;
            this.gridColumn3.VisibleIndex = 2;
            // 
            // gridColumn4
            // 
            this.gridColumn4.Caption = "Adres ID";
            this.gridColumn4.FieldName = "AdresID";
            this.gridColumn4.Name = "gridColumn4";
            this.gridColumn4.Visible = true;
            this.gridColumn4.VisibleIndex = 3;
            // 
            // gridColumn5
            // 
            this.gridColumn5.Caption = "Adres Adı";
            this.gridColumn5.FieldName = "AdresAdi";
            this.gridColumn5.Name = "gridColumn5";
            this.gridColumn5.Visible = true;
            this.gridColumn5.VisibleIndex = 4;
            // 
            // gridColumn6
            // 
            this.gridColumn6.Caption = "İl";
            this.gridColumn6.FieldName = "İl";
            this.gridColumn6.Name = "gridColumn6";
            this.gridColumn6.Visible = true;
            this.gridColumn6.VisibleIndex = 5;
            // 
            // gridColumn7
            // 
            this.gridColumn7.Caption = "İlçe";
            this.gridColumn7.FieldName = "İlce";
            this.gridColumn7.Name = "gridColumn7";
            this.gridColumn7.Visible = true;
            this.gridColumn7.VisibleIndex = 6;
            // 
            // gridColumn8
            // 
            this.gridColumn8.Caption = "Müşteriden Gelen";
            this.gridColumn8.FieldName = "Apak";
            this.gridColumn8.Name = "gridColumn8";
            this.gridColumn8.Visible = true;
            this.gridColumn8.VisibleIndex = 7;
            // 
            // tblEslesmelerleTableAdapter
            // 
            this.tblEslesmelerleTableAdapter.ClearBeforeFill = true;
            // 
            // ribbonControl1
            // 
            this.ribbonControl1.ExpandCollapseItem.Id = 0;
            this.ribbonControl1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.ribbonControl1.ExpandCollapseItem,
            this.BtnOnayla,
            this.BtnEslenEnUstte});
            this.ribbonControl1.Location = new System.Drawing.Point(0, 0);
            this.ribbonControl1.MaxItemId = 3;
            this.ribbonControl1.Name = "ribbonControl1";
            this.ribbonControl1.Pages.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPage[] {
            this.ribbonPage1});
            this.ribbonControl1.Size = new System.Drawing.Size(1248, 150);
            // 
            // BtnOnayla
            // 
            this.BtnOnayla.Caption = "Kaydet";
            this.BtnOnayla.Id = 1;
            this.BtnOnayla.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("BtnOnayla.ImageOptions.SvgImage")));
            this.BtnOnayla.Name = "BtnOnayla";
            this.BtnOnayla.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.BtnOnayla_ItemClick);
            // 
            // BtnEslenEnUstte
            // 
            this.BtnEslenEnUstte.Caption = "Daha Önceden Eşleşenleri Göster";
            this.BtnEslenEnUstte.Id = 2;
            this.BtnEslenEnUstte.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("BtnEslenEnUstte.ImageOptions.SvgImage")));
            this.BtnEslenEnUstte.Name = "BtnEslenEnUstte";
            this.BtnEslenEnUstte.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.BtnEslenEnUstte_ItemClick);
            // 
            // ribbonPage1
            // 
            this.ribbonPage1.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] {
            this.ribbonPageGroup1});
            this.ribbonPage1.Name = "ribbonPage1";
            this.ribbonPage1.Text = "Apak";
            // 
            // ribbonPageGroup1
            // 
            this.ribbonPageGroup1.ItemLinks.Add(this.BtnOnayla);
            this.ribbonPageGroup1.Name = "ribbonPageGroup1";
            // 
            // ApakTeslimNoktalarıEslestir
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1248, 602);
            this.Controls.Add(this.ribbonControl1);
            this.Controls.Add(this.gridControlApak);
            this.Name = "ApakTeslimNoktalarıEslestir";
            this.Text = "ApakTeslimNoktalarıEslestir";
            this.Load += new System.EventHandler(this.ApakTeslimNoktalarıEslestir_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gridControlApak)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tblEslesmelerleBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dbSiparisOtomasyonDataSet13)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn3;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn4;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn5;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn6;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn7;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn8;
        private DbSiparisOtomasyonDataSet13 dbSiparisOtomasyonDataSet13;
        private System.Windows.Forms.BindingSource tblEslesmelerleBindingSource;
        private DbSiparisOtomasyonDataSet13TableAdapters.TblEslesmelerleTableAdapter tblEslesmelerleTableAdapter;
        public DevExpress.XtraGrid.GridControl gridControlApak;
        public DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraBars.Ribbon.RibbonControl ribbonControl1;
        private DevExpress.XtraBars.BarButtonItem BtnOnayla;
        private DevExpress.XtraBars.BarButtonItem BtnEslenEnUstte;
        private DevExpress.XtraBars.Ribbon.RibbonPage ribbonPage1;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup1;
    }
}