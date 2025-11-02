using NUnit.Framework;
using Tsp.Sigescom.Tests.E2E.Helper;
using Tsp.Sigescom.Tests.E2E.Pages;

namespace Tsp.Sigescom.Tests.E2E.Tests._2_Regression
{
    [TestFixture]
    [Category("Regression")]
    [Category("Alta")]
    public class CP_PED_103_Test : TestBase
    {
        [Test]
        [Description("CP-PED-103: Validar cantidad mínima de producto = 1")]
        public void AgregarProducto_CantidadCero_ErrorCantidadMinima()
        {
            // Arrange
            var pedidosPage = new PedidosPage(Driver, this);
            var nuevoPedidoPage = new NuevoPedidoPage(Driver, this);

            pedidosPage.Navigate(BASE_URL);

            string dniCliente = "58471629"; // Jorge Flores
            string producto = "88008-1";
            int cantidadInvalida = 0;

            // Act
            TestContext.WriteLine("📝 Paso 1: Abrir modal y seleccionar cliente");
            nuevoPedidoPage.ClickNuevoPedido();
            nuevoPedidoPage.BuscarCliente(dniCliente);

            TestContext.WriteLine($"📝 Paso 2: Intentar ingresar cantidad {cantidadInvalida}");
            bool pudoIngresar = nuevoPedidoPage.IntentarIngresarCantidad(producto, cantidadInvalida);
            System.Threading.Thread.Sleep(1000);

            // Assert
            bool hayMensajeError = nuevoPedidoPage.VerificarMensajeErrorCantidad();
            Assert.That(hayMensajeError, Is.True,
                "❌ ERROR: No se muestra mensaje 'Cantidad debe ser mayor a 0'");
            TestContext.WriteLine("✅ PV1: Error 'Cantidad debe ser mayor a 0' mostrado");

            bool campoInvalido = nuevoPedidoPage.VerificarCampoMarcadoInvalido("cantidad");
            Assert.That(campoInvalido, Is.True,
                "❌ ERROR: Campo cantidad no está marcado como inválido");
            TestContext.WriteLine("✅ PV2: Campo marcado inválido (borde rojo)");

            bool botonDeshabilitado = !nuevoPedidoPage.IntentarAgregarProductoConCantidadInvalida();
            Assert.That(botonDeshabilitado, Is.True,
                "❌ ERROR: Botón Agregar debería estar deshabilitado");
            TestContext.WriteLine("✅ PV3: No permite agregar producto con cantidad 0");

            TestContext.WriteLine("✅ Validación de cantidad mínima funcionando correctamente");
        }
    }
}
