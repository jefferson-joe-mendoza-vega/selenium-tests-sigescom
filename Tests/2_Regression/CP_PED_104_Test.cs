using NUnit.Framework;
using Tsp.Sigescom.Tests.E2E.Helper;
using Tsp.Sigescom.Tests.E2E.Pages;

namespace Tsp.Sigescom.Tests.E2E.Tests._2_Regression
{
    [TestFixture]
    [Category("Regression")]
    [Category("Alta")]
    public class CP_PED_104_Test : TestBase
    {
        [Test]
        [Description("CP-PED-104: Validar descuento máximo no mayor a 100%")]
        public void AplicarDescuento_Mayor100Porciento_ErrorDescuentoInvalido()
        {
            // Arrange
            var pedidosPage = new PedidosPage(Driver, this);
            var nuevoPedidoPage = new NuevoPedidoPage(Driver, this);

            pedidosPage.Navigate(BASE_URL);

            string dniCliente = "81247593"; // Ana Rodríguez
            string producto = "88008-1";
            decimal descuentoInvalido = 101m;

            // Act
            TestContext.WriteLine("📝 Paso 1: Crear pedido y agregar producto");
            nuevoPedidoPage.ClickNuevoPedido();
            nuevoPedidoPage.BuscarCliente(dniCliente);
            nuevoPedidoPage.AgregarProducto(producto, 2);
            System.Threading.Thread.Sleep(1000);

            TestContext.WriteLine($"📝 Paso 2: Intentar aplicar descuento {descuentoInvalido}%");
            bool seAplico = nuevoPedidoPage.IntentarAplicarDescuentoInvalido(descuentoInvalido);
            System.Threading.Thread.Sleep(1000);

            // Assert
            bool hayMensajeError = nuevoPedidoPage.VerificarMensajeErrorDescuento();
            Assert.That(hayMensajeError, Is.True,
                "❌ ERROR: No se muestra mensaje 'Descuento no puede ser mayor a 100%'");
            TestContext.WriteLine("✅ PV1: Error 'Descuento no puede ser mayor a 100%' mostrado");

            bool campoInvalido = nuevoPedidoPage.VerificarValidacionFrontendDescuento();
            Assert.That(campoInvalido, Is.True,
                "❌ ERROR: Campo descuento no está marcado como inválido");
            TestContext.WriteLine("✅ PV2: Campo inválido (marcado con clase ng-invalid)");

            Assert.That(seAplico, Is.False,
                "❌ ERROR: No debería permitir aplicar descuento mayor a 100%");
            TestContext.WriteLine("✅ PV3: No permite continuar con descuento inválido");

            TestContext.WriteLine("✅ Validación de límite superior de descuento funcionando correctamente");
        }
    }
}
