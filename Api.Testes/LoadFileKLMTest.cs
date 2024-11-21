using Api.Web.Controllers;

namespace Api.Testes

{
    public class LoadFileKLMTest
    {
        private readonly string kmlFilePath = @"C:\Users\ishma\Desktop\ProjetoAPI\TesteAPI\DIRECIONADORES1\DIRECIONADORES1.kml";

        [Fact]
        public void CreateNewKLMFile()
        {
            var controller = new KLMController();
            var document = controller.LoadKmlFile();

            // Valida que o documento não é nulo
            Assert.NotNull(document);

        }
        
    }
}