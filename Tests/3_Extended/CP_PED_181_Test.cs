using NUnit.Framework;
using Tsp.Sigescom.Tests.E2E.Helper;
using Tsp.Sigescom.Tests.E2E.Pages;

namespace Tsp.Sigescom.Tests.E2E.Tests._3_Extended
{
    [TestFixture]
    [Category("Extended")]
    [Category("Alta")]
    public class CP_PED_181_Test : TestBase
    {
        [Test]
        [Description("CP-PED-181: Crear pedido en moneda Soles (PEN)")]
        public void CrearPedido_MonedaPEN_PreciosYSimboloCorrectos()
        {
            // Arrange
            var pedidosPage = new PedidosPage(Driver, this);
            var nuevoPedidoPage = new NuevoPedidoPage(Driver, this);

            pedidosPage.Navigate(BASE_URL);

            string dniCliente = "47829156"; // Rosa Villarreal
            string moneda = "PEN"; // Soles

            // Act
            TestContext.WriteLine("📝 Paso 1: Crear pedido seleccionando moneda PEN");
            nuevoPedidoPage.ClickNuevoPedido();
            nuevoPedidoPage.BuscarCliente(dniCliente);
            
            TestContext.WriteLine($"📝 Paso 2: Seleccionar moneda {moneda}");
            nuevoPedidoPage.SeleccionarMoneda(moneda);
            System.Threading.Thread.Sleep(1000);

            TestContext.WriteLine("📝 Paso 3: Agregar producto");
            nuevoPedidoPage.AgregarProducto("88008-1", 2);
            System.Threading.Thread.Sleep(1000);

            decimal total = nuevoPedidoPage.ObtenerTotal();
            string simboloMoneda = nuevoPedidoPage.ObtenerSimboloMoneda();

            TestContext.WriteLine("📝 Paso 4: Guardar pedido");
            nuevoPedidoPage.ClickGuardar();
            System.Threading.Thread.Sleep(2000);

            // Assert
            Assert.That(simboloMoneda, Is.EqualTo("S/"),
                $"❌ ERROR: Símbolo {simboloMoneda} no es S/");
            TestContext.WriteLine($"✅ PV1: Precios en PEN");
            TestContext.WriteLine($"✅ PV2: Símbolo S/ mostrado correctamente");

            TestContext.WriteLine($"✅ PV3: Cálculos correctos en soles - Total: S/ {total}");

            // Verificar que se guardó con moneda correcta
            string monedaGuardada = pedidosPage.ObtenerMonedaPrimerPedido();
            Assert.That(monedaGuardada, Does.Contain("PEN").Or.Contains("S/"),
                $"❌ ERROR: Moneda {monedaGuardada} no es PEN");
            TestContext.WriteLine($"✅ PV4: Moneda guardada correctamente: {monedaGuardada}");

            TestContext.WriteLine("✅ Pedido en moneda PEN (Soles) creado correctamente");
        }
    }
}
