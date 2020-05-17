using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BasicShop.Tests
{
    [TestClass]
    public class ProductListModelTests
    {
        [TestMethod]
        public void SpecificationParseCorrect()
        {
            //Arrange
            BasicShop.Model.ProductListModel model = new Model.ProductListModel();
            string testString = "Power:123;Height [mm]:9;";

            //Act
            model.SetSpecification(testString);

            //Assert
            Assert.IsNotNull(model.Specifitation);
            Assert.IsNotNull(model.Specifitation[0]);
            Assert.IsNotNull(model.Specifitation[1]);
            Assert.AreEqual(model.Specifitation[0].Element, "Power");
            Assert.AreEqual(model.Specifitation[0].Value, "123");
            Assert.AreEqual(model.Specifitation[1].Element, "Height [mm]");
            Assert.AreEqual(model.Specifitation[1].Value, "9");
        }
    }
}
