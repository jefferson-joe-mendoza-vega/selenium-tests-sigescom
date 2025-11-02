using NUnit.Framework;
using Tsp.Sigescom.Tests.E2E.Helper;
using Tsp.Sigescom.Tests.E2E.Pages;

namespace Tsp.Sigescom.Tests.E2E.Tests._2_Regression
{
    [TestFixture]
    [Category("Regression")]
    [Category("Alta")]
    public class CP_PED_018_Test : TestBase
    {
        [Test]
        [Description("CP-PED-018: Ingresar descuento mayor a 100%")]
        public void IngresarDescuento_Mayor100Porciento_NoPermiteAplicar()
        {
            // Arrange
            var pedidosPage = new PedidosPage(Driver, this);
            var nuevoPedidoPage = new NuevoPedidoPage(Driver, this);

            pedidosPage.Navigate(BASE_URL);

            string dniCliente = "72854193";
            string nombreProducto = "CEMENTO";
            decimal descuentoInvalido = 150.00m;

            // Act
            TestContext.WriteLine("🆕 Creando pedido con descuento inválido");
            nuevoPedidoPage.ClickNuevoPedido();
            nuevoPedidoPage.BuscarCliente(dniCliente);
            nuevoPedidoPage.SeleccionarCliente(dniCliente, "MENDOZA");
            nuevoPedidoPage.AgregarProducto(nombreProducto, 1);

            TestContext.WriteLine($"❌ Intentando aplicar descuento {descuentoInvalido}%");
            bool pudoAplicar = nuevoPedidoPage.IntentarAplicarDescuentoInvalido(descuentoInvalido);

            // Assert
            Assert.That(pudoAplicar, Is.False,
                "❌ ERROR: Se permitió aplicar descuento mayor a 100%");
            TestContext.WriteLine("✅ PV1: No se aplicó descuento mayor a 100%");

            bool mensajeError = nuevoPedidoPage.VerificarMensajeErrorDescuento();
            Assert.That(mensajeError, Is.True,
                "❌ ERROR: No apareció mensaje 'Descuento no puede ser mayor a 100%'");
            TestContext.WriteLine("✅ PV1: Error: 'Descuento no puede ser mayor a 100%'");

            decimal descuentoAplicado = nuevoPedidoPage.ObtenerDescuentoAplicado();
            Assert.That(descuentoAplicado, Is.EqualTo(0).Or.LessThanOrEqualTo(100),
                "❌ ERROR: Descuento inválido fue aplicado");
            TestContext.WriteLine("✅ PV2: Descuento no se aplicó");

            bool validacionFrontend = nuevoPedidoPage.VerificarValidacionFrontendDescuento();
            Assert.That(validacionFrontend, Is.True,
                "❌ ERROR: Validación frontend no funcionó");
            TestContext.WriteLine("✅ PV3: Validación frontend activa");

            TestContext.WriteLine("✅ Validación de descuento inválido exitosa");
        }
    }
}
