using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Linq;
using Tsp.Sigescom.Tests.E2E.Helper;

namespace Tsp.Sigescom.Tests.E2E.Pages
{
    public class PedidosPage
    {
        private readonly IWebDriver _driver;
        private readonly WebDriverWait _wait;
        private readonly TestBase _testBase;

        // Constructor acepta TestBase como OPCIONAL (null por defecto)
        public PedidosPage(IWebDriver driver, TestBase testBase = null)
        {
            _driver = driver;
            _wait = new WebDriverWait(driver, TimeSpan.FromSeconds(15));
            _testBase = testBase;
        }

        public void Navigate(string baseUrl)
        {
            _driver.Navigate().GoToUrl($"{baseUrl}/Pedido/Index");
            System.Threading.Thread.Sleep(3000);

            if (_driver.Url.Contains("no encontrada") || _driver.PageSource.Contains("Página no encontrada"))
            {
                throw new Exception("No se pudo cargar la página de Pedidos");
            }
        }

        public void FiltrarPorFechas(string fechaInicial, string fechaFinal)
        {
            try
            {
                var inputs = _driver.FindElements(By.CssSelector("input.form-control[data-provide='datepicker']"));

                if (inputs.Count >= 2)
                {
                    inputs[0].Clear();
                    inputs[0].SendKeys(fechaInicial);
                    inputs[0].SendKeys(Keys.Tab);
                    System.Threading.Thread.Sleep(300);

                    inputs[1].Clear();
                    inputs[1].SendKeys(fechaFinal);
                    inputs[1].SendKeys(Keys.Tab);
                    System.Threading.Thread.Sleep(300);

                    return;
                }
            }
            catch
            {
                throw new Exception("No se encontraron los campos de fecha para filtrar.");
            }
        }

        public void FiltrarPorEstado(string estado)
        {
            try
            {
                var todosLosInputs = _driver.FindElements(By.CssSelector("table thead input[type='text'].form-control"));

                if (todosLosInputs.Count > 0)
                {
                    var inputEstado = todosLosInputs[todosLosInputs.Count - 1];
                    inputEstado.Clear();
                    inputEstado.SendKeys(estado);
                    System.Threading.Thread.Sleep(1000);
                    return;
                }
            }
            catch { }

            try
            {
                var inputEstado = _driver.FindElement(By.XPath("//th[contains(text(),'ESTADO')]/parent::tr/following-sibling::tr//input[@type='text']"));
                inputEstado.Clear();
                inputEstado.SendKeys(estado);
                System.Threading.Thread.Sleep(1000);
                return;
            }
            catch { }

            try
            {
                var inputEstado = _driver.FindElement(By.XPath("(//table/thead//tr[2]//input[@type='text'])[last()]"));
                inputEstado.Clear();
                inputEstado.SendKeys(estado);
                System.Threading.Thread.Sleep(1000);
                return;
            }
            catch (Exception ex)
            {
                throw new Exception($"❌ No se pudo aplicar el filtro por estado '{estado}': {ex.Message}");
            }
        }

        public void FiltrarPorCliente(string dniCliente)
        {
            try
            {
                var todosLosInputs = _driver.FindElements(By.CssSelector("table thead input[type='text'].form-control"));

                if (todosLosInputs.Count >= 4)
                {
                    var inputCliente = todosLosInputs[3];
                    inputCliente.Clear();
                    inputCliente.SendKeys(dniCliente);
                    System.Threading.Thread.Sleep(1000);
                    return;
                }
            }
            catch { }

            try
            {
                var inputCliente = _driver.FindElement(By.XPath("//th[normalize-space(text())='CLIENTE']/following::input[@type='text'][1]"));
                inputCliente.Clear();
                inputCliente.SendKeys(dniCliente);
                System.Threading.Thread.Sleep(1000);
                return;
            }
            catch { }

            try
            {
                var inputBuscar = _driver.FindElement(By.XPath("//label[contains(text(),'Buscar:')]/following-sibling::input"));
                inputBuscar.Clear();
                inputBuscar.SendKeys(dniCliente);
                System.Threading.Thread.Sleep(1000);
                return;
            }
            catch (Exception ex)
            {
                throw new Exception($"❌ No se pudo aplicar el filtro por cliente con DNI '{dniCliente}': {ex.Message}");
            }
        }

        public bool VerificarClienteEnResultados(string dniCliente, string nombreCliente)
        {
            try
            {
                var filas = _driver.FindElements(By.CssSelector("table tbody tr"));

                foreach (var fila in filas)
                {
                    var textoFila = fila.Text;

                    if (textoFila.Contains("NO HAY DATOS") || string.IsNullOrWhiteSpace(textoFila))
                    {
                        continue;
                    }

                    if (textoFila.Contains(dniCliente) || textoFila.Contains(nombreCliente))
                    {
                        return true;
                    }
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        public void ClickConsultar()
        {
            try
            {
                var btnConsultar = _wait.Until(d => d.FindElement(By.XPath("//button[@title='CONSULTAR']")));
                btnConsultar.Click();
                System.Threading.Thread.Sleep(3000);
            }
            catch (Exception ex)
            {
                throw new Exception($"❌ No se pudo hacer clic en Consultar: {ex.Message}");
            }
        }

        public int ObtenerCantidadPedidos()
        {
            try
            {
                var filas = _driver.FindElements(By.CssSelector("table tbody tr"));

                var filasValidas = filas.Where(f =>
                    !f.Text.Contains("NO HAY DATOS") &&
                    !f.Text.Contains("NO HAY DATOS DISPONIBLES") &&
                    !string.IsNullOrWhiteSpace(f.Text)
                ).ToList();

                return filasValidas.Count;
            }
            catch
            {
                return 0;
            }
        }

        public bool HayPedidos()
        {
            return ObtenerCantidadPedidos() > 0;
        }
    }
}