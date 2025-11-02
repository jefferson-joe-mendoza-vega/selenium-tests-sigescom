using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.IO;
using System.Linq;

namespace Tsp.Sigescom.Tests.E2E.Helper
{
    public class TestBase
    {
        protected IWebDriver Driver;
        protected const string BASE_URL = "http://161.132.67.82:31098";
        protected const string EMAIL = "admin@plazafer.com";
        protected const string PASSWORD = "calidad";

        // HACER PÚBLICO para que Page Objects puedan acceder
        public IWebElement UltimoElementoInteractuado;

        [SetUp]
        public void Setup()
        {
            var options = new ChromeOptions();
            options.AddArgument("--start-maximized");
            options.AddArgument("--disable-gpu");
            options.AddArgument("--no-sandbox");

            Driver = new ChromeDriver(options);
            Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);

            RealizarLogin();
        }

        [TearDown]
        public void Teardown()
        {
            try
            {
                if (TestContext.CurrentContext.Result.Outcome.Status ==
                    NUnit.Framework.Interfaces.TestStatus.Failed)
                {
                    TakeEnhancedScreenshot(TestContext.CurrentContext.Test.Name);
                }
            }
            catch (Exception ex)
            {
                TestContext.WriteLine($"⚠️ Error en TearDown: {ex.Message}");
            }
            finally
            {
                Driver?.Quit();
            }
        }

