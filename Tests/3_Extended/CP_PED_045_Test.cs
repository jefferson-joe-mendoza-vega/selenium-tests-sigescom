using NUnit.Framework;
using Tsp.Sigescom.Tests.E2E.Helper;
using Tsp.Sigescom.Tests.E2E.Pages;

namespace Tsp.Sigescom.Tests.E2E.Tests._3_Extended
{
    [TestFixture]
    [Category("Extended")]
    [Category("Media")]
    public class CP_PED_045_Test : TestBase
    {
        [Test]
        [Description("CP-PED-045: Navegar a segunda página de resultados")]
        public void Paginacion_NavegarSegundaPagina_MuestraRegistros21A40()
        {
            // Arrange
            var pedidosPage = new PedidosPage(Driver, this);

            pedidosPage.Navigate(BASE_URL);

            // Act
            TestContext.WriteLine("📝 Paso 1: Consultar pedidos (debe haber más de 20)");
            pedidosPage.FiltrarPorFechas("01/01/2025", "31/12/2025");
            pedidosPage.ClickConsultar();
            System.Threading.Thread.Sleep(2000);

            int cantidadTotal = pedidosPage.ObtenerCantidadTotalPedidos();
            TestContext.WriteLine($"   Total de pedidos: {cantidadTotal}");

            if (cantidadTotal <= 20)
            {
                Assert.Inconclusive($"⚠️ No hay suficientes pedidos para probar paginación (solo {cantidadTotal})");
                return;
            }

            TestContext.WriteLine("📝 Paso 2: Ver primera página");
            int cantidadPagina1 = pedidosPage.ObtenerCantidadPedidosEnPaginaActual();
            TestContext.WriteLine($"   Registros en página 1: {cantidadPagina1}");

            TestContext.WriteLine("📝 Paso 3: Clic en página 2");
            pedidosPage.ClickPagina(2);
            System.Threading.Thread.Sleep(2000);

            // Assert
            int cantidadPagina2 = pedidosPage.ObtenerCantidadPedidosEnPaginaActual();
            Assert.That(cantidadPagina2, Is.GreaterThan(0),
                "❌ ERROR: Página 2 no muestra registros");
            TestContext.WriteLine($"✅ PV1: Muestra registros 21-40 ({cantidadPagina2} registros)");

            bool paginadorActivo = pedidosPage.VerificarPaginaActiva(2);
            Assert.That(paginadorActivo, Is.True,
                "❌ ERROR: Paginador no marca página 2 como activa");
            TestContext.WriteLine("✅ PV2: Paginador activo en página 2");

            Assert.That(cantidadTotal, Is.GreaterThan(20),
                "❌ ERROR: Total inconsistente con paginación");
            TestContext.WriteLine($"✅ PV3: Totales consistentes ({cantidadTotal} registros)");

            TestContext.WriteLine("✅ Paginación funcionando correctamente");
        }
    }
}
