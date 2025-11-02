using NUnit.Framework;
using Tsp.Sigescom.Tests.E2E.Helper;
using Tsp.Sigescom.Tests.E2E.Pages;

namespace Tsp.Sigescom.Tests.E2E.Tests._2_Regression
{
    [TestFixture]
    [Category("Regression")]
    [Category("Alta")]
    public class CP_PED_016_Test : TestBase
    {
        [Test]
        [Description("CP-PED-016: Ingresar cantidad negativa")]
        public void IngresarCantidadNegativa_NoPermiteAgregar()
        {
            // Arrange
            var pedidosPage = new PedidosPage(Driver, this);
            var nuevoPedidoPage = new NuevoPedidoPage(Driver, this);

            pedidosPage.Navigate(BASE_URL);

            string dniCliente = "72854193";
            string nombreProducto = "CEMENTO";
            int cantidadNegativa = -5;

            // Act
            TestContext.WriteLine("🆕 Creando pedido con cantidad negativa");
            nuevoPedidoPage.ClickNuevoPedido();
            nuevoPedidoPage.BuscarCliente(dniCliente);
            nuevoPedidoPage.SeleccionarCliente(dniCliente, "MENDOZA");

            TestContext.WriteLine($"❌ Intentando ingresar cantidad negativa: {cantidadNegativa}");
            bool pudoIngresar = nuevoPedidoPage.IntentarIngresarCantidad(nombreProducto, cantidadNegativa);

            // Assert
            if (!pudoIngresar)
            {
                TestContext.WriteLine("✅ PV1: Sistema bloquea ingreso de cantidad negativa");
            }
            else
            {
                bool mensajeError = nuevoPedidoPage.VerificarMensajeErrorCantidad();
                Assert.That(mensajeError, Is.True,
                    "❌ ERROR: No apareció mensaje 'Cantidad debe ser mayor a 0'");
                TestContext.WriteLine("✅ PV1: Error: 'Cantidad debe ser mayor a 0'");

                bool campoInvalido = nuevoPedidoPage.VerificarCampoMarcadoInvalido("cantidad");
                Assert.That(campoInvalido, Is.True,
                    "❌ ERROR: Campo cantidad no está marcado como inválido");
                TestContext.WriteLine("✅ PV2: Campo marcado como inválido (borde rojo)");

                bool pudoAgregar = nuevoPedidoPage.IntentarAgregarProductoConCantidadInvalida();
                Assert.That(pudoAgregar, Is.False,
                    "❌ ERROR: Se permitió agregar producto con cantidad negativa");
                TestContext.WriteLine("✅ PV3: No permite agregar producto");
            }

            TestContext.WriteLine("✅ Validación de cantidad negativa exitosa");
        }
    }
}
