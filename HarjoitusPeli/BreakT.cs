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
    /// Breakthrough lautapelin luokka
    /// </summary>
    class BreakT : Lautapeli
    {

        ///
        public BreakT(Brush vari1, Brush vari2) : base(vari1, vari2)
        {
        }

        /// <summary>
        /// Tarkistaa onko haluttu siirto sääntöjen mukainen
        /// </summary>
        /// <param name="valittuNappula">siirrettävä nappula</param>
        /// <param name="mihin">ruutu jonne halutaan siirtää</param>
        /// <returns>true jos laillinen, false jos ei ole</returns>
        public override bool onkoLaillinen(Nappula valittuNappula, Paikka mihin)
        {
            base.onkoLaillinen(valittuNappula, mihin);
            if (kirjainNro1 - kirjainNro2 > 1 || kirjainNro1 - kirjainNro2 < -1) return false; //false jos sivusiirto enemmän kuin yksi
            if (!mihin.OnkoTyhja && kirjainNro1 == kirjainNro2) return false; //estää syömisen suoraan edestä
            if (numero1 - numero2 != pelaajanVuoro) return false; //estetään väärään suuntaan liikkuminen
            return true;
        }
    }
}
