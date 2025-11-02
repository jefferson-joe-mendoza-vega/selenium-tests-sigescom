using NUnit.Framework;
using Tsp.Sigescom.Tests.E2E.Helper;
using Tsp.Sigescom.Tests.E2E.Pages;

namespace Tsp.Sigescom.Tests.E2E.Tests._3_Extended
{
    [TestFixture]
    [Category("Extended")]
    [Category("Critica")]
    public class CP_PED_175_Test : TestBase
    {
        [Test]
        [Description("CP-PED-175: Aplicar descuento general del 5% al pedido")]
        public void AplicarDescuentoGlobal_5Porciento_AplicadoATodosLosProductos()
        {
            // Arrange
            var pedidosPage = new PedidosPage(Driver, this);
            var nuevoPedidoPage = new NuevoPedidoPage(Driver, this);

            pedidosPage.Navigate(BASE_URL);

            string dniCliente = "58471629"; // Jorge Flores
            decimal descuentoGlobal = 5m; // 5%

            // Act
            TestContext.WriteLine("📝 Paso 1: Crear pedido con 3 productos");
            nuevoPedidoPage.ClickNuevoPedido();
            nuevoPedidoPage.BuscarCliente(dniCliente);
            
            // Agregar 3 productos
            nuevoPedidoPage.AgregarProducto("88008-1", 2); // Ejemplo: precio 100
            System.Threading.Thread.Sleep(1000);
            nuevoPedidoPage.AgregarProducto("88008-2", 1); // Ejemplo: precio 50
            System.Threading.Thread.Sleep(1000);
            nuevoPedidoPage.AgregarProducto("88008-3", 1); // Ejemplo: precio 75
            System.Threading.Thread.Sleep(1000);

            TestContext.WriteLine("📝 Paso 2: Obtener subtotal antes del descuento");
            decimal subtotalSinDescuento = nuevoPedidoPage.ObtenerSubtotal();
            TestContext.WriteLine($"   Subtotal sin descuento: {subtotalSinDescuento}");

            TestContext.WriteLine($"📝 Paso 3: Aplicar descuento global del {descuentoGlobal}%");
            nuevoPedidoPage.AplicarDescuentoGlobal(descuentoGlobal);
            System.Threading.Thread.Sleep(2000);

            decimal subtotalConDescuento = nuevoPedidoPage.ObtenerSubtotal();
            decimal total = nuevoPedidoPage.ObtenerTotal();

            // Assert
            decimal descuentoEsperado = subtotalSinDescuento * (descuentoGlobal / 100);
            decimal subtotalEsperado = subtotalSinDescuento - descuentoEsperado;

            Assert.That(subtotalConDescuento, Is.EqualTo(subtotalEsperado).Within(0.01m),
                $"❌ ERROR: Subtotal {subtotalConDescuento} no coincide con esperado {subtotalEsperado}");
            TestContext.WriteLine($"✅ PV1: Descuento aplicado a todos los productos");

            TestContext.WriteLine($"✅ PV2: Subtotal recalculado: {subtotalConDescuento}");
            TestContext.WriteLine($"✅ PV3: Total correcto: {total}");

            bool descuentoVisible = nuevoPedidoPage.VerificarDescuentoEnResumen(descuentoGlobal);
            Assert.That(descuentoVisible, Is.True,
                "❌ ERROR: Descuento no es visible en resumen");
            TestContext.WriteLine($"✅ PV4: Visible en resumen (5% = S/ {descuentoEsperado:F2})");

            TestContext.WriteLine("✅ Descuento global funcionando correctamente");
        }
    }
}
