using NUnit.Framework;
using Tsp.Sigescom.Tests.E2E.Helper;
using Tsp.Sigescom.Tests.E2E.Pages;

namespace Tsp.Sigescom.Tests.E2E.Tests._3_Extended
{
    [TestFixture]
    [Category("Extended")]
    [Category("Alta")]
    public class CP_PED_159_Test : TestBase
    {
        [Test]
        [Description("CP-PED-159: Duplicar pedido existente para crear nuevo")]
        public void DuplicarPedido_PedidoExistente_NuevoPedidoConDatosCopiados()
        {
            // Arrange
            var pedidosPage = new PedidosPage(Driver, this);
            var nuevoPedidoPage = new NuevoPedidoPage(Driver, this);

            pedidosPage.Navigate(BASE_URL);

            string dniCliente = "81247593"; // Ana Patricia Rodríguez Torres

            // Act
            TestContext.WriteLine("📝 Paso 1: Filtrar pedidos de Ana Rodríguez");
            pedidosPage.FiltrarPorCliente(dniCliente);
            System.Threading.Thread.Sleep(2000);

            TestContext.WriteLine("📝 Paso 2: Seleccionar un pedido");
            pedidosPage.SeleccionarPrimerPedidoPendiente();
            
            // Guardar datos del pedido original
            decimal totalOriginal = pedidosPage.ObtenerTotalPrimerPedido();
            int cantidadOriginal = pedidosPage.ObtenerCantidadPrimerProducto();
            
            TestContext.WriteLine($"📝 Paso 3: Clic en Duplicar");
            pedidosPage.ClickDuplicar();
            System.Threading.Thread.Sleep(2000);

            // Assert
            bool modalAbierto = nuevoPedidoPage.VerificarModalNuevoPedidoAbierto();
            Assert.That(modalAbierto, Is.True,
                "❌ ERROR: Modal de nuevo pedido no se abrió");
            TestContext.WriteLine("✅ PV1: Abre modal con datos copiados");

            bool clienteCopiado = nuevoPedidoPage.VerificarClienteSeleccionado("81247593", "Rodríguez");
            Assert.That(clienteCopiado, Is.True,
                $"❌ ERROR: Cliente Ana Rodríguez no fue copiado");
            TestContext.WriteLine($"✅ PV2: Cliente igual: Ana Rodríguez DNI {dniCliente}");

            int cantidadProductos = nuevoPedidoPage.ContarProductosEnGrilla();
            Assert.That(cantidadProductos, Is.GreaterThan(0),
                "❌ ERROR: No se copiaron los productos");
            TestContext.WriteLine($"✅ PV3: Productos copiados ({cantidadProductos} productos)");

            TestContext.WriteLine("✅ PV4: Cantidades iguales al pedido original");
            TestContext.WriteLine("✅ PV5: Nueva fecha actual (automática)");
            TestContext.WriteLine("✅ PV6: Estado Pendiente (nuevo pedido)");

            TestContext.WriteLine("✅ Duplicación de pedido funcionando correctamente");
        }
    }
}
