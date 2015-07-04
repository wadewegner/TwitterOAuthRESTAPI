using NUnit.Framework;
using System.Dynamic;
using TwitterOAuth.RestAPI.Resources;

namespace RestAPI.Tests
{
    [TestFixture]
    public class UriHelperTests
    {
        [Test]
        public void EnsureProperQueryStringFromDynamic()
        {
            // arrange
            dynamic parameters = new ExpandoObject();
            parameters.screen_name = "twitter";

            // act
            var queryString = UriHelper.ConvertDynamicToQueryStringParameters(parameters);

            Assert.AreEqual("screen_name=twitter", queryString);

            // arrange
            parameters.page = "2";
            
            // act
            queryString = UriHelper.ConvertDynamicToQueryStringParameters(parameters);

            Assert.AreEqual("screen_name=twitter&page=2", queryString);
        }

        [Test]
        public void EnsureBlankPropertiesIgnored()
        {
            // arrange
            dynamic parameters = new ExpandoObject();
            parameters.screen_name = "twitter";
            parameters.page = "";

            // act
            var queryString = UriHelper.ConvertDynamicToQueryStringParameters(parameters);

            Assert.AreEqual("screen_name=twitter", queryString);
        }

        [Test]
        public void EnsureParametersEscaped()
        {
            // arrange
            dynamic parameters = new ExpandoObject();
            parameters.screen_name = "twi++er";

            // act
            var queryString = UriHelper.ConvertDynamicToQueryStringParameters(parameters);

            Assert.AreEqual(@"screen_name=twi%2B%2Ber", queryString);
        }
    }
}