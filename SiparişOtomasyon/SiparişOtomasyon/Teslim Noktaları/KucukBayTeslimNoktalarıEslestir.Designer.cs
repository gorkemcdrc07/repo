namespace SiparişOtomasyon.Teslim_Noktaları
{
    partial class KucukBayTeslimNoktalarıEslestir
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(KucukBayTeslimNoktalarıEslestir));
            this.ribbonControl1 = new DevExpress.XtraBars.Ribbon.RibbonControl();
            this.BtnOnayla = new DevExpress.XtraBars.BarButtonItem();
            this.BtnEslenEnUstte = new DevExpress.XtraBars.BarButtonItem();
            this.ribbonPage1 = new DevExpress.XtraBars.Ribbon.RibbonPage();
            this.ribbonPageGroup1 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.tblEslesmelerleTableAdapter1 = new SiparişOtomasyon.DbSiparisOtomasyonDataSet36TableAdapters.TblEslesmelerleTableAdapter();
            this.gridViewKucukBayEslesme = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn4 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn5 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn6 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn7 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn8 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridControlKucukBayEslesme = new DevExpress.XtraGrid.GridControl();
            this.tblEslesmelerleBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.dbSiparisOtomasyonDataSet38 = new SiparişOtomasyon.DbSiparisOtomasyonDataSet38();
            this.tblEslesmelerleTableAdapter = new SiparişOtomasyon.DbSiparisOtomasyonDataSet38TableAdapters.TblEslesmelerleTableAdapter();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewKucukBayEslesme)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControlKucukBayEslesme)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tblEslesmelerleBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dbSiparisOtomasyonDataSet38)).BeginInit();
            this.SuspendLayout();
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
            // 
            // ribbonPage1
            // 
            this.ribbonPage1.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] {
            this.ribbonPageGroup1});
            this.ribbonPage1.Name = "ribbonPage1";
            this.ribbonPage1.Text = "KÜÇÜKBAY";
            // 
            // ribbonPageGroup1
            // 
            this.ribbonPageGroup1.ItemLinks.Add(this.BtnOnayla);
            this.ribbonPageGroup1.ItemLinks.Add(this.BtnEslenEnUstte);
            this.ribbonPageGroup1.Name = "ribbonPageGroup1";
            // 
            // tblEslesmelerleTableAdapter1
            // 
            this.tblEslesmelerleTableAdapter1.ClearBeforeFill = true;
            // 
            // gridViewKucukBayEslesme
            // 
            this.gridViewKucukBayEslesme.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn1,
            this.gridColumn2,
            this.gridColumn3,
            this.gridColumn4,
            this.gridColumn5,
            this.gridColumn6,
            this.gridColumn7,
            this.gridColumn8});
            this.gridViewKucukBayEslesme.GridControl = this.gridControlKucukBayEslesme;
            this.gridViewKucukBayEslesme.Name = "gridViewKucukBayEslesme";
            this.gridViewKucukBayEslesme.OptionsView.ShowGroupPanel = false;
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
            this.gridColumn8.FieldName = "KucukBay";
            this.gridColumn8.Name = "gridColumn8";
            this.gridColumn8.Visible = true;
            this.gridColumn8.VisibleIndex = 7;
            // 
            // gridControlKucukBayEslesme
            // 
            this.gridControlKucukBayEslesme.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridControlKucukBayEslesme.DataSource = this.tblEslesmelerleBindingSource;
            this.gridControlKucukBayEslesme.Location = new System.Drawing.Point(0, 156);
            this.gridControlKucukBayEslesme.MainView = this.gridViewKucukBayEslesme;
            this.gridControlKucukBayEslesme.MenuManager = this.ribbonControl1;
            this.gridControlKucukBayEslesme.Name = "gridControlKucukBayEslesme";
            this.gridControlKucukBayEslesme.Size = new System.Drawing.Size(1248, 434);
            this.gridControlKucukBayEslesme.TabIndex = 2;
            this.gridControlKucukBayEslesme.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridViewKucukBayEslesme});
            // 
            // tblEslesmelerleBindingSource
            // 
            this.tblEslesmelerleBindingSource.DataMember = "TblEslesmelerle";
            this.tblEslesmelerleBindingSource.DataSource = this.dbSiparisOtomasyonDataSet38;
            // 
            // dbSiparisOtomasyonDataSet38
            // 
            this.dbSiparisOtomasyonDataSet38.DataSetName = "DbSiparisOtomasyonDataSet38";
            this.dbSiparisOtomasyonDataSet38.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // tblEslesmelerleTableAdapter
            // 
            this.tblEslesmelerleTableAdapter.ClearBeforeFill = true;
            // 
            // KucukBayTeslimNoktalarıEslestir
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1248, 602);
            this.Controls.Add(this.gridControlKucukBayEslesme);
            this.Controls.Add(this.ribbonControl1);
            this.Name = "KucukBayTeslimNoktalarıEslestir";
            this.Text = "KucukBayTeslimNoktalarıEslestir";
            this.Load += new System.EventHandler(this.KucukBayTeslimNoktalarıEslestir_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewKucukBayEslesme)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControlKucukBayEslesme)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tblEslesmelerleBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dbSiparisOtomasyonDataSet38)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraBars.Ribbon.RibbonControl ribbonControl1;
        private DevExpress.XtraBars.BarButtonItem BtnOnayla;
        private DevExpress.XtraBars.BarButtonItem BtnEslenEnUstte;
        private DevExpress.XtraBars.Ribbon.RibbonPage ribbonPage1;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup1;
        private DbSiparisOtomasyonDataSet36TableAdapters.TblEslesmelerleTableAdapter tblEslesmelerleTableAdapter1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn3;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn4;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn5;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn6;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn7;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn8;
        private DbSiparisOtomasyonDataSet38 dbSiparisOtomasyonDataSet38;
        private System.Windows.Forms.BindingSource tblEslesmelerleBindingSource;
        private DbSiparisOtomasyonDataSet38TableAdapters.TblEslesmelerleTableAdapter tblEslesmelerleTableAdapter;
        public DevExpress.XtraGrid.GridControl gridControlKucukBayEslesme;
        public DevExpress.XtraGrid.Views.Grid.GridView gridViewKucukBayEslesme;
    }
}