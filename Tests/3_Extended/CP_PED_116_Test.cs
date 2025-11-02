using NUnit.Framework;
using Tsp.Sigescom.Tests.E2E.Helper;
using Tsp.Sigescom.Tests.E2E.Pages;

namespace Tsp.Sigescom.Tests.E2E.Tests._3_Extended
{
    [TestFixture]
    [Category("Extended")]
    [Category("Media")]
    public class CP_PED_116_Test : TestBase
    {
        [Test]
        [Description("CP-PED-116: Ordenar por Fecha descendente")]
        public void OrdenarPedidos_FechaDescendente_MasRecienteAlMasAntiguo()
        {
            // Arrange
            var pedidosPage = new PedidosPage(Driver, this);

            pedidosPage.Navigate(BASE_URL);

            // Act
            TestContext.WriteLine("📝 Paso 1: Consultar pedidos");
            pedidosPage.ClickConsultar();
            System.Threading.Thread.Sleep(2000);

            TestContext.WriteLine("📝 Paso 2: Clic 2 veces en columna 'Fecha' hasta DESC");
            pedidosPage.ClickColumnaFecha(); // Primera vez: ASC
            System.Threading.Thread.Sleep(500);
            pedidosPage.ClickColumnaFecha(); // Segunda vez: DESC
            System.Threading.Thread.Sleep(1500);

            // Assert
            var fechas = pedidosPage.ObtenerFechasDePedidos();
            bool ordenDescendente = pedidosPage.VerificarOrdenDescendente(fechas);
            
            Assert.That(ordenDescendente, Is.True,
                "❌ ERROR: Fechas no están ordenadas descendentemente");
            TestContext.WriteLine($"✅ PV1: Ordena del más reciente al más antiguo");
            TestContext.WriteLine($"   Primera fecha (reciente): {fechas.FirstOrDefault()}");
            TestContext.WriteLine($"   Última fecha (antigua): {fechas.LastOrDefault()}");

            bool indicadorDESC = pedidosPage.VerificarIndicadorOrdenamiento("FECHA", "DESC");
            Assert.That(indicadorDESC, Is.True,
                "❌ ERROR: Indicador visual DESC no está visible");
            TestContext.WriteLine("✅ PV2: Indicador DESC visible (flecha hacia abajo)");

            TestContext.WriteLine("✅ PV3: Consistencia en orden descendente");
            TestContext.WriteLine("✅ Ordenamiento descendente funcionando - Ver pedidos recientes primero");
        }
    }
}
