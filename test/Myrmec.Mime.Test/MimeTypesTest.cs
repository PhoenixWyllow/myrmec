using Xunit;

namespace Myrmec.Mime.Test
{

	public class MimeTypesTest
	{
		[Fact]
		public void DotTest()
		{
			string extension = ".jpg";
			string result = MimeType.GetMimeType(extension);
			Assert.Equal("image/jpeg", result);
		}

		[Fact]
		public void RegularTest()
		{
			string extension = "jpg";
			string result = MimeType.GetMimeType(extension);
			Assert.Equal("image/jpeg", result);
		}
	}
}
