﻿using System;
using System.Xml.Linq;
using Unimake.Business.DFe.Servicos;
using Unimake.Business.DFe.Utility;
using Unimake.Business.DFe.Validator.Abstractions;
using Unimake.Business.DFe.Xml.NFe;
using Unimake.Exceptions;

namespace Unimake.Business.DFe.Validator.NFe
{
    /// <summary>
    /// <inheritdoc cref="XmlValidatorBase"/>
    /// </summary>
    public class NFeValidator : XmlValidatorBase
    {
        #region Public Constructors

        /// <summary>
        /// <inheritdoc cref="XmlValidatorBase"/>
        /// </summary>
        public NFeValidator() =>
            ValidateTag(element => element.NameEquals(nameof(EnderDest.CMun)) && element.Parent.NameEquals(nameof(EnderDest)), Tag =>
            {
                if (UConvert.ToInt(Tag.Value) <= 0 || Tag.Value == null)
                {
                    throw new ValidatorDFeException("Código do município do destinatário está sem conteúdo. É obrigatório informar o código IBGE do município." +
                        " [TAG: <cMun> do grupo de tag <dest><enderDest>]");
                }

                if (Tag.Value.Length != 7)
                {
                    throw new ValidatorDFeException("O código do município do destinatário está incorreto. Código informado deve ter 7 dígitos. Valor informado: " + Tag.Value +
                        " [TAG: <cMun> do grupo de tag <dest><enderDest>]");
                }

            }).ValidateTag(element => element.NameEquals("CEAN"), Tag =>
            {
                var value = Tag.Value;
                var cProd = Tag.Parent.GetValue("cProd");
                var xProd = Tag.Parent.GetValue("xProd");
                var nItem = Tag.Parent.Parent.Parent.GetAttributeValue("nItem");

                if (value.ToUpper().Trim() != "SEM GTIN")
                {
                    if (!string.IsNullOrWhiteSpace(value) && value.Length != 0 && value.Length != 8 && value.Length != 12 && value.Length != 13 && value.Length != 14)
                    {
                        throw new ValidatorDFeException("Código EAN (código de barra) \"" + value + "\" informado no produto está incorreto. EAN deve ter 0, 8, 12, 13 ou 14 de tamanho e somente números, ou seja, não pode conter letras." +
                            " [Item: " + nItem + "] [cProd: " + cProd + "] [xProd: " + xProd + "] [TAG: <cEAN> do grupo de tag <det><prod>]");
                    }

                    for (var i = 0; i < value.Length; i++)
                    {
                        if (!"0123456789".Contains(value.Substring(i, 1)))
                        {
                            throw new ValidatorDFeException("Código EAN (código de barra) \"" + value + "\" informado no produto está incorreto. Não pode conter letras, somente números." +
                                " [Item: " + nItem + "] [cProd: " + cProd + "] [xProd: " + xProd + "] [TAG: <cEAN> do grupo de tag <det><prod>]");
                        }
                    }
                }
                else
                {
                    if (value != "SEM GTIN")
                    {
                        throw new ValidatorDFeException("Código EAN (código de barra) \"" + value + "\" informado no produto tem que ser igual a \"SEM GTIN\", ou seja, tudo maiúsculo e sem espaços no final ou inicio da sentença." +
                            " [Item: " + nItem + "] [cProd: " + cProd + "] [xProd: " + xProd + "] [TAG: <cEAN> do grupo de tag <det><prod>]");
                    }
                }
            }).ValidateTag(element => element.NameEquals("CEANTrib"), Tag =>
            {
                var value = Tag.Value;
                var cProd = Tag.Parent.GetValue("cProd");
                var xProd = Tag.Parent.GetValue("xProd");
                var nItem = Tag.Parent.Parent.Parent.GetAttributeValue("nItem");

                if (value.ToUpper().Trim() != "SEM GTIN")
                {
                    if (!string.IsNullOrWhiteSpace(value) && value.Length != 0 && value.Length != 8 && value.Length != 12 && value.Length != 13 && value.Length != 14)
                    {
                        throw new ValidatorDFeException("Código EAN Tributável (código de barra) \"" + value + "\" informado no produto está incorreto. EAN deve ter 0, 8, 12, 13 ou 14 de tamanho e somente números, ou seja, não pode conter letras." +
                            " [Item: " + nItem + "] [cProd: " + cProd + "] [xProd: " + xProd + "] [TAG: <cEANTrib> do grupo de tag <det><prod>]");
                    }

                    for (var i = 0; i < value.Length; i++)
                    {
                        if (!"0123456789".Contains(value.Substring(i, 1)))
                        {
                            throw new ValidatorDFeException("Código EAN Tributável (código de barra) \"" + value + "\" informado no produto está incorreto. Não pode conter letras, somente números." +
                                " [Item: " + nItem + "] [cProd: " + cProd + "] [xProd: " + xProd + "] [TAG: <cEANTrib> do grupo de tag <det><prod>]");
                        }
                    }
                }
                else
                {
                    if (value != "SEM GTIN")
                    {
                        throw new ValidatorDFeException("Código EAN Tributável (código de barra) \"" + value + "\" informado no produto tem que ser igual a \"SEM GTIN\", ou seja, tudo maiúsculo e sem espaços no final ou inicio da sentença." +
                            " [Item: " + nItem + "] [cProd: " + cProd + "] [xProd: " + xProd + "] [TAG: <cEANTrib> do grupo de tag <det><prod>]");
                    }
                }
            }).ValidateTag(element => element.NameEquals(nameof(RefNF.Mod)) && element.Parent.NameEquals(nameof(RefNF)), Tag =>
            {
                var value = Tag.Value;

                if (value != "01" && value != "02")
                {
                    throw new ValidatorDFeException("O modelo do documento fiscal informado nos documentos referenciados está incorreto. Valor informado foi " + value + ", mas, só é permitido informar 01 ou 02. [TAG: <mod> do grupo de tag <NFref><refNF>]");
                }
            }).ValidateTag(element => element.NameEquals(nameof(ISSQN.CListServ)) && element.Parent.NameEquals(nameof(ISSQN)), Tag =>
            {
                var value = Tag.Value;
                var cProd = Tag.Parent.Parent.Parent.GetValue("cProd");
                var xProd = Tag.Parent.Parent.Parent.GetValue("xProd");
                var nItem = Tag.Parent.Parent.Parent.GetAttributeValue("nItem");

                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ValidatorDFeException("Código do item da lista de serviços não foi informado. Em caso de serviços é obrigatório informar este conteúdo." +
                        " [Item: " + nItem + "] [cProd: " + cProd + "] [xProd: " + xProd + "] [TAG: <cListServ> do grupo de tag <det><imposto><ISSQN>]");
                }
                else if (value.Length != 5 || value.Substring(2, 1) != ".")
                {
                    throw new ValidatorDFeException("Código do item da lista de serviços informado está com o formato incorreto. O formato deve ser 99.99, ou seja, 2 dígitos seguidos de um ponto e mais 2 dígitos. Valor informado: " + value + "." +
                        " [Item: " + nItem + "] [cProd: " + cProd + "] [xProd: " + xProd + "] [TAG: <cListServ> do grupo de tag <det><imposto><ISSQN>]");
                }
                else
                {
                    try
                    {
                        var listaServicoISSQN = (ListaServicoISSQN)Enum.Parse(typeof(ListaServicoISSQN), "Servico" + value.Replace(".", ""));
                    }
                    catch
                    {
                        throw new ValidatorDFeException("Código do item da lista de serviços informado não existe na tabela da ABRASF. Valor informado: " + value + "." +
                            " [Item: " + nItem + "] [cProd: " + cProd + "] [xProd: " + xProd + "] [TAG: <cListServ> do grupo de tag <det><imposto><ISSQN>]");
                    }
                }
            }).ValidateTag(element => element.NameEquals(nameof(ISSQN.IndISS)) && element.Parent.NameEquals(nameof(ISSQN)), Tag =>
            {
                var value = Tag.Value;
                var cProd = Tag.Parent.Parent.Parent.GetValue("cProd");
                var xProd = Tag.Parent.Parent.Parent.GetValue("xProd");
                var nItem = Tag.Parent.Parent.Parent.GetAttributeValue("nItem");

                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ValidatorDFeException("É obrigatório informar o indicador da exigibilidade do ISS para itens de prestação de serviços e o mesmo não foi informado." +
                        " [Item: " + nItem + "] [cProd: " + cProd + "] [xProd: " + xProd + "] [TAG: <indISS> do grupo de tag <det><imposto><ISSQN>]");
                }
                else
                {
                    try
                    {
                        var indISS = (IndicadorExigibilidadeISSQN)Enum.Parse(typeof(IndicadorExigibilidadeISSQN), Enum.GetName(typeof(IndicadorExigibilidadeISSQN), Convert.ToInt32(value)));
                    }
                    catch
                    {
                        throw new ValidatorDFeException("O indicador de exigibilidade do ISS informado no item de prestação de serviço está incorreto. Valor informado: " + value + " - Valores aceitos: 1, 2, 3, 4, 5, 6 e 7." +
                            " [Item: " + nItem + "] [cProd: " + cProd + "] [xProd: " + xProd + "] [TAG: <indISS> do grupo de tag <det><imposto><ISSQN>]");
                    }
                }
            }).ValidateTag(element => element.NameEquals(nameof(ICMSUFDest.PICMSInter)) && element.Parent.NameEquals(nameof(ICMSUFDest)), Tag =>
            {
                var value = Converter.ToDouble(Tag.Value);
                var cProd = Tag.Parent.Parent.Parent.GetValue("cProd");
                var xProd = Tag.Parent.Parent.Parent.GetValue("xProd");
                var nItem = Tag.Parent.Parent.Parent.GetAttributeValue("nItem");

                if (value != 4 && value != 7 && value != 12)
                {
                    throw new ValidatorDFeException("A alíquota de ICMS interestadual das UF envolvidas informada está incorreta. Valor informado: " + Tag.Value + " - Valores aceitos: 4.00, 7.00 ou 12.00." +
                        " [Item: " + nItem + "] [cProd: " + cProd + "] [xProd: " + xProd + "] [TAG: <pICMSInter> do grupo de tag <det><imposto><ICMSUFDest>]");
                }
            }).ValidateTag(element => element.NameEquals(nameof(ICMS00.CST)) && element.Parent.NameEquals(nameof(ICMS00)), Tag =>
            {
                var value = Tag.Value;
                var cProd = Tag.Parent.Parent.Parent.Parent.GetValue("cProd");
                var xProd = Tag.Parent.Parent.Parent.Parent.GetValue("xProd");
                var nItem = Tag.Parent.Parent.Parent.Parent.GetAttributeValue("nItem");

                if (value != "00")
                {
                    throw new ValidatorDFeException("O CST do grupo de tributação de ICMS00 está incorreto. Valor informado: " + Tag.Value + " - Valor aceito: 00." +
                        " [Item: " + nItem + "] [cProd: " + cProd + "] [xProd: " + xProd + "] [TAG: <CST> do grupo de tag <det><imposto><ICMS><ICMS00>]");
                }
            }).ValidateTag(element => element.NameEquals(nameof(ICMS10.CST)) && element.Parent.NameEquals(nameof(ICMS10)), Tag =>
            {
                var value = Tag.Value;
                var cProd = Tag.Parent.Parent.Parent.Parent.GetValue("cProd");
                var xProd = Tag.Parent.Parent.Parent.Parent.GetValue("xProd");
                var nItem = Tag.Parent.Parent.Parent.Parent.GetAttributeValue("nItem");

                if (value != "10")
                {
                    throw new ValidatorDFeException("O CST do grupo de tributação de ICMS10 está incorreto. Valor informado: " + Tag.Value + " - Valor aceito: 10." +
                        " [Item: " + nItem + "] [cProd: " + cProd + "] [xProd: " + xProd + "] [TAG: <CST> do grupo de tag <det><imposto><ICMS><ICMS10>]");
                }
            }).ValidateTag(element => element.NameEquals(nameof(ICMS20.CST)) && element.Parent.NameEquals(nameof(ICMS20)), Tag =>
            {
                var value = Tag.Value;
                var cProd = Tag.Parent.Parent.Parent.Parent.GetValue("cProd");
                var xProd = Tag.Parent.Parent.Parent.Parent.GetValue("xProd");
                var nItem = Tag.Parent.Parent.Parent.Parent.GetAttributeValue("nItem");

                if (value != "20")
                {
                    throw new ValidatorDFeException("O CST do grupo de tributação de ICMS20 está incorreto. Valor informado: " + Tag.Value + " - Valor aceito: 20." +
                        " [Item: " + nItem + "] [cProd: " + cProd + "] [xProd: " + xProd + "] [TAG: <CST> do grupo de tag <det><imposto><ICMS><ICMS20>]");
                }
            }).ValidateTag(element => element.NameEquals(nameof(ICMS30.CST)) && element.Parent.NameEquals(nameof(ICMS30)), Tag =>
            {
                var value = Tag.Value;
                var cProd = Tag.Parent.Parent.Parent.Parent.GetValue("cProd");
                var xProd = Tag.Parent.Parent.Parent.Parent.GetValue("xProd");
                var nItem = Tag.Parent.Parent.Parent.Parent.GetAttributeValue("nItem");

                if (value != "30")
                {
                    throw new ValidatorDFeException("O CST do grupo de tributação de ICMS30 está incorreto. Valor informado: " + Tag.Value + " - Valor aceito: 30." +
                        " [Item: " + nItem + "] [cProd: " + cProd + "] [xProd: " + xProd + "] [TAG: <CST> do grupo de tag <det><imposto><ICMS><ICMS30>]");
                }
            }).ValidateTag(element => element.NameEquals(nameof(ICMS40.CST)) && element.Parent.NameEquals(nameof(ICMS40)), Tag =>
            {
                var value = Tag.Value;
                var cProd = Tag.Parent.Parent.Parent.Parent.GetValue("cProd");
                var xProd = Tag.Parent.Parent.Parent.Parent.GetValue("xProd");
                var nItem = Tag.Parent.Parent.Parent.Parent.GetAttributeValue("nItem");

                if (value != "40" && value != "41" && value != "50")
                {
                    throw new ValidatorDFeException("O CST do grupo de tributação de ICMS40 está incorreto. Valor informado: " + Tag.Value + " - Valores aceitos: 40, 41 ou 50." +
                        " [Item: " + nItem + "] [cProd: " + cProd + "] [xProd: " + xProd + "] [TAG: <CST> do grupo de tag <det><imposto><ICMS><ICMS40>]");
                }
            }).ValidateTag(element => element.NameEquals(nameof(ICMS51.CST)) && element.Parent.NameEquals(nameof(ICMS51)), Tag =>
            {
                var value = Tag.Value;
                var cProd = Tag.Parent.Parent.Parent.Parent.GetValue("cProd");
                var xProd = Tag.Parent.Parent.Parent.Parent.GetValue("xProd");
                var nItem = Tag.Parent.Parent.Parent.Parent.GetAttributeValue("nItem");

                if (value != "51")
                {
                    throw new ValidatorDFeException("O CST do grupo de tributação de ICMS51 está incorreto. Valor informado: " + Tag.Value + " - Valor aceito: 51." +
                        " [Item: " + nItem + "] [cProd: " + cProd + "] [xProd: " + xProd + "] [TAG: <CST> do grupo de tag <det><imposto><ICMS><ICMS51>]");
                }
            }).ValidateTag(element => element.NameEquals(nameof(ICMS60.CST)) && element.Parent.NameEquals(nameof(ICMS60)), Tag =>
            {
                var value = Tag.Value;
                var cProd = Tag.Parent.Parent.Parent.Parent.GetValue("cProd");
                var xProd = Tag.Parent.Parent.Parent.Parent.GetValue("xProd");
                var nItem = Tag.Parent.Parent.Parent.Parent.GetAttributeValue("nItem");

                if (value != "60")
                {
                    throw new ValidatorDFeException("O CST do grupo de tributação de ICMS60 está incorreto. Valor informado: " + Tag.Value + " - Valor aceito: 60." +
                        " [Item: " + nItem + "] [cProd: " + cProd + "] [xProd: " + xProd + "] [TAG: <CST> do grupo de tag <det><imposto><ICMS><ICMS60>]");
                }
            }).ValidateTag(element => element.NameEquals(nameof(ICMS70.CST)) && element.Parent.NameEquals(nameof(ICMS70)), Tag =>
            {
                var value = Tag.Value;
                var cProd = Tag.Parent.Parent.Parent.Parent.GetValue("cProd");
                var xProd = Tag.Parent.Parent.Parent.Parent.GetValue("xProd");
                var nItem = Tag.Parent.Parent.Parent.Parent.GetAttributeValue("nItem");

                if (value != "70")
                {
                    throw new ValidatorDFeException("O CST do grupo de tributação de ICMS70 está incorreto. Valor informado: " + Tag.Value + " - Valor aceito: 70." +
                        " [Item: " + nItem + "] [cProd: " + cProd + "] [xProd: " + xProd + "] [TAG: <CST> do grupo de tag <det><imposto><ICMS><ICMS70>]");
                }
            }).ValidateTag(element => element.NameEquals(nameof(ICMS90.CST)) && element.Parent.NameEquals(nameof(ICMS90)), Tag =>
            {
                var value = Tag.Value;
                var cProd = Tag.Parent.Parent.Parent.Parent.GetValue("cProd");
                var xProd = Tag.Parent.Parent.Parent.Parent.GetValue("xProd");
                var nItem = Tag.Parent.Parent.Parent.Parent.GetAttributeValue("nItem");

                if (value != "90")
                {
                    throw new ValidatorDFeException("O CST do grupo de tributação de ICMS90 está incorreto. Valor informado: " + Tag.Value + " - Valor aceito: 90." +
                        " [Item: " + nItem + "] [cProd: " + cProd + "] [xProd: " + xProd + "] [TAG: <CST> do grupo de tag <det><imposto><ICMS><ICMS90>]");
                }
            }).ValidateTag(element => element.NameEquals(nameof(ICMSPart.CST)) && element.Parent.NameEquals(nameof(ICMSPart)), Tag =>
            {
                var value = Tag.Value;
                var cProd = Tag.Parent.Parent.Parent.Parent.GetValue("cProd");
                var xProd = Tag.Parent.Parent.Parent.Parent.GetValue("xProd");
                var nItem = Tag.Parent.Parent.Parent.Parent.GetAttributeValue("nItem");

                if (value != "10" && value != "90")
                {
                    throw new ValidatorDFeException("O CST do grupo de Partilha de ICMS entre UF de origem e UF de destino ou a UF definida na legislação está incorreto. Valor informado: " + Tag.Value + " - Valores aceitos: 10 ou 90." +
                        " [Item: " + nItem + "] [cProd: " + cProd + "] [xProd: " + xProd + "] [TAG: <CST> do grupo de tag <det><imposto><ICMS><ICMSPart>]");
                }
            }).ValidateTag(element => element.NameEquals(nameof(ICMSST.CST)) && element.Parent.NameEquals(nameof(ICMSST)), Tag =>
            {
                var value = Tag.Value;
                var cProd = Tag.Parent.Parent.Parent.Parent.GetValue("cProd");
                var xProd = Tag.Parent.Parent.Parent.Parent.GetValue("xProd");
                var nItem = Tag.Parent.Parent.Parent.Parent.GetAttributeValue("nItem");

                if (value != "41" && value != "60")
                {
                    throw new ValidatorDFeException("O CST do grupo de Repasse de ICMS ST retido anteriormente em operações interestaduais com repasses através do Substituto Tributário está incorreto. Valor informado: " + Tag.Value + " - Valores aceitos: 41 ou 60." +
                        " [Item: " + nItem + "] [cProd: " + cProd + "] [xProd: " + xProd + "] [TAG: <CST> do grupo de tag <det><imposto><ICMS><ICMSST>]");
                }
            }).ValidateTag(element => element.NameEquals(nameof(ICMSSN101.CSOSN)) && element.Parent.NameEquals(nameof(ICMSSN101)), Tag =>
            {
                var value = Tag.Value;
                var cProd = Tag.Parent.Parent.Parent.Parent.GetValue("cProd");
                var xProd = Tag.Parent.Parent.Parent.Parent.GetValue("xProd");
                var nItem = Tag.Parent.Parent.Parent.Parent.GetAttributeValue("nItem");

                if (value != "101")
                {
                    throw new ValidatorDFeException("O CSOSN do grupo de tributação do ICMS pelo Simples Nacional está incorreto. Valor informado: " + Tag.Value + " - Valor aceito: 101." +
                        " [Item: " + nItem + "] [cProd: " + cProd + "] [xProd: " + xProd + "] [TAG: <CST> do grupo de tag <det><imposto><ICMS><ICMSSN101>]");
                }
            }).ValidateTag(element => element.NameEquals(nameof(ICMSSN102.CSOSN)) && element.Parent.NameEquals(nameof(ICMSSN102)), Tag =>
            {
                var value = Tag.Value;
                var cProd = Tag.Parent.Parent.Parent.Parent.GetValue("cProd");
                var xProd = Tag.Parent.Parent.Parent.Parent.GetValue("xProd");
                var nItem = Tag.Parent.Parent.Parent.Parent.GetAttributeValue("nItem");

                if (value != "102" && value != "103" && value != "300" && value != "400")
                {
                    throw new ValidatorDFeException("O CSOSN do grupo de tributação do ICMS pelo Simples Nacional está incorreto. Valor informado: " + Tag.Value + " - Valores aceitos: 102, 103, 300 ou 400." +
                        " [Item: " + nItem + "] [cProd: " + cProd + "] [xProd: " + xProd + "] [TAG: <CST> do grupo de tag <det><imposto><ICMS><ICMSSN102>]");
                }
            }).ValidateTag(element => element.NameEquals(nameof(ICMSSN201.CSOSN)) && element.Parent.NameEquals(nameof(ICMSSN201)), Tag =>
            {
                var value = Tag.Value;
                var cProd = Tag.Parent.Parent.Parent.Parent.GetValue("cProd");
                var xProd = Tag.Parent.Parent.Parent.Parent.GetValue("xProd");
                var nItem = Tag.Parent.Parent.Parent.Parent.GetAttributeValue("nItem");

                if (value != "201")
                {
                    throw new ValidatorDFeException("O CSOSN do grupo de tributação do ICMS pelo Simples Nacional está incorreto. Valor informado: " + Tag.Value + " - Valor aceito: 201." +
                        " [Item: " + nItem + "] [cProd: " + cProd + "] [xProd: " + xProd + "] [TAG: <CST> do grupo de tag <det><imposto><ICMS><ICMSSN201>]");
                }
            }).ValidateTag(element => element.NameEquals(nameof(ICMSSN202.CSOSN)) && element.Parent.NameEquals(nameof(ICMSSN202)), Tag =>
            {
                var value = Tag.Value;
                var cProd = Tag.Parent.Parent.Parent.Parent.GetValue("cProd");
                var xProd = Tag.Parent.Parent.Parent.Parent.GetValue("xProd");
                var nItem = Tag.Parent.Parent.Parent.Parent.GetAttributeValue("nItem");

                if (value != "202" && value != "203")
                {
                    throw new ValidatorDFeException("O CSOSN do grupo de tributação do ICMS pelo Simples Nacional está incorreto. Valor informado: " + Tag.Value + " - Valores aceitos: 202 ou 203." +
                        " [Item: " + nItem + "] [cProd: " + cProd + "] [xProd: " + xProd + "] [TAG: <CST> do grupo de tag <det><imposto><ICMS><ICMSSN202>]");
                }
            }).ValidateTag(element => element.NameEquals(nameof(ICMSSN500.CSOSN)) && element.Parent.NameEquals(nameof(ICMSSN500)), Tag =>
            {
                var value = Tag.Value;
                var cProd = Tag.Parent.Parent.Parent.Parent.GetValue("cProd");
                var xProd = Tag.Parent.Parent.Parent.Parent.GetValue("xProd");
                var nItem = Tag.Parent.Parent.Parent.Parent.GetAttributeValue("nItem");

                if (value != "500")
                {
                    throw new ValidatorDFeException("O CSOSN do grupo de tributação do ICMS pelo Simples Nacional está incorreto. Valor informado: " + Tag.Value + " - Valor aceito: 500." +
                        " [Item: " + nItem + "] [cProd: " + cProd + "] [xProd: " + xProd + "] [TAG: <CST> do grupo de tag <det><imposto><ICMS><ICMSSN500>]");
                }
            }).ValidateTag(element => element.NameEquals(nameof(ICMSSN900.CSOSN)) && element.Parent.NameEquals(nameof(ICMSSN900)), Tag =>
            {
                var value = Tag.Value;
                var cProd = Tag.Parent.Parent.Parent.Parent.GetValue("cProd");
                var xProd = Tag.Parent.Parent.Parent.Parent.GetValue("xProd");
                var nItem = Tag.Parent.Parent.Parent.Parent.GetAttributeValue("nItem");

                if (value != "900")
                {
                    throw new ValidatorDFeException("O CSOSN do grupo de tributação do ICMS pelo Simples Nacional está incorreto. Valor informado: " + Tag.Value + " - Valor aceito: 900." +
                        " [Item: " + nItem + "] [cProd: " + cProd + "] [xProd: " + xProd + "] [TAG: <CST> do grupo de tag <det><imposto><ICMS><ICMSSN900>]");
                }
            }).ValidateTag(element => element.NameEquals(nameof(IPITrib.CST)) && element.Parent.NameEquals(nameof(IPITrib)), Tag =>
            {
                var value = Tag.Value;
                var cProd = Tag.Parent.Parent.Parent.Parent.GetValue("cProd");
                var xProd = Tag.Parent.Parent.Parent.Parent.GetValue("xProd");
                var nItem = Tag.Parent.Parent.Parent.Parent.GetAttributeValue("nItem");

                if (value != "00" && value != "49" && value != "50" && value != "99")
                {
                    throw new ValidatorDFeException("O CST do grupo de tributação do IPI tributado está incorreto. Valor informado: " + Tag.Value + " - Valores aceitos: 00, 49, 50 ou 99." +
                        " [Item: " + nItem + "] [cProd: " + cProd + "] [xProd: " + xProd + "] [TAG: <CST> do grupo de tag <det><imposto><IPI><IPITrib>]");
                }
            }).ValidateTag(element => element.NameEquals(nameof(IPINT.CST)) && element.Parent.NameEquals(nameof(IPINT)), Tag =>
            {
                var value = Tag.Value;
                var cProd = Tag.Parent.Parent.Parent.Parent.GetValue("cProd");
                var xProd = Tag.Parent.Parent.Parent.Parent.GetValue("xProd");
                var nItem = Tag.Parent.Parent.Parent.Parent.GetAttributeValue("nItem");

                if (value != "01" && value != "02" && value != "03" && value != "04" && value != "05" && value != "51" && value != "52" && value != "53" && value != "54" && value != "55")
                {
                    throw new ValidatorDFeException("O CST do grupo de tributação do IPI não tributado está incorreto. Valor informado: " + Tag.Value + " - Valores aceitos: 01, 02, 03, 04, 05, 51, 52, 53, 54 ou 55." +
                        " [Item: " + nItem + "] [cProd: " + cProd + "] [xProd: " + xProd + "] [TAG: <CST> do grupo de tag <det><imposto><IPI><IPINT>]");
                }
            }).ValidateTag(element => element.NameEquals(nameof(PISAliq.CST)) && element.Parent.NameEquals(nameof(PISAliq)), Tag =>
            {
                var value = Tag.Value;
                var cProd = Tag.Parent.Parent.Parent.Parent.GetValue("cProd");
                var xProd = Tag.Parent.Parent.Parent.Parent.GetValue("xProd");
                var nItem = Tag.Parent.Parent.Parent.Parent.GetAttributeValue("nItem");

                if (value != "01" && value != "02")
                {
                    throw new ValidatorDFeException("O CST do grupo de tributação do PIS tributado pela alíquota está incorreto. Valor informado: " + Tag.Value + " - Valores aceitos: 01 ou 02." +
                        " [Item: " + nItem + "] [cProd: " + cProd + "] [xProd: " + xProd + "] [TAG: <CST> do grupo de tag <det><imposto><PIS><PISAliq>]");
                }
            }).ValidateTag(element => element.NameEquals(nameof(PISQtde.CST)) && element.Parent.NameEquals(nameof(PISQtde)), Tag =>
            {
                var value = Tag.Value;
                var cProd = Tag.Parent.Parent.Parent.Parent.GetValue("cProd");
                var xProd = Tag.Parent.Parent.Parent.Parent.GetValue("xProd");
                var nItem = Tag.Parent.Parent.Parent.Parent.GetAttributeValue("nItem");

                if (value != "03")
                {
                    throw new ValidatorDFeException("O CST do grupo de tributação do PIS tributado por quantidade está incorreto. Valor informado: " + Tag.Value + " - Valore aceito: 03." +
                        " [Item: " + nItem + "] [cProd: " + cProd + "] [xProd: " + xProd + "] [TAG: <CST> do grupo de tag <det><imposto><PIS><PISQtde>]");
                }
            }).ValidateTag(element => element.NameEquals(nameof(PISNT.CST)) && element.Parent.NameEquals(nameof(PISNT)), Tag =>
            {
                var value = Tag.Value;
                var cProd = Tag.Parent.Parent.Parent.Parent.GetValue("cProd");
                var xProd = Tag.Parent.Parent.Parent.Parent.GetValue("xProd");
                var nItem = Tag.Parent.Parent.Parent.Parent.GetAttributeValue("nItem");

                if (value != "04" && value != "05" && value != "06" && value != "07" && value != "08" && value != "09")
                {
                    throw new ValidatorDFeException("O CST do grupo de tributação do PIS não tributado está incorreto. Valor informado: " + Tag.Value + " - Valores aceitos: 04, 05, 06, 07, 08 ou 09." +
                        " [Item: " + nItem + "] [cProd: " + cProd + "] [xProd: " + xProd + "] [TAG: <CST> do grupo de tag <det><imposto><PIS><PISNT>]");
                }
            }).ValidateTag(element => element.NameEquals(nameof(PISOutr.CST)) && element.Parent.NameEquals(nameof(PISOutr)), Tag =>
            {
                var value = Tag.Value;
                var cProd = Tag.Parent.Parent.Parent.Parent.GetValue("cProd");
                var xProd = Tag.Parent.Parent.Parent.Parent.GetValue("xProd");
                var nItem = Tag.Parent.Parent.Parent.Parent.GetAttributeValue("nItem");

                if (value != "49" &&
                value != "50" && value != "51" && value != "52" && value != "53" && value != "54" && value != "55" && value != "56" &&
                value != "60" && value != "61" && value != "62" && value != "63" && value != "64" && value != "65" && value != "66" && value != "67" &&
                value != "70" && value != "71" && value != "72" && value != "73" && value != "74" && value != "75" &&
                value != "98" && value != "99")
                {
                    throw new ValidatorDFeException("O CST do grupo de tributação do PIS outras operações está incorreto. Valor informado: " + Tag.Value + " - Valores aceitos: 49, 50, 51, 52, 53, 54, 55, 56, 60, 61, 62, 63, 64, 65, 66, 67, 70, 71, 72, 73, 74, 75, 98 ou 99." +
                        " [Item: " + nItem + "] [cProd: " + cProd + "] [xProd: " + xProd + "] [TAG: <CST> do grupo de tag <det><imposto><PIS><PISOutr>]");
                }
            }).ValidateTag(element => element.NameEquals(nameof(COFINSAliq.CST)) && element.Parent.NameEquals(nameof(COFINSAliq)), Tag =>
            {
                var value = Tag.Value;
                var cProd = Tag.Parent.Parent.Parent.Parent.GetValue("cProd");
                var xProd = Tag.Parent.Parent.Parent.Parent.GetValue("xProd");
                var nItem = Tag.Parent.Parent.Parent.Parent.GetAttributeValue("nItem");

                if (value != "01" && value != "02")
                {
                    throw new ValidatorDFeException("O CST do grupo de tributação do COFINS tributado pela alíquota está incorreto. Valor informado: " + Tag.Value + " - Valores aceitos: 01 ou 02." +
                        " [Item: " + nItem + "] [cProd: " + cProd + "] [xProd: " + xProd + "] [TAG: <CST> do grupo de tag <det><imposto><COFINS><COFINSAliq>]");
                }
            }).ValidateTag(element => element.NameEquals(nameof(COFINSQtde.CST)) && element.Parent.NameEquals(nameof(COFINSQtde)), Tag =>
            {
                var value = Tag.Value;
                var cProd = Tag.Parent.Parent.Parent.Parent.GetValue("cProd");
                var xProd = Tag.Parent.Parent.Parent.Parent.GetValue("xProd");
                var nItem = Tag.Parent.Parent.Parent.Parent.GetAttributeValue("nItem");

                if (value != "03")
                {
                    throw new ValidatorDFeException("O CST do grupo de tributação do COFINS tributado por quantidade está incorreto. Valor informado: " + Tag.Value + " - Valore aceito: 03." +
                        " [Item: " + nItem + "] [cProd: " + cProd + "] [xProd: " + xProd + "] [TAG: <CST> do grupo de tag <det><imposto><COFINS><COFINSQtde>]");
                }
            }).ValidateTag(element => element.NameEquals(nameof(COFINSNT.CST)) && element.Parent.NameEquals(nameof(COFINSNT)), Tag =>
            {
                var value = Tag.Value;
                var cProd = Tag.Parent.Parent.Parent.Parent.GetValue("cProd");
                var xProd = Tag.Parent.Parent.Parent.Parent.GetValue("xProd");
                var nItem = Tag.Parent.Parent.Parent.Parent.GetAttributeValue("nItem");

                if (value != "04" && value != "05" && value != "06" && value != "07" && value != "08" && value != "09")
                {
                    throw new ValidatorDFeException("O CST do grupo de tributação do COFINS não tributado está incorreto. Valor informado: " + Tag.Value + " - Valores aceitos: 04, 05, 06, 07, 08 ou 09." +
                        " [Item: " + nItem + "] [cProd: " + cProd + "] [xProd: " + xProd + "] [TAG: <CST> do grupo de tag <det><imposto><COFINS><COFINSNT>]");
                }
            }).ValidateTag(element => element.NameEquals(nameof(COFINSOutr.CST)) && element.Parent.NameEquals(nameof(COFINSOutr)), Tag =>
            {
                var value = Tag.Value;
                var cProd = Tag.Parent.Parent.Parent.Parent.GetValue("cProd");
                var xProd = Tag.Parent.Parent.Parent.Parent.GetValue("xProd");
                var nItem = Tag.Parent.Parent.Parent.Parent.GetAttributeValue("nItem");

                if (value != "49" &&
                value != "50" && value != "51" && value != "52" && value != "53" && value != "54" && value != "55" && value != "56" &&
                value != "60" && value != "61" && value != "62" && value != "63" && value != "64" && value != "65" && value != "66" && value != "67" &&
                value != "70" && value != "71" && value != "72" && value != "73" && value != "74" && value != "75" &&
                value != "98" && value != "99")
                {
                    throw new ValidatorDFeException("O CST do grupo de tributação do COFINS outras operações está incorreto. Valor informado: " + Tag.Value + " - Valores aceitos: 49, 50, 51, 52, 53, 54, 55, 56, 60, 61, 62, 63, 64, 65, 66, 67, 70, 71, 72, 73, 74, 75, 98 ou 99." +
                        " [Item: " + nItem + "] [cProd: " + cProd + "] [xProd: " + xProd + "] [TAG: <CST> do grupo de tag <det><imposto><COFINS><COFINSOutr>]");
                }
            }).ValidateTag(element => element.NameEquals(nameof(Ide.CMunFG)) && element.Parent.NameEquals(nameof(Ide)), Tag =>
            {
                var value = Tag.Value;

                if (value.Length != 7)
                {
                    throw new ValidatorDFeException("O código do município de ocorrência do fato gerador está incorreto. Código informado deve ter 7 dígitos. Valor informado: " + Tag.Value +
                        " [TAG: <cMunFG> do grupo de tag <ide>]");

                }
            }).ValidateTag(element => element.NameEquals(nameof(EnderEmit.CMun)) && element.Parent.NameEquals(nameof(EnderEmit)), Tag =>
            {
                var value = Tag.Value;

                if (value.Length != 7)
                {
                    throw new ValidatorDFeException("O código do município do emitente está incorreto. Código informado deve ter 7 dígitos. Valor informado: " + Tag.Value +
                        " [TAG: <cMun> do grupo de tag <emit><enderEmit>]");

                }
            }).ValidateTag(element => element.NameEquals(nameof(Retirada.CMun)) && element.Parent.NameEquals(nameof(Retirada)), Tag =>
            {
                var value = Tag.Value;

                if (value.Length != 7)
                {
                    throw new ValidatorDFeException("O código do município do local de retirada está incorreto. Código informado deve ter 7 dígitos. Valor informado: " + Tag.Value +
                        " [TAG: <cMun> do grupo de tag <retirada>]");

                }
            }).ValidateTag(element => element.NameEquals(nameof(Entrega.CMun)) && element.Parent.NameEquals(nameof(Entrega)), Tag =>
            {
                var value = Tag.Value;

                if (value.Length != 7)
                {
                    throw new ValidatorDFeException("O código do município do local de entrega está incorreto. Código informado deve ter 7 dígitos. Valor informado: " + Tag.Value +
                        " [TAG: <cMun> do grupo de tag <entrega>]");

                }
            }).ValidateTag(element => element.NameEquals(nameof(ISSQN.CMunFG)) && element.Parent.NameEquals(nameof(ISSQN)), Tag =>
            {
                var value = Tag.Value;

                if (value.Length != 7)
                {
                    throw new ValidatorDFeException("O código do município de ocorrência do fato gerador do ISSQN está incorreto. Código informado deve ter 7 dígitos. Valor informado: " + Tag.Value +
                        " [TAG: <cMunFG> do grupo de tag <ISSQN>]");

                }
            }).ValidateTag(element => element.NameEquals(nameof(ISSQN.CMun)) && element.Parent.NameEquals(nameof(ISSQN)), Tag =>
            {
                var value = Tag.Value;

                if (value.Length != 7)
                {
                    throw new ValidatorDFeException("O código do município de incidência do imposto ISSQN está incorreto. Código informado deve ter 7 dígitos. Valor informado: " + Tag.Value +
                        " [TAG: <cMun> do grupo de tag <ISSQN>]");

                }
            }).ValidateTag(element => element.NameEquals(nameof(RetTransp.CMunFG)) && element.Parent.NameEquals(nameof(RetTransp)), Tag =>
            {
                var value = Tag.Value;

                if (value.Length != 7)
                {
                    throw new ValidatorDFeException("O código do município de ocorrência do fato gerador do ICMS de transporte está incorreto. Código informado deve ter 7 dígitos. Valor informado: " + Tag.Value +
                        " [TAG: <cMunFG> do grupo de tag <transp><retTransp>]");

                }
            }).ValidateTag(element => element.NameEquals(nameof(Comb.UFCons)) && element.Parent.NameEquals(nameof(Comb)) && element.Parent.Parent.NameEquals(nameof(Prod)), Tag =>
            {
                var value = Tag.Value;
                var cProd = Tag.Parent.Parent.GetValue("cProd");
                var xProd = Tag.Parent.Parent.GetValue("xProd");
                var nItem = Tag.Parent.Parent.Parent.GetAttributeValue("nItem");

                if (string.IsNullOrWhiteSpace(value.Trim()))
                {
                    throw new ValidatorDFeException("A sigla da UF de consumo das informações específicas para combustíveis líquidos e lubrificantes está em branco e é obrigatória." +
                        " [Item: " + nItem + "] [cProd: " + cProd + "] [xProd: " + xProd + "] [TAG: <UFCons> do grupo de tag <det><prod><comb>]");

                }
            });

        #endregion Public Constructors

        #region Public Methods

        /// <summary>
        /// <inheritdoc cref="XmlValidatorBase.CanValidate(XElement)"/>
        /// </summary>
        /// <param name="element"><inheritdoc cref="XmlValidatorBase.CanValidate(XElement)"/></param>
        /// <returns></returns>
        public override bool CanValidate(XElement element) => XMLUtility.DetectDFeType(element.ToString()) == TipoDFe.NFe || XMLUtility.DetectDFeType(element.ToString()) == TipoDFe.NFCe;

        #endregion Public Methods
    }
}