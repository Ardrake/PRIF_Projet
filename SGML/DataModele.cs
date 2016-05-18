using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGML
{
    class DataModele
    {
        public class Client // classe du client
        {
            private int _idclient { get; }
            private string _prenom { get; set; }
            private string _nom { get; set; }
            private string _adresse { get; set; }
            private string _ville { get; set; }
            private string _province { get; set; }
            private string _codepostal { get; set; }
            private string _telephone { get; set; }

            public Client(string prenom, string nom, string adresse, string ville, string province,
                          string codepostal, string telephone)
            {
                _prenom = prenom;
                _nom = nom;
                _adresse = adresse;
                _ville = ville;
                _province = province;
                _codepostal = codepostal;
                _telephone = telephone;
            }

            public string Prenom
            {
                get { return this._prenom; }
                set { this._prenom = value; }
            }

            public string GetNomPrenom(int idclient)
            {
                return this._nom + ", " + this._prenom;
            }
        }


    }
}
