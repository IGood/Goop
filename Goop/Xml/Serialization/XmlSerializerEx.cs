namespace Goop.Xml.Serialization
{
	using System.IO;
	using System.Xml.Serialization;

	public static class XmlSerializerEx
	{
		public static T Deserialize<T>(this XmlSerializer x, string path)
		{
			using var stream = File.OpenRead(path);
			return (T)x.Deserialize(stream)!;
		}
	}
}
