using NUnit.Framework;
using Tsp.Sigescom.Tests.E2E.Helper;
using Tsp.Sigescom.Tests.E2E.Pages;

namespace Tsp.Sigescom.Tests.E2E.Tests._2_Regression
{
    [TestFixture]
    [Category("Regression")]
    [Category("Critica")]
    public class CP_PED_013_Test : TestBase
    {
        [Test]
        [Description("CP-PED-013: Verificar cálculo de IGV (18%)")]
        public void CalculoIGV_Producto100_Cantidad2_CalculaCorrecto()
        {
            // Arrange
            var pedidosPage = new PedidosPage(Driver, this);
            var nuevoPedidoPage = new NuevoPedidoPage(Driver, this);

            pedidosPage.Navigate(BASE_URL);

            string dniCliente = "72854193"; // Carlos Mendoza
            string nombreProducto = "CEMENTO"; // Producto con precio conocido
            int cantidad = 2;
            decimal precioUnitario = 100.00m;
            decimal subtotalEsperado = 200.00m;
            decimal igvEsperado = 36.00m; // 18% de 200
            decimal totalEsperado = 236.00m;

            // Act
            TestContext.WriteLine("🆕 Creando pedido para validar cálculo IGV");
            nuevoPedidoPage.ClickNuevoPedido();

            TestContext.WriteLine($"🔍 Seleccionando cliente DNI: {dniCliente}");
            nuevoPedidoPage.BuscarCliente(dniCliente);
            nuevoPedidoPage.SeleccionarCliente(dniCliente, "MENDOZA");

            TestContext.WriteLine($"📦 Agregando producto cantidad: {cantidad}");
            nuevoPedidoPage.AgregarProducto(nombreProducto, cantidad);

            // Obtener valores calculados
            decimal subtotalCalculado = nuevoPedidoPage.ObtenerSubtotal();
            decimal igvCalculado = nuevoPedidoPage.ObtenerIGV();
            decimal totalCalculado = nuevoPedidoPage.ObtenerTotal();

            TestContext.WriteLine($"💰 Subtotal: S/ {subtotalCalculado}");
            TestContext.WriteLine($"💰 IGV (18%): S/ {igvCalculado}");
            TestContext.WriteLine($"💰 Total: S/ {totalCalculado}");

            // Assert
            Assert.That(subtotalCalculado, Is.EqualTo(subtotalEsperado).Within(0.01m),
                $"❌ ERROR: Subtotal incorrecto. Esperado: {subtotalEsperado}, Obtenido: {subtotalCalculado}");
            TestContext.WriteLine("✅ PV1: Subtotal=200 correcto");

            Assert.That(igvCalculado, Is.EqualTo(igvEsperado).Within(0.01m),
                $"❌ ERROR: IGV incorrecto. Esperado: {igvEsperado}, Obtenido: {igvCalculado}");
            TestContext.WriteLine("✅ PV2: IGV=36 (18% de 200) correcto");

            Assert.That(totalCalculado, Is.EqualTo(totalEsperado).Within(0.01m),
                $"❌ ERROR: Total incorrecto. Esperado: {totalEsperado}, Obtenido: {totalCalculado}");
            TestContext.WriteLine("✅ PV3: Total=236 correcto");

            // Verificar precisión de 2 decimales
            bool precision2Decimales = nuevoPedidoPage.VerificarPrecisionDecimal(2);
            Assert.That(precision2Decimales, Is.True,
                "❌ ERROR: Valores no tienen precisión de 2 decimales");
            TestContext.WriteLine("✅ PV4: Precisión 2 decimales correcta");

            TestContext.WriteLine("✅ Cálculo de IGV validado exitosamente");
        }
    }
}
