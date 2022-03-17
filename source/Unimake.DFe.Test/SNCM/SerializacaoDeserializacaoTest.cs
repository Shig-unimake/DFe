﻿using System.Diagnostics;
using System.IO;
using System.Xml;
using Unimake.Business.DFe.Xml.SNCM;
using Xunit;

namespace Unimake.DFe.Test.SNCM
{
    /// <summary>
    /// Testar a serialização e deserialização dos XMLs do SNCM
    /// </summary>
    public class SerializacaoDeserializacaoTest
    {
        /// <summary>
        /// Testar a serialização e deserialização do XML MsgParam
        /// </summary>
        [Theory]
        [Trait("DFe", "SNCM")]
        [InlineData(@"..\..\..\SNCM\Resources\msgParam-consparam.xml")]
        public void SerializacaoDeserializacaoMsgParam(string arqXML)
        {
            Debug.Assert(File.Exists(arqXML), "Arquivo " + arqXML + " não foi localizado para a realização da serialização/deserialização.");

            var doc = new XmlDocument();
            doc.Load(arqXML);

            var xml = new MsgParam();
            xml = xml.LoadFromFile(arqXML);
            XmlDocument xmlSerializado = xml.GerarXML();

            Debug.Assert(doc.InnerText == xmlSerializado.InnerText, "XML gerado pela DLL está diferente do conteúdo do arquivo serializado.");
        }

        /// <summary>
        /// Testar a serialização e deserialização do XML MsgParam
        /// </summary>
        [Theory]
        [Trait("DFe", "SNCM")]
        [InlineData(@"..\..\..\SNCM\Resources\retMbtAgtMgmt.xml")]
        public void SerializacaoDeserializacaoRetMbtAgtMgmt(string arqXML)
        {
            Debug.Assert(File.Exists(arqXML), "Arquivo " + arqXML + " não foi localizado para a realização da serialização/deserialização.");

            var doc = new XmlDocument();
            doc.Load(arqXML);

            var xml = new RetMbtAgtMgmt();
            xml = xml.LoadFromFile(arqXML);
            XmlDocument xmlSerializado = xml.GerarXML();

            Debug.Assert(doc.InnerText == xmlSerializado.InnerText, "XML gerado pela DLL está diferente do conteúdo do arquivo serializado.");
        }

        /// <summary>
        /// Testar a serialização e deserialização do XML MsgParam
        /// </summary>
        [Theory]
        [Trait("DFe", "SNCM")]
        [InlineData(@"..\..\..\SNCM\Resources\retEvtIn.xml")]
        public void SerializacaoDeserializacaoRetEvtIn(string arqXML)
        {
            Debug.Assert(File.Exists(arqXML), "Arquivo " + arqXML + " não foi localizado para a realização da serialização/deserialização.");

            var doc = new XmlDocument();
            doc.Load(arqXML);

            var xml = new RetEvtIn();
            xml = xml.LoadFromFile(arqXML);
            XmlDocument xmlSerializado = xml.GerarXML();

            Debug.Assert(doc.InnerText == xmlSerializado.InnerText, "XML gerado pela DLL está diferente do conteúdo do arquivo serializado.");
        }
    }
}