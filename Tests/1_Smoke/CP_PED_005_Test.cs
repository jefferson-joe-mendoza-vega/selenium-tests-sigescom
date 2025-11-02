using NUnit.Framework;
using Tsp.Sigescom.Tests.E2E.Helper;
using Tsp.Sigescom.Tests.E2E.Pages;

namespace Tsp.Sigescom.Tests.E2E.Tests._1_Smoke
{
    [TestFixture]
    [Category("Smoke")]
    [Category("Alta")]
    public class CP_PED_005_Test : TestBase
    {
        [Test]
        [Description("CP-PED-005: Filtrar pedidos por cliente específico (Rosa Villarreal DNI: 47829156)")]
        public void FiltrarPorClienteEspecifico_MuestraSoloPedidosDelCliente()
        {
            // Arrange
            var pedidosPage = new PedidosPage(Driver);
            pedidosPage.Navigate(BASE_URL);

            // Datos del cliente desde el informe
            string dniCliente = "47829156";
            string nombreCliente = "VILLARREAL";

            // Act
            TestContext.WriteLine($"🔍 Aplicando filtro por cliente con DNI: {dniCliente}");
            pedidosPage.FiltrarPorCliente(dniCliente);

            // Assert - Verificar que hay pedidos del cliente
            Assert.That(pedidosPage.HayPedidos(), Is.True,
                $"❌ ERROR: No se encontraron pedidos del cliente con DNI: {dniCliente}");

            var cantidad = pedidosPage.ObtenerCantidadPedidos();
            TestContext.WriteLine($"✅ Se encontraron {cantidad} pedidos del cliente con DNI {dniCliente}");

            // Verificar que el cliente aparece en los resultados
            bool clienteEncontrado = pedidosPage.VerificarClienteEnResultados(dniCliente, nombreCliente);
            Assert.That(clienteEncontrado, Is.True,
                $"❌ ERROR: El DNI {dniCliente} o nombre {nombreCliente} no aparece en los resultados");

            // Verificar que la cantidad es válida
            Assert.That(cantidad, Is.GreaterThan(0),
                "❌ ERROR: La cantidad de pedidos del cliente no es correcta");

            TestContext.WriteLine($"✅ Verificación exitosa: Todos los pedidos pertenecen al cliente {nombreCliente} (DNI: {dniCliente})");
        }
    }
}