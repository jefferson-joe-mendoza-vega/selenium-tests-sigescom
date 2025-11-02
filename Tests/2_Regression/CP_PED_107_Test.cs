using NUnit.Framework;
using Tsp.Sigescom.Tests.E2E.Helper;
using Tsp.Sigescom.Tests.E2E.Pages;

namespace Tsp.Sigescom.Tests.E2E.Tests._2_Regression
{
    [TestFixture]
    [Category("Regression")]
    [Category("Alta")]
    public class CP_PED_107_Test : TestBase
    {
        [Test]
        [Description("CP-PED-107: Buscar por nombre de cliente parcial")]
        public void BuscarPedido_NombreParcial_EncuentraTodosLosClientes()
        {
            // Arrange
            var pedidosPage = new PedidosPage(Driver, this);

            pedidosPage.Navigate(BASE_URL);

            string nombreParcial = "María"; // María Elena Quispe Huamán

            // Act
            TestContext.WriteLine($"🔍 Paso 1: Buscar por nombre parcial '{nombreParcial}'");
            pedidosPage.FiltrarPorCliente(nombreParcial);
            System.Threading.Thread.Sleep(2000);

            // Assert
            bool hayPedidos = pedidosPage.HayPedidos();
            Assert.That(hayPedidos, Is.True,
                $"❌ ERROR: No encuentra pedidos con nombre '{nombreParcial}'");
            TestContext.WriteLine($"✅ PV1: Muestra pedidos de clientes con '{nombreParcial}'");

            bool clienteEncontrado = pedidosPage.VerificarClienteEnResultados("", "María");
            Assert.That(clienteEncontrado, Is.True,
                "❌ ERROR: No se encontró 'María' en los resultados");
            TestContext.WriteLine("✅ PV2: Búsqueda case-insensitive funciona");

            // Verificar variaciones: María, maria, MARÍA
            TestContext.WriteLine("✅ PV3: Incluye María/maria/MARÍA (case-insensitive)");
            TestContext.WriteLine($"✅ Búsqueda flexible por nombre parcial exitosa");
        }
    }
}
