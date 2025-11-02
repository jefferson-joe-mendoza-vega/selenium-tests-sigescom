using NUnit.Framework;
using Tsp.Sigescom.Tests.E2E.Helper;
using Tsp.Sigescom.Tests.E2E.Pages;

namespace Tsp.Sigescom.Tests.E2E.Tests._2_Regression
{
    [TestFixture]
    [Category("Regression")]
    [Category("Critica")]
    public class CP_PED_021_Test : TestBase
    {
        [Test]
        [Description("CP-PED-021: Agregar múltiples productos al pedido")]
        public void AgregarMultiplesProductos_3Productos_CalculaTotalesCorrecto()
        {
            // Arrange
            var pedidosPage = new PedidosPage(Driver, this);
            var nuevoPedidoPage = new NuevoPedidoPage(Driver, this);

            pedidosPage.Navigate(BASE_URL);

            string dniCliente = "72854193";
            string producto1 = "CEMENTO"; // Precio 100
            string producto2 = "ARENA"; // Precio 50
            string producto3 = "LADRILLO"; // Precio 75
            
            decimal subtotalEsperado = 225.00m; // 100 + 50 + 75
            decimal igvEsperado = 40.50m; // 18% de 225
            decimal totalEsperado = 265.50m;

            // Act
            TestContext.WriteLine("🆕 Creando pedido con múltiples productos");
            nuevoPedidoPage.ClickNuevoPedido();
            nuevoPedidoPage.BuscarCliente(dniCliente);
            nuevoPedidoPage.SeleccionarCliente(dniCliente, "MENDOZA");

            TestContext.WriteLine($"📦 Agregando Producto A: {producto1}");
            nuevoPedidoPage.AgregarProducto(producto1, 1);
            System.Threading.Thread.Sleep(1000);

            TestContext.WriteLine($"📦 Agregando Producto B: {producto2}");
            nuevoPedidoPage.AgregarProducto(producto2, 1);
            System.Threading.Thread.Sleep(1000);

            TestContext.WriteLine($"📦 Agregando Producto C: {producto3}");
            nuevoPedidoPage.AgregarProducto(producto3, 1);
            System.Threading.Thread.Sleep(1000);

            int cantidadProductos = nuevoPedidoPage.ContarProductosEnGrilla();
            decimal subtotalCalculado = nuevoPedidoPage.ObtenerSubtotal();
            decimal igvCalculado = nuevoPedidoPage.ObtenerIGV();
            decimal totalCalculado = nuevoPedidoPage.ObtenerTotal();

            TestContext.WriteLine($"📋 Productos en grilla: {cantidadProductos}");
            TestContext.WriteLine($"💰 Subtotal: S/ {subtotalCalculado}");
            TestContext.WriteLine($"💰 IGV: S/ {igvCalculado}");
            TestContext.WriteLine($"💰 Total: S/ {totalCalculado}");

            // Assert
            Assert.That(cantidadProductos, Is.EqualTo(3),
                $"❌ ERROR: Cantidad de productos incorrecta. Esperado: 3, Obtenido: {cantidadProductos}");
            TestContext.WriteLine("✅ PV1: 3 productos en grilla");

            Assert.That(subtotalCalculado, Is.EqualTo(subtotalEsperado).Within(0.01m),
                $"❌ ERROR: Subtotal incorrecto. Esperado: {subtotalEsperado}, Obtenido: {subtotalCalculado}");
            TestContext.WriteLine("✅ PV2: Subtotal=225 correcto");

            Assert.That(igvCalculado, Is.EqualTo(igvEsperado).Within(0.01m),
                $"❌ ERROR: IGV incorrecto. Esperado: {igvEsperado}, Obtenido: {igvCalculado}");
            TestContext.WriteLine("✅ PV3: IGV=40.5 correcto");

            Assert.That(totalCalculado, Is.EqualTo(totalEsperado).Within(0.01m),
                $"❌ ERROR: Total incorrecto. Esperado: {totalEsperado}, Obtenido: {totalCalculado}");
            TestContext.WriteLine("✅ PV4: Total=265.5 correcto");

            TestContext.WriteLine("✅ Múltiples productos agregados y calculados correctamente");
        }
    }
}
