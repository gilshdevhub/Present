using System.ComponentModel.DataAnnotations;

namespace Core.Entities;

public class TxnSetupRequestDto
{
    [Required]
    public string user { get; set; }
    [Required]
    public string password { get; set; }

    public string int_in { get; set; }
}
public class TxnInternalRequestDto
{
    public decimal totalAmount { get; set; }
    public string language { get; set; }
    public string successUrl { get; set; }
    public string errorUrl { get; set; }
    public string cancelUrl { get; set; }
    public string ppsJSONConfig { get; set; }

}
public class AutoCommRequestDto
{
    public string cardId { get; set; }
    public string total { get; set; }
    public string sessionCD { get; set; }
    public string language { get; set; }
}
public class InquireTransactionsRequestDto
{
    public string language { get; set; }
    public string txId { get; set; }
}
public class LoginRequestDto
{
    [Required]
    public string PersonID { get; set; }
    [Required]
    public string FineID { get; set; }
    public int StationID { get; set; }
    public int MachineTypeID { get; set; }
    public int IdentificationCardTypes { get; set; } = 1;
    public int MachineNumber { get; set; }

}

public class LoginShortRequestDto
{
    [Required]
    public string PersonID { get; set; }
    [Required]
    public string FineID { get; set; }

}


public class TxnSetupResponseDto
{
    [Required]
    public string total

    { get; set; }
    [Required]
    public string paymentNumber { get; set; }
}

public class LoginResponseList
{
    public List<LoginResponseDto> lstFineDEtailsByPassengerID { get; set; }
}


public class LoginResponseDto
{
    public DateTime EventDate { get; set; } = DateTime.Now;
    public decimal TotalAmount { get; set; } = 9;
    public string FirstName { get; set; } = "רון";
    public string LastName { get; set; } = "משו";
    public string Address { get; set; } = "קרני יהודה 0 תל אביב - יפו  ";
    public string RequestNumber { get; set; } = "7139";
    public int FineID { get; set; } = 802600103;
    public string ProfileMagnetic { get; set; } = "1";
    public string OriginalStationCode { get; set; } = "3500";
    public string DestinationStationCode { get; set; } = "3100";
    public string OriginalStationName { get; set; } = "הרצליה";
    public string DestinationStationName { get; set; } = "חדרה מערב";
    public decimal ProfileBasePrice { get; set; } = 18;
    public decimal ChargeBasePrice { get; set; } = 18;
    public decimal ChargeAdditionalPrice { get; set; } = 0;
    public DateTime LastPossiblePayDate { get; set; } = DateTime.Now;
}

public class InquireTransactions
{
    [Key]
    public int Id { get; set; }
    public string mpiTransactionId { get; set; }
    public string internalCommand { get; set; }
    public string internalDateTime { get; set; }
    public string internalrequestId { get; set; }
    public string internaltranId { get; set; }
    public string internalresult { get; set; }
    public string internalmessage { get; set; }
    public string internaluserMessage { get; set; }
    public string internaluseradditionalInfo { get; set; }
    public string internalversion { get; set; }
    public string internallanguage { get; set; }
    public string status { get; set; }
    public string internalstatusText { get; set; }
    public string extendedStatus { get; set; }
    public string extendedStatusText { get; set; }
    public string extendedUserMessage { get; set; }
    public string terminalNumber { get; set; }
    public string internalcardId { get; set; }
    public string cardBin { get; set; }
    public string cardMask { get; set; }
    public string cardLength { get; set; }
    public string cardNo { get; set; }
    public string cardName { get; set; }
    public string cardExpiration { get; set; }
    public string cardType { get; set; }
    public string extendedCardType { get; set; }
    public string blockedCard { get; set; }
    public string lifeStyle { get; set; }
    public string customCardType { get; set; }
    public string creditCompany { get; set; }
    public string cardBrand { get; set; }
    public string cardAcquirer { get; set; }
    public string serviceCode { get; set; }
    public string transactionType { get; set; }
    public string creditType { get; set; }
    public string internalcurrency { get; set; }
    public string baseCurrency { get; set; }
    public string baseAmount { get; set; }
    public string transactionCode { get; set; }
    public string total { get; set; }
    public string firstPayment { get; set; }
    public string periodicalPayment { get; set; }
    public string numberOfPayments { get; set; }
    public string clubId { get; set; }
    public string validation { get; set; }
    public string idStatus { get; set; }
    public string cvvStatus { get; set; }
    public string authSource { get; set; }
    public string authNumber { get; set; }
    public string fileNumber { get; set; }
    public string slaveTerminalNumber { get; set; }
    public string slaveTerminalSequence { get; set; }
    public string eci { get; set; }
    public string clientIp { get; set; }
    public string email { get; set; }
    public string cavv { get; set; }
    public string user { get; set; }
    public string addonData { get; set; }
    public string supplierNumber { get; set; }
    public string internalid { get; set; }
    public string shiftId1 { get; set; }
    public string shiftId2 { get; set; }
    public string shiftId3 { get; set; }
    public string shiftTxnDate { get; set; }
    public string cgUid { get; set; }
    public string cardHash { get; set; }
    public string gateway { get; set; }
    public string idFlag { get; set; }
    public string cvvFlag { get; set; }
    public string mti { get; set; }
    public string sessionCD { get; set; }
    public string userData1 { get; set; }
    public string userData2 { get; set; }
    public string userData3 { get; set; }
    public string userData4 { get; set; }
    public string userData5 { get; set; }
    public string userData6 { get; set; }
    public string userData7 { get; set; }
    public string userData8 { get; set; }
    public string userData9 { get; set; }
    public string userData10 { get; set; }
    public byte railStatus { get; set; }

}


public class Rootobject
{
    public Ashrait ashrait { get; set; }
}

