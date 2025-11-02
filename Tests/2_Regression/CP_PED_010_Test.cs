using NUnit.Framework;
using Tsp.Sigescom.Tests.E2E.Helper;
using Tsp.Sigescom.Tests.E2E.Pages;

namespace Tsp.Sigescom.Tests.E2E.Tests._2_Regression
{
    [TestFixture]
    [Category("Regression")]
    [Category("Critica")]
    public class CP_PED_010_Test : TestBase
    {
        [Test]
        [Description("CP-PED-010: Agregar producto sin stock disponible")]
        public void AgregarProducto_SinStock_NoPermiteAgregar()
        {
            // Arrange
            var pedidosPage = new PedidosPage(Driver, this);
            var nuevoPedidoPage = new NuevoPedidoPage(Driver, this);

            pedidosPage.Navigate(BASE_URL);

            string dniCliente = "72854193"; // Carlos Mendoza
            string productoSinStock = "PRODUCTO_SIN_STOCK"; // Buscar producto con stock=0

            // Act
            TestContext.WriteLine("🆕 Creando pedido con producto sin stock");
            nuevoPedidoPage.ClickNuevoPedido();

            TestContext.WriteLine($"🔍 Seleccionando cliente DNI: {dniCliente}");
            nuevoPedidoPage.BuscarCliente(dniCliente);
            nuevoPedidoPage.SeleccionarCliente(dniCliente, "MENDOZA");

            TestContext.WriteLine($"❌ Intentando agregar producto sin stock");
            bool pudoAgregar = nuevoPedidoPage.IntentarAgregarProductoSinStock(productoSinStock);

            // Assert
            Assert.That(pudoAgregar, Is.False,
                "❌ ERROR: Se permitió agregar producto sin stock");
            TestContext.WriteLine("✅ PV1: No se permitió agregar producto sin stock");

            bool mensajeError = nuevoPedidoPage.VerificarMensajeStockInsuficiente();
            Assert.That(mensajeError, Is.True,
                "❌ ERROR: No apareció mensaje 'Producto sin stock'");
            TestContext.WriteLine("✅ PV2: Mensaje de error visible");

            bool stockMostrado = nuevoPedidoPage.VerificarStockActualMostrado();
            Assert.That(stockMostrado, Is.True,
                "❌ ERROR: No se muestra el stock actual");
            TestContext.WriteLine("✅ PV3: Stock actual mostrado correctamente");

            TestContext.WriteLine("✅ Validación exitosa - No se permite agregar producto sin stock");
        }
    }
}
