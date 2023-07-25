using System.Xml;

namespace IBetting.Services.DataConsumeService
{
    public interface IDataConsumeService
    {
        Task<XmlDocument> LoadFile();
    }
}