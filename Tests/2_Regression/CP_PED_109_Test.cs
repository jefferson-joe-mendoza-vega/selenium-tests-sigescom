using NUnit.Framework;
using Tsp.Sigescom.Tests.E2E.Helper;
using Tsp.Sigescom.Tests.E2E.Pages;

namespace Tsp.Sigescom.Tests.E2E.Tests._2_Regression
{
    [TestFixture]
    [Category("Regression")]
    [Category("Alta")]
    public class CP_PED_109_Test : TestBase
    {
        [Test]
        [Description("CP-PED-109: Buscar por DNI de cliente")]
        public void BuscarPedido_DNI_EncuentraClienteExacto()
        {
            // Arrange
            var pedidosPage = new PedidosPage(Driver, this);

            pedidosPage.Navigate(BASE_URL);

            string dni = "81247593"; // Ana Patricia Rodríguez Torres

            // Act
            TestContext.WriteLine($"🔍 Paso 1: Buscar por DNI '{dni}'");
            pedidosPage.FiltrarPorCliente(dni);
            System.Threading.Thread.Sleep(2000);

            // Assert
            bool hayPedidos = pedidosPage.HayPedidos();
            Assert.That(hayPedidos, Is.True,
                $"❌ ERROR: No encuentra pedidos con DNI '{dni}'");
            TestContext.WriteLine($"✅ PV1: Encuentra pedidos del cliente Ana Patricia Rodríguez Torres");

            bool clienteEncontrado = pedidosPage.VerificarClienteEnResultados(dni, "Rodríguez");
            Assert.That(clienteEncontrado, Is.True,
                $"❌ ERROR: No se encontró cliente con DNI {dni}");
            TestContext.WriteLine($"✅ PV2: DNI {dni} correcto en resultados");

            bool verificaDni = pedidosPage.VerificarClienteEnPrimerPedido(dni);
            Assert.That(verificaDni, Is.True,
                "❌ ERROR: DNI no aparece en los resultados mostrados");
            TestContext.WriteLine("✅ PV3: Búsqueda exacta por documento funciona");

            TestContext.WriteLine($"✅ Búsqueda por DNI exitosa");
        }
    }
}
