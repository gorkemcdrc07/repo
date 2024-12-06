using SiparişOtomasyon.Sipariş_Ekranları;
using System;
using System.Windows.Forms;
using System.Drawing;
using DevExpress.XtraTab;


namespace SiparişOtomasyon
{
    public partial class AnaSayfa : Form
    {
        public AnaSayfa()
        {
            InitializeComponent();

            // Arka plan rengini beyaz yap
            this.BackColor = System.Drawing.Color.White;

            // Varsayılan metni ve rengini ayarlayın
            TxtArama.Text = "Arama yapın...";
            TxtArama.ForeColor = System.Drawing.Color.Gray;

            // Olayları bağla
            TxtArama.GotFocus += TxtArama_GotFocus;
            TxtArama.LostFocus += TxtArama_LostFocus;
            TxtArama.TextChanged += TxtArama_TextChanged;

            // panel1 başlangıçta gizli olacak şekilde ayarlayın
        }

        private void AnaSayfa_Load(object sender, EventArgs e)
        {
            // Arka plan rengini beyaz yap (gerekirse)
            this.BackColor = System.Drawing.Color.White;
        }

        private void pictureEdit1_Click(object sender, EventArgs e)
        {
            // pictureEdit1'e tıklandığında TxtArama'ya odaklan
            TxtArama.Focus();
        }

        private void TxtArama_GotFocus(object sender, EventArgs e)
        {
            if (TxtArama.Text == "Arama yapın...")
            {
                TxtArama.Text = string.Empty;
                TxtArama.ForeColor = System.Drawing.Color.Black;
            }
        }

