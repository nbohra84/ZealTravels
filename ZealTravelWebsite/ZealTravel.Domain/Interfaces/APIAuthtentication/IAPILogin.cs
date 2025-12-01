using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZealTravel.Domain.Models;

namespace ZealTravel.Domain.Interfaces.APIAuthtentication
{
    public interface IAPILogin
    {
        public string GetTokenid(TokenRequestModel model);
    }
}
