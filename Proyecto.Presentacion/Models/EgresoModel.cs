using System.ComponentModel;

namespace FRONT_web_personal_saving.Models
{
    public class EgresoModel
    {

        [DisplayName("Código")]
        public int id { get; set; }

        [DisplayName("Fecha de registro")]
        public DateTime fecha { get; set; }

        [DisplayName("Monto")]
        public double monto { get; set; }

        [DisplayName("Descripción del egreso")]
        public string? descripcion { get; set; }

    }
}
