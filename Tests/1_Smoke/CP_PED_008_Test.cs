using NUnit.Framework;
using Tsp.Sigescom.Tests.E2E.Helper;
using Tsp.Sigescom.Tests.E2E.Pages;

namespace Tsp.Sigescom.Tests.E2E.Tests._1_Smoke
{
    [TestFixture]
    [Category("Smoke")]
    [Category("Critica")]
    public class CP_PED_008_Test : TestBase
    {
        [Test]
        [Description("CP-PED-008: Validar que no se puede crear pedido con cliente inexistente")]
        public void CrearPedido_ClienteInexistente_NoPermiteGuardar()
        {
            // Arrange
            var pedidosPage = new PedidosPage(Driver, this);
            var nuevoPedidoPage = new NuevoPedidoPage(Driver, this);

            pedidosPage.Navigate(BASE_URL);

            string dniInexistente = "99999999";

            // Act
            TestContext.WriteLine("🆕 Intentando crear pedido con cliente inexistente");
            nuevoPedidoPage.ClickNuevoPedido();

            TestContext.WriteLine($"🔍 Buscando cliente inexistente DNI: {dniInexistente}");
            bool clienteEncontrado = nuevoPedidoPage.BuscarClienteInexistente(dniInexistente);

            // Assert
            Assert.That(clienteEncontrado, Is.False,
                "❌ ERROR: El sistema encontró un cliente que no debería existir");
            TestContext.WriteLine("✅ PV1: Cliente inexistente no encontrado correctamente");

            bool mensajeError = nuevoPedidoPage.VerificarMensajeClienteNoExiste();
            Assert.That(mensajeError, Is.True,
                "❌ ERROR: No apareció mensaje 'Cliente no existe' o 'No encontrado'");
            TestContext.WriteLine("✅ PV1: Mensaje de error visible");

            bool botonDeshabilitado = nuevoPedidoPage.VerificarBotonGuardarDeshabilitado();
            Assert.That(botonDeshabilitado, Is.True,
                "❌ ERROR: El botón GUARDAR está habilitado sin cliente seleccionado");
            TestContext.WriteLine("✅ PV2: Botón GUARDAR deshabilitado correctamente");

            TestContext.WriteLine("✅ Validación exitosa - No se permite crear pedido sin cliente");
        }
    }
}