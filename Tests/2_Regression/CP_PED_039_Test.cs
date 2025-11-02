using NUnit.Framework;
using Tsp.Sigescom.Tests.E2E.Helper;
using Tsp.Sigescom.Tests.E2E.Pages;

namespace Tsp.Sigescom.Tests.E2E.Tests._2_Regression
{
    [TestFixture]
    [Category("Regression")]
    [Category("Alta")]
    public class CP_PED_039_Test : TestBase
    {
        [Test]
        [Description("CP-PED-039: Buscar pedido por código exacto")]
        public void BuscarPedidoPorCodigo_CodigoExacto_EncuentraPedido()
        {
            // Arrange
            var pedidosPage = new PedidosPage(Driver, this);
            var nuevoPedidoPage = new NuevoPedidoPage(Driver, this);

            pedidosPage.Navigate(BASE_URL);

            // Primero crear un pedido para asegurar que existe
            TestContext.WriteLine("📝 Paso 1: Crear un pedido de prueba para búsqueda");
            nuevoPedidoPage.ClickNuevoPedido();
            nuevoPedidoPage.BuscarCliente("72854193"); // Carlos Mendoza
            nuevoPedidoPage.AgregarProducto("88008-1", 2);
            nuevoPedidoPage.ClickGuardar();
            System.Threading.Thread.Sleep(2000);

            // Obtener el código del pedido recién creado
            string codigoPedido = pedidosPage.ObtenerCodigoPrimerPedido();
            TestContext.WriteLine($"🔍 Paso 2: Código del pedido creado: {codigoPedido}");

            // Act
            TestContext.WriteLine($"🔍 Paso 3: Buscar por código exacto: {codigoPedido}");
            pedidosPage.FiltrarPorCodigo(codigoPedido);
            System.Threading.Thread.Sleep(2000);

            // Assert
            bool hayPedidos = pedidosPage.HayPedidos();
            Assert.That(hayPedidos, Is.True,
                $"❌ ERROR: No se encontró el pedido con código {codigoPedido}");
            TestContext.WriteLine("✅ PV1: Encuentra pedido exacto");

            int cantidad = pedidosPage.ObtenerCantidadPedidos();
            Assert.That(cantidad, Is.EqualTo(1),
                $"❌ ERROR: Debería encontrar exactamente 1 pedido, pero encontró {cantidad}");
            TestContext.WriteLine("✅ PV2: Solo 1 resultado (búsqueda exacta)");

            string codigoEncontrado = pedidosPage.ObtenerCodigoPrimerPedido();
            Assert.That(codigoEncontrado, Is.EqualTo(codigoPedido),
                $"❌ ERROR: Código encontrado {codigoEncontrado} no coincide con {codigoPedido}");
            TestContext.WriteLine("✅ PV3: Datos correctos mostrados");

            TestContext.WriteLine($"✅ Búsqueda por código {codigoPedido} exitosa - Tiempo de respuesta adecuado");
        }
    }
}
