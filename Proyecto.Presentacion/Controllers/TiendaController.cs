using FRONT_web_personal_saving.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace FRONT_web_personal_saving.Controllers
{
    public class TiendaController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }



        private readonly ILogger<TiendaController> _logger;
        private readonly string? _cadena;

        public TiendaController(ILogger<TiendaController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _cadena = configuration.GetConnectionString("cn");
        }

        List<Articulo> listadoArticulos()
        {
            List<Articulo> aArticulo = new List<Articulo>();

            using (var cn = new SqlConnection(_cadena))
            {
                SqlCommand cmd = new SqlCommand("SP_LISTAARTICULOS", cn);
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    Articulo objA = new Articulo()
                    {
                        codigo = int.Parse(dr[0].ToString()),
                        descripcion = dr[1].ToString(),
                        precio = double.Parse(dr[2].ToString()),
                        stockActual = int.Parse(dr[3].ToString()),
                        stockMinimo = int.Parse(dr[4].ToString()),
                        unidad = dr[5].ToString()
                    };
                    aArticulo.Add(objA);
                }
            }
            return aArticulo;
        }

        // Método para listar los artículos
        public ActionResult carritoCompras()
        {
        // Método para seleccionar los artículos adquiridos
            return View(listadoArticulos());
        }

        public ActionResult Select(int id)
        {
            Articulo objA = listadoArticulos().Where(a => a.codigo == id).FirstOrDefault();
            return View(objA);
        }

        // Método para agregar un producto adquirido desde carrito de compras
        public ActionResult Agregar(int id)
        {
            var art = listadoArticulos().Where(a => a.codigo == id).FirstOrDefault();
            if (art != null)
            {
                Item objI = new Item
                {
                    codigo = art.codigo,
                    descripcion = art.descripcion,
                    precio = art.precio,
                    unidad = art.unidad,
                    cantidad = 1
                };

                List<Item> miCarrito = HttpContext.Session.GetObjectFromJson<List<Item>>("carrito") ?? new List<Item>();
                miCarrito.Add(objI);
                HttpContext.Session.SetObjectAsJson("carrito", miCarrito);
            }
            return RedirectToAction("carritoCompras");
        }

        // Método que calcula el subtotal de toda la compra
        public ActionResult Comprar()
        {
            var carrito = HttpContext.Session.GetObjectFromJson<List<Item>>("carrito");
            if (carrito == null)
            {
                return RedirectToAction("carritoCompras");
            }
            ViewBag.monto = carrito.Sum(p => p.subtotal);
            return View(carrito);
        }

        // Método para eliminar un producto del carrito
        public ActionResult Eliminar(int id)
        {
            var carrito = HttpContext.Session.GetObjectFromJson<List<Item>>("carrito");
            if (carrito != null)
            {
                var item = carrito.Where(i => i.codigo == id).FirstOrDefault();
                if (item != null)
                {
                    carrito.Remove(item);
                    HttpContext.Session.SetObjectAsJson("carrito", carrito);
                }
            }
            return RedirectToAction("Comprar");
        }

        // Método para pagar
        public ActionResult Pago()
        {
            var detalle = HttpContext.Session.GetObjectFromJson<List<Item>>("carrito");
            double mt = 0;
            if (detalle != null)
            {
                mt = detalle.Sum(it => it.subtotal);
            }
            ViewBag.mt = mt;
            return View(detalle);
        }
    }


}

// Extensiones de sesión para almacenar y recuperar objetos complejos
public static class SessionExtensions
{
    public static void SetObjectAsJson(this ISession session, string key, object value)
    {
        session.SetString(key, JsonConvert.SerializeObject(value));
    }

    public static T GetObjectFromJson<T>(this ISession session, string key)
    {
        var value = session.GetString(key);
        return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
    }
}
