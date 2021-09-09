﻿using System.IO;
using System.Xml;
using Unimake.Business.DFe.Utility;

namespace Unimake.Business.DFe.Servicos.NFSe
{
    /// <summary>
    /// Enviar o XML de Consulta/Downloa do PDF da NFSe para o webservice
    /// </summary>
    public class ConsultarNfsePDF: ConsultarNfse
    {
        /// <summary>
        /// Construtor
        /// </summary>
        public ConsultarNfsePDF() : base()
        { }

        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="conteudoXML">Conteúdo do XML que será enviado para o WebService</param>
        /// <param name="configuracao">Objeto "Configuracoes" com as propriedade necessária para a execução do serviço</param>
        public ConsultarNfsePDF(XmlDocument conteudoXML, Configuracao configuracao) : base(conteudoXML, configuracao)
        { }

        /// <summary>
        /// Extrair o PDF do retorno obtido em Base64
        /// </summary>
        /// <param name="pasta">Pasta onde deve ser gravado o PDF</param>
        /// <param name="nomePDF">Nome do arquivo PDF a ser gravado</param>
        /// <param name="nomeTagPDF">Nome da tag que tem o conteúdo do PDF em BASE64</param>
        public void ExtrairPDF(string pasta, string nomePDF, string nomeTagPDF)
        {
            if(RetornoWSXML.GetElementsByTagName(nomeTagPDF)[0] != null)
            {
                Converter.Base64ToPDF(RetornoWSXML.GetElementsByTagName("Base64Pdf")[0].InnerText, Path.Combine(pasta, nomePDF));
            }
            else
            {
                throw new System.Exception("Não foi possível localizar a TAG com o conteúdo do PDF no XML retornado pela prefeitura.");
            }
        }
    }
}