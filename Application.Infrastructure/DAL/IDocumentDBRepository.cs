using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;        
namespace Application.Infrastructure.DAL
{
    public interface IDocumentDbRepository<T>
    {

          Task<IEnumerable<T>> GetItemsAsync(Expression<Func<T, bool>> predicate);
    }
}