        private void TxtArama_LostFocus(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtArama.Text))
            {
                TxtArama.Text = "Arama yapın...";
                TxtArama.ForeColor = System.Drawing.Color.Gray;
            }
        }

        private void TxtArama_TextChanged(object sender, EventArgs e)
        {
            if (TxtArama.ForeColor == System.Drawing.Color.Gray && TxtArama.Text != "Arama yapın...")
            {
                TxtArama.Text = string.Empty;
                TxtArama.ForeColor = System.Drawing.Color.Black;
            }

            // Ara ve filtrele
            SearchAccordionItems(TxtArama.Text);
        }

        private void SearchAccordionItems(string searchText)
        {
            foreach (var item in accordionControl1.Elements)
            {
                if (item.Text.ToLower().Contains(searchText.ToLower()))
                {
                    item.Visible = true;
                }
                else
                {
                    item.Visible = false;
                }
            }
        }

        private void pictureEdit1_EditValueChanged(object sender, EventArgs e)
        {
            // Bu olayı istediğiniz gibi kullanabilirsiniz
        }


        private void pictureEdit2_Click(object sender, EventArgs e)
        {
            // pictureEdit2'ye tıklandığında panel1'i kapat
            panel1.Visible = false; // panel1'i gizle

        }

        private void pictureEdit2_EditValueChanged(object sender, EventArgs e)
        {
            // Bu olayı istediğiniz gibi kullanabilirsiniz
        }

        private void AdkSiparisEkranı_Click(object sender, EventArgs e)
        {
            // Yeni formun örneğini oluştur
            FrmAdkoTurkSiparisEkrani siparisEkrani = new FrmAdkoTurkSiparisEkrani();

            // Formu göster
            siparisEkrani.Show();
        }

        private void ApakSiparisEkrani_Click(object sender, EventArgs e)
        {
            // ApakSiparisEkrani formunu oluştur
            ApakSiparisEkrani apakSiparisForm = new ApakSiparisEkrani();

            // Formu aç
            apakSiparisForm.Show();
        }

        private void RekaSiparisEkrani_Click(object sender, EventArgs e)
        {
            RekaSiparisEkranı rekaSiparisForm = new RekaSiparisEkranı();

            rekaSiparisForm.Show();
        }

        private void TekirdagUnSiparisEkrani_Click(object sender, EventArgs e)
        {
            TekirdagUnSiparisEkrani tekirdagUnSiparisForm = new TekirdagUnSiparisEkrani();

            tekirdagUnSiparisForm.Show();
        }

        private void SudesanSiparisEkrani_Click(object sender, EventArgs e)
        {
            SudesanSiparisEkrani sudesanSiparisForm = new SudesanSiparisEkrani();

            sudesanSiparisForm.Show();
        }

        private void MilhansSiparisEkrani_Click(object sender, EventArgs e)
        {
            // Eğer form zaten açık değilse, yeni bir form oluştur ve göster
            Form milhansForm = new MilhansSiparisEkrani(); // MilhansSiparisEkrani formunun bir örneğini oluştur
            milhansForm.Show(); // Formu göster
        }

        private void DogtasSiparisEkrani_Click(object sender, EventArgs e)
        {

            // Eğer form zaten açık değilse, yeni bir form oluştur ve göster
            Form DogtasForm = new DogtasSiparisEkranı(); // MilhansSiparisEkrani formunun bir örneğini oluştur
            DogtasForm.Show(); // Formu göster
        }

        private void GezerSiparisEkranı_Click(object sender, EventArgs e)
        {
            // Eğer form zaten açık değilse, yeni bir form oluştur ve göster
            Form GezerForm = new GezerSiparisEkranı(); // GezerSiparisEkranı formunun bir örneğini oluştur
            GezerForm.Show(); // Formu göster

        }

        private void EsGlobalSiparisEkrani_Click(object sender, EventArgs e)
        {
            // Eğer form zaten açık değilse, yeni bir form oluştur ve göster
            Form EsGlobalForm = new EsGlobalSiparisEkranı(); // GezerSiparisEkranı formunun bir örneğini oluştur
            EsGlobalForm.Show(); // Formu göster
        }


        private void BtnTanıtımKartı_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Form YüklemeAdresleriForm = new YüklemeAdresleri();
            YüklemeAdresleriForm.Show();
        }

        private void SaruhanSiparisEkranı_Click(object sender, EventArgs e)
        {
            Form SaruhanSiparisEkranı = new SaruhanSiparisEkranı();
            SaruhanSiparisEkranı.Show();
        }

        private void BtnKucukBaySiparisEkranı_Click(object sender, EventArgs e)
        {
            // Küçük Bay Sipariş Ekranı formunu oluştur ve göster
            KucukBaySiparisEkrani kucukBaySiparisEkrani = new KucukBaySiparisEkrani();
            kucukBaySiparisEkrani.Show(); // Formu göster
        }

        private void BtnBungeSiparisEkranı_Click(object sender, EventArgs e)
        {
            // Proje seçimini yapacak bir form açmak
            string selectedProject = GetSelectedProject();

            // Proje seçildiyse BungeSiparisEkranı formunu seçilen proje adıyla aç
            if (!string.IsNullOrEmpty(selectedProject))
            {
                // Seçilen projeyi parametre olarak alıp yeni bir form oluşturuyoruz
                BungeSiparisEkranı BungeSiparisEkrani = new BungeSiparisEkranı(selectedProject);
                BungeSiparisEkrani.Show(); // Formu göster
            }
            else
            {
                MessageBox.Show("Lütfen bir proje seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // Bu metod, projeleri kullanıcıya seçtirmek için bir seçim ekranı sağlar
        private string GetSelectedProject()
        {
            // Burada projeleri tanımlıyoruz
            string[] projects = new string[] { "LÜLEBURGAZ FTL", "GEBZE", "EDİRNE" };

            using (Form projectForm = new Form())
            {
                // Form özelliklerini ayarlıyoruz
                projectForm.Text = "Proje Seçin";
                projectForm.Width = 250;
                projectForm.Height = projects.Length * 50 + 50; // Butonlar için yeterli alan sağlıyoruz
                projectForm.StartPosition = FormStartPosition.CenterParent;

                // Projeleri göstermek için butonlar oluşturuyoruz
                int yOffset = 10; // Butonların başlangıç y-offset değeri
                foreach (string project in projects)
                {
                    Button projectButton = new Button();
                    projectButton.Text = project;
                    projectButton.Width = 200;
                    projectButton.Height = 40;
                    projectButton.Top = yOffset;
                    projectButton.Left = 10;
                    projectButton.Click += (sender, e) =>
                    {
                        // Butona tıklanırsa, projeyi seç ve formu kapat
                        projectForm.DialogResult = DialogResult.OK;
                        projectForm.Tag = project; // Seçilen projeyi Tag'de saklıyoruz
                    };
                    projectForm.Controls.Add(projectButton);
                    yOffset += 50; // Bir sonraki butonun altına gelmesi için offset arttır
                }

                // Formu modal olarak açıyoruz
                projectForm.ShowDialog();

                // Seçilen projeyi döndür
                if (projectForm.DialogResult == DialogResult.OK)
                {
                    return projectForm.Tag.ToString(); // Seçilen projeyi döndür
                }
            }

            return null; // Eğer hiçbir proje seçilmemişse null döndür
        }

        private void BtnHedefSiparisEkranı_Click(object sender, EventArgs e)
        {
            // Küçük Bay Sipariş Ekranı formunu oluştur ve göster
            HedefSiparisEkrani HedefSiparisEkrani = new HedefSiparisEkrani();
            HedefSiparisEkrani.Show(); // Formu göster
        }
    }
}


