﻿using System;
using System.Collections.Generic;
using Diag = System.Diagnostics;
using System.IO;
using System.Xml;
using Unimake.Business.DFe.Servicos;
using Unimake.Business.DFe.Servicos.NFSe;
using Xunit;

namespace Unimake.DFe.Test.NFSe
{
    /// <summary>
    /// Testar o serviço: ConsultarNotaFiscal
    /// </summary>
    public class ConsultarNotaFiscalTest
    {
        /// <summary>
        /// Monta o parâmetros, de forma dinâmica, para o cenário de testes
        /// </summary>
        public static IEnumerable<object[]> Parametros => TestUtility.PreparaDadosCenario("ConsultarNotaFiscal");

        /// <summary>
        /// Consultar Situação para saber se a conexão com o webservice está ocorrendo corretamente.
        /// </summary>
        /// <param name="tipoAmbiente">Ambiente para onde deve ser enviado o XML</param>
        [Theory]
        [Trait("DFe", "NFSe")]
        [MemberData(nameof(Parametros))]
        public void ConsultarNotaFiscal(TipoAmbiente tipoAmbiente, PadraoNFSe padraoNFSe, string versaoSchema, int codMunicipio, string nomeMunicipio)
        {          
            var nomeXMLEnvio = "consultarNotaFiscal-ped-sitnfserps.xml";
            
            string arqXML;

            switch (padraoNFSe)
            {
                case PadraoNFSe.NOBESISTEMAS:
                    arqXML = "..\\..\\..\\NFSe\\Resources\\" + padraoNFSe.ToString() + "\\" + versaoSchema + "\\" + tipoAmbiente.ToString() + "\\" + nomeXMLEnvio;
                    break;

                default:
                    arqXML = "..\\..\\..\\NFSe\\Resources\\" + padraoNFSe.ToString() + "\\" + versaoSchema + "\\" + nomeXMLEnvio;
                    break;
            }

            Diag.Debug.Assert(File.Exists(arqXML), "Arquivo " + arqXML + " não foi localizado.");

            try
            {
                var conteudoXML = new XmlDocument();
                conteudoXML.Load(arqXML);

                var configuracao = new Configuracao
                {
                    TipoDFe = TipoDFe.NFSe,
                    CertificadoDigital = PropConfig.CertificadoDigital,
                    TipoAmbiente = tipoAmbiente,
                    CodigoMunicipio = codMunicipio,
                    Servico = Servico.NFSeConsultaNotaFiscal,
                    SchemaVersao = versaoSchema
                };

                var consultaNotaFiscal = new ConsultarNotaFiscal(conteudoXML, configuracao);
                consultaNotaFiscal.Executar();
            }
            catch(Exception ex)
            {
                Diag.Debug.Assert(false, "Falha na hora de consumir o serviço: " + nomeMunicipio + " - IBGE: " + codMunicipio + " - Padrão: " + padraoNFSe.ToString() + " - Versão schema: " + versaoSchema + "\r\nExceção: " + ex.Message, ex.StackTrace);
            }
        }
    }
}