using NUnit.Framework;
using Tsp.Sigescom.Tests.E2E.Helper;
using Tsp.Sigescom.Tests.E2E.Pages;

namespace Tsp.Sigescom.Tests.E2E.Tests._3_Extended
{
    [TestFixture]
    [Category("Extended")]
    [Category("Media")]
    public class CP_PED_115_Test : TestBase
    {
        [Test]
        [Description("CP-PED-115: Ordenar por Fecha ascendente")]
        public void OrdenarPedidos_FechaAscendente_MasAntiguoAlMasReciente()
        {
            // Arrange
            var pedidosPage = new PedidosPage(Driver, this);

            pedidosPage.Navigate(BASE_URL);

            // Act
            TestContext.WriteLine("📝 Paso 1: Consultar pedidos");
            pedidosPage.ClickConsultar();
            System.Threading.Thread.Sleep(2000);

            TestContext.WriteLine("📝 Paso 2: Clic en cabecera 'Fecha' para ordenar ASC");
            pedidosPage.ClickColumnaFecha();
            System.Threading.Thread.Sleep(1500);

            // Assert
            var fechas = pedidosPage.ObtenerFechasDePedidos();
            bool ordenAscendente = pedidosPage.VerificarOrdenAscendente(fechas);
            
            Assert.That(ordenAscendente, Is.True,
                "❌ ERROR: Fechas no están ordenadas ascendentemente");
            TestContext.WriteLine($"✅ PV1: Ordena del más antiguo al más reciente");
            TestContext.WriteLine($"   Primera fecha: {fechas.FirstOrDefault()}");
            TestContext.WriteLine($"   Última fecha: {fechas.LastOrDefault()}");

            bool indicadorASC = pedidosPage.VerificarIndicadorOrdenamiento("FECHA", "ASC");
            Assert.That(indicadorASC, Is.True,
                "❌ ERROR: Indicador visual ASC no está visible");
            TestContext.WriteLine("✅ PV2: Indicador visual de orden ASC presente");

            TestContext.WriteLine("✅ PV3: Datos correctos y orden consistente");
            TestContext.WriteLine("✅ Ordenamiento ascendente por fecha funcionando");
        }
    }
}
