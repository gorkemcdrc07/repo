namespace SiparişOtomasyon.Sipariş_Ekranları
{
    partial class FrmAdkoTurkSiparisEkrani
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmAdkoTurkSiparisEkrani));
            this.gridControlAdko = new DevExpress.XtraGrid.GridControl();
            this.gridViewAdko = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn4 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn5 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn6 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn7 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn8 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn9 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn10 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn11 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn12 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn13 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn14 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn15 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn16 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn17 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn18 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn19 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn20 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ribbonControl1 = new DevExpress.XtraBars.Ribbon.RibbonControl();
            this.BtnVeriler = new DevExpress.XtraBars.BarButtonItem();
            this.BtnEslestir = new DevExpress.XtraBars.BarButtonItem();
            this.BtnExcel = new DevExpress.XtraBars.BarButtonItem();
            this.BtnTeslimleriGetir = new DevExpress.XtraBars.BarButtonItem();
            this.BtnDısarıCıkart = new DevExpress.XtraBars.BarButtonItem();
            this.BtnSilme = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem1 = new DevExpress.XtraBars.BarButtonItem();
            this.BtnEslestirmeler = new DevExpress.XtraBars.BarButtonItem();
            this.ribbonPage1 = new DevExpress.XtraBars.Ribbon.RibbonPage();
            this.ribbonPageGroup1 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonPageGroup2 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonPageGroup3 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonPageGroup4 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.gridControlAdko)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewAdko)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).BeginInit();
            this.SuspendLayout();
            // 
            // gridControlAdko
            // 
            this.gridControlAdko.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControlAdko.Location = new System.Drawing.Point(0, 150);
            this.gridControlAdko.MainView = this.gridViewAdko;
            this.gridControlAdko.Name = "gridControlAdko";
            this.gridControlAdko.Size = new System.Drawing.Size(1248, 452);
            this.gridControlAdko.TabIndex = 0;
            this.gridControlAdko.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridViewAdko});
            // 
            // gridViewAdko
            // 
            this.gridViewAdko.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn1,
            this.gridColumn2,
            this.gridColumn3,
            this.gridColumn4,
            this.gridColumn5,
            this.gridColumn6,
            this.gridColumn7,
            this.gridColumn8,
            this.gridColumn9,
            this.gridColumn10,
            this.gridColumn11,
            this.gridColumn12,
            this.gridColumn13,
            this.gridColumn14,
            this.gridColumn15,
            this.gridColumn16,
            this.gridColumn17,
            this.gridColumn18,
            this.gridColumn19,
            this.gridColumn20});
            this.gridViewAdko.GridControl = this.gridControlAdko;
            this.gridViewAdko.Name = "gridViewAdko";
            this.gridViewAdko.OptionsView.ShowGroupPanel = false;
            // 
            // gridColumn1
            // 
            this.gridColumn1.Caption = "Müşteri VKN";
            this.gridColumn1.FieldName = "MusteriVkn";
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.OptionsColumn.AllowEdit = false;
            this.gridColumn1.Visible = true;
            this.gridColumn1.VisibleIndex = 0;
            this.gridColumn1.Width = 93;
            // 
            // gridColumn2
            // 
            this.gridColumn2.Caption = "Proje";
            this.gridColumn2.FieldName = "Proje";
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.OptionsColumn.AllowEdit = false;
            this.gridColumn2.Visible = true;
            this.gridColumn2.VisibleIndex = 1;
            this.gridColumn2.Width = 93;
            // 
            // gridColumn3
            // 
            this.gridColumn3.Caption = "Sipariş Tarihi";
            this.gridColumn3.FieldName = "SiparisTarihi";
            this.gridColumn3.Name = "gridColumn3";
            this.gridColumn3.OptionsColumn.AllowEdit = false;
            this.gridColumn3.Visible = true;
            this.gridColumn3.VisibleIndex = 2;
            this.gridColumn3.Width = 93;
            // 
            // gridColumn4
            // 
            this.gridColumn4.Caption = "Yükleme Tarihi";
            this.gridColumn4.FieldName = "YuklemeTarihi";
            this.gridColumn4.Name = "gridColumn4";
            this.gridColumn4.OptionsColumn.AllowEdit = false;
            this.gridColumn4.Visible = true;
            this.gridColumn4.VisibleIndex = 3;
            this.gridColumn4.Width = 93;
            // 
            // gridColumn5
            // 
            this.gridColumn5.Caption = "Teslim Tarihi";
            this.gridColumn5.FieldName = "TeslimTarihi";
            this.gridColumn5.Name = "gridColumn5";
            this.gridColumn5.OptionsColumn.AllowEdit = false;
            this.gridColumn5.Visible = true;
            this.gridColumn5.VisibleIndex = 4;
            this.gridColumn5.Width = 93;
            // 
            // gridColumn6
            // 
            this.gridColumn6.Caption = "Müşteri Sipariş No";
            this.gridColumn6.FieldName = "MusteriSiparisNo";
            this.gridColumn6.Name = "gridColumn6";
            this.gridColumn6.OptionsColumn.AllowEdit = false;
            this.gridColumn6.Visible = true;
            this.gridColumn6.VisibleIndex = 5;
            this.gridColumn6.Width = 93;
            // 
            // gridColumn7
            // 
            this.gridColumn7.Caption = "Müşteri Referans No";
            this.gridColumn7.FieldName = "MusteriReferansNo";
            this.gridColumn7.Name = "gridColumn7";
            this.gridColumn7.OptionsColumn.AllowEdit = false;
            this.gridColumn7.Visible = true;
            this.gridColumn7.VisibleIndex = 6;
            this.gridColumn7.Width = 142;
            // 
            // gridColumn8
            // 
            this.gridColumn8.Caption = "İstenilen Araç Tipi";
            this.gridColumn8.FieldName = "İstenilenAracTipi";
            this.gridColumn8.Name = "gridColumn8";
            this.gridColumn8.OptionsColumn.AllowEdit = false;
            this.gridColumn8.Visible = true;
            this.gridColumn8.VisibleIndex = 7;
            this.gridColumn8.Width = 135;
            // 
            // gridColumn9
            // 
            this.gridColumn9.Caption = "Açıklama";
            this.gridColumn9.FieldName = "Aciklama";
            this.gridColumn9.Name = "gridColumn9";
            this.gridColumn9.OptionsColumn.AllowEdit = false;
            this.gridColumn9.Visible = true;
            this.gridColumn9.VisibleIndex = 8;
            this.gridColumn9.Width = 85;
            // 
            // gridColumn10
            // 
            this.gridColumn10.Caption = "Yükleme Firması Adres Adı";
            this.gridColumn10.FieldName = "YuklemeFirmasıAdresAdı";
            this.gridColumn10.Name = "gridColumn10";
            this.gridColumn10.OptionsColumn.AllowEdit = false;
            this.gridColumn10.Visible = true;
            this.gridColumn10.VisibleIndex = 9;
            this.gridColumn10.Width = 147;
            // 
            // gridColumn11
            // 
            this.gridColumn11.Caption = "Alıcı Firma Cari Ünvanı";
            this.gridColumn11.FieldName = "AlıcıFirmaCariUnvanı";
            this.gridColumn11.Name = "gridColumn11";
            this.gridColumn11.OptionsColumn.AllowEdit = false;
            this.gridColumn11.Visible = true;
            this.gridColumn11.VisibleIndex = 10;
            this.gridColumn11.Width = 136;
            // 
            // gridColumn12
            // 
            this.gridColumn12.Caption = "Teslim Firma Adres Adı";
            this.gridColumn12.FieldName = "TeslimFirmaAdresAdı";
            this.gridColumn12.Name = "gridColumn12";
            this.gridColumn12.Visible = true;
            this.gridColumn12.VisibleIndex = 11;
            this.gridColumn12.Width = 130;
            // 
            // gridColumn13
            // 
            this.gridColumn13.Caption = "İrsaliye No";
            this.gridColumn13.FieldName = "İrsaliyeNo";
            this.gridColumn13.Name = "gridColumn13";
            this.gridColumn13.OptionsColumn.AllowEdit = false;
            this.gridColumn13.Visible = true;
            this.gridColumn13.VisibleIndex = 12;
            this.gridColumn13.Width = 71;
            // 
            // gridColumn14
            // 
            this.gridColumn14.Caption = "İrsaliye Miktar";
            this.gridColumn14.FieldName = "İrsaliyeMiktar";
            this.gridColumn14.Name = "gridColumn14";
            this.gridColumn14.OptionsColumn.AllowEdit = false;
            this.gridColumn14.Visible = true;
            this.gridColumn14.VisibleIndex = 13;
            this.gridColumn14.Width = 107;
            // 
            // gridColumn15
            // 
            this.gridColumn15.Caption = "Ürün";
            this.gridColumn15.FieldName = "Urun";
            this.gridColumn15.Name = "gridColumn15";
            this.gridColumn15.OptionsColumn.AllowEdit = false;
            this.gridColumn15.Visible = true;
            this.gridColumn15.VisibleIndex = 14;
            this.gridColumn15.Width = 71;
            // 
            // gridColumn16
            // 
            this.gridColumn16.Caption = "Kap Adet";
            this.gridColumn16.FieldName = "KapAdet";
            this.gridColumn16.Name = "gridColumn16";
            this.gridColumn16.OptionsColumn.AllowEdit = false;
            this.gridColumn16.Visible = true;
            this.gridColumn16.VisibleIndex = 15;
            this.gridColumn16.Width = 71;
            // 
            // gridColumn17
            // 
            this.gridColumn17.Caption = "Ambalaj Tipi";
            this.gridColumn17.FieldName = "AmbalajTipi";
            this.gridColumn17.Name = "gridColumn17";
            this.gridColumn17.OptionsColumn.AllowEdit = false;
            this.gridColumn17.Visible = true;
            this.gridColumn17.VisibleIndex = 16;
            this.gridColumn17.Width = 71;
            // 
            // gridColumn18
            // 
            this.gridColumn18.Caption = "Brüt KG";
            this.gridColumn18.FieldName = "BrutKG";
            this.gridColumn18.Name = "gridColumn18";
            this.gridColumn18.OptionsColumn.AllowEdit = false;
            this.gridColumn18.Visible = true;
            this.gridColumn18.VisibleIndex = 17;
            this.gridColumn18.Width = 71;
            // 
            // gridColumn19
            // 
            this.gridColumn19.Caption = "M3";
            this.gridColumn19.FieldName = "M3";
            this.gridColumn19.Name = "gridColumn19";
            this.gridColumn19.OptionsColumn.AllowEdit = false;
            this.gridColumn19.Visible = true;
            this.gridColumn19.VisibleIndex = 18;
            this.gridColumn19.Width = 71;
            // 
            // gridColumn20
            // 
            this.gridColumn20.Caption = "Desi";
            this.gridColumn20.FieldName = "Desi";
            this.gridColumn20.Name = "gridColumn20";
            this.gridColumn20.OptionsColumn.AllowEdit = false;
            this.gridColumn20.Visible = true;
            this.gridColumn20.VisibleIndex = 19;
            this.gridColumn20.Width = 108;
            // 
            // ribbonControl1
            // 
            this.ribbonControl1.ExpandCollapseItem.Id = 0;
            this.ribbonControl1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.ribbonControl1.ExpandCollapseItem,
            this.BtnVeriler,
            this.BtnEslestir,
            this.BtnExcel,
            this.BtnTeslimleriGetir,
            this.BtnDısarıCıkart,
            this.BtnSilme,
            this.barButtonItem1,
            this.BtnEslestirmeler});
            this.ribbonControl1.Location = new System.Drawing.Point(0, 0);
            this.ribbonControl1.MaxItemId = 9;
            this.ribbonControl1.Name = "ribbonControl1";
            this.ribbonControl1.Pages.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPage[] {
            this.ribbonPage1});
            this.ribbonControl1.Size = new System.Drawing.Size(1248, 150);
            // 
            // BtnVeriler
            // 
            this.BtnVeriler.Caption = "verileri Getir";
            this.BtnVeriler.Id = 1;
            this.BtnVeriler.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("BtnVeriler.ImageOptions.SvgImage")));
            this.BtnVeriler.Name = "BtnVeriler";
            // 
            // BtnEslestir
            // 
            this.BtnEslestir.Caption = "Teslim Noktası Eşleştir";
            this.BtnEslestir.Id = 2;
            this.BtnEslestir.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("BtnEslestir.ImageOptions.SvgImage")));
            this.BtnEslestir.Name = "BtnEslestir";
            this.BtnEslestir.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.BtnEslestir_ItemClick);
            // 
            // BtnExcel
            // 
            this.BtnExcel.Caption = "Teslim Noktaları";
            this.BtnExcel.Id = 3;
            this.BtnExcel.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("BtnExcel.ImageOptions.SvgImage")));
            this.BtnExcel.Name = "BtnExcel";
            this.BtnExcel.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.BtnExcel_ItemClick);
            // 
            // BtnTeslimleriGetir
            // 
            this.BtnTeslimleriGetir.Caption = "Teslim Noktalarını Getir";
            this.BtnTeslimleriGetir.Id = 4;
            this.BtnTeslimleriGetir.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("BtnTeslimleriGetir.ImageOptions.SvgImage")));
            this.BtnTeslimleriGetir.Name = "BtnTeslimleriGetir";
            this.BtnTeslimleriGetir.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.BtnTeslimleriGetir_ItemClick);
            // 
            // BtnDısarıCıkart
            // 
            this.BtnDısarıCıkart.Caption = "Excel\'e Aktar";
            this.BtnDısarıCıkart.Id = 5;
            this.BtnDısarıCıkart.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("BtnDısarıCıkart.ImageOptions.SvgImage")));
            this.BtnDısarıCıkart.Name = "BtnDısarıCıkart";
            this.BtnDısarıCıkart.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.BtnDısarıCıkart_ItemClick);
            // 
            // BtnSilme
            // 
            this.BtnSilme.Caption = "Temizle";
            this.BtnSilme.Id = 6;
            this.BtnSilme.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("BtnSilme.ImageOptions.SvgImage")));
            this.BtnSilme.Name = "BtnSilme";
            this.BtnSilme.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.BtnSilme_ItemClick);
            // 
            // barButtonItem1
            // 
            this.barButtonItem1.Caption = "barButtonItem1";
            this.barButtonItem1.Id = 7;
            this.barButtonItem1.Name = "barButtonItem1";
            // 
            // BtnEslestirmeler
            // 
            this.BtnEslestirmeler.Caption = "Eşleşmeyi Başlat";
            this.BtnEslestirmeler.Id = 8;
            this.BtnEslestirmeler.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("BtnEslestirmeler.ImageOptions.SvgImage")));
            this.BtnEslestirmeler.Name = "BtnEslestirmeler";
            this.BtnEslestirmeler.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.BtnEslestirmeler_ItemClick);
            // 
            // ribbonPage1
            // 
            this.ribbonPage1.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] {
            this.ribbonPageGroup1,
            this.ribbonPageGroup2,
            this.ribbonPageGroup3,
            this.ribbonPageGroup4});
            this.ribbonPage1.Name = "ribbonPage1";
            this.ribbonPage1.Text = "ADKOTÜRK";
            // 
            // ribbonPageGroup1
            // 
            this.ribbonPageGroup1.ItemLinks.Add(this.BtnEslestir);
            this.ribbonPageGroup1.Name = "ribbonPageGroup1";
            // 
            // ribbonPageGroup2
            // 
            this.ribbonPageGroup2.ItemLinks.Add(this.BtnTeslimleriGetir);
            this.ribbonPageGroup2.ItemLinks.Add(this.BtnEslestirmeler);
            this.ribbonPageGroup2.Name = "ribbonPageGroup2";
            // 
            // ribbonPageGroup3
            // 
            this.ribbonPageGroup3.ItemLinks.Add(this.BtnDısarıCıkart);
            this.ribbonPageGroup3.Name = "ribbonPageGroup3";
            // 
            // ribbonPageGroup4
            // 
            this.ribbonPageGroup4.ItemLinks.Add(this.BtnSilme);
            this.ribbonPageGroup4.Name = "ribbonPageGroup4";
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.BackColor = System.Drawing.Color.Gainsboro;
            this.labelControl1.Appearance.ForeColor = System.Drawing.Color.ForestGreen;
            this.labelControl1.Appearance.Options.UseBackColor = true;
            this.labelControl1.Appearance.Options.UseForeColor = true;
            this.labelControl1.Location = new System.Drawing.Point(1099, 87);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(63, 13);
            this.labelControl1.TabIndex = 9;
            this.labelControl1.Text = "labelControl1";
            // 
            // FrmAdkoTurkSiparisEkrani
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1248, 602);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.gridControlAdko);
            this.Controls.Add(this.ribbonControl1);
            this.Name = "FrmAdkoTurkSiparisEkrani";
            this.Text = "ADKOTÜRK SİPARİŞ EKRANI";
            this.Load += new System.EventHandler(this.FrmAdkoTurkSiparisEkrani_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gridControlAdko)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewAdko)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private DevExpress.XtraGrid.Views.Grid.GridView gridViewAdko;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn3;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn4;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn5;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn6;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn7;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn8;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn9;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn10;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn11;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn12;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn13;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn14;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn15;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn16;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn17;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn18;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn19;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn20;
        private DevExpress.XtraBars.Ribbon.RibbonControl ribbonControl1;
        private DevExpress.XtraBars.Ribbon.RibbonPage ribbonPage1;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup1;
        private DevExpress.XtraBars.BarButtonItem BtnVeriler;
        private DevExpress.XtraBars.BarButtonItem BtnEslestir;
        private DevExpress.XtraBars.BarButtonItem BtnExcel;
        public DevExpress.XtraGrid.GridControl gridControlAdko;
        private DevExpress.XtraBars.BarButtonItem BtnTeslimleriGetir;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup2;
        private DevExpress.XtraBars.BarButtonItem BtnDısarıCıkart;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup3;
        private DevExpress.XtraBars.BarButtonItem BtnSilme;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup4;
        private DevExpress.XtraBars.BarButtonItem barButtonItem1;
        private DevExpress.XtraBars.BarButtonItem BtnEslestirmeler;
        private DevExpress.XtraEditors.LabelControl labelControl1;
    }
}