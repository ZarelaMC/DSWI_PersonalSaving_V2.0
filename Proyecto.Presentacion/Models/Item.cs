using System.ComponentModel;

namespace FRONT_web_personal_saving.Models
{
    public class Item
    {
        [DisplayName("CODIGO")]
        public int codigo { get; set; }
        [DisplayName("DESCRIPCION")]
        public string descripcion { get; set; }
        [DisplayName("PRECIO")]
        public double precio { get; set; }
        [DisplayName("CANTIDAD")]
        public int cantidad { get; set; }
        private string _unidad = "";
        [DisplayName("UNIDAD")]
        public string unidad
        {
            set
            {
                if (value.Equals("MLL"))
                    _unidad = "MILLAR";
                else if (value.Equals("UNI"))
                    _unidad = "UNIDAD";
                else if (value.Equals("DOC"))
                    _unidad = "DOCENA";
                else if (value.Equals("CIE"))
                    _unidad = "CIENTO";
            }
            get
            {
                return _unidad;
            }
        }
        [DisplayName("SUBTOTAL")]
        public double subtotal
        {
            get { return precio * cantidad; }
        }
    }
}
