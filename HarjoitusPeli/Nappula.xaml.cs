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
    /// Interaction logic for Nappula.xaml
    /// </summary>
    public partial class Nappula : UserControl
    {

        private bool valittu = false;
        public bool Valittu {get { return valittu; } set { valittu = value;}}
        private int siirtojaPaatyyn; //Tämän avulla lasketaan, milloin nappula voittaa
        private Brush normaalivari;
        private bool p_syoty = false;
        public bool Syoty { get { return p_syoty; } set { p_syoty = value; } }
        public delegate void NappulanValinta(Nappula sender);
        public event NappulanValinta Valinta;

        public Nappula()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Muodostaja, joka asettaa nappulan värin ja siirtojen määrän jotta voittaa
        /// </summary>
        /// <param name="vari">nappulan väri</param>
        /// <param name="siirtoja">monestiko nappulaa pitää siirtää jotta se voittaa</param>
        public Nappula(Brush vari, int siirtoja)
        {
            InitializeComponent();
            this.normaalivari = vari;
            this.ympyra.Fill = normaalivari;
            this.siirtojaPaatyyn = siirtoja;
        }

        /// <summary>
        /// Vähentää nappulan siirtojen määrää, ja palauttaa true jos siirtojen määrä vähennyksen jälkeen on 0.
        /// </summary>
        /// <returns>true jos siirtojaVoittoon jäi 0, muuten false</returns>
        public bool kohtiPaatya()
        {
            this.siirtojaPaatyyn -= 1;
            if (siirtojaPaatyyn == 0) return true;
            return false;
        }

        /// <summary>
        /// antaa sen paikan jossa nappula sijaitsee
        /// </summary>
        /// <returns>Paikka jossa nappula sijaitsee</returns>
        public Paikka annaPaikka()
        {
            Grid control = (Grid)this.Parent;
            return (Paikka)control.Parent;
        }

        /// <summary>
        /// Nappulaa clickattaessa muuttaa valittu -statusta ja aiheuttaa Valinta -tapahtuman
        /// </summary>
        private void ympyra_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!Valittu)
            {
                if (Valinta != null) Valinta(this);
                this.Valittu = true;
            }
            else poistaValinta();
        }

        /// <summary>
        /// Poistaa mousedown toiminnon nappulasta
        /// </summary>
        public void poistaPainallus()
        {
            this.ympyra.MouseDown -= ympyra_MouseDown;
        }

        /// <summary>
        /// Poistaa valinnan nappulasta
        /// </summary>
        public void poistaValinta()
        {
            ympyra.StrokeThickness = 0; this.Valittu = false;
        }
    }
}
