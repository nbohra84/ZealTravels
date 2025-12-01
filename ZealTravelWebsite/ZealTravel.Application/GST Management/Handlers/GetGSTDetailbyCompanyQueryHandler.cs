using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZealTravel.Application.GSTManagement.Queries;
using ZealTravel.Domain.BookingManagement;
using ZealTravel.Domain.Interfaces.GSTManagement;
using ZealTravel.Domain.Interfaces.Handlers;

namespace ZealTravel.Application.GSTManagement.Handlers
{
    public class GetGSTDetailbyCompanyQueryHandler : IHandlesQueryAsync<string, GSTDetails>
    {
        private readonly IGSTService _gstService;
        private readonly IMapper _mapper;

        public GetGSTDetailbyCompanyQueryHandler(IGSTService gstService, IMapper mapper)
        {
            _gstService = gstService;
            _mapper = mapper;
        }


        public async Task<GSTDetails> HandleAsync(string companyID)
        {
            try
            {
                var data = await _gstService.GetGSTdetail(companyID);
                if (data != null)
                {
                    return _mapper.Map<GSTDetails>(data);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error get gse details for {companyID}: {ex.Message}", ex);
            }
        }
    }
}
