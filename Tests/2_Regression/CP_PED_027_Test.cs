using NUnit.Framework;
using Tsp.Sigescom.Tests.E2E.Helper;
using Tsp.Sigescom.Tests.E2E.Pages;

namespace Tsp.Sigescom.Tests.E2E.Tests._2_Regression
{
    [TestFixture]
    [Category("Regression")]
    [Category("Critica")]
    public class CP_PED_027_Test : TestBase
    {
        [Test]
        [Description("CP-PED-027: Intentar editar pedido confirmado")]
        public void EditarPedido_EstadoConfirmado_NoPermiteEditar()
        {
            // Arrange
            var pedidosPage = new PedidosPage(Driver, this);
            pedidosPage.Navigate(BASE_URL);

            string dniCliente = "81247593"; // Ana Rodríguez

            // Act
            TestContext.WriteLine($"🔍 Filtrando pedidos de Ana Rodríguez DNI: {dniCliente}");
            pedidosPage.FiltrarPorCliente(dniCliente);
            
            TestContext.WriteLine("🔍 Buscando pedido CONFIRMADO");
            bool pedidoConfirmadoEncontrado = pedidosPage.SeleccionarPrimerPedidoConfirmado();
            
            if (!pedidoConfirmadoEncontrado)
            {
                Assert.Inconclusive("⚠️ No hay pedidos CONFIRMADOS de Ana Rodríguez para probar");
                return;
            }

            // Assert
            bool botonEditarDeshabilitado = pedidosPage.VerificarBotonEditarDeshabilitado();
            TestContext.WriteLine($"🔍 Botón Editar deshabilitado: {botonEditarDeshabilitado}");

            if (botonEditarDeshabilitado)
            {
                Assert.That(botonEditarDeshabilitado, Is.True);
                TestContext.WriteLine("✅ PV1: Botón Editar deshabilitado correctamente");
            }
            else
            {
                TestContext.WriteLine("⚠️ Botón Editar habilitado, intentando editar...");
                pedidosPage.ClickEditar();
                
                bool mensajeError = pedidosPage.VerificarMensajeNoSePuedeEditar();
                Assert.That(mensajeError, Is.True,
                    "❌ ERROR: No apareció mensaje 'No se puede editar pedido confirmado'");
                TestContext.WriteLine("✅ PV1-PV2: Mensaje de error mostrado");
            }

            bool pedidoSinCambios = pedidosPage.VerificarPedidoSinCambios();
            Assert.That(pedidoSinCambios, Is.True,
                "❌ ERROR: El pedido fue modificado");
            TestContext.WriteLine("✅ PV3: Pedido sin cambios");

            bool clienteVisible = pedidosPage.VerificarClienteEnResultados("81247593", "RODRIGUEZ");
            Assert.That(clienteVisible, Is.True);
            TestContext.WriteLine("✅ PV4: Cliente Ana Rodríguez visible");

            TestContext.WriteLine("✅ Validación: No se puede editar pedido confirmado");
        }
    }
}
