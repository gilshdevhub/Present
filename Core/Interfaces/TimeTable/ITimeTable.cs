namespace Core.Interfaces.TimeTable;

[System.ServiceModel.ServiceContractAttribute(ConfigurationName = "ITimeTable")]
public interface ITimeTable
{
    [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/GetLuz", ReplyAction = "*")]
    [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults = true)]
    System.Threading.Tasks.Task<System.Xml.XmlNode> GetLuzAsync(int SystemId, string SystemUserName, string SystemPass, int Orign, int Destination, string Date, int Hours);
}
