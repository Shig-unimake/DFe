﻿#pragma warning disable CS1591

#if INTEROP
using System.Runtime.InteropServices;
#endif
using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Unimake.Business.DFe.Servicos;

namespace Unimake.Business.DFe.Xml.NFe
{
#if INTEROP
    [ClassInterface(ClassInterfaceType.AutoDual)]
    [ProgId("Unimake.Business.DFe.Xml.NFe.RetDistDFeInt")]
    [ComVisible(true)]
#endif
    [XmlRoot("retDistDFeInt", Namespace = "http://www.portalfiscal.inf.br/nfe", IsNullable = false)]
    public class RetDistDFeInt : XMLBase
    {
        [XmlAttribute(AttributeName = "versao", DataType = "token")]
        public string Versao { get; set; }

        [XmlElement("tpAmb")]
        public TipoAmbiente TpAmb { get; set; }

        [XmlElement("verAplic")]
        public string VerAplic { get; set; }

        [XmlElement("cStat")]
        public int CStat { get; set; }

        [XmlElement("xMotivo")]
        public string XMotivo { get; set; }

        [XmlIgnore]
#if INTEROP
        public DateTime DhResp { get; set; }
#else
        public DateTimeOffset DhResp { get; set; }
#endif

        [XmlElement("dhResp")]
        public string DhRespField
        {
            get => DhResp.ToString("yyyy-MM-ddTHH:mm:sszzz");
#if INTEROP
            set => DhResp = DateTime.Parse(value);
#else
            set => DhResp = DateTimeOffset.Parse(value);
#endif
        }

        [XmlElement("ultNSU", DataType = "token")]
        public string UltNSU { get; set; }

        [XmlElement("maxNSU", DataType = "token")]
        public string MaxNSU { get; set; }

        [XmlElement("loteDistDFeInt")]
        public LoteDistDFeInt LoteDistDFeInt { get; set; }
    }

    /// <remarks/>
#if INTEROP
    [ClassInterface(ClassInterfaceType.AutoDual)]
    [ProgId("Unimake.Business.DFe.Xml.NFe.LoteDistDFeInt")]
    [ComVisible(true)]
#endif
    [XmlType(AnonymousType = true, Namespace = "http://www.portalfiscal.inf.br/nfe")]
    public class LoteDistDFeInt
    {
        [XmlElement("docZip")]
        public List<DocZip> DocZip { get; set; }

#if INTEROP

        /// <summary>
        /// Retorna o elemento da lista DocZip (Utilizado para linguagens diferentes do CSharp que não conseguem pegar o conteúdo da lista)
        /// </summary>
        /// <param name="index">Índice da lista a ser retornado (Começa com 0 (zero))</param>
        /// <returns>Conteúdo do index passado por parâmetro da DocZip</returns>
        public DocZip GetDocZip(int index)
        {
            if ((DocZip?.Count ?? 0) == 0)
            {
                return default;
            };

            return DocZip[index];
        }

        /// <summary>
        /// Retorna a quantidade de elementos existentes na lista DocZip
        /// </summary>
        public int GetDocZipCount => (DocZip != null ? DocZip.Count : 0);

#endif
    }

#if INTEROP
    [ClassInterface(ClassInterfaceType.AutoDual)]
    [ProgId("Unimake.Business.DFe.Xml.NFe.DocZip")]
    [ComVisible(true)]
#endif
    [XmlType(AnonymousType = true, Namespace = "http://www.portalfiscal.inf.br/nfe")]
    public class DocZip
    {
        [XmlAttribute("NSU", DataType = "token")]
        public string NSU { get; set; }

        [XmlAttribute("schema")]
        public string Schema { get; set; }

        [XmlText(DataType = "base64Binary")]
        public byte[] Value { get; set; }
    }
}
