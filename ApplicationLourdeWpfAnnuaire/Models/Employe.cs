using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationLourdeWpfAnnuaire.Models
{
    public class Employe
    {
        public int EmployeID { get; set; }
        public string? Nom { get; set; }
        public string? Prenom { get; set; }
        public string? TelephoneFixe { get; set; }
        public string? TelephonePortable { get; set; }
        public string? Email { get; set; }
        public int SiteID { get; set; }
        public virtual Site? Site { get; set; }
        public int DepartementID { get; set; }
        public virtual Departement? Departement { get; set; }
        public string? EmployeSite { get; set; }
        public string? EmployeDepartement { get; set; }
    }
}
