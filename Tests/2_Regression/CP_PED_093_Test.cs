using NUnit.Framework;
using Tsp.Sigescom.Tests.E2E.Helper;
using Tsp.Sigescom.Tests.E2E.Pages;

namespace Tsp.Sigescom.Tests.E2E.Tests._2_Regression
{
    [TestFixture]
    [Category("Regression")]
    [Category("Critica")]
    public class CP_PED_093_Test : TestBase
    {
        [Test]
        [Description("CP-PED-093: Verificar redondeo en cálculo de IGV con decimales")]
        public void CalcularIGV_PrecioConDecimales_RedondeoCorrectoA2Decimales()
        {
            // Arrange
            var pedidosPage = new PedidosPage(Driver, this);
            var nuevoPedidoPage = new NuevoPedidoPage(Driver, this);

            pedidosPage.Navigate(BASE_URL);

            // Datos con decimales: Precio=10.33, Cantidad=3
            string dniCliente = "58471629"; // Jorge Flores
            string productoConDecimales = "88008-1"; // Ajustar si es necesario
            int cantidad = 3;
            decimal precioUnitario = 10.33m;

            // Act
            TestContext.WriteLine("📝 Paso 1: Crear pedido con precio decimal");
            nuevoPedidoPage.ClickNuevoPedido();
            nuevoPedidoPage.BuscarCliente(dniCliente);
            
            TestContext.WriteLine($"📝 Paso 2: Agregar producto con precio {precioUnitario}, cantidad {cantidad}");
            nuevoPedidoPage.AgregarProducto(productoConDecimales, cantidad);
            System.Threading.Thread.Sleep(2000);

            TestContext.WriteLine("📝 Paso 3: Obtener valores calculados");
            decimal subtotal = nuevoPedidoPage.ObtenerSubtotal();
            decimal igv = nuevoPedidoPage.ObtenerIGV();
            decimal total = nuevoPedidoPage.ObtenerTotal();

            // Assert
            decimal subtotalEsperado = precioUnitario * cantidad; // 10.33 * 3 = 30.99
            Assert.That(subtotal, Is.EqualTo(subtotalEsperado).Within(0.01m),
                $"❌ ERROR: Subtotal {subtotal} no coincide con esperado {subtotalEsperado}");
            TestContext.WriteLine($"✅ PV1: Subtotal correcto = {subtotal}");

            // IGV = 30.99 * 0.18 = 5.5782 → debe redondear a 5.58
            decimal igvEsperado = 5.58m;
            Assert.That(igv, Is.EqualTo(igvEsperado).Within(0.01m),
                $"❌ ERROR: IGV {igv} no coincide con esperado {igvEsperado} (redondeo a 2 decimales)");
            TestContext.WriteLine($"✅ PV2: IGV redondeado correctamente = {igv} (de 5.5782)");

            decimal totalEsperado = 36.57m; // 30.99 + 5.58
            Assert.That(total, Is.EqualTo(totalEsperado).Within(0.01m),
                $"❌ ERROR: Total {total} no coincide con esperado {totalEsperado}");
            TestContext.WriteLine($"✅ PV3: Total correcto = {total}");

            bool precision2Decimales = nuevoPedidoPage.VerificarPrecisionDecimal(2);
            Assert.That(precision2Decimales, Is.True,
                "❌ ERROR: Los valores no tienen precisión de 2 decimales");
            TestContext.WriteLine("✅ PV4: Precisión de 2 decimales verificada");

            TestContext.WriteLine($"✅ Cálculo con redondeo verificado: Subtotal={subtotal}, IGV={igv}, Total={total}");
        }
    }
}