public class Ashrait
{
    public Response response { get; set; }
}

public class Response
{
    public string command { get; set; }
    public string dateTime { get; set; }
    public object requestId { get; set; }
    public string tranId { get; set; }
    public string result { get; set; }
    public string message { get; set; }
    public string userMessage { get; set; }
    public object additionalInfo { get; set; }
    public string version { get; set; }
    public string language { get; set; }
    public Inquiretransactions2 inquireTransactions { get; set; }
}

public class Inquiretransactions2
{
    public Row row { get; set; }
    public Totals totals { get; set; }
}

public class Row
{
    public string mpiTransactionId { get; set; }
    public string uniqueid { get; set; }
    public string amount { get; set; }
    public string currency { get; set; }
    public object authNumber { get; set; }
    public string cardId { get; set; }
    public string languageCode { get; set; }
    public string statusCode { get; set; }
    public string statusText { get; set; }
    public string errorCode { get; set; }
    public string errorText { get; set; }
    public string cgGatewayResponseCode { get; set; }
    public string cgGatewayResponseText { get; set; }
    public Cggatewayresponsexml cgGatewayResponseXML { get; set; }
    public string queryErrorText { get; set; }
    public object xRem { get; set; }
    public object personalId { get; set; }
    public string cardExpiration { get; set; }
}

public class Cggatewayresponsexml
{
    public Ashrait1 ashrait { get; set; }
}

public class Ashrait1
{
    public Response1 response { get; set; }
}

public class Response1
{
    public string command { get; set; }
    public string dateTime { get; set; }
    public object requestId { get; set; }
    public string tranId { get; set; }
    public string result { get; set; }
    public string message { get; set; }
    public string userMessage { get; set; }
    public object additionalInfo { get; set; }
    public string version { get; set; }
    public string language { get; set; }
    public Dodeal doDeal { get; set; }
}

public class Dodeal
{
    public string status { get; set; }
    public string statusText { get; set; }
    public object extendedStatus { get; set; }
    public object extendedStatusText { get; set; }
    public object extendedUserMessage { get; set; }
    public string terminalNumber { get; set; }
    public string cardId { get; set; }
    public string cardBin { get; set; }
    public string cardMask { get; set; }
    public string cardLength { get; set; }
    public string cardNo { get; set; }
    public string cardName { get; set; }
    public string cardExpiration { get; set; }
    public Cardtype cardType { get; set; }
    public Extendedcardtype extendedCardType { get; set; }
    public object blockedCard { get; set; }
    public object lifeStyle { get; set; }
    public object customCardType { get; set; }
    public Creditcompany creditCompany { get; set; }
    public Cardbrand cardBrand { get; set; }
    public Cardacquirer cardAcquirer { get; set; }
    public object serviceCode { get; set; }
    public Transactiontype transactionType { get; set; }
    public Credittype creditType { get; set; }
    public Currency currency { get; set; }
    public object baseCurrency { get; set; }
    public object baseAmount { get; set; }
    public Transactioncode transactionCode { get; set; }
    public string total { get; set; }
    public object firstPayment { get; set; }
    public object periodicalPayment { get; set; }
    public string numberOfPayments { get; set; }
    public object clubId { get; set; }
    public Validation validation { get; set; }
    public Idstatus idStatus { get; set; }
    public Cvvstatus cvvStatus { get; set; }
    public Authsource authSource { get; set; }
    public object authNumber { get; set; }
    public object fileNumber { get; set; }
    public string slaveTerminalNumber { get; set; }
    public string slaveTerminalSequence { get; set; }
    public object eci { get; set; }
    public string clientIp { get; set; }
    public object email { get; set; }
    public Cavv cavv { get; set; }
    public object user { get; set; }
    public object addonData { get; set; }
    public string supplierNumber { get; set; }
    public object id { get; set; }
    public object shiftId1 { get; set; }
    public object shiftId2 { get; set; }
    public object shiftId3 { get; set; }
    public object shiftTxnDate { get; set; }
    public string cgUid { get; set; }
    public object cardHash { get; set; }
    public Acquirerdata acquirerData { get; set; }
    public Ashraitemvdata ashraitEmvData { get; set; }
    public string sessionCD { get; set; }
}

public class Cardtype
{
    public string code { get; set; }
    public string text { get; set; }
}

public class Extendedcardtype
{
    public string code { get; set; }
    public string text { get; set; }
}

public class Creditcompany
{
    public string code { get; set; }
    public string text { get; set; }
}

public class Cardbrand
{
    public string code { get; set; }
    public string text { get; set; }
}

public class Cardacquirer
{
    public string code { get; set; }
    public string text { get; set; }
}

public class Transactiontype
{
    public string code { get; set; }
    public string text { get; set; }
}

public class Credittype
{
    public string code { get; set; }
    public string text { get; set; }
}

public class Currency
{
    public string code { get; set; }
    public string text { get; set; }
}

public class Transactioncode
{
    public string code { get; set; }
    public string text { get; set; }
}

public class Validation
{
    public string code { get; set; }
    public string text { get; set; }
}

public class Idstatus
{
    public string code { get; set; }
}

public class Cvvstatus
{
    public string code { get; set; }
}

public class Authsource
{
    public string code { get; set; }
}

public class Cavv
{
    public string code { get; set; }
}

public class Acquirerdata
{
    public string gateway { get; set; }
}

public class Ashraitemvdata
{
    public string idFlag { get; set; }
    public string cvvFlag { get; set; }
    public string mti { get; set; }
}

public class Totals
{
    public object pageNumber { get; set; }
    public object pagesAmount { get; set; }
    public object queryResultId { get; set; }
    public object total { get; set; }
    public object totalMatch { get; set; }
}


