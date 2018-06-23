using Domain.Repositories.Abstract;
using Domain.Repositories.Concrete;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace ReceivablesAnticipationTests
{
    public class TransactionsControllerTests
    {
        [Fact]
        public void ObtainAllTest()
        {
            // Arrange
            //ITransactionRepository mockRepo = new EFTransactionRepository();
            //var controller = new HomeController(mockRepo.Object);

            //// Act
            //var result = await controller.Index();

            //// Assert
            //var viewResult = Assert.IsType<ViewResult>(result);
            //var model = Assert.IsAssignableFrom<IEnumerable<StormSessionViewModel>>(
            //    viewResult.ViewData.Model);
            //Assert.Equal(2, model.Count());
        }
    }
}
