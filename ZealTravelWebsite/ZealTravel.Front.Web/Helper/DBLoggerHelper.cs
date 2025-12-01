using ZealTravel.Application.BankManagement.Handler;
using ZealTravel.Application.DBCommonManagement.Commands;
using ZealTravel.Domain.Interfaces.Handlers;

namespace ZealTravel.Front.Web.Helper
{
    public class DBLoggerHelper
    {
        public static async Task DBLogAsync(string CompanyID, Int32 BookingRef, string MethodName, string Location, string SearchCriteria, string SearchID, string ErrorMessage, IHandlesCommandAsync<AddDBLogCommand> addDbLogCommandHandler)
        {
            var addDBLogger = new AddDBLogCommand
            {
                CompanyID = CompanyID,
                BookingRef = BookingRef,
                MethodName = MethodName,
                Location = Location,
                SearchCriteria = SearchCriteria,
                SearchID = SearchID,
                ErrorMessage = ErrorMessage
            };

            await addDbLogCommandHandler.HandleAsync(addDBLogger);
        }
    }
}
