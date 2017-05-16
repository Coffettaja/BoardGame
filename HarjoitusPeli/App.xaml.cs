using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;

namespace HarjoitusPeli
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Ohjelman käynnistyessä avataan asetusikkuna ja asetetaan halutut asetukset peliin
        /// </summary>
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            MainWindow paaIkkuna = new MainWindow();
            SettingsWindow aloitusIkkuna = new SettingsWindow();
            aloitusIkkuna.ShowDialog();
            int koko = aloitusIkkuna.Koko;
            string nimi1 = aloitusIkkuna.Pelaaja1;
            string nimi2 = aloitusIkkuna.Pelaaja2;
            var ruutuvari1 = aloitusIkkuna.RuutuVari1;
            var ruutuvari2 = aloitusIkkuna.RuutuVari2;
            var nappulavari1 = aloitusIkkuna.NappulaVari1;
            var nappulavari2 = aloitusIkkuna.NappulaVari2;
            string peli = aloitusIkkuna.ValittuPeli;
            if (aloitusIkkuna.Asetetaanko == true) paaIkkuna.asetaTiedot(nimi1, nimi2, koko, ruutuvari1, ruutuvari2, nappulavari1, nappulavari2);
            paaIkkuna.Show();
        }
    }
}
