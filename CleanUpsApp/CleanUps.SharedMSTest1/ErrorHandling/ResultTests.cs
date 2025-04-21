using Microsoft.VisualStudio.TestTools.UnitTesting;
using CleanUps.Shared.ErrorHandling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanUps.Shared.ErrorHandling.Tests
{
    [TestClass()]
    public class ResultTests
    {

        [TestInitialize]
        public void Setup()
        {

        }

        [TestMethod()]
        public void IsResultGenericForString()
        {
            //Arrange
            string data = "test data";
            //Act
            var result = Result<string>.Ok(data);


            //Assert
            Assert.IsTrue(result.IsSuccess);
        }

        [TestMethod()]
        public void IsResultGenericForBool()
        {
            //Arrange
            bool data = true;
            //Act
            var result = Result<bool>.Ok(data);


            //Assert
            Assert.IsTrue(result.IsSuccess);
        }

        [TestMethod()]
        public void IsResultGenericForDecimal()
        {
            //Arrange
            decimal data = 1000.0m;
            //Act
            var result = Result<decimal>.Ok(data);


            //Assert
            Assert.IsTrue(result.IsSuccess);
        }


        [TestMethod()]
        public void DoesResultsStatusCodeWork_PositiveCode()
        {
            //Arrange
            bool data = true;
            //Act
            var result = Result<bool>.Ok(data);


            //Assert
            Assert.AreEqual(200, result.StatusCode);
        }

        [TestMethod()]
        public void DoesResultsDataPropertyWork()
        {
            //Arrange
            string data = "true";
            //Act
            var result = Result<string>.Ok(data);


            //Assert
            Assert.AreEqual(data, result.Data);
        }


        [TestMethod()]
        public void DoesResultsErrorMessageWork()
        {
            //Arrange
            string data = "true";
            //Act
            var result = Result<string>.Ok(data);


            //Assert
            Assert.IsNull(result.ErrorMessage);
        }

        [TestMethod()]
        public void CreatedTest()
        {
            string data = "new data";

            var result = Result<string>.Created(data);

            Assert.IsTrue(result.IsSuccess);
            Assert.AreEqual(201, result.StatusCode);
            Assert.AreEqual(data, result.Data);
            Assert.IsNull(result.ErrorMessage);
        }

        [TestMethod()]
        public void NoContentReturns204InItsStatusCodeProperty()
        {
            var result = Result<string>.NoContent();

            Assert.AreEqual(204, result.StatusCode);
        }

        [TestMethod()]
        public void NotModifiedTest()
        {
            var result = Result<string>.NotModified();

            Assert.AreEqual(304, result.StatusCode);
        }

        [TestMethod()]
        public void BadRequestReturns400AndSavesTheStringMessageCorrectly()
        {
            //Arrange
            string errorMsg = "You had a bad request. Try again.";

            //Act
            var result = Result<string>.BadRequest(errorMsg);

            //Assert
            Assert.IsFalse(result.IsSuccess); //We don't reeeeealy need both of these. The one below will do as it should confirm that which statuscode range we are in.
            Assert.AreEqual(400, result.StatusCode);
            Assert.AreEqual(errorMsg, result.ErrorMessage);
        }

        [TestMethod()]
        public void UnauthorizedReturns401AndSavesTheStringMessageCorrectly()
        {
            //Arrange
            string errorMsg = "Unauthorized Attempt Access. Step away from the computer. The Holy Inquisition in on its way to apprehend you.";

            //Act
            var result = Result<string>.Unauthorized(errorMsg);

            //Assert
            Assert.AreEqual(401, result.StatusCode);
            Assert.AreEqual(errorMsg, result.ErrorMessage);
        }

        [TestMethod()]
        public void ForbiddenReturns403AndSavesTheStringMessageCorrectly()
        {
            //Arrange
            string errorMsg = "Forbidden!!!!!! Desist! Stop! Heresy yonder!!";

            //Act
            var result = Result<string>.Forbidden(errorMsg);

            //Assert
            Assert.AreEqual(403, result.StatusCode);
            Assert.AreEqual(errorMsg, result.ErrorMessage);
        }

        [TestMethod()]
        public void NotFoundReturns404AndSavesTheStringMessageCorrectly()
        {
            //Arrange
            string errorMsg = "The page was not found. Try again.";

            //Act
            var result = Result<string>.NotFound(errorMsg);

            //Assert
            Assert.AreEqual(404, result.StatusCode);
            Assert.AreEqual(errorMsg, result.ErrorMessage);
        }

        [TestMethod()]
        public void ConflictReturns409AndSavesTheStringMessageCorrectly()
        {
            //Arrange
            string errorMsg = "Conflict! RUN AWAAAAAAY!.";

            //Act
            Result<string> result = Result<string>.Conflict(errorMsg);

            //Assert
            Assert.AreEqual(409, result.StatusCode);
            Assert.AreEqual(errorMsg, result.ErrorMessage);
        }

        [TestMethod()]
        public void InternalServerErrorReturns500AndSavesTheStringMessageCorrectly()
        {
            //Arrange
            string errorMsg = "The issue in question is being dealt with under internal departmental jurisdictional scrutiny and tribual.";

            //Act
            var result = Result<string>.InternalServerError(errorMsg);

            //Assert
            Assert.AreEqual(500, result.StatusCode);
            Assert.AreEqual(errorMsg, result.ErrorMessage);
        }

        [TestMethod()]
        public void TransformTest()
        {
            Assert.Fail();
        }

        //https://developer.mozilla.org/en-US/docs/Web/HTTP/Reference/Status/400
        //So like with other classes I need to test edge-cases. Test if they return true. Test when they shouldn't return true.
    }
}