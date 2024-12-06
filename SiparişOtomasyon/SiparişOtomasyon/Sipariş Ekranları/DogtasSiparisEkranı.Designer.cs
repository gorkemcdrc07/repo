namespace SiparişOtomasyon.Sipariş_Ekranları
{
    partial class DogtasSiparisEkranı
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DogtasSiparisEkranı));
            this.ribbonControl1 = new DevExpress.XtraBars.Ribbon.RibbonControl();
            this.BtnVeriler = new DevExpress.XtraBars.BarButtonItem();
            this.BtnEslestir = new DevExpress.XtraBars.BarButtonItem();
            this.BtnExcel = new DevExpress.XtraBars.BarButtonItem();
            this.BtnTeslimleriGetir = new DevExpress.XtraBars.BarButtonItem();
            this.BtnDısarıCıkart = new DevExpress.XtraBars.BarButtonItem();
            this.BtnSilme = new DevExpress.XtraBars.BarButtonItem();
            this.BtnEslesme = new DevExpress.XtraBars.BarButtonItem();
            this.BtnBosHucreSil = new DevExpress.XtraBars.BarButtonItem();
            this.BtnYuklemeFirma = new DevExpress.XtraBars.BarButtonItem();
            this.ribbonPage1 = new DevExpress.XtraBars.Ribbon.RibbonPage();
            this.ribbonPageGroup1 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonPageGroup2 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonPageGroup3 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonPageGroup4 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.gridControlDogtas = new DevExpress.XtraGrid.GridControl();
            this.gridViewDogtas = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn21 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn22 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn23 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn24 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn25 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn26 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn27 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn28 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn29 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn30 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn31 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn32 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn33 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn34 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn35 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn36 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn37 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn38 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn39 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn40 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControlDogtas)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewDogtas)).BeginInit();
            this.SuspendLayout();
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
            this.BtnEslesme,
            this.BtnBosHucreSil,
            this.BtnYuklemeFirma});
            this.ribbonControl1.Location = new System.Drawing.Point(0, 0);
            this.ribbonControl1.MaxItemId = 10;
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
            // BtnEslesme
            // 
            this.BtnEslesme.Caption = "Eşleşmeyi Başlat";
            this.BtnEslesme.Id = 7;
            this.BtnEslesme.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("BtnEslesme.ImageOptions.SvgImage")));
            this.BtnEslesme.Name = "BtnEslesme";
            this.BtnEslesme.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.BtnEslesme_ItemClick);
            // 
            // BtnBosHucreSil
            // 
            this.BtnBosHucreSil.Caption = "Boş Hücre Sil";
            this.BtnBosHucreSil.Id = 8;
            this.BtnBosHucreSil.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("BtnBosHucreSil.ImageOptions.SvgImage")));
            this.BtnBosHucreSil.Name = "BtnBosHucreSil";
            // 
            // BtnYuklemeFirma
            // 
            this.BtnYuklemeFirma.Caption = "Yükleme Firma";
            this.BtnYuklemeFirma.Id = 9;
            this.BtnYuklemeFirma.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("BtnYuklemeFirma.ImageOptions.SvgImage")));
            this.BtnYuklemeFirma.Name = "BtnYuklemeFirma";
            this.BtnYuklemeFirma.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.BtnYuklemeFirma_ItemClick);
            // 
            // ribbonPage1
            // 
            this.ribbonPage1.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] {
            this.ribbonPageGroup1,
            this.ribbonPageGroup2,
            this.ribbonPageGroup3,
            this.ribbonPageGroup4});
            this.ribbonPage1.Name = "ribbonPage1";
            this.ribbonPage1.Text = "DOĞTAŞ";
            // 
            // ribbonPageGroup1
            // 
            this.ribbonPageGroup1.ItemLinks.Add(this.BtnEslestir);
            this.ribbonPageGroup1.Name = "ribbonPageGroup1";
            // 
            // ribbonPageGroup2
            // 
            this.ribbonPageGroup2.ItemLinks.Add(this.BtnTeslimleriGetir);
            this.ribbonPageGroup2.ItemLinks.Add(this.BtnEslesme);
            this.ribbonPageGroup2.ItemLinks.Add(this.BtnYuklemeFirma);
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
            // gridControlDogtas
            // 
            this.gridControlDogtas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControlDogtas.Location = new System.Drawing.Point(0, 150);
            this.gridControlDogtas.MainView = this.gridViewDogtas;
            this.gridControlDogtas.Name = "gridControlDogtas";
            this.gridControlDogtas.Size = new System.Drawing.Size(1248, 452);
            this.gridControlDogtas.TabIndex = 9;
            this.gridControlDogtas.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridViewDogtas});
            // 
            // gridViewDogtas
            // 
            this.gridViewDogtas.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn21,
            this.gridColumn22,
            this.gridColumn23,
            this.gridColumn24,
            this.gridColumn25,
            this.gridColumn26,
            this.gridColumn27,
            this.gridColumn28,
            this.gridColumn29,
            this.gridColumn30,
            this.gridColumn31,
            this.gridColumn32,
            this.gridColumn33,
            this.gridColumn34,
            this.gridColumn35,
            this.gridColumn36,
            this.gridColumn37,
            this.gridColumn38,
            this.gridColumn39,
            this.gridColumn40});
            this.gridViewDogtas.GridControl = this.gridControlDogtas;
            this.gridViewDogtas.Name = "gridViewDogtas";
            this.gridViewDogtas.OptionsView.ShowGroupPanel = false;
            // 
            // gridColumn21
            // 
            this.gridColumn21.Caption = "Müşteri VKN";
            this.gridColumn21.FieldName = "MusteriVkn";
            this.gridColumn21.Name = "gridColumn21";
            this.gridColumn21.Visible = true;
            this.gridColumn21.VisibleIndex = 0;
            this.gridColumn21.Width = 93;
            // 
            // gridColumn22
            // 
            this.gridColumn22.Caption = "Proje";
            this.gridColumn22.FieldName = "Proje";
            this.gridColumn22.Name = "gridColumn22";
            this.gridColumn22.Visible = true;
            this.gridColumn22.VisibleIndex = 1;
            this.gridColumn22.Width = 93;
            // 
            // gridColumn23
            // 
            this.gridColumn23.Caption = "Sipariş Tarihi";
            this.gridColumn23.FieldName = "SiparisTarihi";
            this.gridColumn23.Name = "gridColumn23";
            this.gridColumn23.Visible = true;
            this.gridColumn23.VisibleIndex = 2;
            this.gridColumn23.Width = 93;
            // 
            // gridColumn24
            // 
            this.gridColumn24.Caption = "Yükleme Tarihi";
            this.gridColumn24.FieldName = "YuklemeTarihi";
            this.gridColumn24.Name = "gridColumn24";
            this.gridColumn24.Visible = true;
            this.gridColumn24.VisibleIndex = 3;
            this.gridColumn24.Width = 93;
            // 
            // gridColumn25
            // 
            this.gridColumn25.Caption = "Teslim Tarihi";
            this.gridColumn25.FieldName = "TeslimTarihi";
            this.gridColumn25.Name = "gridColumn25";
            this.gridColumn25.Visible = true;
            this.gridColumn25.VisibleIndex = 4;
            this.gridColumn25.Width = 93;
            // 
            // gridColumn26
            // 
            this.gridColumn26.Caption = "Müşteri Sipariş No";
            this.gridColumn26.FieldName = "MusteriSiparisNo";
            this.gridColumn26.Name = "gridColumn26";
            this.gridColumn26.Visible = true;
            this.gridColumn26.VisibleIndex = 5;
            this.gridColumn26.Width = 129;
            // 
            // gridColumn27
            // 
            this.gridColumn27.Caption = "Müşteri Referans No";
            this.gridColumn27.FieldName = "MusteriReferansNo";
            this.gridColumn27.Name = "gridColumn27";
            this.gridColumn27.Visible = true;
            this.gridColumn27.VisibleIndex = 6;
            this.gridColumn27.Width = 142;
            // 
            // gridColumn28
            // 
            this.gridColumn28.Caption = "İstenilen Araç Tipi";
            this.gridColumn28.FieldName = "İstenilenAracTipi";
            this.gridColumn28.Name = "gridColumn28";
            this.gridColumn28.Visible = true;
            this.gridColumn28.VisibleIndex = 7;
            this.gridColumn28.Width = 135;
            // 
            // gridColumn29
            // 
            this.gridColumn29.Caption = "Açıklama";
            this.gridColumn29.FieldName = "Aciklama";
            this.gridColumn29.Name = "gridColumn29";
            this.gridColumn29.Visible = true;
            this.gridColumn29.VisibleIndex = 8;
            this.gridColumn29.Width = 85;
            // 
            // gridColumn30
            // 
            this.gridColumn30.Caption = "Yükleme Firması Adres Adı";
            this.gridColumn30.FieldName = "YuklemeFirmasıAdresAdı";
            this.gridColumn30.Name = "gridColumn30";
            this.gridColumn30.OptionsColumn.AllowEdit = false;
            this.gridColumn30.Visible = true;
            this.gridColumn30.VisibleIndex = 9;
            this.gridColumn30.Width = 147;
            // 
            // gridColumn31
            // 
            this.gridColumn31.Caption = "Alıcı Firma Cari Ünvanı";
            this.gridColumn31.FieldName = "AlıcıFirmaCariUnvanı";
            this.gridColumn31.Name = "gridColumn31";
            this.gridColumn31.OptionsColumn.AllowEdit = false;
            this.gridColumn31.Visible = true;
            this.gridColumn31.VisibleIndex = 10;
            this.gridColumn31.Width = 136;
            // 
            // gridColumn32
            // 
            this.gridColumn32.Caption = "Teslim Firma Adres Adı";
            this.gridColumn32.FieldName = "TeslimFirmaAdresAdı";
            this.gridColumn32.Name = "gridColumn32";
            this.gridColumn32.Visible = true;
            this.gridColumn32.VisibleIndex = 11;
            this.gridColumn32.Width = 130;
            // 
            // gridColumn33
            // 
            this.gridColumn33.Caption = "İrsaliye No";
            this.gridColumn33.FieldName = "İrsaliyeNo";
            this.gridColumn33.Name = "gridColumn33";
            this.gridColumn33.OptionsColumn.AllowEdit = false;
            this.gridColumn33.Visible = true;
            this.gridColumn33.VisibleIndex = 12;
            this.gridColumn33.Width = 71;
            // 
            // gridColumn34
            // 
            this.gridColumn34.Caption = "İrsaliye Miktar";
            this.gridColumn34.FieldName = "İrsaliyeMiktar";
            this.gridColumn34.Name = "gridColumn34";
            this.gridColumn34.OptionsColumn.AllowEdit = false;
            this.gridColumn34.Visible = true;
            this.gridColumn34.VisibleIndex = 13;
            this.gridColumn34.Width = 107;
            // 
            // gridColumn35
            // 
            this.gridColumn35.Caption = "Ürün";
            this.gridColumn35.FieldName = "Urun";
            this.gridColumn35.Name = "gridColumn35";
            this.gridColumn35.OptionsColumn.AllowEdit = false;
            this.gridColumn35.Visible = true;
            this.gridColumn35.VisibleIndex = 14;
            this.gridColumn35.Width = 71;
            // 
            // gridColumn36
            // 
            this.gridColumn36.Caption = "Kap Adet";
            this.gridColumn36.FieldName = "KapAdet";
            this.gridColumn36.Name = "gridColumn36";
            this.gridColumn36.OptionsColumn.AllowEdit = false;
            this.gridColumn36.Visible = true;
            this.gridColumn36.VisibleIndex = 15;
            this.gridColumn36.Width = 71;
            // 
            // gridColumn37
            // 
            this.gridColumn37.Caption = "Ambalaj Tipi";
            this.gridColumn37.FieldName = "AmbalajTipi";
            this.gridColumn37.Name = "gridColumn37";
            this.gridColumn37.OptionsColumn.AllowEdit = false;
            this.gridColumn37.Visible = true;
            this.gridColumn37.VisibleIndex = 16;
            this.gridColumn37.Width = 71;
            // 
            // gridColumn38
            // 
            this.gridColumn38.Caption = "Brüt KG";
            this.gridColumn38.FieldName = "BrutKG";
            this.gridColumn38.Name = "gridColumn38";
            this.gridColumn38.OptionsColumn.AllowEdit = false;
            this.gridColumn38.Visible = true;
            this.gridColumn38.VisibleIndex = 17;
            this.gridColumn38.Width = 71;
            // 
            // gridColumn39
            // 
            this.gridColumn39.Caption = "M3";
            this.gridColumn39.FieldName = "M3";
            this.gridColumn39.Name = "gridColumn39";
            this.gridColumn39.OptionsColumn.AllowEdit = false;
            this.gridColumn39.Visible = true;
            this.gridColumn39.VisibleIndex = 18;
            this.gridColumn39.Width = 71;
            // 
            // gridColumn40
            // 
            this.gridColumn40.Caption = "Desi";
            this.gridColumn40.FieldName = "Desi";
            this.gridColumn40.Name = "gridColumn40";
            this.gridColumn40.OptionsColumn.AllowEdit = false;
            this.gridColumn40.Visible = true;
            this.gridColumn40.VisibleIndex = 19;
            this.gridColumn40.Width = 108;
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(0, 0);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(0, 13);
            this.labelControl1.TabIndex = 0;
            // 
            // labelControl2
            // 
            this.labelControl2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelControl2.Appearance.BackColor = System.Drawing.Color.Gainsboro;
            this.labelControl2.Appearance.ForeColor = System.Drawing.Color.ForestGreen;
            this.labelControl2.Appearance.Options.UseBackColor = true;
            this.labelControl2.Appearance.Options.UseForeColor = true;
            this.labelControl2.Location = new System.Drawing.Point(1106, 92);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(63, 13);
            this.labelControl2.TabIndex = 11;
            this.labelControl2.Text = "labelControl2";
            // 
            // DogtasSiparisEkranı
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1248, 602);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.gridControlDogtas);
            this.Controls.Add(this.ribbonControl1);
            this.Name = "DogtasSiparisEkranı";
            this.Text = "DogtasSiparisEkranı";
            this.Load += new System.EventHandler(this.DogtasSiparisEkranı_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControlDogtas)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewDogtas)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraBars.Ribbon.RibbonControl ribbonControl1;
        private DevExpress.XtraBars.BarButtonItem BtnVeriler;
        private DevExpress.XtraBars.BarButtonItem BtnEslestir;
        private DevExpress.XtraBars.BarButtonItem BtnExcel;
        private DevExpress.XtraBars.BarButtonItem BtnTeslimleriGetir;
        private DevExpress.XtraBars.BarButtonItem BtnDısarıCıkart;
        private DevExpress.XtraBars.BarButtonItem BtnSilme;
        private DevExpress.XtraBars.BarButtonItem BtnEslesme;
        private DevExpress.XtraBars.Ribbon.RibbonPage ribbonPage1;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup1;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup2;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup3;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup4;
        private DevExpress.XtraBars.BarButtonItem BtnBosHucreSil;
        public DevExpress.XtraGrid.GridControl gridControlDogtas;
        private DevExpress.XtraGrid.Views.Grid.GridView gridViewDogtas;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn21;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn22;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn23;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn24;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn25;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn26;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn27;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn28;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn29;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn30;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn31;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn32;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn33;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn34;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn35;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn36;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn37;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn38;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn39;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn40;
        private DevExpress.XtraBars.BarButtonItem BtnYuklemeFirma;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LabelControl labelControl2;
    }
}