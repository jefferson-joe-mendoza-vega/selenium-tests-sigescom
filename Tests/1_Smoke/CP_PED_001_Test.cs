using NUnit.Framework;
using Tsp.Sigescom.Tests.E2E.Helper;
using Tsp.Sigescom.Tests.E2E.Pages;

namespace Tsp.Sigescom.Tests.E2E.Tests._1_Smoke
{
    [TestFixture]
    [Category("Smoke")]
    [Category("Alta")]
    public class CP_PED_001_Test : TestBase
    {
        [Test]
        [Description("CP-PED-001: Consultar pedidos por rango de fechas válido")]
        public void ConsultarPedidos_RangoFechasValido_MuestraResultados()
        {
            // Arrange
            var pedidosPage = new PedidosPage(Driver);
            pedidosPage.Navigate(BASE_URL);

            // Act
            pedidosPage.FiltrarPorFechas("01/10/2025", "31/10/2025");
            pedidosPage.ClickConsultar();

            // Assert - FORMA EXPLÍCITA DE NUNIT 3
            Assert.That(pedidosPage.HayPedidos(), Is.True,
                "❌ ERROR: No se encontraron pedidos en el rango de fechas");

            var cantidad = pedidosPage.ObtenerCantidadPedidos();
            TestContext.WriteLine($"✅ Se encontraron {cantidad} pedidos");

            Assert.That(cantidad, Is.GreaterThan(0));
        }
    }
}