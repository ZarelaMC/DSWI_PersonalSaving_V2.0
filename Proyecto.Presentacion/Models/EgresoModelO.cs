using System.ComponentModel;

namespace FRONT_web_personal_saving.Models
{
    public class EgresoModelO
    {

        [DisplayName("CÓDIGO")]

        public int id { get; set; }
        [DisplayName("CÓDIGO USUARIO")]

        public int id_usuario { get; set; }

        [DisplayName("CÓDIGO TRANSACCIÓN")]

        public int id_transaccion { get; set; }

        [DisplayName("FECHA")]

        public DateTime fecha { get; set; }

        [DisplayName("MONTO")]
        public double monto { get; set; }

        [DisplayName("DESCRIPCIÓN")]
        public string? descripcion { get; set; }

        [DisplayName("ESTADO")]

        public int estado { get; set; }
    }
}
