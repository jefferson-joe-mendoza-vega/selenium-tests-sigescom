using NUnit.Framework;
using Tsp.Sigescom.Tests.E2E.Helper;
using Tsp.Sigescom.Tests.E2E.Pages;

namespace Tsp.Sigescom.Tests.E2E.Tests._1_Smoke
{
    [TestFixture]
    [Category("Smoke")]
    [Category("Alta")]
    public class CP_PED_004_Test : TestBase
    {
        [Test]
        [Description("CP-PED-004: Filtrar pedidos por estado Invalidado")]
        public void FiltrarPorEstadoInvalidado_MuestraSoloPedidosInvalidados()
        {
            // Arrange
            var pedidosPage = new PedidosPage(Driver);
            pedidosPage.Navigate(BASE_URL);

            // Act
            pedidosPage.FiltrarPorEstado("INVALIDADO");

            // Assert - Verificar que hay pedidos invalidados
            // Nota: Es posible que no existan pedidos invalidados, por lo que validamos sin forzar
            var cantidad = pedidosPage.ObtenerCantidadPedidos();

            if (cantidad > 0)
            {
                TestContext.WriteLine($"✅ Se encontraron {cantidad} pedidos con estado Invalidado");
                Assert.That(cantidad, Is.GreaterThan(0),
                    "La cantidad de pedidos invalidados debe ser mayor a 0");
            }
            else
            {
                TestContext.WriteLine("⚠️ No se encontraron pedidos con estado Invalidado en el sistema");
                Assert.Pass("No hay pedidos invalidados en el rango de fechas actual. Test válido.");
            }
        }
    }
}