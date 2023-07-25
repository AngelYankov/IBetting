using System.Xml;

namespace IBetting.Services.DataConsumeService
{
    public class DataConsumeService : IDataConsumeService
    {
        /// <summary>
        /// Loads XML data from source
        /// </summary>
        /// <returns>Loaded XML data</returns>
        public async Task<XmlDocument> LoadFile()
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                string url = Constants.XmlFeedLink;

                using (var client = new HttpClient())
                {
                    var response = await client.GetAsync(url);
                    response.EnsureSuccessStatusCode();
                    string result = await response.Content.ReadAsStringAsync();

                    doc.LoadXml(result);
                }

                //string filePath = @"C:\Users\AngelYankov\Desktop\data.xml";
                //doc.Load(filePath);

                return doc;
            }
            catch (HttpRequestException e)
            {
                throw new Exception("Error while downloading the XML document: " + e.Message);
            }
            catch (XmlException e)
            {
                throw new Exception("Error while parsing the XML document: " + e.Message);
            }
        }
    }
}
