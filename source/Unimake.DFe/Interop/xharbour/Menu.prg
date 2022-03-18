/*

Unimake Software
Exemplo de uso da DLL Unimake.Dfe
Data: 15/02/2022 
Autores: 
- Edson Mundin Ferreira
- Wandrey Mundin Ferreira

*/
Function Main()
   Local aOpcoes
   
   aOpcoes := {}
   AAdd(aOpcoes, "1-Carregar Certificado A1")
   AAdd(aOpcoes, "2-Consulta Status Nfe")
   AAdd(aOpcoes, "3-Consulta Situacao Nfe")
   AAdd(aOpcoes, "4-Inutilizar Numero Nfe")
   AAdd(aOpcoes, "5-Consulta Recibo Nfe")
   AAdd(aOpcoes, "6-Enviar Nfe - Assincrono")
   AAdd(aOpcoes, "7-Cancelar Nfe")
   Aadd(aOpcoes, "8-Teste diversos com certificado digital")
   
   Do While .T.
      Cls

      @ 1,2 Say "Unimake.Dfe DLL"
	  
      nOpcao := Achoice( 3, 2, 20, 50, aOpcoes)

      Cls

      do case
         case LastKey() = 27
              Exit

         case nOpcao = 1
              CarregarCertificadoA1()

         case nOpcao = 2
              ConsultaStatusNfe()

         case nOpcao = 3
              ConsultaSituacaoNfe()

         case nOpcao = 4
              InutilizarNumeroNfe()

         case nOpcao = 5
              ConsultaReciboNfe()

         case nOpcao = 6
              EnviarNfeAssincrono()

         case nOpcao = 7
              CancelarNfe()
			  
         case nOpcao = 8
              TesteDiversoCertificado()
       endcase
   EndDo
Return