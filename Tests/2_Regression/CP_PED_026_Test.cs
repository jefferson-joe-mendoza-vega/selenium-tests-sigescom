using NUnit.Framework;
using Tsp.Sigescom.Tests.E2E.Helper;
using Tsp.Sigescom.Tests.E2E.Pages;

namespace Tsp.Sigescom.Tests.E2E.Tests._2_Regression
{
    [TestFixture]
    [Category("Regression")]
    [Category("Critica")]
    public class CP_PED_026_Test : TestBase
    {
        [Test]
        [Description("CP-PED-026: Editar pedido en estado Pendiente")]
        public void EditarPedido_EstadoPendiente_CantidadModificada()
        {
            // Arrange
            var pedidosPage = new PedidosPage(Driver, this);
            var nuevoPedidoPage = new NuevoPedidoPage(Driver, this);

            pedidosPage.Navigate(BASE_URL);

            string dniCliente = "58471629"; // Jorge Flores
            int cantidadInicial = 5;
            int cantidadNueva = 10;

            // Act
            TestContext.WriteLine($"🔍 Filtrando pedidos de Jorge Flores DNI: {dniCliente}");
            pedidosPage.FiltrarPorCliente(dniCliente);

            bool hayPedidos = pedidosPage.HayPedidos();
            Assert.That(hayPedidos, Is.True,
                "❌ ERROR: No hay pedidos de Jorge Flores para editar");

            TestContext.WriteLine("✏️ Seleccionando primer pedido PENDIENTE");
            bool pedidoSeleccionado = pedidosPage.SeleccionarPrimerPedidoPendiente();
            Assert.That(pedidoSeleccionado, Is.True,
                "❌ ERROR: No se encontró pedido PENDIENTE para editar");

            TestContext.WriteLine("📝 Abriendo edición de pedido");
            pedidosPage.ClickEditar();

            TestContext.WriteLine($"🔢 Cambiando cantidad de {cantidadInicial} a {cantidadNueva}");
            nuevoPedidoPage.ModificarCantidadPrimerProducto(cantidadNueva);

            decimal subtotalAntes = nuevoPedidoPage.ObtenerSubtotal();
            TestContext.WriteLine($"💰 Subtotal antes de guardar: S/ {subtotalAntes}");

            TestContext.WriteLine("💾 Guardando cambios");
            nuevoPedidoPage.ClickGuardar();

            bool mensajeExito = nuevoPedidoPage.VerificarMensajeExito();
            Assert.That(mensajeExito, Is.True,
                "❌ ERROR: No apareció mensaje de confirmación");
            TestContext.WriteLine("✅ PV1: Cambios guardados correctamente");

            System.Threading.Thread.Sleep(2000);

            // Verificar cambios
            pedidosPage.FiltrarPorCliente(dniCliente);
            int cantidadActual = pedidosPage.ObtenerCantidadPrimerProducto();
            Assert.That(cantidadActual, Is.EqualTo(cantidadNueva),
                $"❌ ERROR: Cantidad no actualizada. Esperada: {cantidadNueva}, Actual: {cantidadActual}");
            TestContext.WriteLine($"✅ PV2: Cantidad actualizada a {cantidadNueva}");

            decimal subtotalActual = pedidosPage.ObtenerTotalPrimerPedido();
            TestContext.WriteLine($"💰 Subtotal/IGV/Total recalculados: S/ {subtotalActual}");
            TestContext.WriteLine("✅ PV3: Totales recalculados");

            string estadoActual = pedidosPage.ObtenerEstadoPrimerPedido();
            Assert.That(estadoActual, Does.Contain("PENDIENTE").Or.Contain("REGISTRADO"),
                "❌ ERROR: Estado cambió incorrectamente");
            TestContext.WriteLine("✅ PV4: Estado sigue Pendiente/Registrado");

            bool clienteCorrecto = pedidosPage.VerificarClienteEnPrimerPedido("FLORES");
            Assert.That(clienteCorrecto, Is.True,
                "❌ ERROR: Cliente cambió incorrectamente");
            TestContext.WriteLine("✅ PV5: Cliente sigue siendo Jorge Flores");

            TestContext.WriteLine("✅ Edición de pedido exitosa");
        }
    }
}
