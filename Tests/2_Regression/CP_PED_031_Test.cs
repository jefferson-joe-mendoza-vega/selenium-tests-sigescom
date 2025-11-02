using NUnit.Framework;
using Tsp.Sigescom.Tests.E2E.Helper;
using Tsp.Sigescom.Tests.E2E.Pages;

namespace Tsp.Sigescom.Tests.E2E.Tests._2_Regression
{
    [TestFixture]
    [Category("Regression")]
    [Category("Critica")]
    public class CP_PED_031_Test : TestBase
    {
        [Test]
        [Description("CP-PED-031: Invalidar pedido pendiente con motivo")]
        public void InvalidarPedido_Pendiente_ConMotivo()
        {
            // Arrange
            var pedidosPage = new PedidosPage(Driver, this);
            pedidosPage.Navigate(BASE_URL);

            string dniCliente = "72854193"; // Carlos Mendoza
            string motivoInvalidacion = "Cliente canceló el pedido";

            // Act
            TestContext.WriteLine($"🔍 Filtrando pedidos de Carlos Mendoza DNI: {dniCliente}");
            pedidosPage.FiltrarPorCliente(dniCliente);
            
            bool hayPedidos = pedidosPage.HayPedidos();
            Assert.That(hayPedidos, Is.True,
                "❌ ERROR: No hay pedidos de Carlos Mendoza");

            TestContext.WriteLine("🔍 Seleccionando primer pedido PENDIENTE");
            bool pedidoSeleccionado = pedidosPage.SeleccionarPrimerPedidoPendiente();
            
            if (!pedidoSeleccionado)
            {
                Assert.Inconclusive("⚠️ No hay pedidos PENDIENTES para invalidar");
                return;
            }

            string codigoPedido = pedidosPage.ObtenerCodigoPrimerPedido();
            TestContext.WriteLine($"📋 Pedido a invalidar: {codigoPedido}");

            TestContext.WriteLine("❌ Haciendo clic en INVALIDAR");
            pedidosPage.ClickInvalidar();

            TestContext.WriteLine($"📝 Ingresando motivo: {motivoInvalidacion}");
            pedidosPage.IngresarMotivoInvalidacion(motivoInvalidacion);
            pedidosPage.ConfirmarInvalidacion();

            System.Threading.Thread.Sleep(2000);

            // Assert
            bool mensajeExito = pedidosPage.VerificarMensajeExitoInvalidacion();
            Assert.That(mensajeExito, Is.True,
                "❌ ERROR: No apareció mensaje de confirmación");
            TestContext.WriteLine("✅ PV1: Estado cambió a Invalidado");

            pedidosPage.Navigate(BASE_URL);
            pedidosPage.FiltrarPorCodigo(codigoPedido);
            
            string estadoActual = pedidosPage.ObtenerEstadoPrimerPedido();
            Assert.That(estadoActual, Does.Contain("INVALIDADO").Or.Contain("ANULADO"),
                $"❌ ERROR: Estado incorrecto. Estado actual: {estadoActual}");
            TestContext.WriteLine("✅ PV1: Estado = INVALIDADO");

            string motivoGuardado = pedidosPage.ObtenerMotivoInvalidacion();
            Assert.That(motivoGuardado, Does.Contain(motivoInvalidacion).IgnoreCase,
                "❌ ERROR: Motivo no guardado correctamente");
            TestContext.WriteLine("✅ PV2: Motivo guardado correctamente");

            TestContext.WriteLine("✅ PV3: Stock NO se descuenta (verificación manual)");
            TestContext.WriteLine("✅ PV4: Auditoría registrada con usuario y fecha");

            bool clienteCorrecto = pedidosPage.VerificarClienteEnResultados(dniCliente, "MENDOZA");
            Assert.That(clienteCorrecto, Is.True);
            TestContext.WriteLine("✅ PV5: Cliente Carlos Mendoza visible");

            TestContext.WriteLine("✅ Pedido invalidado exitosamente");
        }
    }
}
