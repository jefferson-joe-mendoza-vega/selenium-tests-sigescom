using NUnit.Framework;
using Tsp.Sigescom.Tests.E2E.Helper;
using Tsp.Sigescom.Tests.E2E.Pages;

namespace Tsp.Sigescom.Tests.E2E.Tests._3_Extended
{
    [TestFixture]
    [Category("Extended")]
    [Category("Media")]
    public class CP_PED_118_Test : TestBase
    {
        [Test]
        [Description("CP-PED-118: Ordenar por Total mayor a menor")]
        public void OrdenarPedidos_TotalDescendente_MayorAMenor()
        {
            // Arrange
            var pedidosPage = new PedidosPage(Driver, this);

            pedidosPage.Navigate(BASE_URL);

            // Act
            TestContext.WriteLine("📝 Paso 1: Consultar pedidos");
            pedidosPage.ClickConsultar();
            System.Threading.Thread.Sleep(2000);

            TestContext.WriteLine("📝 Paso 2: Clic 2 veces en columna 'Total' hasta DESC");
            pedidosPage.ClickColumnaTotal();
            System.Threading.Thread.Sleep(500);
            pedidosPage.ClickColumnaTotal(); // Segunda vez para DESC
            System.Threading.Thread.Sleep(1500);

            // Assert
            var totales = pedidosPage.ObtenerTotalesDePedidos();
            bool ordenDescendente = pedidosPage.VerificarOrdenNumericoDescendente(totales);
            
            Assert.That(ordenDescendente, Is.True,
                "❌ ERROR: Totales no están ordenados de mayor a menor");
            TestContext.WriteLine($"✅ PV1: Mayor a menor correcto");
            TestContext.WriteLine($"   Mayor: S/ {totales.Max():F2}");
            TestContext.WriteLine($"   Menor: S/ {totales.Min():F2}");

            // Verificar que es orden numérico, no alfabético
            // Ejemplo: 1000, 500, 50 (numérico) vs "1000", "50", "500" (alfabético)
            bool ordenNumerico = true;
            for (int i = 0; i < totales.Count - 1; i++)
            {
                if (totales[i] < totales[i + 1])
                {
                    ordenNumerico = false;
                    break;
                }
            }
            Assert.That(ordenNumerico, Is.True,
                "❌ ERROR: Orden no es numérico (parece alfabético)");
            TestContext.WriteLine("✅ PV2: Valores numéricos ordenados correctamente");

            TestContext.WriteLine("✅ PV3: No es orden alfabético, es numérico");
            TestContext.WriteLine("✅ Ordenamiento por Total funcionando - Ver pedidos de mayor valor");
        }
    }
}
