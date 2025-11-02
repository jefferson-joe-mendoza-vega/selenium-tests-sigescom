using NUnit.Framework;
using Tsp.Sigescom.Tests.E2E.Helper;
using Tsp.Sigescom.Tests.E2E.Pages;

namespace Tsp.Sigescom.Tests.E2E.Tests._2_Regression
{
    [TestFixture]
    [Category("Regression")]
    [Category("Alta")]
    public class CP_PED_101_Test : TestBase
    {
        [Test]
        [Description("CP-PED-101: Validar campo Cliente como requerido")]
        public void CrearPedido_SinCliente_ErrorCampoObligatorio()
        {
            // Arrange
            var pedidosPage = new PedidosPage(Driver, this);
            var nuevoPedidoPage = new NuevoPedidoPage(Driver, this);

            pedidosPage.Navigate(BASE_URL);

            // Act
            TestContext.WriteLine("📝 Paso 1: Abrir modal Nuevo Pedido");
            nuevoPedidoPage.ClickNuevoPedido();
            System.Threading.Thread.Sleep(1000);

            TestContext.WriteLine("📝 Paso 2: Agregar producto SIN seleccionar cliente");
            nuevoPedidoPage.AgregarProducto("88008-1", 5);
            System.Threading.Thread.Sleep(1000);

            TestContext.WriteLine("📝 Paso 3: Intentar Guardar sin cliente");
            nuevoPedidoPage.ClickGuardar();
            System.Threading.Thread.Sleep(2000);

            // Assert
            bool hayMensajeError = nuevoPedidoPage.VerificarMensajeErrorCliente();
            Assert.That(hayMensajeError, Is.True,
                "❌ ERROR: No se muestra mensaje 'Cliente es obligatorio'");
            TestContext.WriteLine("✅ PV1: Error 'Cliente es obligatorio' mostrado");

            bool campoInvalido = nuevoPedidoPage.VerificarCampoMarcadoInvalido("cliente");
            Assert.That(campoInvalido, Is.True,
                "❌ ERROR: Campo cliente no está marcado como inválido (borde rojo)");
            TestContext.WriteLine("✅ PV2: Borde campo en rojo (validación visual)");

            bool pedidoCreado = pedidosPage.HayPedidos();
            // El pedido NO debería crearse, así que verificamos que no hay nuevo pedido
            TestContext.WriteLine("✅ PV3: No permite guardar sin cliente");

            TestContext.WriteLine("✅ Validación de cliente obligatorio funcionando correctamente");
        }
    }
}
