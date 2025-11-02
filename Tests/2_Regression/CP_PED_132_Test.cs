using NUnit.Framework;
using Tsp.Sigescom.Tests.E2E.Helper;
using Tsp.Sigescom.Tests.E2E.Pages;

namespace Tsp.Sigescom.Tests.E2E.Tests._2_Regression
{
    [TestFixture]
    [Category("Regression")]
    [Category("Critica")]
    public class CP_PED_132_Test : TestBase
    {
        [Test]
        [Description("CP-PED-132: Validar stock suficiente antes de confirmar pedido")]
        public void ConfirmarPedido_StockInsuficiente_ErrorYPedidoQuedaPendiente()
        {
            // Arrange
            var pedidosPage = new PedidosPage(Driver, this);
            var nuevoPedidoPage = new NuevoPedidoPage(Driver, this);

            pedidosPage.Navigate(BASE_URL);

            string dniCliente = "47829156"; // Rosa Villarreal
            // Nota: Necesitas un producto con stock bajo (ej. stock=5)
            string productoStockBajo = "PRODUCTO-STOCK-5";
            int cantidadSolicitada = 10; // Más que el stock disponible

            // Act
            TestContext.WriteLine("📝 Paso 1: Crear pedido con cantidad mayor al stock");
            nuevoPedidoPage.ClickNuevoPedido();
            nuevoPedidoPage.BuscarCliente(dniCliente);
            nuevoPedidoPage.AgregarProducto(productoStockBajo, cantidadSolicitada);
            nuevoPedidoPage.ClickGuardar();
            System.Threading.Thread.Sleep(2000);

            TestContext.WriteLine("📝 Paso 2: Intentar confirmar el pedido");
            pedidosPage.SeleccionarPrimerPedidoPendiente();
            pedidosPage.ClickConfirmar();
            System.Threading.Thread.Sleep(1000);

            // Assert
            bool hayMensajeStockInsuficiente = nuevoPedidoPage.VerificarMensajeStockInsuficiente();
            Assert.That(hayMensajeStockInsuficiente, Is.True,
                "❌ ERROR: No se muestra mensaje de stock insuficiente");
            TestContext.WriteLine("✅ PV1: Error 'Stock insuficiente' mostrado");

            bool stockActualMostrado = nuevoPedidoPage.VerificarStockActualMostrado();
            Assert.That(stockActualMostrado, Is.True,
                "❌ ERROR: No se muestra el stock disponible actual");
            TestContext.WriteLine("✅ PV2: Detalla producto y stock disponible");

            string estadoPedido = pedidosPage.ObtenerEstadoPrimerPedido();
            Assert.That(estadoPedido, Is.EqualTo("PENDIENTE").Or.EqualTo("REGISTRADO"),
                $"❌ ERROR: Estado {estadoPedido} debería ser PENDIENTE, no se confirmó");
            TestContext.WriteLine($"✅ PV3: No permite confirmar - Estado: {estadoPedido}");

            TestContext.WriteLine("✅ PV4: Pedido queda Pendiente correctamente");
            TestContext.WriteLine("✅ Validación de stock antes de confirmar funcionando correctamente");
        }
    }
}
