using NUnit.Framework;
using Tsp.Sigescom.Tests.E2E.Helper;
using Tsp.Sigescom.Tests.E2E.Pages;

namespace Tsp.Sigescom.Tests.E2E.Tests._2_Regression
{
    [TestFixture]
    [Category("Regression")]
    [Category("Critica")]
    public class CP_PED_095_Test : TestBase
    {
        [Test]
        [Description("CP-PED-095: Calcular con producto gravado y no gravado mixtos")]
        public void CalcularPedido_ProductosGravadosYNoGravados_IGVSoloEnGravados()
        {
            // Arrange
            var pedidosPage = new PedidosPage(Driver, this);
            var nuevoPedidoPage = new NuevoPedidoPage(Driver, this);

            pedidosPage.Navigate(BASE_URL);

            string dniCliente = "81247593"; // Ana Rodríguez
            // Nota: Necesitarás identificar productos gravados y no gravados en el sistema
            string productoGravado = "88008-1"; // Precio asumido: 100
            string productoNoGravado = "PRODUCTO-NO-GRAVADO"; // Precio asumido: 50
            
            decimal precioProductoGravado = 100m;
            decimal precioProductoNoGravado = 50m;

            // Act
            TestContext.WriteLine("📝 Paso 1: Crear pedido con productos mixtos");
            nuevoPedidoPage.ClickNuevoPedido();
            nuevoPedidoPage.BuscarCliente(dniCliente);

            TestContext.WriteLine($"📝 Paso 2: Agregar producto gravado (precio {precioProductoGravado})");
            nuevoPedidoPage.AgregarProducto(productoGravado, 1);
            System.Threading.Thread.Sleep(1000);

            TestContext.WriteLine($"📝 Paso 3: Agregar producto NO gravado (precio {precioProductoNoGravado})");
            nuevoPedidoPage.AgregarProducto(productoNoGravado, 1);
            System.Threading.Thread.Sleep(2000);

            TestContext.WriteLine("📝 Paso 4: Obtener cálculos finales");
            decimal subtotal = nuevoPedidoPage.ObtenerSubtotal();
            decimal igv = nuevoPedidoPage.ObtenerIGV();
            decimal total = nuevoPedidoPage.ObtenerTotal();

            int cantidadProductos = nuevoPedidoPage.ContarProductosEnGrilla();

            // Assert
            Assert.That(cantidadProductos, Is.EqualTo(2),
                $"❌ ERROR: Debería haber 2 productos, pero hay {cantidadProductos}");
            TestContext.WriteLine("✅ Productos mixtos agregados");

            // IGV solo sobre el producto gravado: 100 * 0.18 = 18
            decimal igvEsperado = 18m;
            Assert.That(igv, Is.EqualTo(igvEsperado).Within(0.01m),
                $"❌ ERROR: IGV {igv} no coincide con esperado {igvEsperado} (solo sobre gravados)");
            TestContext.WriteLine($"✅ PV1: IGV solo sobre productos gravados = {igv}");

            decimal subtotalEsperado = 150m; // 100 + 50
            Assert.That(subtotal, Is.EqualTo(subtotalEsperado).Within(0.01m),
                $"❌ ERROR: Subtotal {subtotal} no coincide con {subtotalEsperado}");
            TestContext.WriteLine($"✅ PV2: Subtotal correcto = {subtotal}");

            decimal totalEsperado = 168m; // 150 + 18
            Assert.That(total, Is.EqualTo(totalEsperado).Within(0.01m),
                $"❌ ERROR: Total {total} no coincide con {totalEsperado}");
            TestContext.WriteLine($"✅ PV4: Total correcto = {total}");

            TestContext.WriteLine($"✅ Cálculo mixto verificado: Subtotal={subtotal}, IGV={igv} (solo P1), Total={total}");
        }
    }
}
