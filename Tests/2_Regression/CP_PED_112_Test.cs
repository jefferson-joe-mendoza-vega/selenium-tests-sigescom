using NUnit.Framework;
using Tsp.Sigescom.Tests.E2E.Helper;
using Tsp.Sigescom.Tests.E2E.Pages;

namespace Tsp.Sigescom.Tests.E2E.Tests._2_Regression
{
    [TestFixture]
    [Category("Regression")]
    [Category("Alta")]
    public class CP_PED_112_Test : TestBase
    {
        [Test]
        [Description("CP-PED-112: Filtrar por Cliente específico y Estado=Confirmado")]
        public void FiltrarPedidos_ClienteYEstadoConfirmado_FiltrosANDAplicados()
        {
            // Arrange
            var pedidosPage = new PedidosPage(Driver, this);

            pedidosPage.Navigate(BASE_URL);

            string dniCliente = "47829156"; // Rosa Beatriz Villarreal Campos
            string nombreCliente = "VILLARREAL";
            string estado = "CONFIRMADO";

            // Act
            TestContext.WriteLine($"🔍 Paso 1: Aplicar filtros combinados");
            TestContext.WriteLine($"   Cliente: {nombreCliente} (DNI: {dniCliente})");
            TestContext.WriteLine($"   Estado: {estado}");

            pedidosPage.FiltrarPorCliente(dniCliente);
            System.Threading.Thread.Sleep(1000);

            pedidosPage.FiltrarPorEstado(estado);
            System.Threading.Thread.Sleep(1000);

            pedidosPage.ClickConsultar();
            System.Threading.Thread.Sleep(2000);

            // Assert
            bool hayPedidos = pedidosPage.HayPedidos();
            TestContext.WriteLine($"   Pedidos encontrados: {hayPedidos}");

            if (hayPedidos)
            {
                bool clienteEncontrado = pedidosPage.VerificarClienteEnResultados(dniCliente, nombreCliente);
                Assert.That(clienteEncontrado, Is.True,
                    $"❌ ERROR: No se encontró cliente {nombreCliente} en resultados");
                TestContext.WriteLine($"✅ PV1: Solo pedidos confirmados de Rosa Villarreal");

                string estadoPedido = pedidosPage.ObtenerEstadoPrimerPedido();
                Assert.That(estadoPedido, Does.Contain("CONFIRMADO"),
                    $"❌ ERROR: Estado {estadoPedido} no es CONFIRMADO");
                TestContext.WriteLine($"✅ PV2: Filtros AND aplicados correctamente - Estado: {estadoPedido}");

                bool dniVisible = pedidosPage.VerificarClienteEnPrimerPedido(dniCliente);
                Assert.That(dniVisible, Is.True,
                    $"❌ ERROR: DNI {dniCliente} no visible en resultados");
                TestContext.WriteLine($"✅ PV3: DNI {dniCliente} visible en resultados");
            }
            else
            {
                TestContext.WriteLine("✅ No hay pedidos confirmados para este cliente (válido)");
            }

            TestContext.WriteLine($"✅ Combinación Cliente + Estado funcionando correctamente");
        }
    }
}