        private void RealizarLogin()
        {
            try
            {
                Driver.Navigate().GoToUrl($"{BASE_URL}/Account/Login");
                System.Threading.Thread.Sleep(1500);

                Driver.FindElement(By.Id("Email")).SendKeys(EMAIL);
                Driver.FindElement(By.Id("Password")).SendKeys(PASSWORD);
                Driver.FindElement(By.CssSelector("button[type='submit']")).Click();

                System.Threading.Thread.Sleep(3000);

                var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(15));

                try
                {
                    var ddlCentro = Driver.FindElements(By.Name("IdCentroDeAtencionSeleccionado"));

                    if (ddlCentro.Count > 0)
                    {
                        var selectElement = new SelectElement(ddlCentro[0]);

                        if (selectElement.SelectedOption.Text.Contains("Seleccione"))
                        {
                            selectElement.SelectByIndex(1);
                            System.Threading.Thread.Sleep(500);
                        }
                    }
                }
                catch
                {
                    TestContext.WriteLine("⚠️ Dropdown no encontrado, asumiendo centro ya seleccionado");
                }

                try
                {
                    var btnAceptar = wait.Until(d => d.FindElement(By.XPath("//button[text()='Aceptar']")));
                    btnAceptar.Click();
                    TestContext.WriteLine("✅ Clic en Aceptar exitoso (XPath texto)");
                }
                catch
                {
                    try
                    {
                        var btnAceptar = wait.Until(d => d.FindElement(By.CssSelector("button.btn-primary")));
                        btnAceptar.Click();
                        TestContext.WriteLine("✅ Clic en Aceptar exitoso (CSS clase)");
                    }
                    catch
                    {
                        try
                        {
                            var btnAceptar = wait.Until(d => d.FindElement(By.XPath("//button[contains(text(),'Aceptar')]")));
                            btnAceptar.Click();
                            TestContext.WriteLine("✅ Clic en Aceptar exitoso (XPath contains)");
                        }
                        catch (Exception ex)
                        {
                            TestContext.WriteLine($"❌ No se pudo hacer clic en Aceptar: {ex.Message}");
                            throw;
                        }
                    }
                }

                System.Threading.Thread.Sleep(2000);
                TestContext.WriteLine("✅ Login completado exitosamente");
            }
            catch (Exception ex)
            {
                TestContext.WriteLine($"❌ Error en login: {ex.Message}");
                TakeEnhancedScreenshot("Login_Error");
                throw;
            }
        }

        /// <summary>
        /// Método público para trackear elementos y resaltarlos con borde rojo
        /// </summary>
        public void TrackElement(IWebElement element)
        {
            try
            {
                UltimoElementoInteractuado = element;

                // Resaltar inmediatamente con borde rojo usando propiedades directas del estilo
                ((IJavaScriptExecutor)Driver).ExecuteScript(
                    @"arguments[0].style.border = '5px solid #FF0000';
                      arguments[0].style.boxShadow = '0 0 20px #FF0000';
                      arguments[0].style.backgroundColor = 'rgba(255, 0, 0, 0.15)';
                      arguments[0].style.transition = 'all 0.3s ease';
                      
                      // Agregar un marcador circular rojo permanente
                      var marker = document.createElement('div');
                      marker.className = 'selenium-marker';
                      marker.style.cssText = 'position: absolute; width: 40px; height: 40px; background-color: #FF0000; border-radius: 50%; z-index: 999999; pointer-events: none; border: 4px solid white; box-shadow: 0 0 15px rgba(0,0,0,0.7); opacity: 0.8;';
                      
                      var rect = arguments[0].getBoundingClientRect();
                      marker.style.left = (rect.left + window.scrollX + rect.width/2 - 20) + 'px';
                      marker.style.top = (rect.top + window.scrollY + rect.height/2 - 20) + 'px';
                      
                      document.body.appendChild(marker);",
                    element
                );

                TestContext.WriteLine($"🔴 Elemento trackeado: {GetElementDescription(element)}");
                System.Threading.Thread.Sleep(300);
            }
            catch (Exception ex)
            {
                TestContext.WriteLine($"⚠️ No se pudo trackear elemento: {ex.Message}");
            }
        }

        private void TakeEnhancedScreenshot(string testName)
        {
            try
            {
                if (Driver != null)
                {
                    TestContext.WriteLine($"🔍 DEBUG - UltimoElementoInteractuado es null: {UltimoElementoInteractuado == null}");

                    // PASO 1: Resaltar el último elemento SI existe
                    if (UltimoElementoInteractuado != null)
                    {
                        try
                        {
                            TestContext.WriteLine($"🔍 DEBUG - Intentando resaltar elemento...");

                            // Resaltar con borde MUY GRUESO para que sea visible en la captura
                            ((IJavaScriptExecutor)Driver).ExecuteScript(
                                @"try {
                                    arguments[0].style.border = '15px solid #FF0000';
                                    arguments[0].style.boxShadow = '0 0 50px #FF0000, inset 0 0 50px rgba(255, 0, 0, 0.5)';
                                    arguments[0].style.backgroundColor = 'rgba(255, 0, 0, 0.5)';
                                    arguments[0].style.outline = '10px dashed #FFFF00';
                                    arguments[0].style.outlineOffset = '5px';
                                    arguments[0].style.zIndex = '999998';
                                    
                                    // Scroll hacia el elemento para asegurarse de que esté visible
                                    arguments[0].scrollIntoView({block: 'center', behavior: 'instant'});
                                } catch(e) {
                                    console.error('Error al resaltar:', e);
                                }",
                                UltimoElementoInteractuado
                            );

                            TestContext.WriteLine($"🔴 Elemento resaltado para captura");
                        }
                        catch (Exception ex)
                        {
                            TestContext.WriteLine($"⚠️ No se pudo resaltar elemento: {ex.Message}");
                        }
                    }
                    else
                    {
                        TestContext.WriteLine("⚠️ No hay elemento para resaltar (UltimoElementoInteractuado es null)");
                    }

                    // PASO 2: Agregar overlay en ESQUINA INFERIOR IZQUIERDA
                    try
                    {
                        var elementoInfo = GetElementDescription(UltimoElementoInteractuado);

                        var infoScript = @"
                            // Eliminar overlay anterior si existe
                            var oldDiv = document.getElementById('selenium-debug-info');
                            if (oldDiv) oldDiv.remove();
                            
                            var infoDiv = document.createElement('div');
                            infoDiv.id = 'selenium-debug-info';
                            infoDiv.style.cssText = 'position: fixed; bottom: 20px; left: 20px; background-color: rgba(220, 53, 69, 0.95); color: white; padding: 20px; border-radius: 10px; z-index: 999999; font-size: 16px; font-family: Arial, sans-serif; box-shadow: 0 8px 16px rgba(0,0,0,0.4); max-width: 550px; line-height: 1.5;';
                            infoDiv.innerHTML = '<div style=""font-size: 22px; font-weight: bold; margin-bottom: 12px;"">✖ TEST FAILED</div>' +
                                                '<div style=""margin-bottom: 8px; word-wrap: break-word;""><strong>Test:</strong><br/>' + arguments[0] + '</div>' +
                                                '<div style=""margin-bottom: 8px;""><strong>Time:</strong> ' + arguments[1] + '</div>' +
                                                '<div style=""margin-bottom: 8px;""><strong>User:</strong> jefferson-joe-mendoza-vega</div>' +
                                                '<div style=""margin-top: 12px; padding-top: 12px; border-top: 2px solid white; font-size: 13px; word-wrap: break-word;"">' +
                                                '<strong>🔴 Último elemento clickeado:</strong><br/>' + arguments[2] + '</div>';
                            document.body.appendChild(infoDiv);
                        ";

                        ((IJavaScriptExecutor)Driver).ExecuteScript(
                            infoScript,
                            testName,
                            DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss") + " UTC",
                            elementoInfo.Replace("\"", "'").Replace("\n", " ")
                        );
                    }
                    catch (Exception ex)
                    {
                        TestContext.WriteLine($"⚠️ No se pudo agregar info overlay: {ex.Message}");
                    }

                    System.Threading.Thread.Sleep(800); // Esperar render

                    // PASO 3: Capturar screenshot
                    var screenshot = ((ITakesScreenshot)Driver).GetScreenshot();
                    var fileName = $"{testName}_{DateTime.Now:yyyyMMdd_HHmmss}.png";

                    var directory = @"C:\Users\jefferson\Downloads\caoturasfallas";

                    if (!Directory.Exists(directory))
                    {
                        Directory.CreateDirectory(directory);
                        TestContext.WriteLine($"📁 Carpeta creada: {directory}");
                    }

                    var path = Path.Combine(directory, fileName);
                    screenshot.SaveAsFile(path);

                    TestContext.WriteLine($"📸 Screenshot mejorado guardado: {path}");
                    TestContext.WriteLine($"🔴 Último elemento interactuado: {GetElementDescription(UltimoElementoInteractuado)}");

                    CapturarInformacionAdicional(testName, directory);
                }
            }
            catch (Exception ex)
            {
                TestContext.WriteLine($"⚠️ No se pudo capturar screenshot mejorado: {ex.Message}");
            }
        }

        private string GetElementDescription(IWebElement element)
        {
            if (element == null) return "No disponible - ningún elemento fue trackeado";

            try
            {
                var tagName = element.TagName;
                var id = element.GetAttribute("id");
                var className = element.GetAttribute("class");
                var text = element.Text;

                return $"Tag: {tagName}, ID: {id ?? "N/A"}, Class: {className ?? "N/A"}, Text: {(string.IsNullOrEmpty(text) ? "N/A" : text.Substring(0, Math.Min(50, text.Length)))}";
            }
            catch
            {
                return "No se pudo obtener descripción del elemento";
            }
        }

        private void CapturarInformacionAdicional(string testName, string directory)
        {
            try
            {
                var logFileName = $"{testName}_{DateTime.Now:yyyyMMdd_HHmmss}_LOG.txt";
                var logPath = Path.Combine(directory, logFileName);

                using (var writer = new StreamWriter(logPath))
                {
                    writer.WriteLine("═══════════════════════════════════════════════");
                    writer.WriteLine($"TEST FAILURE LOG - {testName}");
                    writer.WriteLine("═══════════════════════════════════════════════");
                    writer.WriteLine($"Usuario: jefferson-joe-mendoza-vega");
                    writer.WriteLine($"Fecha/Hora UTC: {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss}");
                    writer.WriteLine($"URL Actual: {Driver.Url}");
                    writer.WriteLine($"Título Página: {Driver.Title}");
                    writer.WriteLine("═══════════════════════════════════════════════");

                    if (UltimoElementoInteractuado != null)
                    {
                        writer.WriteLine("\n🔴 ÚLTIMO ELEMENTO INTERACTUADO:");
                        writer.WriteLine($"   {GetElementDescription(UltimoElementoInteractuado)}");
                    }
                    else
                    {
                        writer.WriteLine("\n⚠️ NO SE TRACKEÓ NINGÚN ELEMENTO");
                    }

                    writer.WriteLine("\n📄 PÁGINA HTML (primeros 5000 caracteres):");
                    var pageSource = Driver.PageSource;
                    writer.WriteLine(pageSource.Substring(0, Math.Min(5000, pageSource.Length)));

                    writer.WriteLine("\n═══════════════════════════════════════════════");
                }

                TestContext.WriteLine($"📄 Log adicional guardado: {logPath}");
            }
            catch (Exception ex)
            {
                TestContext.WriteLine($"⚠️ No se pudo guardar log adicional: {ex.Message}");
            }
        }

        private void TakeScreenshot(string testName)
        {
            TakeEnhancedScreenshot(testName);
        }
    }
}