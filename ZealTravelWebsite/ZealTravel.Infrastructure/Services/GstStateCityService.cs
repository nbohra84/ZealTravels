using System;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Threading.Tasks;
using ZealTravelWebsite.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using ZealTravel.Domain.Data.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using ZealTravel.Domain.Interfaces.Services;
using System.ComponentModel.Design;
using ZealTravel.Domain.Data.Entities;

namespace ZealTravel.Infrastructure.Services
{
    public class GstStateCityService : IGstStateCityService
    {
        private readonly ZealdbNContext _context;

        public GstStateCityService(ZealdbNContext context)
        {
            _context = context;
        }

        public async Task<List<GstStateCity>> GetCityByStateNameAsync(string stateName)
        {
            try
            {
                var cities = await _context.Database.SqlQuery<GstStateCity>($"EXECUTE Gst_State_City_Proc @ProcNo = 4, @StateName = {stateName}").ToListAsync();
                return cities;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving cities for state {stateName}: {ex.Message}", ex);
            }
        }

        public async Task<List<GstState>> GetStatesAsync()
        {
            var dtStates = new DataTable();

            try
            {
                var states = await _context.Database.SqlQuery<GstState>($"EXECUTE Gst_State_City_Proc @ProcNo = 2").ToListAsync();
                return states;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving states: {ex.Message}");
            }
        }

        public async Task<List<CityState>> GetCityByStateAsync(string stateName = null)
        {
            List<CityState> cityStates = new List<CityState>();

            try
            {
                var states = await GetStatesAsync();

                if (string.IsNullOrWhiteSpace(stateName))
                {
                    foreach (var state in states)
                    {
                        string stateCode = state.StateCode;
                        string stateNameValue = state.State;

                        var cities = await GetCityByStateNameAsync(stateNameValue);

                        CityState cityState = new CityState
                        {
                            StateCode = stateCode,
                            StateName = stateNameValue,
                            Cities = cities.Select(c => c.City).ToList()
                        };

                        cityStates.Add(cityState);
                    }
                }
                else
                {
                    var cities = await GetCityByStateNameAsync(stateName);
                    if (cities != null && cities.Any())
                    {
                        var state = states.FirstOrDefault(s => s.State == stateName);
                        if (state != null)
                        {
                            cityStates.Add(new CityState
                            {
                                StateCode = state.StateCode,
                                StateName = stateName,
                                Cities = cities.Select(c => c.City).ToList()
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving city states: {ex.Message}", ex);
            }

            return cityStates;
        }




    }
}
