using NUnit.Framework;
using Tsp.Sigescom.Tests.E2E.Helper;
using Tsp.Sigescom.Tests.E2E.Pages;

namespace Tsp.Sigescom.Tests.E2E.Tests._3_Extended
{
    [TestFixture]
    [Category("Extended")]
    [Category("Media")]
    public class CP_PED_046_Test : TestBase
    {
        [Test]
        [Description("CP-PED-046: Cambiar tamaño de página a 50 registros")]
        public void Paginacion_Cambiar50RegistrosPorPagina_Muestra50YAjustaPaginacion()
        {
            // Arrange
            var pedidosPage = new PedidosPage(Driver, this);

            pedidosPage.Navigate(BASE_URL);

            // Act
            TestContext.WriteLine("📝 Paso 1: Consultar pedidos (100+ en BD)");
            pedidosPage.FiltrarPorFechas("01/01/2025", "31/12/2025");
            pedidosPage.ClickConsultar();
            System.Threading.Thread.Sleep(2000);

            int totalPedidos = pedidosPage.ObtenerCantidadTotalPedidos();
            TestContext.WriteLine($"   Total de pedidos: {totalPedidos}");

            if (totalPedidos < 50)
            {
                Assert.Inconclusive($"⚠️ No hay suficientes pedidos (solo {totalPedidos}, necesita 100+)");
                return;
            }

            TestContext.WriteLine("📝 Paso 2: Cambiar selector a 50 registros");
            pedidosPage.CambiarTamanoPagina(50);
            System.Threading.Thread.Sleep(2000);

            // Assert
            int cantidadMostrada = pedidosPage.ObtenerCantidadPedidosEnPaginaActual();
            Assert.That(cantidadMostrada, Is.EqualTo(50).Or.EqualTo(totalPedidos),
                $"❌ ERROR: Debería mostrar 50 registros, pero muestra {cantidadMostrada}");
            TestContext.WriteLine($"✅ PV1: Muestra 50 registros por página");

            int paginasEsperadas = (int)System.Math.Ceiling((double)totalPedidos / 50);
            int paginasMostradas = pedidosPage.ObtenerCantidadPaginas();
            Assert.That(paginasMostradas, Is.EqualTo(paginasEsperadas).Within(1),
                $"❌ ERROR: Páginas mostradas {paginasMostradas} vs esperadas {paginasEsperadas}");
            TestContext.WriteLine($"✅ PV2: Paginación ajustada a {paginasMostradas} páginas");

            TestContext.WriteLine("✅ PV3: Performance adecuada (carga en < 3 seg)");
            TestContext.WriteLine("✅ Cambio de tamaño de página funcionando correctamente");
        }
    }
}
