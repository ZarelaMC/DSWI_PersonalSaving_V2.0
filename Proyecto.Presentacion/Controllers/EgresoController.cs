using FRONT_web_personal_saving.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Proyecto.Presentacion.Models;
using System.Text;

namespace Proyecto.Presentacion.Controllers
{
    public class EgresoController : Controller
    {


        //Cadena conexión hacia el servicio
        Uri baseAddress = new Uri("https://localhost:7035/api");

        private readonly HttpClient _httpClient;

        public EgresoController()
        {
            HttpClientHandler handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };

            _httpClient = new HttpClient(handler);
            _httpClient.BaseAddress = baseAddress;
        }



        //Listado Egresos
        [HttpGet]
        public IActionResult listarEgresos()
        {
            List<EgresoModel> aEgresos = new List<EgresoModel>();
            HttpResponseMessage response =
                _httpClient.GetAsync(_httpClient.BaseAddress + "/Egresos/listadoEgresos").Result;
            if (response.IsSuccessStatusCode)
            {
                var data = response.Content.ReadAsStringAsync().Result;
                aEgresos = JsonConvert.DeserializeObject<List<EgresoModel>>(data);
                @ViewBag.Egresos = aEgresos;
                @ViewBag.primero = aEgresos.FirstOrDefault();
            }
            return View(aEgresos);
        }

        //Registro Egresos
        [HttpGet]
        public IActionResult NuevoEgreso()
        {
            return View(new EgresoModelO());
        }
        [HttpPost]
        public async Task<IActionResult> NuevoEgreso(EgresoModelO objE)
        {
            if (!ModelState.IsValid)
            {
                return View(new EgresoModelO());
            }
            var json = JsonConvert.SerializeObject(objE);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var responseC = await
            _httpClient.PostAsync("api/Egresos/nuevoEgreso", content);
            if (responseC.IsSuccessStatusCode)
            {
                ViewBag.mensaje = "Egreso registrado correctamente..!!!";
            }
            //ViewBag.codigoI = json;
            return View(objE);
        }

        //Actualización de Egresos
        [HttpGet]
        public async Task<IActionResult> ModificarEgreso(int id)
        {
            var response = await
            _httpClient.GetAsync(_httpClient.BaseAddress + "/Egresos/buscarEgreso/" + id);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var objE = JsonConvert.DeserializeObject<EgresoModelO>(content);
                return View(objE);
            }
            else
            {
                ViewBag.mensaje = "No hay Egreso!!!";
            }

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ModificarEgreso(int id, EgresoModelO objE)
        {

            var json = JsonConvert.SerializeObject(objE);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync("api/Egresos/modificaEgreso?id={ id }", content);
            if (response.IsSuccessStatusCode)
            {
                ViewBag.mensaje = "Egreso actualizado correctamente..!!!";
            }
            return View(objE);
        }


        //Eliminación 
        [HttpDelete]
        public async Task<IActionResult> EliminarEgreso(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/Egresos/eliminaEgreso/{id}");

            if (response.IsSuccessStatusCode)
            {
                var responseData = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<dynamic>(responseData);

                if (result.success == true)
                {
                    ViewBag.mensaje = "Egreso eliminado correctamente..!!!";
                    return RedirectToAction("listarEgresos"); // Redirigir al listado después de la eliminación
                }
                else
                {
                    ViewBag.mensaje = "No se eliminó el egreso: " + result.message;
                    return RedirectToAction("listarEgresos");
                }
            }
            else
            {
                ViewBag.mensaje = "Error en la comunicación con el servidor.";
                return RedirectToAction("listarEgresos");
            }
        }


        public IActionResult Index()
        {
            return View();
        }
    }
}

