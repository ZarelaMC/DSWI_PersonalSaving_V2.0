namespace FRONT_web_personal_saving.Models
{
    public class Articulo
    {
        public int codigo { get; set; }
        public string descripcion { get; set; }
        public double precio { get; set; }
        public int stockActual { get; set; }
        public int stockMinimo { get; set; }
        public string unidad { get; set; }
    }
}
