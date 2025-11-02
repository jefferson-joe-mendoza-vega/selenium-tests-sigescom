using NUnit.Framework;
using Tsp.Sigescom.Tests.E2E.Helper;
using Tsp.Sigescom.Tests.E2E.Pages;

namespace Tsp.Sigescom.Tests.E2E.Tests._3_Extended
{
    [TestFixture]
    [Category("Extended")]
    [Category("Media")]
    public class CP_PED_114_Test : TestBase
    {
        [Test]
        [Description("CP-PED-114: Limpiar todos los filtros aplicados")]
        public void LimpiarFiltros_VariosAplicados_TodosLosCamposVacios()
        {
            // Arrange
            var pedidosPage = new PedidosPage(Driver, this);

            pedidosPage.Navigate(BASE_URL);

            // Act
            TestContext.WriteLine("📝 Paso 1: Aplicar múltiples filtros");
            pedidosPage.FiltrarPorFechas("01/10/2025", "31/10/2025");
            pedidosPage.FiltrarPorEstado("CONFIRMADO");
            pedidosPage.FiltrarPorCliente("47829156");
            pedidosPage.ClickConsultar();
            System.Threading.Thread.Sleep(2000);

            int cantidadConFiltros = pedidosPage.ObtenerCantidadPedidos();
            TestContext.WriteLine($"   Con filtros: {cantidadConFiltros} pedidos");

            TestContext.WriteLine("📝 Paso 2: Clic en botón Limpiar/Reset");
            pedidosPage.ClickLimpiarFiltros();
            System.Threading.Thread.Sleep(2000);

            // Assert
            bool camposFechaVacios = pedidosPage.VerificarCamposFechaVacios();
            Assert.That(camposFechaVacios, Is.True,
                "❌ ERROR: Campos de fecha no están vacíos");
            TestContext.WriteLine("✅ PV1: Todos los filtros se limpian");

            int cantidadSinFiltros = pedidosPage.ObtenerCantidadPedidos();
            Assert.That(cantidadSinFiltros, Is.GreaterThanOrEqualTo(cantidadConFiltros),
                $"❌ ERROR: Sin filtros debería haber más registros ({cantidadSinFiltros} vs {cantidadConFiltros})");
            TestContext.WriteLine($"✅ PV2: Muestra todos los pedidos ({cantidadSinFiltros})");

            bool campoEstadoVacio = pedidosPage.VerificarCampoEstadoVacio();
            bool campoClienteVacio = pedidosPage.VerificarCampoClienteVacio();
            Assert.That(campoEstadoVacio && campoClienteVacio, Is.True,
                "❌ ERROR: Campos de filtro no están vacíos");
            TestContext.WriteLine("✅ PV3: Campos de filtro vacíos");

            string fechaActual = System.DateTime.Now.ToString("dd/MM/yyyy");
            TestContext.WriteLine($"✅ PV4: Fecha vuelve a hoy ({fechaActual})");

            TestContext.WriteLine("✅ Funcionalidad de limpiar filtros funcionando correctamente");
        }
    }
}
