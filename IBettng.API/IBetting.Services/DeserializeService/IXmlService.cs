using System.Xml;

namespace IBetting.Services.DeserializeService
{
    public interface IXmlService
    {
        Task<XmlDocument> TransformXml();
    }
}