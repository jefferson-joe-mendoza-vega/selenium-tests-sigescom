using NUnit.Framework;
using Tsp.Sigescom.Tests.E2E.Helper;
using Tsp.Sigescom.Tests.E2E.Pages;

namespace Tsp.Sigescom.Tests.E2E.Tests._1_Smoke
{
    [TestFixture]
    [Category("Smoke")]
    [Category("Critica")]
    public class CP_PED_007_Test : TestBase
    {
        [Test]
        [Description("CP-PED-007: Crear pedido válido completo con cliente Carlos Mendoza")]
        public void CrearPedidoValido_ClienteCarlosMendoza_PedidoRegistradoExitosamente()
        {
            // Arrange - PASAR 'this' como segundo parámetro
            var pedidosPage = new PedidosPage(Driver, this);
            var nuevoPedidoPage = new NuevoPedidoPage(Driver, this); // ← CAMBIO AQUÍ

            pedidosPage.Navigate(BASE_URL);

            // Datos del cliente y producto
            string dniCliente = "72854193";
            string nombreCliente = "MENDOZA QUIROZ CARLOS ALBERTO";
            string alias = "Carlos Mendoza";
            string nombreProducto = "CEMENTO";
            int cantidad = 5;
            decimal totalEsperado = 590.00m;

            // Act
            TestContext.WriteLine("🆕 Iniciando creación de nuevo pedido");

            nuevoPedidoPage.ClickNuevoPedido();

            TestContext.WriteLine($"🔍 Buscando cliente DNI: {dniCliente}");
            nuevoPedidoPage.BuscarCliente(dniCliente);
            nuevoPedidoPage.SeleccionarCliente(dniCliente, nombreCliente);

            TestContext.WriteLine($"📝 Ingresando alias: {alias}");
            nuevoPedidoPage.IngresarAlias(alias);

            TestContext.WriteLine($"📦 Agregando producto cantidad: {cantidad}");
            nuevoPedidoPage.AgregarProducto(nombreProducto, cantidad);

            bool hayInconsistencias = nuevoPedidoPage.VerificarInconsistencias();
            if (hayInconsistencias)
            {
                Assert.Fail("❌ Hay inconsistencias que impiden guardar el pedido");
            }

            var totalCalculado = nuevoPedidoPage.ObtenerTotal();
            TestContext.WriteLine($"💰 Total calculado: S/ {totalCalculado}");

            TestContext.WriteLine("💾 Guardando pedido...");
            nuevoPedidoPage.ClickGuardar();

            // Assert
            bool mensajeExito = nuevoPedidoPage.VerificarMensajeExito();
            Assert.That(mensajeExito, Is.True,
                "❌ ERROR: No apareció mensaje de confirmación");
            TestContext.WriteLine("✅ PV1: Mensaje de éxito confirmado");

            System.Threading.Thread.Sleep(2000);
            Assert.That(pedidosPage.HayPedidos(), Is.True,
                "❌ ERROR: El nuevo pedido no aparece en el listado");
            TestContext.WriteLine("✅ PV2: Pedido aparece en listado");

            pedidosPage.FiltrarPorCliente(dniCliente);
            bool clienteEncontrado = pedidosPage.VerificarClienteEnResultados(dniCliente, "MENDOZA");
            Assert.That(clienteEncontrado, Is.True,
                $"❌ ERROR: No se encontró el pedido del cliente {nombreCliente}");
            TestContext.WriteLine($"✅ PV4: Cliente verificado en resultados");

            TestContext.WriteLine("✅ Pedido creado exitosamente - Test completado");
        }
    }
}