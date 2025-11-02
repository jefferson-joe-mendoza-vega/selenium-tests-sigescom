using NUnit.Framework;
using Tsp.Sigescom.Tests.E2E.Helper;
using Tsp.Sigescom.Tests.E2E.Pages;

namespace Tsp.Sigescom.Tests.E2E.Tests._3_Extended
{
    [TestFixture]
    [Category("Extended")]
    [Category("Media")]
    public class CP_PED_084_Test : TestBase
    {
        [Test]
        [Description("CP-PED-084: Exportar con 0 registros (caso límite)")]
        public void ExportarPedidos_RangoVacio_NoGeneraArchivo()
        {
            // Arrange
            var pedidosPage = new PedidosPage(Driver, this);

            pedidosPage.Navigate(BASE_URL);

            // Act
            TestContext.WriteLine("📝 Paso 1: Aplicar filtro sin resultados");
            // Fecha rango vacío (antes de existir registros)
            pedidosPage.FiltrarPorFechas("01/01/2000", "31/12/2000");
            pedidosPage.ClickConsultar();
            System.Threading.Thread.Sleep(2000);

            bool hayPedidos = pedidosPage.HayPedidos();
            TestContext.WriteLine($"   Hay pedidos: {hayPedidos}");

            TestContext.WriteLine("📝 Paso 2: Intentar exportar");
            pedidosPage.ClickExportar();
            System.Threading.Thread.Sleep(1500);

            // Assert
            bool hayMensaje = pedidosPage.VerificarMensajeNoHayDatosExportar();
            Assert.That(hayMensaje, Is.True,
                "❌ ERROR: No muestra mensaje 'No hay datos para exportar'");
            TestContext.WriteLine("✅ PV1: Mensaje 'No hay datos para exportar' visible");

            bool archivoVacio = pedidosPage.VerificarArchivoDescargado("*.xlsx");
            Assert.That(archivoVacio, Is.False,
                "❌ ERROR: No debe generar archivo vacío");
            TestContext.WriteLine("✅ PV2: No genera archivo vacío");

            // Verificar que no hay errores de JavaScript
            var logs = Driver.Manage().Logs.GetLog("browser");
            bool hayErrores = logs.Any(l => l.Level == OpenQA.Selenium.LogLevel.Severe);
            Assert.That(hayErrores, Is.False,
                "❌ ERROR: Hay errores de JavaScript en la consola");
            TestContext.WriteLine("✅ PV3: Sin errores de JavaScript");

            TestContext.WriteLine("✅ Caso límite de exportación manejado correctamente");
        }
    }
}
