using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebCrawler.CrossCutting;

namespace WebCrawler.Spider.Tests
{
    [TestClass]
    public class UrlHelperTests
    {
        [TestMethod]
        public void Helper_Consegue_Validar_Dominio_Vazio()
        {
            var url = string.Empty;

            var valid = url.HasValidDomainName();

            Assert.IsFalse(valid);
        }

        [TestMethod]
        public void Helper_Consegue_Validar_Dominio_Inexistente()
        {
            const string url = "teste";

            var valid = url.HasValidDomainName();

            Assert.IsFalse(valid);
        }

        [TestMethod]
        public void Helper_Consegue_Validar_Dominio_Incorreto()
        {
            const string url = "teste.jgbvhg";

            var valid = url.HasValidDomainName();

            Assert.IsFalse(valid);
        }

        [TestMethod]
        public void Helper_Consegue_Validar_Dominio_Correto()
        {
            const string url = "teste.co.uk";

            var valid = url.HasValidDomainName();

            Assert.IsTrue(valid);
        }

        [TestMethod]
        public void Helper_Consegue_Validar_URL()
        {
            var url = string.Empty;

            var valid = UrlHelper.CheckUrlValid(url);

            Assert.IsFalse(valid);
        }

        [TestMethod]
        public void Helper_Consegue_Extrair_Domain_Name()
        {
            const string url = "https://teste.subdominio.professorsemfronteiras.com/";

            var domain = url.GetDomainName();

            Assert.IsTrue(domain.Equals("professorsemfronteiras.com"));
        }

        [TestMethod]
        public void Helper_Consegue_Ignorar_Query_String()
        {
            const string url = "http://www.calculador.com.br/conta/login?returnUrl=%2Fconta%2Flogin%3FreturnUrl%3D%252Fconta%252Flogin%253FreturnUrl%253D%25252Fconta%25252Flogin%25253FreturnUrl%25253D%2525252Fconta%2525252Flogin%2525253FreturnUrl%2525253D%252525252Fconta%252525252Flogin%252525253FreturnUrl%252525253D%25252525252Fconta%25252525252Flogin%25252525253FreturnUrl%25252525253D%2525252525252Fconta%2525252525252Flogin%2525252525253FreturnUrl%2525252525253D%252525252525252Fconta%252525252525252Flogin%252525252525253FreturnUrl%252525252525253D%25252525252525252Fconta%25252525252525252Flogin%25252525252525253FreturnUrl%25252525252525253D%2525252525252525252Fconta%2525252525252525252Flogin%2525252525252525253FreturnUrl%2525252525252525253D%252525252525252525252Fconta%252525252525252525252Flogin%252525252525252525253FreturnUrl%252525252525252525253D%25252525252525252525252Fconta%25252525252525252525252Flogin%25252525252525252525253FreturnUrl%25252525252525252525253D%2525252525252525252525252Fconta%2525252525252525252525252Flogin%2525252525252525252525253FreturnUrl%2525252525252525252525253D%252525252525252525252525252Fconta%252525252525252525252525252Flogin%252525252525252525252525253FreturnUrl%252525252525252525252525253D%25252525252525252525252525252Fconta%25252525252525252525252525252Flogin%25252525252525252525252525253FreturnUrl%25252525252525252525252525253D%2525252525252525252525252525252Fconta%2525252525252525252525252525252Flogin%2525252525252525252525252525253FreturnUrl%2525252525252525252525252525253D%252525252525252525252525252525252Fconta%252525252525252525252525252525252Flogin%252525252525252525252525252525253FreturnUrl%252525252525252525252525252525253D%25252525252525252525252525252525252Fcalculo%25252525252525252525252525252525252Fhora-extra";

            var fixedUrl = UrlHelper.IgnoreQueryString(url);

            Assert.IsTrue(fixedUrl.Equals("http://www.calculador.com.br/conta/login"));
        }
    }
}
