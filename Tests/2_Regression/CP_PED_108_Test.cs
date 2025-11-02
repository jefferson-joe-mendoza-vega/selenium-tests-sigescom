using NUnit.Framework;
using Tsp.Sigescom.Tests.E2E.Helper;
using Tsp.Sigescom.Tests.E2E.Pages;

namespace Tsp.Sigescom.Tests.E2E.Tests._2_Regression
{
    [TestFixture]
    [Category("Regression")]
    [Category("Alta")]
    public class CP_PED_108_Test : TestBase
    {
        [Test]
        [Description("CP-PED-108: Buscar por apellido de cliente")]
        public void BuscarPedido_Apellido_EncuentraClienteCorrect()
        {
            // Arrange
            var pedidosPage = new PedidosPage(Driver, this);

            pedidosPage.Navigate(BASE_URL);

            string apellido = "Flores"; // Jorge Luis Flores Sánchez DNI 58471629

            // Act
            TestContext.WriteLine($"🔍 Paso 1: Buscar por apellido '{apellido}'");
            pedidosPage.FiltrarPorCliente(apellido);
            System.Threading.Thread.Sleep(2000);

            // Assert
            bool hayPedidos = pedidosPage.HayPedidos();
            Assert.That(hayPedidos, Is.True,
                $"❌ ERROR: No encuentra pedidos con apellido '{apellido}'");
            TestContext.WriteLine($"✅ PV1: Encuentra pedidos del cliente Jorge Luis Flores Sánchez");

            bool clienteEncontrado = pedidosPage.VerificarClienteEnResultados("58471629", "Flores");
            Assert.That(clienteEncontrado, Is.True,
                "❌ ERROR: No se encontró cliente con apellido Flores");
            TestContext.WriteLine("✅ PV2: Búsqueda por apellido funciona correctamente");

            int cantidad = pedidosPage.ObtenerCantidadPedidos();
            TestContext.WriteLine($"✅ PV3: Múltiples pedidos encontrados ({cantidad}) si existen");
            TestContext.WriteLine($"✅ Búsqueda por apellido exitosa");
        }
    }
}
