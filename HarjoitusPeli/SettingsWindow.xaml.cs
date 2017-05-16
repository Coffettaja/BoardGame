using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace HarjoitusPeli
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        private String p_pelaaja1 = ""; //Ykköspelaajan nimi
        public String Pelaaja1 { get { return p_pelaaja1; } set { p_pelaaja1 = value; } }

        private string p_pelaaja2 = ""; //Kakkospelaajan nimi
        public string Pelaaja2 { get { return p_pelaaja2; } set { p_pelaaja2 = value; } }

        private Brush p_ruutuVari1 = Brushes.Brown; //Ruudun ykkösväri
        public Brush RuutuVari1 { get { return p_ruutuVari1; } set { p_ruutuVari1 = value; } }

        private Brush p_ruutuVari2 = Brushes.Beige; //Ruudun kakkosväri
        public Brush RuutuVari2 { get { return p_ruutuVari2; } set { p_ruutuVari2 = value; } }

        private Brush p_nappulaVari1 = Brushes.Orange; //Ykköspelaajan nappuloiden väri
        public Brush NappulaVari1 { get { return p_nappulaVari1; } set { p_nappulaVari1 = value; } }

        private Brush p_nappulaVari2 = Brushes.Black; //Kakkospelaajan nappuloiden väri
        public Brush NappulaVari2 { get { return p_nappulaVari2; } set { p_nappulaVari2 = value; } }

        private int p_koko = 8; //Pelilaudan pituus ja leveys
        public int Koko { get { return p_koko; } set { p_koko = value; } }

        private string p_valittuPeli = "Tammi";
        public string ValittuPeli { get { return p_valittuPeli; } set { p_valittuPeli = value; } }

        private string virheteksti = ""; //Virhelabelissa esitettävä teksti

        //Comboboxissa esitettävät kokovaihtoehdot
        private string[] comboBoxSisalto = new string[] {"6","7","8","9","10","11","12","13","14","15","16"};

        private bool p_asetetaanko = false; //Asetetaanko dialogissa määritetyt asetukset peliin
        public bool Asetetaanko { get { return p_asetetaanko; } set { p_asetetaanko = value; } }

        public SettingsWindow()
        {
            InitializeComponent();
            Pelaaja1 = "Alempi pelaaja";
            Pelaaja2 = "Ylempi pelaaja";
            pelaaja1Button.Foreground = NappulaVari1;
            pelaaja2Button.Foreground = NappulaVari2;
            variButton1.Foreground = RuutuVari1;
            variButton2.Foreground = RuutuVari2;
        }

        /// <summary>
        /// Otetaan comboboxin sisältö taulukosta, ja asetetaan alussa valituksi 2 indeksi, eli oletusarvo 8
        /// </summary>
        private void ComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.ComboBox comboBox = sender as System.Windows.Controls.ComboBox;
            comboBox.ItemsSource = comboBoxSisalto;
            comboBox.SelectedIndex = 2;
        }

        /// <summary>
        /// Asetetaan koko combobox valinnan perusteella
        /// </summary>
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            System.Windows.Controls.ComboBox comboBox = sender as System.Windows.Controls.ComboBox;

            Koko = Convert.ToInt32(comboBox.SelectedItem as string);
        }

        /// <summary>
        /// Suljetaan ikkuna ja vahvistetaan asetukset
        /// </summary>
        private void Aloita_Click(object sender, RoutedEventArgs e)
        {          
            Asetetaanko = true;
            this.Close();
        }

        private void Peruuta_Click(object sender, RoutedEventArgs e)
        {
            Asetetaanko = false; 
            this.Close();
        }

        /// <summary>
        /// RadioButton valinta, annetaan virhe jos valittu huonosti
        /// </summary>
        private void Peli_Checked(object sender, RoutedEventArgs e)
        {
            if ((sender as RadioButton) == tammiButton)
            {
                ValittuPeli = "Tammi";
            }
            else
            {
                ValittuPeli = "Breakthrough";
            }
        }

        /// <summary>
        /// Asetetaan pelilaudan ruutujen värit colorDialogin avulla. Mikä väri asetetaan riippuu nappulasta
        /// </summary>
        private void Vari_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.ColorDialog variDialog = new System.Windows.Forms.ColorDialog();
            if (variDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                System.Drawing.Color variValinta = variDialog.Color;
                System.Windows.Media.Color uusiVari = System.Windows.Media.Color.FromArgb(variValinta.A, variValinta.R, variValinta.G, variValinta.B);
                if ((Button)sender == variButton1)
                {
                    RuutuVari1 = new SolidColorBrush(uusiVari);
                    variButton1.Foreground = RuutuVari1;
                }
                else
                {
                    RuutuVari2 = new SolidColorBrush(uusiVari);
                    variButton2.Foreground = RuutuVari2;
                }
            }
        }     

        /// <summary>
        /// Asetetaan pelinappuloiden värit. Jos värit ovat samat, annetaan virhe.
        /// </summary>
        private void Pelaaja_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.ColorDialog variDialog = new System.Windows.Forms.ColorDialog();
            if (variDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                System.Drawing.Color variValinta = variDialog.Color;
                System.Windows.Media.Color uusiVari = System.Windows.Media.Color.FromArgb(variValinta.A, variValinta.R, variValinta.G, variValinta.B);
                if ((Button)sender == pelaaja1Button)
                {
                    NappulaVari1 = new SolidColorBrush(uusiVari);
                    pelaaja1Button.Foreground = NappulaVari1;
                }
                else
                {
                    NappulaVari2 = new SolidColorBrush(uusiVari);
                    pelaaja2Button.Foreground = NappulaVari2;
                }
            }

            //Jos nappuloiden värit ovat samat niin ei anneta jatkaa (koska ohjelma hajoaisi... eikä muutenkaan järkeä)
            if (((SolidColorBrush)NappulaVari1).Color == ((SolidColorBrush)NappulaVari2).Color) 
            {                                                                           
                virheteksti = "Pelaajien värit eivät saa olla samat!";
                aloitaButton.IsEnabled = false;
            } 
            else
            {
                virheteksti = "";
                aloitaButton.IsEnabled = true;
            }

            virheLabel.Content = virheteksti;
        }
    }

    /// <summary>
    /// Tarkistusluokka, joka tarkistaa, onko merkkijonossa vain kirjaimia ja välilyöntejä
    /// </summary>
    class PelkkiaMerkkeja : ValidationRule
    {
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            String merkkijono = "";
            try
            {
                merkkijono = value.ToString();
                if (!merkkijono.All(c => Char.IsLetter(c) || c == ' ')) return new ValidationResult(false, "virhe"); //hyväksyy kirjaimia ja välilyöntejä
            }
            catch (FormatException)
            {
                return new ValidationResult(false, "Ei ole pelkkiä kirjaimia.");
            }
            return ValidationResult.ValidResult;
        }
    }
}
