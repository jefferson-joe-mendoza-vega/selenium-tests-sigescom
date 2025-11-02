using NUnit.Framework;
using Tsp.Sigescom.Tests.E2E.Helper;
using Tsp.Sigescom.Tests.E2E.Pages;

namespace Tsp.Sigescom.Tests.E2E.Tests._2_Regression
{
    [TestFixture]
    [Category("Regression")]
    [Category("Alta")]
    public class CP_PED_106_Test : TestBase
    {
        [Test]
        [Description("CP-PED-106: Buscar por código de pedido exacto")]
        public void BuscarPedido_CodigoExacto_EncuentraUnicoResultado()
        {
            // Arrange
            var pedidosPage = new PedidosPage(Driver, this);
            var nuevoPedidoPage = new NuevoPedidoPage(Driver, this);

            pedidosPage.Navigate(BASE_URL);

            // Crear pedido de prueba
            TestContext.WriteLine("📝 Paso 1: Crear pedido para búsqueda");
            nuevoPedidoPage.ClickNuevoPedido();
            nuevoPedidoPage.BuscarCliente("72854193"); // Carlos Mendoza
            nuevoPedidoPage.AgregarProducto("88008-1", 3);
            nuevoPedidoPage.ClickGuardar();
            System.Threading.Thread.Sleep(2000);

            string codigoBuscado = pedidosPage.ObtenerCodigoPrimerPedido();
            TestContext.WriteLine($"📝 Paso 2: Código a buscar: {codigoBuscado}");

            // Act
            TestContext.WriteLine($"🔍 Paso 3: Buscar código exacto '{codigoBuscado}'");
            var inicioTiempo = System.DateTime.Now;
            pedidosPage.FiltrarPorCodigo(codigoBuscado);
            System.Threading.Thread.Sleep(1000);
            var tiempoTranscurrido = (System.DateTime.Now - inicioTiempo).TotalSeconds;

            // Assert
            bool hayPedidos = pedidosPage.HayPedidos();
            Assert.That(hayPedidos, Is.True,
                $"❌ ERROR: No encuentra pedido con código {codigoBuscado}");
            TestContext.WriteLine("✅ PV1: Encuentra pedido exacto");

            int cantidad = pedidosPage.ObtenerCantidadPedidos();
            Assert.That(cantidad, Is.EqualTo(1),
                $"❌ ERROR: Debería encontrar 1 resultado, pero encontró {cantidad}");
            TestContext.WriteLine("✅ PV2: Solo 1 resultado (búsqueda exacta)");

            string codigoEncontrado = pedidosPage.ObtenerCodigoPrimerPedido();
            decimal total = pedidosPage.ObtenerTotalPrimerPedido();
            Assert.That(codigoEncontrado, Is.EqualTo(codigoBuscado),
                $"❌ ERROR: Código {codigoEncontrado} no coincide con {codigoBuscado}");
            TestContext.WriteLine($"✅ PV3: Datos correctos - Código: {codigoEncontrado}, Total: {total}");

            Assert.That(tiempoTranscurrido, Is.LessThan(2),
                $"❌ ERROR: Tiempo de respuesta {tiempoTranscurrido:F2}s excede 2 segundos");
            TestContext.WriteLine($"✅ PV4: Tiempo de respuesta < 2 seg ({tiempoTranscurrido:F2}s)");

            TestContext.WriteLine($"✅ Búsqueda directa por código exitosa");
        }
    }
}
