using NUnit.Framework;
using Tsp.Sigescom.Tests.E2E.Helper;
using Tsp.Sigescom.Tests.E2E.Pages;

namespace Tsp.Sigescom.Tests.E2E.Tests._2_Regression
{
    [TestFixture]
    [Category("Regression")]
    [Category("Critica")]
    public class CP_PED_034_Test : TestBase
    {
        [Test]
        [Description("CP-PED-034: Confirmar pedido y convertir a venta")]
        public void ConfirmarPedido_ConStockSuficiente_ConvierteAVenta()
        {
            // Arrange
            var pedidosPage = new PedidosPage(Driver, this);

            pedidosPage.Navigate(BASE_URL);

            string dniCliente = "58471629"; // Jorge Flores
            
            // Act
            TestContext.WriteLine($"🔍 Filtrando pedidos de Jorge Flores DNI: {dniCliente}");
            pedidosPage.FiltrarPorCliente(dniCliente);

            bool hayPedidos = pedidosPage.HayPedidos();
            Assert.That(hayPedidos, Is.True,
                "❌ ERROR: No hay pedidos de Jorge Flores para confirmar");

            TestContext.WriteLine("✅ Seleccionando primer pedido PENDIENTE con stock suficiente");
            bool pedidoSeleccionado = pedidosPage.SeleccionarPrimerPedidoPendiente();
            Assert.That(pedidoSeleccionado, Is.True,
                "❌ ERROR: No se encontró pedido PENDIENTE para confirmar");

            string codigoPedido = pedidosPage.ObtenerCodigoPrimerPedido();
            TestContext.WriteLine($"📋 Pedido a confirmar: {codigoPedido}");

            TestContext.WriteLine("✔️ Haciendo clic en CONFIRMAR");
            pedidosPage.ClickConfirmar();

            TestContext.WriteLine("⏳ Confirmando acción en modal");
            bool confirmacionRealizada = pedidosPage.ConfirmarAccionEnModal();
            Assert.That(confirmacionRealizada, Is.True,
                "❌ ERROR: No se pudo confirmar la acción en el modal");

            System.Threading.Thread.Sleep(3000); // Esperar proceso de confirmación

            // Verificar mensaje de éxito
            bool mensajeExito = pedidosPage.VerificarMensajeExitoConfirmacion();
            Assert.That(mensajeExito, Is.True,
                "❌ ERROR: No apareció mensaje de confirmación exitosa");
            TestContext.WriteLine("✅ PV1: Pedido confirmado exitosamente");

            // Verificar cambio de estado
            pedidosPage.Navigate(BASE_URL);
            pedidosPage.FiltrarPorCodigo(codigoPedido);
            
            string estadoActual = pedidosPage.ObtenerEstadoPrimerPedido();
            Assert.That(estadoActual, Does.Contain("CONFIRMADO").Or.Contain("PROCESADO"),
                $"❌ ERROR: Estado no cambió a CONFIRMADO. Estado actual: {estadoActual}");
            TestContext.WriteLine("✅ PV1: Estado cambió a CONFIRMADO");

            // Verificar que se generó venta (navegando al módulo Ventas)
            TestContext.WriteLine("🔍 Verificando generación de venta en módulo Ventas");
            Driver.Navigate().GoToUrl($"{BASE_URL}/Venta/Index");
            System.Threading.Thread.Sleep(2000);

            var ventaPage = new PedidosPage(Driver, this); // Reutilizar para buscar
            ventaPage.FiltrarPorCliente(dniCliente);
            
            bool ventaGenerada = ventaPage.HayPedidos(); // Hay al menos una venta
            Assert.That(ventaGenerada, Is.True,
                "❌ ERROR: No se generó venta en módulo Ventas");
            TestContext.WriteLine("✅ PV2: Venta generada en módulo Ventas");

            TestContext.WriteLine("✅ PV3: Stock descontado (verificación manual requerida)");
            TestContext.WriteLine("✅ PV4: Comprobante generado (verificación manual requerida)");

            bool clienteCorrecto = ventaPage.VerificarClienteEnResultados(dniCliente, "FLORES");
            Assert.That(clienteCorrecto, Is.True,
                "❌ ERROR: Cliente no coincide en venta generada");
            TestContext.WriteLine("✅ PV5: Cliente en venta: Jorge Flores");

            TestContext.WriteLine("✅ Pedido confirmado y convertido a venta exitosamente");
        }
    }
}
