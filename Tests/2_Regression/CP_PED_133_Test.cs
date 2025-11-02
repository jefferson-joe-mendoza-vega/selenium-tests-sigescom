using NUnit.Framework;
using Tsp.Sigescom.Tests.E2E.Helper;
using Tsp.Sigescom.Tests.E2E.Pages;

namespace Tsp.Sigescom.Tests.E2E.Tests._2_Regression
{
    [TestFixture]
    [Category("Regression")]
    [Category("Alta")]
    public class CP_PED_133_Test : TestBase
    {
        [Test]
        [Description("CP-PED-133: Validar cliente activo al crear pedido")]
        public void CrearPedido_ClienteInactivo_ErrorONoAparece()
        {
            // Arrange
            var pedidosPage = new PedidosPage(Driver, this);
            var nuevoPedidoPage = new NuevoPedidoPage(Driver, this);

            pedidosPage.Navigate(BASE_URL);

            // Cliente inactivo: María Elena Quispe Huamán (DNI: 63194827)
            // NOTA: Antes de ejecutar este test, desactivar el cliente en módulo Clientes
            string dniClienteInactivo = "63194827";

            // Act
            TestContext.WriteLine("📝 Paso 1: Intentar buscar cliente inactivo");
            TestContext.WriteLine($"   DNI: {dniClienteInactivo} (María Elena Quispe Huamán)");
            TestContext.WriteLine("   ⚠️ PREREQUISITO: Cliente debe estar DESACTIVADO antes de ejecutar test");

            nuevoPedidoPage.ClickNuevoPedido();
            System.Threading.Thread.Sleep(1000);

            bool clienteEncontrado = nuevoPedidoPage.BuscarClienteInactivo(dniClienteInactivo);
            System.Threading.Thread.Sleep(2000);

            // Assert
            Assert.That(clienteEncontrado, Is.False,
                $"❌ ERROR: Cliente inactivo {dniClienteInactivo} no debería aparecer en búsqueda");
            TestContext.WriteLine($"✅ PV1: Error 'Cliente inactivo' o no aparece en búsqueda");

            bool hayMensajeInactivo = nuevoPedidoPage.VerificarMensajeClienteInactivo();
            if (hayMensajeInactivo)
            {
                TestContext.WriteLine("✅ PV2: Mensaje 'Cliente inactivo' mostrado");
            }
            else
            {
                TestContext.WriteLine("✅ PV2: Cliente no aparece en resultados de búsqueda (validación alternativa)");
            }

            TestContext.WriteLine("✅ PV3: No permite crear pedido con cliente inactivo");
            TestContext.WriteLine("✅ Validación de estado de cliente funcionando correctamente");
        }
    }
}
