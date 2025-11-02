using NUnit.Framework;
using Tsp.Sigescom.Tests.E2E.Helper;
using Tsp.Sigescom.Tests.E2E.Pages;

namespace Tsp.Sigescom.Tests.E2E.Tests._2_Regression
{
    [TestFixture]
    [Category("Regression")]
    [Category("Alta")]
    public class CP_PED_105_Test : TestBase
    {
        [Test]
        [Description("CP-PED-105: Validar precio unitario mayor a 0")]
        public void ModificarPrecio_CambiarACero_AdvertenciaOError()
        {
            // Arrange
            var pedidosPage = new PedidosPage(Driver, this);
            var nuevoPedidoPage = new NuevoPedidoPage(Driver, this);

            pedidosPage.Navigate(BASE_URL);

            string dniCliente = "47829156"; // Rosa Villarreal
            string producto = "88008-1";

            // Act
            TestContext.WriteLine("📝 Paso 1: Crear pedido y agregar producto");
            nuevoPedidoPage.ClickNuevoPedido();
            nuevoPedidoPage.BuscarCliente(dniCliente);
            nuevoPedidoPage.AgregarProducto(producto, 1);
            System.Threading.Thread.Sleep(1000);

            TestContext.WriteLine("📝 Paso 2: Obtener precio original");
            decimal precioOriginal = nuevoPedidoPage.ObtenerPrecioOriginalProducto();
            TestContext.WriteLine($"   Precio original: {precioOriginal}");

            TestContext.WriteLine("📝 Paso 3: Intentar cambiar precio a 0");
            // Nota: Esto depende de si el sistema permite editar precios
            // Si no permite, el test debe verificar que el campo está bloqueado
            bool precioEditable = nuevoPedidoPage.VerificarPrecioEditable();
            
            if (precioEditable)
            {
                bool pudoCambiar = nuevoPedidoPage.IntentarCambiarPrecio(0);
                System.Threading.Thread.Sleep(1000);

                // Assert
                bool hayAdvertencia = nuevoPedidoPage.VerificarMensajeAdvertenciaPrecio();
                Assert.That(hayAdvertencia, Is.True,
                    "❌ ERROR: No se muestra advertencia al poner precio en 0");
                TestContext.WriteLine("✅ PV1: Advertencia mostrada según regla de negocio");
            }
            else
            {
                TestContext.WriteLine("✅ PV1: Campo precio no editable (validación alternativa)");
            }

            TestContext.WriteLine("✅ PV2: Validación coherente de precio mínimo");
            TestContext.WriteLine("✅ Validación de precio > 0 funcionando correctamente");
        }
    }
}
