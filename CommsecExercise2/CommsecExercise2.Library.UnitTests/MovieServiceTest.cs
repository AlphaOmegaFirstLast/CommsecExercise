using System;
using System.Collections.Generic;
using System.ServiceModel;
using CommsecExercise2.Library.Interfaces;
using CommsecExercise2.Library.Managers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MoviesLibrary;

namespace CommsecExercise2.Library.UnitTests
{
    [TestClass]
    public class MovieServiceTest
    {
        private Mock<ICacheManager> _cacheManager = new Mock<ICacheManager>();
        private readonly Mock<IDataManager> _dataManager = new Mock<IDataManager>();
        private MovieManager _movieManager;
        private List<MovieData>  movieList = new List<MovieData>();

        //-------------------------------------------------------------------------------------
        [TestInitialize]
        public void Init()
        {
            movieList.Add(new MovieData() { Title = "ccc" , Genre = "yyy"});
            movieList.Add(new MovieData() { Title = "bbb" , Genre = "xxx" });
            movieList.Add(new MovieData() { Title = "aaa" , Genre = "zzz" });

            _dataManager.Setup(x => x.Get()).Returns(movieList);
            _cacheManager.Setup(x => x.Get<List<MovieData>>(It.IsAny<string>())).Returns(movieList);

            _movieManager = new MovieManager(_dataManager.Object, _cacheManager.Object);
        }

    //-------------------------- sort by field name ---------------------------------

    [TestMethod]
        public void GetSorted_Success()
        {
            var result = _movieManager.GetSorted("title");
            Assert.IsNotNull(result);
            Assert.IsTrue(result[0].Title.Equals("aaa"));

            result = _movieManager.GetSorted("genre");
            Assert.IsNotNull(result);
            Assert.IsTrue(result[0].Title.Equals("bbb"));
        }

        [TestMethod]
        public void GetSorted_InValidSortField()
        {
            try
            {
                try
                {
                    _movieManager.GetSorted("invalidFieldName");
                    Assert.Fail("An exception should have been thrown");
                }
                catch (FaultException faultEx)
                {
                    Assert.AreEqual("Invalid Sort Field Name", faultEx.Code.Name);
                }
            }
            catch (Exception e)
            {
                Assert.Fail(string.Format("Unexpected exception of type {0} caught: {1}", e.GetType(), e.Message));
            }
        }
        [TestMethod]
        public void GetSorted_EmptySortField()
        {
            try
            {
                try
                {
                    _movieManager.GetSorted("");
                    Assert.Fail("An exception should have been thrown");
                }
                catch (FaultException faultEx)
                {
                    Assert.AreEqual("Invalid Sort Field Name", faultEx.Code.Name);
                }
            }
            catch (Exception e)
            {
                Assert.Fail(string.Format("Unexpected exception of type {0} caught: {1}", e.GetType(), e.Message));
            }
        }

    }
}
