using System.ServiceModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CommsecExercise2.Library.IntegrationTests
{
    [TestClass]
    public class MovieServiceTest
    {
        internal static ServiceHost Instance = null;

        [ClassInitialize()]
        public static void ClassInitialize(TestContext testContext)
        {
            Instance = new ServiceHost(typeof(MovieService.MovieServiceClient));
            //no need at the current time as CommsecExercise2.Library has the option "Start WCF service host..." on, in "debug" mode the service is up by default.
            //    Instance.Open();  
        }

        [ClassCleanup()]
        public static void MyClassCleanup()
        {
            if (Instance.State != CommunicationState.Closed)
            {
                Instance.Close();
            }
        }

        //------------------------------------------------------------

        [TestMethod]
        public void Get_Success()
        {
            using (var factory = new ChannelFactory<MovieService.IMovieService>("BasicHttpBinding_IMovieService"))
            {
                var client = factory.CreateChannel();
                var response = client.Get();

                Assert.IsNotNull(response);
            }
        }

    }
}
