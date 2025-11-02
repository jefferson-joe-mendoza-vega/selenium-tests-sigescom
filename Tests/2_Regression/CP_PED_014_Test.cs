using NUnit.Framework;
using Tsp.Sigescom.Tests.E2E.Helper;
using Tsp.Sigescom.Tests.E2E.Pages;

namespace Tsp.Sigescom.Tests.E2E.Tests._2_Regression
{
    [TestFixture]
    [Category("Regression")]
    [Category("Critica")]
    public class CP_PED_014_Test : TestBase
    {
        [Test]
        [Description("CP-PED-014: Aplicar descuento a producto")]
        public void AplicarDescuento_10Porciento_CalculaCorrecto()
        {
            // Arrange
            var pedidosPage = new PedidosPage(Driver, this);
            var nuevoPedidoPage = new NuevoPedidoPage(Driver, this);

            pedidosPage.Navigate(BASE_URL);

            string dniCliente = "72854193";
            string nombreProducto = "CEMENTO";
            int cantidad = 1;
            decimal precioOriginal = 100.00m;
            decimal descuentoPorcentaje = 10.00m;
            decimal descuentoEsperado = 10.00m;
            decimal precioFinalEsperado = 90.00m;

            // Act
            TestContext.WriteLine("🆕 Creando pedido con descuento");
            nuevoPedidoPage.ClickNuevoPedido();
            nuevoPedidoPage.BuscarCliente(dniCliente);
            nuevoPedidoPage.SeleccionarCliente(dniCliente, "MENDOZA");

            TestContext.WriteLine($"📦 Agregando producto");
            nuevoPedidoPage.AgregarProducto(nombreProducto, cantidad);

            TestContext.WriteLine($"💸 Aplicando descuento {descuentoPorcentaje}%");
            nuevoPedidoPage.AplicarDescuentoAProducto(descuentoPorcentaje);

            decimal precioMostrado = nuevoPedidoPage.ObtenerPrecioOriginalProducto();
            decimal descuentoAplicado = nuevoPedidoPage.ObtenerDescuentoAplicado();
            decimal precioFinalCalculado = nuevoPedidoPage.ObtenerPrecioFinalProducto();
            decimal igvCalculado = nuevoPedidoPage.ObtenerIGV();

            TestContext.WriteLine($"💰 Precio original: S/ {precioMostrado}");
            TestContext.WriteLine($"💰 Descuento: S/ {descuentoAplicado}");
            TestContext.WriteLine($"💰 Precio final: S/ {precioFinalCalculado}");

            // Assert
            Assert.That(precioMostrado, Is.EqualTo(precioOriginal).Within(0.01m),
                "❌ ERROR: Precio original incorrecto");
            TestContext.WriteLine("✅ PV1: Precio original 100 correcto");

            Assert.That(descuentoAplicado, Is.EqualTo(descuentoEsperado).Within(0.01m),
                "❌ ERROR: Descuento aplicado incorrecto");
            TestContext.WriteLine("✅ PV2: Descuento aplicado=10 correcto");

            Assert.That(precioFinalCalculado, Is.EqualTo(precioFinalEsperado).Within(0.01m),
                "❌ ERROR: Precio final incorrecto");
            TestContext.WriteLine("✅ PV3: Precio final=90 correcto");

            // Verificar que IGV se calcula sobre precio con descuento
            decimal subtotalEsperado = 90.00m;
            decimal igvEsperado = 16.20m; // 18% de 90
            Assert.That(igvCalculado, Is.EqualTo(igvEsperado).Within(0.01m),
                "❌ ERROR: IGV no se calculó sobre precio con descuento");
            TestContext.WriteLine("✅ PV4: IGV calculado sobre precio con descuento");

            TestContext.WriteLine("✅ Descuento aplicado y validado exitosamente");
        }
    }
}
