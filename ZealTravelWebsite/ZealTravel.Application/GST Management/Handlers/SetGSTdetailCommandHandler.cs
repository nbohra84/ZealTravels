using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZealTravel.Application.GST_Management.Commands;
using ZealTravel.Domain.Interfaces.GSTManagement;
using ZealTravel.Domain.Interfaces.Handlers;

namespace ZealTravel.Application.GST_Management.Handlers
{
    public class SetGSTdetailCommandHandler : IHandlesCommandAsync<SetGSTdetailCommand>
    {
        private readonly IGSTService _gstService;
        public SetGSTdetailCommandHandler(IGSTService gstService)
        {
            _gstService = gstService;
        }
        public async Task HandleAsync(SetGSTdetailCommand command)
        {
           try
            {
                var result = await _gstService.SETGSTdetail(command.CompanyID, command.PassengerResponse, command.GSTInfo);
               
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }
    }
}
