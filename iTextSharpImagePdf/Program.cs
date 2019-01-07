using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.IO;

namespace iTextSharpImagePdf
{
    class Program
    {
        static void Main(string[] args)
        {
            GerarDocumentoPdf();
            Console.WriteLine("Fim Geraçao PDF...");
            Console.ReadKey();
        }

        /// <summary>
        /// Geração de documentos PDF (iTextSharp) embutidando imagens de um determinado Diretorio
        /// </summary>
        private static void GerarDocumentoPdf()
        {
            var diretorioRaiz = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
            var pastaImagem = $"{diretorioRaiz}\\Imagens";

            DirectoryInfo imageDiretorio = new DirectoryInfo(pastaImagem);

            foreach (var imagem in imageDiretorio.GetFiles("*.jpg"))
            {
                var nomeImagem = Helper.NomeSemExtensao(imagem.Name);
                var caminhopdf = $"{diretorioRaiz}\\{nomeImagem}.pdf";

                using (var ms = new MemoryStream())
                {
                    var document = new Document(PageSize.A4, 50, 30, 30, 30);

                    PdfWriter.GetInstance(document, ms).SetFullCompression();
                    document.Open();

                    var image = Image.GetInstance(imagem.FullName);
                    RedimImagem(document, image);

                    document.Add(image);
                    document.Close();

                    File.WriteAllBytes(caminhopdf, ms.ToArray());
                }
            }

        }

        /// <summary>
        /// Redimensionamento de imagens para serem inseridas no documento PDF
        /// respeitando a area util do documento
        /// </summary>
        /// <param name="document"></param>
        /// <param name="image"></param>
        private static void RedimImagem(Document document, Image image)
        {
            var margins = (document.TopMargin + document.LeftMargin + document.BottomMargin + document.RightMargin);
            var areaUtilDoc = document.PageSize.Width - margins;

            //somente se image for maior que documento
            if (image.Width > areaUtilDoc)
            {
                float scaler = (areaUtilDoc / image.Width) * 100;
                image.ScalePercent(scaler);
            }
        }
    }

    public static class Helper
    {
        /// <summary>
        /// Retorna  nome do arquivo sem a extensão
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string NomeSemExtensao(String path)
        {
            return Path.Combine(Path.GetDirectoryName(path), Path.GetFileNameWithoutExtension(path));
        }

    }


}
