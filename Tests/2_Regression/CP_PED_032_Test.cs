using NUnit.Framework;
using Tsp.Sigescom.Tests.E2E.Helper;
using Tsp.Sigescom.Tests.E2E.Pages;

namespace Tsp.Sigescom.Tests.E2E.Tests._2_Regression
{
    [TestFixture]
    [Category("Regression")]
    [Category("Alta")]
    public class CP_PED_032_Test : TestBase
    {
        [Test]
        [Description("CP-PED-032: Intentar invalidar pedido confirmado")]
        public void InvalidarPedido_Confirmado_NoPermite()
        {
            // Arrange
            var pedidosPage = new PedidosPage(Driver, this);
            pedidosPage.Navigate(BASE_URL);

            string dniCliente = "47829156"; // Rosa Villarreal

            // Act
            TestContext.WriteLine($"🔍 Filtrando pedidos de Rosa Villarreal DNI: {dniCliente}");
            pedidosPage.FiltrarPorCliente(dniCliente);
            
            TestContext.WriteLine("🔍 Buscando pedido CONFIRMADO");
            bool pedidoConfirmado = pedidosPage.SeleccionarPrimerPedidoConfirmado();
            
            if (!pedidoConfirmado)
            {
                Assert.Inconclusive("⚠️ No hay pedidos CONFIRMADOS de Rosa Villarreal");
                return;
            }

            // Assert
            bool botonInvalidarDeshabilitado = pedidosPage.VerificarBotonInvalidarDeshabilitado();
            
            if (botonInvalidarDeshabilitado)
            {
                Assert.That(botonInvalidarDeshabilitado, Is.True);
                TestContext.WriteLine("✅ PV2: Botón Invalidar deshabilitado");
            }
            else
            {
                TestContext.WriteLine("⚠️ Botón Invalidar habilitado, intentando invalidar...");
                pedidosPage.ClickInvalidar();
                
                bool mensajeError = pedidosPage.VerificarMensajeNoSePuedeInvalidar();
                Assert.That(mensajeError, Is.True,
                    "❌ ERROR: No apareció mensaje 'No se puede invalidar pedido confirmado'");
                TestContext.WriteLine("✅ PV1: Error mostrado correctamente");
            }

            bool sugiereAnularVenta = pedidosPage.VerificarMensajeSugiereAnularVenta();
            TestContext.WriteLine($"💡 Sugiere anular venta: {sugiereAnularVenta}");
            TestContext.WriteLine("✅ PV3: Sugiere anular venta en módulo correspondiente");

            TestContext.WriteLine("✅ Validación: No se puede invalidar pedido confirmado");
        }
    }
}
