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
    /// Interaction logic for Lauta.xaml
    /// </summary>
    public partial class Lauta : UserControl
    {
        private int koko;
        public int Koko { get { return koko; } set { koko = value; } }
        private Paikka[,] paikat;
        private List<Nappula> mustat = new List<Nappula>();
        private List<Nappula> valkoiset = new List<Nappula>();
        private Nappula valittuNappula;
        private Brush nappuloidenVari1;
        private Brush nappuloidenVari2;
        private Brush paikkojenVari1;
        private Brush paikkojenVari2;
        private Lautapeli lautaPeli;
        private string pelaaja1;
        private string pelaaja2;
        private int pelaajanVuoro = -1; //onko pelaajan 1 vuoro (-1) vai pelaajan 2 vuoro (1)... laskennallisista syistä näin 

        public Lauta()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Asetetaan pelilaudan tiedot
        /// </summary>
        /// <param name="nimi1">alemman pelaajan nimi</param>
        /// <param name="nimi2">ylemmän pelajaan nimi</param>
        /// <param name="laajuus">pelialueen laajuus</param>
        /// <param name="ruutu1">joka toisen ruudun väri</param>
        /// <param name="ruutu2">joka toisen ruudun väri</param>
        /// <param name="nappula1">alempien nappuloiden väri</param>
        /// <param name="nappula2">ylempien nappuloiden väri</param>
        public void asetaTiedot(string nimi1, string nimi2, int laajuus, Brush ruutu1, Brush ruutu2, Brush nappula1, Brush nappula2)
        {
            pelaaja1 = nimi1;
            pelaaja2 = nimi2;
            Koko = laajuus;
            nappuloidenVari1 = nappula1;
            nappuloidenVari2 = nappula2;
            paikkojenVari1 = ruutu1;
            paikkojenVari2 = ruutu2;
            lautaPeli = new BreakT(nappula1, nappula2);
            lautaPeli.Paaty += peli_Voitto;
            luoKentta();
        }

        /// <summary>
        /// Luo pelinappulat halutulle riville halutun värisinä
        /// </summary>
        /// <param name="rivi">jolle pelinappulat luodaan</param>
        /// <param name="vari">luotavien pelinappuloiden väri</param>
        public void luoNappulat(int rivi, Brush vari)
        {           
                if (rivi < paikat.GetLength(1) / 2) luoNappulatBreakT(rivi, vari, mustat);
                else luoNappulatBreakT(rivi, vari, valkoiset);           
        }


        /// <summary>
        /// Luo pelinappulat halutulle riville halutun värisinä ja lisää luodut nappulat listaan
        /// </summary>
        /// <param name="rivi">jolle pelinappulat luodaan</param>
        /// <param name="vari">luotavien pelinappuloiden väri</param>
        /// <param name="nappulat">lista jonne pelinappulat lisätään</param>
        public void luoNappulatBreakT(int rivi, Brush vari, List<Nappula> nappulat)
        {
            int siirtojaVoittoon = paikat.GetLength(0) - 2;                           // lasketaan rivin perusteella monestiko 
            if (rivi == 0 || rivi == paikat.GetLength(0) - 1) siirtojaVoittoon += 1;  // pitää siirtää jotta voittaa
            for (int i = 0; i < paikat.GetLength(1); i++)
            {
                Nappula uusi = new Nappula(vari, siirtojaVoittoon);
                uusi.Valinta += nappula_Valinta;
                nappulat.Add(uusi);
                paikat[i, rivi].lisaaNappula(uusi);
                paikat[i, rivi].OnkoTyhja = false;
            }
        }

        /// <summary>
        /// Luo pelinappulat pelikentän paikkoihin
        /// </summary>
        public void luoNappulat()
        {
            luoNappulat(0, nappuloidenVari1);
            luoNappulat(1, nappuloidenVari1);
            luoNappulat(paikat.GetLength(0) - 1, nappuloidenVari2);
            luoNappulat(paikat.GetLength(0) - 2, nappuloidenVari2);
        }


        /// <summary>
        /// Luo paikat pelialueelle koon mukaan
        /// </summary>
        public void luoPaikat()
        {
            this.paikat = new Paikka[Koko, Koko];

            for (int i = Koko; i > 0; i--)
            {
                if (i % 2 == 0) luoRivi(i, paikkojenVari2, paikkojenVari1); //rivin ensimmäisen ruudun väri sen  
                else luoRivi(i, paikkojenVari1, paikkojenVari2);            //mukaan, onko parillinen vai ei
            }
        }

        /// <summary>
        /// Asettaa valitun nappulan ja muuttaa sen ulkoasua
        /// </summary>
        /// <param name="sender">joka asetetaan valituksi nappulaksi</param>
        private void nappula_Valinta(Nappula sender)
        {
            if (!oikeaVuoro(sender)) return; //jos tullaan väärällä vuorolla, ei tehdä mitään

            foreach (Nappula n in mustat) n.poistaValinta();
            foreach (Nappula n in valkoiset) n.poistaValinta();
            valittuNappula = sender;

            //Mahdolliset muutokset tämän hetkiseen valittuun nappulaan
            valittuNappula.ympyra.StrokeThickness = 3;
        }

        /// <summary>
        /// Siirtää nappulan paikasta toiseen ja päivittää kyseisten paikkojen OnkoTyhja -arvon 
        /// </summary>
        /// <param name="sender">Paikka joka valittu, eli jonne nappula siirtyy</param>
        private void paikka_Valinta(Paikka sender)
        {
            if (lautaPeli.paikkaValinta(valittuNappula, sender))
            {
                vuoronvaihto();
            }
        }

        /// <summary>
        /// Kun peli voitetaan, niin poistetaan toiminnot pelistä ja asetetaan voittaneen nappulan ulkoasu erilaiseksi
        /// </summary>
        private void peli_Voitto()
        {
            // Voittaneen nappulan layout muutokset
            valittuNappula.Margin = new Thickness(0);
            valittuNappula.ympyra.StrokeThickness = 7;
            valittuNappula.ympyra.Stroke = Brushes.Purple;

            valittuNappula.poistaPainallus(); // jotta voittaneen nappulan layout ei ole enää muutettavissa clickaamalla
            poistaToimet(mustat); poistaToimet(valkoiset); //peli päättyi, nappuloita ei voi enää valita

            //Poistaa virhevarjayksen kaikista paikoista
            for (int i = 0; i < paikat.GetLength(0); i++)
            {
                for (int k = 0; k < paikat.GetLength(1); k++)
                {
                    paikat[i, k].poistaVirhevarjays();
                }
            }

        }

        /// <summary>
        /// Luo uuden pelikentän
        /// </summary>
        public void luoKentta()
        {
            mustat = new List<Nappula>();
            valkoiset = new List<Nappula>(); //Tyhjennetään nappulalistat
            pelialue.Children.Clear();
            luoPaikat();           
            pelaajanVuoro = -1;
            lautaPeli.asetaVuoro(pelaajanVuoro);
            luoNappulat();
            lautaPeli.asetaNappulat(mustat, valkoiset);
        }


        /// <summary>
        /// Luo rivin paikkoja
        /// </summary>
        /// <param name="monesko">monesko rivi luodaan</param>
        /// <param name="ensimmainenVari">joka toisen paikan väri</param>
        /// <param name="toinenVari">joka toisen paikan väri</param>
        public void luoRivi(int monesko, Brush ensimmainenVari, Brush toinenVari)
        {
            Brush vari;
            for (int i = 0; i < Koko; i++)
            {
                if (i % 2 == 0) vari = ensimmainenVari;
                else vari = toinenVari;
                Paikka uusi = new Paikka(i, monesko - 1, vari);
                uusi.PaikkaValittu += paikka_Valinta;
                paikat[i, monesko - 1] = uusi;
                lautaPeli.asetaPaikat(paikat);
                pelialue.Children.Add(uusi);
            }
        }

        /// <summary>
        /// Poistaa kaikki valinta -eventit jonkin nappulalistan kaikista pelinappuloista
        /// </summary>
        /// <param name="nappulat">lista jonka nappuloista poistetaan eventti</param>
        private void poistaToimet(List<Nappula> nappulat)
        {
            for (int i = 0; i < nappulat.Count; i++)
            {
                nappulat[i].Valinta -= nappula_Valinta;
            }
        }

        /// <summary>
        /// Katsotaan onko valitun nappulan vuoro pelissä
        /// </summary>
        /// <param name="sender">nappula jonka perusteella vuoro tarkistetaan</param>
        /// <returns>onko oikea vuoro (true) vai ei (false)</returns>
        private bool oikeaVuoro(Nappula sender)
        {
            if (pelaajanVuoro == -1 && sender.ympyra.Fill == nappuloidenVari2) return false;
            if (pelaajanVuoro == 1 && sender.ympyra.Fill == nappuloidenVari1) return false;
            return true;
        }

        /// <summary>
        /// Vaihtaa vuoron vastapelaajalle ja poistaa valinnan edellisestä pelinappulasta
        /// </summary>
        private void vuoronvaihto()
        {
            valittuNappula.poistaValinta();
            valittuNappula = null;
            this.pelaajanVuoro = this.pelaajanVuoro * -1;
            lautaPeli.asetaVuoro(pelaajanVuoro);
        }
    }
}
