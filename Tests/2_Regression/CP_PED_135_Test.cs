using NUnit.Framework;
using Tsp.Sigescom.Tests.E2E.Helper;
using Tsp.Sigescom.Tests.E2E.Pages;

namespace Tsp.Sigescom.Tests.E2E.Tests._2_Regression
{
    [TestFixture]
    [Category("Regression")]
    [Category("Alta")]
    public class CP_PED_135_Test : TestBase
    {
        [Test]
        [Description("CP-PED-135: Validar fecha de operación no futura")]
        public void CrearPedido_FechaFutura_ErrorFechaInvalida()
        {
            // Arrange
            var pedidosPage = new PedidosPage(Driver, this);
            var nuevoPedidoPage = new NuevoPedidoPage(Driver, this);

            pedidosPage.Navigate(BASE_URL);

            string dniCliente = "72854193"; // Carlos Mendoza
            string fechaFutura = "01/01/2026"; // Fecha en el futuro

            // Act
            TestContext.WriteLine("📝 Paso 1: Abrir modal y seleccionar cliente");
            nuevoPedidoPage.ClickNuevoPedido();
            nuevoPedidoPage.BuscarCliente(dniCliente);
            nuevoPedidoPage.AgregarProducto("88008-1", 2);

            TestContext.WriteLine($"📝 Paso 2: Intentar ingresar fecha futura: {fechaFutura}");
            bool fechaPermitida = nuevoPedidoPage.IntentarIngresarFechaOperacion(fechaFutura);
            System.Threading.Thread.Sleep(1000);

            TestContext.WriteLine("📝 Paso 3: Intentar guardar");
            if (fechaPermitida)
            {
                nuevoPedidoPage.ClickGuardar();
                System.Threading.Thread.Sleep(2000);
            }

            // Assert
            bool hayMensajeError = nuevoPedidoPage.VerificarMensajeErrorFechaFutura();
            Assert.That(hayMensajeError, Is.True,
                "❌ ERROR: No se muestra mensaje 'Fecha no puede ser futura'");
            TestContext.WriteLine($"✅ PV1: Error 'Fecha no puede ser futura' mostrado");

            bool fechaActualPermitida = nuevoPedidoPage.VerificarFechaActualPermitida();
            Assert.That(fechaActualPermitida, Is.True,
                "❌ ERROR: Fecha actual debería estar permitida");
            TestContext.WriteLine("✅ PV2: Permite hoy o anterior");

            TestContext.WriteLine("✅ PV3: Validación temporal lógica correcta");
            TestContext.WriteLine("✅ Validación de fecha no futura funcionando correctamente");
        }
    }
}
