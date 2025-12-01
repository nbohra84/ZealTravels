using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZealTravel.Domain.Interfaces.Handlers
{
    public interface IHandlesQuery<in TQuery, out TResult>
    {
        TResult Handle(TQuery query);
    }

    public interface IHandlesQueryAsync<in TQuery, TResult>
    {
        Task<TResult> HandleAsync(TQuery query);
    }

    public interface IHandlesQueryAsync<TResult>
    {
        Task<TResult> HandleAsync();
    }
    public interface IHandlesCommandAsync<TCommand>
    {
        Task HandleAsync(TCommand command);
    }
}
