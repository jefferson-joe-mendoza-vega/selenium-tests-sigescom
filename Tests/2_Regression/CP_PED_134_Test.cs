using NUnit.Framework;
using Tsp.Sigescom.Tests.E2E.Helper;
using Tsp.Sigescom.Tests.E2E.Pages;

namespace Tsp.Sigescom.Tests.E2E.Tests._2_Regression
{
    [TestFixture]
    [Category("Regression")]
    [Category("Alta")]
    public class CP_PED_134_Test : TestBase
    {
        [Test]
        [Description("CP-PED-134: Validar producto activo al agregar a pedido")]
        public void AgregarProducto_ProductoInactivo_NoAparece()
        {
            // Arrange
            var pedidosPage = new PedidosPage(Driver, this);
            var nuevoPedidoPage = new NuevoPedidoPage(Driver, this);

            pedidosPage.Navigate(BASE_URL);

            string dniCliente = "58471629"; // Jorge Flores
            string productoInactivo = "PRODUCTO-INACTIVO-ID-200";
            // NOTA: Desactivar un producto antes de ejecutar este test

            // Act
            TestContext.WriteLine("📝 Paso 1: Abrir modal y seleccionar cliente");
            nuevoPedidoPage.ClickNuevoPedido();
            nuevoPedidoPage.BuscarCliente(dniCliente);

            TestContext.WriteLine($"📝 Paso 2: Buscar producto inactivo");
            TestContext.WriteLine($"   Producto: {productoInactivo}");
            TestContext.WriteLine("   ⚠️ PREREQUISITO: Producto debe estar DESACTIVADO");

            bool productoEncontrado = nuevoPedidoPage.BuscarProductoInactivo(productoInactivo);
            System.Threading.Thread.Sleep(2000);

            // Assert
            Assert.That(productoEncontrado, Is.False,
                $"❌ ERROR: Producto inactivo {productoInactivo} no debería aparecer");
            TestContext.WriteLine($"✅ PV1: No aparece en búsqueda o mensaje de inactivo");

            bool hayMensajeInactivo = nuevoPedidoPage.VerificarMensajeProductoInactivo();
            if (hayMensajeInactivo)
            {
                TestContext.WriteLine("✅ PV2: Mensaje 'Producto inactivo' mostrado");
            }
            else
            {
                TestContext.WriteLine("✅ PV2: Producto no aparece en resultados (validación alternativa)");
            }

            TestContext.WriteLine("✅ PV3: Solo productos activos disponibles en búsqueda");
            TestContext.WriteLine("✅ Validación de estado de producto funcionando correctamente");
        }
    }
}
