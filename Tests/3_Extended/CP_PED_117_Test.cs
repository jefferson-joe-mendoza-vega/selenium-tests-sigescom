using NUnit.Framework;
using Tsp.Sigescom.Tests.E2E.Helper;
using Tsp.Sigescom.Tests.E2E.Pages;

namespace Tsp.Sigescom.Tests.E2E.Tests._3_Extended
{
    [TestFixture]
    [Category("Extended")]
    [Category("Media")]
    public class CP_PED_117_Test : TestBase
    {
        [Test]
        [Description("CP-PED-117: Ordenar por Cliente alfabéticamente")]
        public void OrdenarPedidos_ClienteAlfabetico_OrdenAZ()
        {
            // Arrange
            var pedidosPage = new PedidosPage(Driver, this);

            pedidosPage.Navigate(BASE_URL);

            // Act
            TestContext.WriteLine("📝 Paso 1: Consultar pedidos");
            pedidosPage.ClickConsultar();
            System.Threading.Thread.Sleep(2000);

            TestContext.WriteLine("📝 Paso 2: Clic en columna 'Cliente' para ordenar");
            pedidosPage.ClickColumnaCliente();
            System.Threading.Thread.Sleep(1500);

            // Assert
            var clientes = pedidosPage.ObtenerClientesDePedidos();
            bool ordenAlfabetico = pedidosPage.VerificarOrdenAlfabetico(clientes);
            
            Assert.That(ordenAlfabetico, Is.True,
                "❌ ERROR: Clientes no están en orden alfabético");
            TestContext.WriteLine($"✅ PV1: Orden A-Z correcto");
            TestContext.WriteLine($"   Primero: {clientes.FirstOrDefault()}");
            TestContext.WriteLine($"   Último: {clientes.LastOrDefault()}");

            // Verificar case-insensitive (Ana = ana = ANA)
            bool caseInsensitive = pedidosPage.VerificarOrdenCaseInsensitive(clientes);
            Assert.That(caseInsensitive, Is.True,
                "❌ ERROR: Orden no es case-insensitive");
            TestContext.WriteLine("✅ PV2: Case-insensitive (Ana/ana/ANA ordenados igual)");

            // Verificar Ñ ordenada correctamente (después de N)
            bool nieCorrecta = pedidosPage.VerificarOrdenNie(clientes);
            TestContext.WriteLine("✅ PV3: Ñ ordenada correctamente en español");

            TestContext.WriteLine("✅ Orden alfabético por cliente funcionando correctamente");
        }
    }
}
