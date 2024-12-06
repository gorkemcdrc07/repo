namespace SiparişOtomasyon.Yükleme_Noktaları
{
    partial class HedefYuklemeNoktaları
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HedefYuklemeNoktaları));
            this.ribbonControl1 = new DevExpress.XtraBars.Ribbon.RibbonControl();
            this.BtnOnayla = new DevExpress.XtraBars.BarButtonItem();
            this.BtnEslenEnUstte = new DevExpress.XtraBars.BarButtonItem();
            this.BtnKaydet = new DevExpress.XtraBars.BarButtonItem();
            this.ribbonPage1 = new DevExpress.XtraBars.Ribbon.RibbonPage();
            this.ribbonPageGroup1 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn4 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn5 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.hedefBindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.dbSiparisOtomasyonDataSet42 = new SiparişOtomasyon.DbSiparisOtomasyonDataSet42();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn6 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn7 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn8 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn9 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn10 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn11 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.hedefTableAdapter1 = new SiparişOtomasyon.DbSiparisOtomasyonDataSet42TableAdapters.HedefTableAdapter();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.hedefBindingSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dbSiparisOtomasyonDataSet42)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // ribbonControl1
            // 
            this.ribbonControl1.ExpandCollapseItem.Id = 0;
            this.ribbonControl1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.ribbonControl1.ExpandCollapseItem,
            this.BtnOnayla,
            this.BtnEslenEnUstte,
            this.BtnKaydet});
            this.ribbonControl1.Location = new System.Drawing.Point(0, 0);
            this.ribbonControl1.MaxItemId = 4;
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
            // 
            // BtnEslenEnUstte
            // 
            this.BtnEslenEnUstte.Caption = "Daha Önceden Eşleşenleri Göster";
            this.BtnEslenEnUstte.Id = 2;
            this.BtnEslenEnUstte.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("BtnEslenEnUstte.ImageOptions.SvgImage")));
            this.BtnEslenEnUstte.Name = "BtnEslenEnUstte";
            // 
            // BtnKaydet
            // 
            this.BtnKaydet.Caption = "Kaydet";
            this.BtnKaydet.Id = 3;
            this.BtnKaydet.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("BtnKaydet.ImageOptions.SvgImage")));
            this.BtnKaydet.Name = "BtnKaydet";
            this.BtnKaydet.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.BtnKaydet_ItemClick);
            // 
            // ribbonPage1
            // 
            this.ribbonPage1.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] {
            this.ribbonPageGroup1});
            this.ribbonPage1.Name = "ribbonPage1";
            this.ribbonPage1.Text = "HEDEF";
            // 
            // ribbonPageGroup1
            // 
            this.ribbonPageGroup1.ItemLinks.Add(this.BtnKaydet);
            this.ribbonPageGroup1.Name = "ribbonPageGroup1";
            // 
            // gridColumn2
            // 
            this.gridColumn2.Caption = "Adres Adı";
            this.gridColumn2.FieldName = "AdresAdi";
            this.gridColumn2.Name = "gridColumn2";
            // 
            // gridColumn1
            // 
            this.gridColumn1.Caption = "Adres ID";
            this.gridColumn1.FieldName = "AdresID";
            this.gridColumn1.Name = "gridColumn1";
            // 
            // gridColumn3
            // 
            this.gridColumn3.Caption = "İl";
            this.gridColumn3.FieldName = "İl";
            this.gridColumn3.Name = "gridColumn3";
            // 
            // gridColumn4
            // 
            this.gridColumn4.Caption = "İlçe";
            this.gridColumn4.FieldName = "İlce";
            this.gridColumn4.Name = "gridColumn4";
            // 
            // gridColumn5
            // 
            this.gridColumn5.Caption = "Müşteriden Gelen";
            this.gridColumn5.FieldName = "Tanımla";
            this.gridColumn5.Name = "gridColumn5";
            // 
            // gridControl1
            // 
            this.gridControl1.DataSource = this.hedefBindingSource1;
            this.gridControl1.Location = new System.Drawing.Point(0, 156);
            this.gridControl1.MainView = this.gridView1;
            this.gridControl1.MenuManager = this.ribbonControl1;
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.Size = new System.Drawing.Size(1248, 434);
            this.gridControl1.TabIndex = 4;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // hedefBindingSource1
            // 
            this.hedefBindingSource1.DataMember = "Hedef";
            this.hedefBindingSource1.DataSource = this.dbSiparisOtomasyonDataSet42;
            // 
            // dbSiparisOtomasyonDataSet42
            // 
            this.dbSiparisOtomasyonDataSet42.DataSetName = "DbSiparisOtomasyonDataSet42";
            this.dbSiparisOtomasyonDataSet42.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // gridView1
            // 
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn6,
            this.gridColumn7,
            this.gridColumn8,
            this.gridColumn9,
            this.gridColumn10,
            this.gridColumn11});
            this.gridView1.GridControl = this.gridControl1;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsView.ShowGroupPanel = false;
            // 
            // gridColumn6
            // 
            this.gridColumn6.Caption = "Ünvan";
            this.gridColumn6.FieldName = "Unvan";
            this.gridColumn6.Name = "gridColumn6";
            this.gridColumn6.Visible = true;
            this.gridColumn6.VisibleIndex = 0;
            // 
            // gridColumn7
            // 
            this.gridColumn7.Caption = "Adres Adı";
            this.gridColumn7.FieldName = "AdresAdı";
            this.gridColumn7.Name = "gridColumn7";
            this.gridColumn7.Visible = true;
            this.gridColumn7.VisibleIndex = 1;
            // 
            // gridColumn8
            // 
            this.gridColumn8.Caption = "Adres ID";
            this.gridColumn8.FieldName = "AdresID";
            this.gridColumn8.Name = "gridColumn8";
            this.gridColumn8.Visible = true;
            this.gridColumn8.VisibleIndex = 2;
            // 
            // gridColumn9
            // 
            this.gridColumn9.Caption = "İl";
            this.gridColumn9.FieldName = "İl";
            this.gridColumn9.Name = "gridColumn9";
            this.gridColumn9.Visible = true;
            this.gridColumn9.VisibleIndex = 3;
            // 
            // gridColumn10
            // 
            this.gridColumn10.Caption = "İlçe";
            this.gridColumn10.FieldName = "İlce";
            this.gridColumn10.Name = "gridColumn10";
            this.gridColumn10.Visible = true;
            this.gridColumn10.VisibleIndex = 4;
            // 
            // gridColumn11
            // 
            this.gridColumn11.Caption = "Müşteriden Gelen";
            this.gridColumn11.FieldName = "Tanımla";
            this.gridColumn11.Name = "gridColumn11";
            this.gridColumn11.Visible = true;
            this.gridColumn11.VisibleIndex = 5;
            // 
            // hedefTableAdapter1
            // 
            this.hedefTableAdapter1.ClearBeforeFill = true;
            // 
            // HedefYuklemeNoktaları
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1248, 602);
            this.Controls.Add(this.gridControl1);
            this.Controls.Add(this.ribbonControl1);
            this.Name = "HedefYuklemeNoktaları";
            this.Text = "HedefYuklemeNoktaları";
            this.Load += new System.EventHandler(this.HedefYuklemeNoktaları_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.hedefBindingSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dbSiparisOtomasyonDataSet42)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraBars.Ribbon.RibbonControl ribbonControl1;
        private DevExpress.XtraBars.BarButtonItem BtnOnayla;
        private DevExpress.XtraBars.BarButtonItem BtnEslenEnUstte;
        private DevExpress.XtraBars.BarButtonItem BtnKaydet;
        private DevExpress.XtraBars.Ribbon.RibbonPage ribbonPage1;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn3;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn4;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn5;
        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn6;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn7;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn8;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn9;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn10;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn11;
        private DbSiparisOtomasyonDataSet42 dbSiparisOtomasyonDataSet42;
        private System.Windows.Forms.BindingSource hedefBindingSource1;
        private DbSiparisOtomasyonDataSet42TableAdapters.HedefTableAdapter hedefTableAdapter1;
    }
}