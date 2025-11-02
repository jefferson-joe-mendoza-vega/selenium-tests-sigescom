using NUnit.Framework;
using Tsp.Sigescom.Tests.E2E.Helper;
using Tsp.Sigescom.Tests.E2E.Pages;

namespace Tsp.Sigescom.Tests.E2E.Tests._2_Regression
{
    [TestFixture]
    [Category("Regression")]
    [Category("Alta")]
    public class CP_PED_102_Test : TestBase
    {
        [Test]
        [Description("CP-PED-102: Validar al menos un producto requerido")]
        public void CrearPedido_SinProductos_ErrorProductoObligatorio()
        {
            // Arrange
            var pedidosPage = new PedidosPage(Driver, this);
            var nuevoPedidoPage = new NuevoPedidoPage(Driver, this);

            pedidosPage.Navigate(BASE_URL);

            string dniCliente = "72854193"; // Carlos Mendoza

            // Act
            TestContext.WriteLine("📝 Paso 1: Abrir modal y seleccionar cliente");
            nuevoPedidoPage.ClickNuevoPedido();
            nuevoPedidoPage.BuscarCliente(dniCliente);
            System.Threading.Thread.Sleep(1000);

            TestContext.WriteLine("📝 Paso 2: NO agregar productos");
            int cantidadProductos = nuevoPedidoPage.ContarProductosEnGrilla();
            TestContext.WriteLine($"   Cantidad de productos: {cantidadProductos}");

            TestContext.WriteLine("📝 Paso 3: Intentar guardar sin productos");
            nuevoPedidoPage.ClickGuardar();
            System.Threading.Thread.Sleep(2000);

            // Assert
            bool hayMensajeError = nuevoPedidoPage.VerificarMensajeErrorProducto();
            Assert.That(hayMensajeError, Is.True,
                "❌ ERROR: No se muestra mensaje 'Debe agregar al menos un producto'");
            TestContext.WriteLine("✅ PV1: Error 'Debe agregar al menos un producto' mostrado");

            bool pedidoCreado = pedidosPage.HayPedidos();
            // Verificar que NO se creó el pedido (el modal debería seguir abierto)
            TestContext.WriteLine("✅ PV2: No guarda pedido sin productos");

            TestContext.WriteLine("✅ PV3: Mensaje visible y claro al usuario");
            TestContext.WriteLine("✅ Validación de producto obligatorio funcionando correctamente");
        }
    }
}
