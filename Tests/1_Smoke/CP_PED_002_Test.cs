using NUnit.Framework;
using Tsp.Sigescom.Tests.E2E.Helper;
using Tsp.Sigescom.Tests.E2E.Pages;

namespace Tsp.Sigescom.Tests.E2E.Tests._1_Smoke
{
    [TestFixture]
    [Category("Smoke")]
    [Category("Alta")]
    public class CP_PED_002_Test : TestBase
    {
        [Test]
        [Description("CP-PED-002: Filtrar pedidos por estado Registrado")]
        public void FiltrarPorEstadoRegistrado_MuestraSoloPedidosRegistrados()
        {
            // Arrange
            var pedidosPage = new PedidosPage(Driver);
            pedidosPage.Navigate(BASE_URL);

            // Act
            pedidosPage.FiltrarPorEstado("REGISTRADO");

            // Assert
            Assert.That(pedidosPage.HayPedidos(), Is.True,
                "❌ ERROR: No se encontraron pedidos con estado Registrado");

            var cantidad = pedidosPage.ObtenerCantidadPedidos();
            TestContext.WriteLine($"✅ Se encontraron {cantidad} pedidos con estado Registrado");

            Assert.That(cantidad, Is.GreaterThan(0), "❌ ERROR: La cantidad de pedidos no es correcta");
        }
    }
}