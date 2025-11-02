using NUnit.Framework;
using Tsp.Sigescom.Tests.E2E.Helper;
using Tsp.Sigescom.Tests.E2E.Pages;

namespace Tsp.Sigescom.Tests.E2E.Tests._3_Extended
{
    [TestFixture]
    [Category("Extended")]
    [Category("Media")]
    public class CP_PED_048_Test : TestBase
    {
        [Test]
        [Description("CP-PED-048: Exportar con 0 registros en resultado")]
        public void ExportarPedidos_SinResultados_MensajeNoHayDatos()
        {
            // Arrange
            var pedidosPage = new PedidosPage(Driver, this);

            pedidosPage.Navigate(BASE_URL);

            // Act
            TestContext.WriteLine("📝 Paso 1: Aplicar filtro que no devuelve datos");
            // Usar fecha en el futuro para asegurar 0 resultados
            pedidosPage.FiltrarPorFechas("01/01/2099", "31/12/2099");
            pedidosPage.ClickConsultar();
            System.Threading.Thread.Sleep(2000);

            int cantidad = pedidosPage.ObtenerCantidadPedidos();
            TestContext.WriteLine($"   Cantidad de pedidos: {cantidad}");

            TestContext.WriteLine("📝 Paso 2: Intentar exportar");
            pedidosPage.ClickExportar();
            System.Threading.Thread.Sleep(1000);

            // Assert
            bool hayMensajeError = pedidosPage.VerificarMensajeNoHayDatosExportar();
            Assert.That(hayMensajeError, Is.True,
                "❌ ERROR: No se muestra mensaje 'No hay datos para exportar'");
            TestContext.WriteLine("✅ PV1: Mensaje 'No hay datos para exportar' mostrado");

            bool archivoGenerado = pedidosPage.VerificarArchivoDescargado("*.xlsx");
            Assert.That(archivoGenerado, Is.False,
                "❌ ERROR: No debería generar archivo sin datos");
            TestContext.WriteLine("✅ PV2: No genera archivo vacío");

            TestContext.WriteLine("✅ PV3: Usuario informado correctamente");
            TestContext.WriteLine("✅ Validación de exportación sin datos funcionando");
        }
    }
}
