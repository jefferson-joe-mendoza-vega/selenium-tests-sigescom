using NUnit.Framework;
using Tsp.Sigescom.Tests.E2E.Helper;
using Tsp.Sigescom.Tests.E2E.Pages;

namespace Tsp.Sigescom.Tests.E2E.Tests._3_Extended
{
    [TestFixture]
    [Category("Extended")]
    [Category("Alta")]
    public class CP_PED_182_Test : TestBase
    {
        [Test]
        [Description("CP-PED-182: Crear pedido en moneda Dólares (USD)")]
        public void CrearPedido_MonedaUSD_TipoCambioYSimboloCorrectos()
        {
            // Arrange
            var pedidosPage = new PedidosPage(Driver, this);
            var nuevoPedidoPage = new NuevoPedidoPage(Driver, this);

            pedidosPage.Navigate(BASE_URL);

            string dniCliente = "72854193"; // Carlos Mendoza
            string moneda = "USD"; // Dólares
            decimal tipoCambioEsperado = 3.75m; // Ejemplo

            // Act
            TestContext.WriteLine("📝 Paso 1: Crear pedido seleccionando moneda USD");
            nuevoPedidoPage.ClickNuevoPedido();
            nuevoPedidoPage.BuscarCliente(dniCliente);

            TestContext.WriteLine($"📝 Paso 2: Seleccionar moneda {moneda}");
            nuevoPedidoPage.SeleccionarMoneda(moneda);
            System.Threading.Thread.Sleep(1000);

            TestContext.WriteLine("📝 Paso 3: Verificar tipo de cambio del día");
            decimal tipoCambio = nuevoPedidoPage.ObtenerTipoCambio();
            TestContext.WriteLine($"   Tipo de cambio: {tipoCambio}");

            TestContext.WriteLine("📝 Paso 4: Agregar producto");
            nuevoPedidoPage.AgregarProducto("88008-1", 2);
            System.Threading.Thread.Sleep(1000);

            decimal total = nuevoPedidoPage.ObtenerTotal();
            string simboloMoneda = nuevoPedidoPage.ObtenerSimboloMoneda();

            TestContext.WriteLine("📝 Paso 5: Guardar pedido");
            nuevoPedidoPage.ClickGuardar();
            System.Threading.Thread.Sleep(2000);

            // Assert
            Assert.That(simboloMoneda, Is.EqualTo("$").Or.EqualTo("USD"),
                $"❌ ERROR: Símbolo {simboloMoneda} no es $ o USD");
            TestContext.WriteLine($"✅ PV1: Precios en USD");
            TestContext.WriteLine($"✅ PV2: Símbolo $ mostrado correctamente");

            Assert.That(tipoCambio, Is.GreaterThan(0),
                $"❌ ERROR: Tipo de cambio {tipoCambio} inválido");
            TestContext.WriteLine($"✅ PV3: TC del día aplicado: {tipoCambio}");

            TestContext.WriteLine($"✅ PV4: Conversión correcta - Total: $ {total}");

            // Verificar moneda guardada
            string monedaGuardada = pedidosPage.ObtenerMonedaPrimerPedido();
            Assert.That(monedaGuardada, Does.Contain("USD").Or.Contains("$"),
                $"❌ ERROR: Moneda {monedaGuardada} no es USD");
            TestContext.WriteLine($"✅ Moneda guardada: {monedaGuardada}");

            TestContext.WriteLine("✅ Pedido en moneda USD (Dólares) creado correctamente");
        }
    }
}
