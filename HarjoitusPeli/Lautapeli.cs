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
    /// Base-class lautapeleille
    /// </summary>
    class Lautapeli
    {
        protected Brush nappuloidenVari1;
        protected Brush nappuloidenVari2;
        protected int pelaajanVuoro;
        protected int kirjainNro1;
        protected int kirjainNro2;
        protected int numero1;
        protected int numero2;
        protected List<Nappula> mustat;
        protected List<Nappula> valkoiset;
        protected Paikka[,] paikat;
        protected Paikka valittuPaikka;
        public delegate void NappulaPaaty();
        public event NappulaPaaty Paaty;

        /// <summary>
        /// Muodostaja jossa asetetaan pelinappuloiden värit
        /// </summary>
        /// <param name="vari1">alempien nappuloiden väri</param>
        /// <param name="vari2">ylempien nappuloiden väri</param>
        public Lautapeli(Brush vari1, Brush vari2)
        {
            this.nappuloidenVari1 = vari1;
            this.nappuloidenVari2 = vari2;
        }

        /// <summary>
        /// Asettaa lautapelin nappulalistat
        /// </summary>
        /// <param name="mustat">alemmat nappulat</param>
        /// <param name="valkoiset">ylemmät nappulat</param>
        public void asetaNappulat(List<Nappula> mustat, List<Nappula> valkoiset)
        {
            this.mustat = mustat;
            this.valkoiset = valkoiset;
        }

        /// <summary>
        /// Asetetaan pelaajan vuoro
        /// </summary>
        /// <param name="vuoro">pelaajan vuoro</param>
        public void asetaVuoro(int vuoro)
        {
            this.pelaajanVuoro = vuoro;
        }

        /// <summary>
        /// Jos mahdollista, niin siirtää nappulan valittuun paikkaan
        /// </summary>
        /// <param name="valittuNappula">nappula joka siirretään</param>
        /// <param name="valittu">paikka jonne siirretään</param>
        /// <returns>onnistuiko siirto vai ei (true, false)</returns>
        public bool paikkaValinta(Nappula valittuNappula, Paikka valittu)
        {
            this.valittuPaikka = valittu;
            if (valittuNappula == null || !valittuNappula.Valittu) return false; //Jos ei ole nappulaa valittuna, ei tehdä mitään
            if (!valittuPaikka.OnkoTyhja && !onkoVastustaja(valittuPaikka)) return false; //Jos valitussa paikassa oma nappula, ei tehdä mitään
            if (onkoLaillinen(valittuNappula, valittuPaikka)) return siirra(valittuPaikka, valittuNappula);
            valittuPaikka.virheellinen(); return false;
        }


        /// <summary>
        /// Asetetaan paikkataulukko
        /// </summary>
        /// <param name="paikat">jotka asetetaan</param>
        public void asetaPaikat(Paikka[,] paikat)
        {
            this.paikat = paikat;
        }

        /// <summary>
        /// Siirretään haluttu nappula haluttuun paikkaan, jos mahdollista
        /// </summary>
        /// <param name="valittuPaikka">paikka jonne siirretään</param>
        /// <param name="valittuNappula">nappula joka siirretään</param>
        /// <returns>true jos siirto onnistui, false jos ei onnistunut tai jos peli päättyi</returns>
        protected bool siirra(Paikka valittuPaikka, Nappula valittuNappula)
        {
            Grid control = (Grid)valittuNappula.Parent;
            Paikka alkuPaikka = valittuNappula.annaPaikka();

            control.Children.Remove(valittuNappula);  //Poistetaan nappula vanhalta paikaltaan
            alkuPaikka.OnkoTyhja = true;
            if (!valittuPaikka.OnkoTyhja) //Jos valitussa paikassa on jo nappula niin se poistetaan
            {
                valittuPaikka.annaNappula().Syoty = true;
                valittuPaikka.poistaNappula();               
            }
            valittuPaikka.lisaaNappula(valittuNappula);
            alkuPaikka.poistaNappula();;
            if (!onkoJaljella()) //jos kaikki vastustajan nappulat syöty
            {
                Paaty();
                return false;
            }
            if (onkoPaadyssa(valittuNappula)) //jos nappula saapui pelilaudan päätyyn
            {
                Paaty();
                return false; // Jos voittoehdot täyttyvät
            }
            return true;           
        }

        /// <summary>
        /// Onko vastustajalla enää nappuloita jäljellä
        /// </summary>
        /// <returns>true tai false sen mukaan onko nappuloita jäljellä vai ei</returns>
        private bool onkoJaljella()
        {
            int laskuri = 0;
            if (pelaajanVuoro == -1)
            {
                for (int i = 0; i < valkoiset.Count; i++)
                {
                    if (valkoiset[i].Syoty) laskuri++;
                }
                if (laskuri == valkoiset.Count) return false;
            }
            else
            {
                for (int i = 0; i < mustat.Count; i++)
                {
                    if (mustat[i].Syoty) laskuri++;
                }
                if (laskuri == mustat.Count) return false;
            }
            return true;
        }

        /// <summary>
        /// katsotaan onko nappula vastustajan puoleisessa päädyssä
        /// </summary>
        /// <param name="valittuNappula">nappula joka tarkistetaan</param>
        /// <returns></returns>
        protected bool onkoPaadyssa(Nappula valittuNappula)
        {
            return valittuNappula.kohtiPaatya();
        }

        /// <summary>
        /// Katsoo onko jollakin pelipaikalla oleva nappula vastustajan, eli sen pelaajan, jonka vuoro ei ole tällä hetkellä
        /// </summary>
        /// <param name="sender">Paikka jonka nappula tarkistetaan</param>
        /// <returns>true jos vasustajan, false jos ei ole</returns>
        public bool onkoVastustaja(Paikka sender)
        {
            Nappula paikalla = sender.annaNappula();
            if (paikalla == null) return false;
            if (paikalla.ympyra.Fill == nappuloidenVari1 && pelaajanVuoro == 1) return true;
            if (paikalla.ympyra.Fill == nappuloidenVari2 && pelaajanVuoro == -1) return true;
            return false;
        }

        /// <summary>
        /// Selvittää onko tehtävä siirto sääntöjen mukainen
        /// </summary>
        /// <param name="mista">mistä siirretään</param>
        /// <param name="mihin">minne siirrettään</param>
        /// <returns>true jos saa siirtää, false jos ei saa siirtää</returns>
        public virtual bool onkoLaillinen(Nappula valittuNappula, Paikka mihin)
        {
            Paikka mista = valittuNappula.annaPaikka();

            string alku = mista.Name;
            string loppu = mihin.Name;

            kirjainNro1 = char.ToUpper(alku[0]) - 65; // 'A' on 65 joten siksi -65; ei varsinaisesti väliä
            kirjainNro2 = char.ToUpper(loppu[0]) - 65;

            numero1 = Int32.Parse(alku.Substring(1, alku.Length - 1));
            numero2 = Int32.Parse(loppu.Substring(1, loppu.Length - 1));

            return true;
        }
    }
}
