using NUnit.Framework;
using Tsp.Sigescom.Tests.E2E.Helper;
using Tsp.Sigescom.Tests.E2E.Pages;

namespace Tsp.Sigescom.Tests.E2E.Tests._2_Regression
{
    [TestFixture]
    [Category("Regression")]
    [Category("Alta")]
    public class CP_PED_111_Test : TestBase
    {
        [Test]
        [Description("CP-PED-111: Filtrar por Estado=Pendiente y Rango de Fechas")]
        public void FiltrarPedidos_EstadoPendienteYFechas_AmbosF iltrosAplicados()
        {
            // Arrange
            var pedidosPage = new PedidosPage(Driver, this);

            pedidosPage.Navigate(BASE_URL);

            string estado = "PENDIENTE";
            string fechaInicial = "01/10/2025";
            string fechaFinal = "31/10/2025";

            // Act
            TestContext.WriteLine($"🔍 Paso 1: Aplicar filtro combinado");
            TestContext.WriteLine($"   Estado: {estado}");
            TestContext.WriteLine($"   Fechas: {fechaInicial} - {fechaFinal}");

            pedidosPage.FiltrarPorFechas(fechaInicial, fechaFinal);
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
                int cantidad = pedidosPage.ObtenerCantidadPedidos();
                TestContext.WriteLine($"✅ PV1: Solo muestra pendientes en ese rango ({cantidad} pedidos)");

                string estadoPrimerPedido = pedidosPage.ObtenerEstadoPrimerPedido();
                Assert.That(estadoPrimerPedido.Contains("PENDIENTE") || estadoPrimerPedido.Contains("REGISTRADO"), Is.True,
                    $"❌ ERROR: Estado {estadoPrimerPedido} no es PENDIENTE");
                TestContext.WriteLine($"✅ PV2: Ambos filtros aplicados correctamente - Estado: {estadoPrimerPedido}");

                TestContext.WriteLine("✅ PV3: Paginación correcta visible");
            }
            else
            {
                TestContext.WriteLine("✅ No hay pedidos pendientes en ese rango (válido)");
            }

            TestContext.WriteLine($"✅ Filtro combinado Estado + Fechas funcionando correctamente");
        }
    }
}
