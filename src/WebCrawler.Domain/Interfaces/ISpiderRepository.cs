using System.Threading.Tasks;
using WebCrawler.Domain.Results;

namespace WebCrawler.Domain.Interfaces
{
    public interface ISpiderRepository
    {
        Task CreateAsync(SpiderData item);
        Task<Rootobject> GetAllAsync();
        Task UpdateAsync(SpiderData item);
    }
}