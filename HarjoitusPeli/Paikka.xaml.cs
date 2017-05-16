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
using System.Timers;

namespace HarjoitusPeli
{
    /// <summary>
    /// Interaction logic for Paikka.xaml
    /// </summary>
    public partial class Paikka : UserControl
    {

        private bool onkoTyhja = true; //onko paikassa nappula vai ei
        public bool OnkoTyhja { get { return onkoTyhja; } set { onkoTyhja = value; } }
        private int p_kolumni; //paikan sijainti pelilaudalla
        public int Kolumni { get { return p_kolumni; } set { p_kolumni = value; } }
        private int p_rivi; // paikan sijainti pelilaudalla
        public int Rivi { get { return p_rivi; } set { p_rivi = value; } }
        private Timer ajastin = new Timer(200); //ajastin jota käytetään virhevärjäyksessä
        private Brush virhevari;
        private Brush normaalivari;
        private string kirjaimet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ"; //käytetään paikan nimeämiseen
        private Nappula nappulaPaikalla; //tässä paikassa oleva nappula
        public delegate void PaikanValinta(Paikka sender);
        public event PaikanValinta PaikkaValittu;

        public Paikka()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Muodostaja jossa asetaan paikan tiedot
        /// </summary>
        /// <param name="column">paikka peliruudukossa (kolumni)</param>
        /// <param name="row">paikka peliruudukossa (rivi)</param>
        public Paikka(int column, int row, Brush vari)
        {
            InitializeComponent();
            Kolumni = column;
            Rivi = row;
            this.normaalivari = vari;
            this.virhevari = Brushes.Red;
            this.sisa.Fill = normaalivari;
            ajastin.Elapsed += ajastin_Elapsed;
            nimea();
        }

        /// <summary>
        /// Lisää pelinappulan pelipaikkaan
        /// </summary>
        /// <param name="nappula">pelinappula joka lisätään</param>
        public void lisaaNappula(Nappula nappula)
        {
            if (nappula == null) return;
            this.nelio.Children.Add(nappula); //Lisätään nappula valittuun paikkaan
            nappulaPaikalla = nappula;
            this.OnkoTyhja = false;
        }

        /// <summary>
        /// Antaa paikassa olevan pelinappulan
        /// </summary>
        /// <returns>paikassa oleva pelinappula</returns>
        public Nappula annaNappula()
        {
            return nappulaPaikalla;
        }
      
        /// <summary>
        /// Poistaa nappulan pelipaikalta
        /// </summary>
        public void poistaNappula()
        {
            this.onkoTyhja = true;
            if (nappulaPaikalla == null) return;
            this.nelio.Children.Remove(nappulaPaikalla);
            nappulaPaikalla = null;
        }


        /// <summary>
        /// Nimeää pelipaikan sijainnin perusteella
        /// </summary>
        private void nimea()
        {
            this.Name = kirjaimet[Kolumni].ToString() + Rivi.ToString();
        }

        /// <summary>
        /// Pelipaikkaa clickattaessa PaikkaValittu -tapahtuma
        /// </summary>
        private void nelio_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (PaikkaValittu != null) PaikkaValittu(this);
        }

        /// <summary>
        /// Värjää paikan virhevärillä ja käynnistää ajastimen (joka vaihtaa värin takaisin normaaliksi)
        /// </summary>
        public void virheellinen()
        {
            this.sisa.Fill = virhevari;
            ajastin.Enabled = true;            
        }

        /// <summary>
        /// Poistaa virhevärjäyksen asettamalla virhevärin samaksi kuin normaaliväri
        /// </summary>
        public void poistaVirhevarjays()
        {
            virhevari = normaalivari;
        }

        /// <summary>
        /// Asettaa paikan värin normaaliksi
        /// </summary>    
        private void ajastin_Elapsed(object sender, ElapsedEventArgs e)
        {
            this.Dispatcher.Invoke((Action)(() => // ei toimi ilman tätä, mutta en ihan ymmärrä miksi...
            {
                this.sisa.Fill = normaalivari;
                ajastin.Enabled = false;
            }));

        }
    }
}
