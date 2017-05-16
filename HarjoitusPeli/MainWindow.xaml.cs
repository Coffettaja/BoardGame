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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HarjoitusPeli
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();        
        }

        /// <summary>
        /// Asetetaan pelilaudan tiedot
        /// </summary>
        /// <param name="nimi1">alemman pelaajan nimi</param>
        /// <param name="nimi2">ylemmän pelaajan nimi</param>
        /// <param name="laajuus">pelilaudan leveys ja korkeus</param>
        /// <param name="ruutu1">joka toisen ruudun väri</param>
        /// <param name="ruutu2">joka toisen ruudun väri</param>
        /// <param name="nappula1">alempien nappuloiden väri</param>
        /// <param name="nappula2">ylempien nappuloiden väri</param>
        public void asetaTiedot(string nimi1, string nimi2, int laajuus, Brush ruutu1, Brush ruutu2, Brush nappula1, Brush nappula2)
        {
            pelilauta.asetaTiedot(nimi1, nimi2, laajuus, ruutu1, ruutu2, nappula1, nappula2);
            pelilauta.luoKentta();
        }

   
        /// <summary>
        /// Esitetään asetusdialogi ja mikäli asetusdialogissa valittiin ok, niin luodaan uusi peli
        /// </summary>
        private void UusiPeli_Click(object sender, RoutedEventArgs e)
        {
            SettingsWindow asetukset = new SettingsWindow();           
            asetukset.ShowDialog();
            int koko = asetukset.Koko;
            string nimi1 = asetukset.Pelaaja1;
            string nimi2 = asetukset.Pelaaja2;
            var ruutuvari1 = asetukset.RuutuVari1;
            var ruutuvari2 = asetukset.RuutuVari2;
            var nappulavari1 = asetukset.NappulaVari1;
            var nappulavari2 = asetukset.NappulaVari2;
            string peli = asetukset.ValittuPeli;
            if (asetukset.Asetetaanko == true) asetaTiedot(nimi1, nimi2, koko, ruutuvari1, ruutuvari2, nappulavari1, nappulavari2);
        }

        /// <summary>
        /// suljetaan ohjelma
        /// </summary>
        private void Lopeta_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Näytetään avustusdialogi
        /// </summary>
        private void Avustus_Click(object sender, RoutedEventArgs e)
        {
            HelpWindow apua = new HelpWindow();
            apua.ShowDialog();
        }

        /// <summary>
        /// Näytetään about dialogi
        /// </summary>
        private void About_Click(object sender, RoutedEventArgs e)
        {
            AboutWindow about = new AboutWindow();
            about.ShowDialog();
        }
    }
}
