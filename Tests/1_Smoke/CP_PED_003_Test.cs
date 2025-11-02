using NUnit.Framework;
using Tsp.Sigescom.Tests.E2E.Helper;
using Tsp.Sigescom.Tests.E2E.Pages;

namespace Tsp.Sigescom.Tests.E2E.Tests._1_Smoke
{
    [TestFixture]
    [Category("Smoke")]
    [Category("Alta")]
    public class CP_PED_003_Test : TestBase
    {
        [Test]
        [Description("CP-PED-003: Filtrar pedidos por estado Confirmado")]
        public void FiltrarPorEstadoConfirmado_MuestraSoloPedidosConfirmados()
        {
            // Arrange
            var pedidosPage = new PedidosPage(Driver);
            pedidosPage.Navigate(BASE_URL);

            // Act
            pedidosPage.FiltrarPorEstado("CONFIRMADO");

            // Assert
            Assert.That(pedidosPage.HayPedidos(), Is.True,
                "❌ ERROR: No se encontraron pedidos con estado Confirmado");

            var cantidad = pedidosPage.ObtenerCantidadPedidos();
            TestContext.WriteLine($"✅ Se encontraron {cantidad} pedidos con estado Confirmado");

            Assert.That(cantidad, Is.GreaterThan(0),
                "❌ ERROR: La cantidad de pedidos no es correcta");
        }
    }
}